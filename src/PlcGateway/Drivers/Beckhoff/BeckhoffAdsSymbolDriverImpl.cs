using PlcGateway.Drivers.Beckhoff.Exceptions;
using PlcGateway.Drivers.Converter;
using System;
using System.Collections.Concurrent;
using System.Text;
using TwinCAT;
using TwinCAT.Ads;
using TwinCAT.Ads.TypeSystem;
using static PlcGateway.Drivers.Beckhoff.AdsErrorCode;
using BeckhoffData = PlcGateway.Drivers.Beckhoff.Data;

namespace PlcGateway.Drivers.Beckhoff
{
    internal class BeckhoffAdsSymbolDriverImpl : BeckhoffDriverImplBase
    {
        private volatile ConcurrentDictionary<string, AdsSymbolMetadata>? Symbols = null;
        private readonly object _initLock = new object();

        public BeckhoffAdsSymbolDriverImpl(AmsNetId amsNetId, AmsPort port, Encoding encoding) : base(amsNetId, port, encoding)
        {

        }

        public BeckhoffAdsSymbolDriverImpl(AmsNetId amsNetId, AmsPort port) : base(amsNetId, port)
        {
        }

        public BeckhoffAdsSymbolDriverImpl(string amsNetId, int port, Encoding encoding) : base(amsNetId, port, encoding)
        {
        }

        public BeckhoffAdsSymbolDriverImpl(string amsNetId, int port) : base(amsNetId, port)
        {
        }

        public override void Connect()
        {
            base.Connect();
            try
            {
                this.InitializeSymbols();
            }
            catch
            {
                this.Disconnect();
                throw;
            }
        }

        public override void Disconnect()
        {
            base.Disconnect();
            Symbols = null;
        }

        /// <summary>
        /// Gets a PLC symbol by its instance path
        /// </summary>
        /// <param name="instancePath">The instance path of the symbol (e.g., "MAIN.MyVariable")</param>
        /// <returns>The ISymbol instance for the requested symbol</returns>
        /// <exception cref="BeckhoffException">Thrown when the symbol is not found or cache is not initialized</exception>
        private AdsSymbolMetadata GetSymbol(string instancePath)
        {
            // Validate input parameter
            if (string.IsNullOrWhiteSpace(instancePath))
            {
                throw new BeckhoffException(
                    code: ADS_SYMBOL_INVALID,
                    message: "Symbol instance path cannot be null or empty",
                    details: "The instancePath parameter must contain a valid PLC symbol path."
                );
            }

            // Snapshot Symbols reference to avoid TOCTOU race with Disconnect()
            var symbols = Symbols;
            if (symbols == null)
            {
                throw new BeckhoffException(
                    code: ADS_SYMBOL_CACHE_NOT_INITIALIZED,
                    message: "PLC symbol cache is not initialized",
                    details: "Symbol cache must be initialized before accessing symbols. Call InitializeSymbolCache() first."
                );
            }

            string normalizedPath = instancePath.Trim();

            // First attempt: Try to get symbol from cache
            if (symbols.TryGetValue(normalizedPath, out var symbol))
            {
                return symbol;
            }

            // Second attempt: read the legacy ADS symbol information directly from the PLC.
            ITcAdsSymbol symbolInfo;
            try
            {
                symbolInfo = this.AdsClient.ReadSymbolInfo(normalizedPath);
            }
            catch (AdsErrorException ex)
            {
                throw new BeckhoffException(
                    code: ADS_SYMBOL_NOT_FOUND,
                    message: $"Symbol '{normalizedPath}' not found in PLC",
                    details: $"ADS Error: {ex.ErrorCode} (0x{(int)ex.ErrorCode:X8}). The symbol may not exist or PLC is not accessible.",
                    innerException: ex
                );
            }

            AdsSymbolMetadata newSymbol;
            try
            {
                newSymbol = new AdsSymbolMetadata(
                    normalizedPath,
                    checked((uint)symbolInfo.IndexGroup),
                    checked((uint)symbolInfo.IndexOffset),
                    symbolInfo.Size
                );
            }
            catch (OverflowException ex)
            {
                throw new BeckhoffException(
                    code: ADS_SYMBOL_INVALID,
                    message: $"Symbol '{normalizedPath}' contains an invalid process image address",
                    details: $"IndexGroup: {symbolInfo.IndexGroup}, IndexOffset: {symbolInfo.IndexOffset}.",
                    innerException: ex
                );
            }

            // Add symbol to cache for future use (re-read Symbols in case it was replaced by InitializeSymbols)
            var currentSymbols = Symbols;
            if (currentSymbols != null)
            {
                currentSymbols[newSymbol.InstancePath] = newSymbol;
            }

            return newSymbol;
        }

        /// <summary>
        /// 初始化倍福调试信息
        /// </summary>
        public void InitializeSymbols()
        {
            lock (_initLock)
            {
                TwinCAT.TypeSystem.ReadOnlySymbolCollection resultSymbols;
                try
                {
                    var loader = SymbolLoaderFactory.Create(this.AdsClient, SymbolLoaderSettings.Default)
                        as IAdsSymbolLoader;

                    if (loader == null)
                    {
                        throw new InvalidOperationException("The ADS symbol loader does not support ADS symbols.");
                    }

                    resultSymbols = loader.Symbols;
                }
                catch (AdsErrorException ex)
                {
                    throw new BeckhoffException(
                        code: ADS_SYMBOL_LOAD_FAILED,
                        message: "Failed to load PLC symbols from target device",
                        details: $"ADS Error Code: {ex.ErrorCode} ({(int)ex.ErrorCode}). " +
                                $"Possible causes: Target PLC is not running, ADS service is not started, " +
                                $"or insufficient permissions to access PLC symbols.",
                        innerException: ex
                    );
                }

                if (resultSymbols.Count == 0)
                {
                    throw new BeckhoffException(
                        code: ADS_NO_SYMBOLS_FOUND,
                        message: "No PLC symbols found on the target device",
                        details: "The PLC symbol table appears to be empty. " +
                                "Possible causes: PLC program is not compiled with debug information, " +
                                "or the symbol table is not available in the current runtime mode."
                    );
                }

                // Save symbol information
                var newSymbols = new ConcurrentDictionary<string, AdsSymbolMetadata>();

                int validSymbols = 0;
                int invalidSymbols = 0;

                foreach (var symbol in resultSymbols)
                {
                    var adsSymbol = symbol as IAdsSymbol;

                    if (string.IsNullOrWhiteSpace(symbol.InstancePath) || adsSymbol == null)
                    {
                        invalidSymbols++;
                        continue;
                    }

                    newSymbols[symbol.InstancePath] = new AdsSymbolMetadata(
                        symbol.InstancePath,
                        adsSymbol.IndexGroup,
                        adsSymbol.IndexOffset,
                        adsSymbol.Size
                    );
                    validSymbols++;
                }

                if (validSymbols == 0)
                {
                    throw new BeckhoffException(
                        code: ADS_NO_VALID_SYMBOLS,
                        message: "No valid symbols with instance paths found",
                        details: $"Total symbols loaded: {resultSymbols.Count}, " +
                                $"Symbols with invalid/empty instance paths: {invalidSymbols}. " +
                                "Check if the PLC program contains proper symbol definitions with instance paths."
                    );
                }

                this.Symbols = newSymbols;
            }
        }

        private sealed class AdsSymbolMetadata
        {
            public string InstancePath { get; }

            public uint IndexGroup { get; }

            public uint IndexOffset { get; }

            public int Size { get; }

            public AdsSymbolMetadata(string instancePath, uint indexGroup, uint indexOffset, int size)
            {
                InstancePath = instancePath;
                IndexGroup = indexGroup;
                IndexOffset = indexOffset;
                Size = size;
            }
        }

        public void Write(string instancePath, sbyte value)
        {
            var symbol = GetSymbol(instancePath);
            var code = this.AdsClient.TryWrite(symbol.IndexGroup, symbol.IndexOffset, new[] { unchecked((byte)value) });

            if (code != TwinCAT.Ads.AdsErrorCode.NoError)
            {
                throw new BeckhoffException(
                    code: ADS_WRITE_ERROR,
                    message: $"Failed to write sbyte value '{value}' to PLC at address {instancePath}",
                    details: $"ADS Error Code: {code} (0x{(uint)code:X8}) - {GetAdsErrorMessage(code)}"
                );
            }
        }

        public void Write(string instancePath, byte value)
        {
            var symbol = GetSymbol(instancePath);
            var code = this.AdsClient.TryWrite(symbol.IndexGroup, symbol.IndexOffset, new[] { value });

            if (code != TwinCAT.Ads.AdsErrorCode.NoError)
            {
                throw new BeckhoffException(
                    code: ADS_WRITE_ERROR,
                    message: $"Failed to write byte value '{value}' to PLC at address {instancePath}",
                    details: $"ADS Error Code: {code} (0x{(uint)code:X8}) - {GetAdsErrorMessage(code)}"
                );
            }
        }

        public void Write(string instancePath, bool value)
        {
            var symbol = GetSymbol(instancePath);
            var code = this.AdsClient.TryWrite(symbol.IndexGroup, symbol.IndexOffset, BitConverter.GetBytes(value));

            if (code != TwinCAT.Ads.AdsErrorCode.NoError)
            {
                throw new BeckhoffException(
                    code: ADS_WRITE_ERROR,
                    message: $"Failed to write bool value '{value}' to PLC at address {instancePath}",
                    details: $"ADS Error Code: {code} (0x{(uint)code:X8}) - {GetAdsErrorMessage(code)}"
                );
            }
        }

        public void Write(string instancePath, short value)
        {
            var symbol = GetSymbol(instancePath);
            var code = this.AdsClient.TryWrite(symbol.IndexGroup, symbol.IndexOffset, BitConverter.GetBytes(value));

            if (code != TwinCAT.Ads.AdsErrorCode.NoError)
            {
                throw new BeckhoffException(
                    code: ADS_WRITE_ERROR,
                    message: $"Failed to write short value '{value}' to PLC at address {instancePath}",
                    details: $"ADS Error Code: {code} (0x{(uint)code:X8}) - {GetAdsErrorMessage(code)}"
                );
            }
        }

        public void Write(string instancePath, ushort value)
        {
            var symbol = GetSymbol(instancePath);
            var code = this.AdsClient.TryWrite(symbol.IndexGroup, symbol.IndexOffset, BitConverter.GetBytes(value));

            if (code != TwinCAT.Ads.AdsErrorCode.NoError)
            {
                throw new BeckhoffException(
                    code: ADS_WRITE_ERROR,
                    message: $"Failed to write ushort value '{value}' to PLC at address {instancePath}",
                    details: $"ADS Error Code: {code} (0x{(uint)code:X8}) - {GetAdsErrorMessage(code)}"
                );
            }
        }

        public void Write(string instancePath, int value)
        {
            var symbol = GetSymbol(instancePath);
            var code = this.AdsClient.TryWrite(symbol.IndexGroup, symbol.IndexOffset, BitConverter.GetBytes(value));

            if (code != TwinCAT.Ads.AdsErrorCode.NoError)
            {
                throw new BeckhoffException(
                    code: ADS_WRITE_ERROR,
                    message: $"Failed to write int value '{value}' to PLC at address {instancePath}",
                    details: $"ADS Error Code: {code} (0x{(uint)code:X8}) - {GetAdsErrorMessage(code)}"
                );
            }
        }

        public void Write(string instancePath, uint value)
        {
            var symbol = GetSymbol(instancePath);
            var code = this.AdsClient.TryWrite(symbol.IndexGroup, symbol.IndexOffset, BitConverter.GetBytes(value));

            if (code != TwinCAT.Ads.AdsErrorCode.NoError)
            {
                throw new BeckhoffException(
                    code: ADS_WRITE_ERROR,
                    message: $"Failed to write uint value '{value}' to PLC at address {instancePath}",
                    details: $"ADS Error Code: {code} (0x{(uint)code:X8}) - {GetAdsErrorMessage(code)}"
                );
            }
        }

        public void Write(string instancePath, long value)
        {
            var symbol = GetSymbol(instancePath);
            var code = this.AdsClient.TryWrite(symbol.IndexGroup, symbol.IndexOffset, BitConverter.GetBytes(value));

            if (code != TwinCAT.Ads.AdsErrorCode.NoError)
            {
                throw new BeckhoffException(
                    code: ADS_WRITE_ERROR,
                    message: $"Failed to write long value '{value}' to PLC at address {instancePath}",
                    details: $"ADS Error Code: {code} (0x{(uint)code:X8}) - {GetAdsErrorMessage(code)}"
                );
            }
        }

        public void Write(string instancePath, ulong value)
        {
            var symbol = GetSymbol(instancePath);
            var code = this.AdsClient.TryWrite(symbol.IndexGroup, symbol.IndexOffset, BitConverter.GetBytes(value));

            if (code != TwinCAT.Ads.AdsErrorCode.NoError)
            {
                throw new BeckhoffException(
                    code: ADS_WRITE_ERROR,
                    message: $"Failed to write ulong value '{value}' to PLC at indices {instancePath}",
                    details: $"ADS Error Code: {code} (0x{(uint)code:X8}) - {GetAdsErrorMessage(code)}"
                );
            }
        }

        public void Write(string instancePath, float value)
        {
            var symbol = GetSymbol(instancePath);
            var code = this.AdsClient.TryWrite(symbol.IndexGroup, symbol.IndexOffset, BitConverter.GetBytes(value));

            if (code != TwinCAT.Ads.AdsErrorCode.NoError)
            {
                throw new BeckhoffException(
                    code: ADS_WRITE_ERROR,
                    message: $"Failed to write float value '{value}' to PLC at address {instancePath}",
                    details: $"ADS Error Code: {code} (0x{(uint)code:X8}) - {GetAdsErrorMessage(code)}"
                );
            }
        }

        public void Write(string instancePath, double value)
        {
            var symbol = GetSymbol(instancePath);
            var code = this.AdsClient.TryWrite(symbol.IndexGroup, symbol.IndexOffset, BitConverter.GetBytes(value));

            if (code != TwinCAT.Ads.AdsErrorCode.NoError)
            {
                throw new BeckhoffException(
                    code: ADS_WRITE_ERROR,
                    message: $"Failed to write double value '{value}' to PLC at indices {instancePath}",
                    details: $"ADS Error Code: {code} (0x{(uint)code:X8}) - {GetAdsErrorMessage(code)}"
                );
            }
        }

        public void Write(string instancePath, string value)
        {
            var symbol = GetSymbol(instancePath);

            var bytes = EncodePlcString(value, symbol.Size);
            var code = this.AdsClient.TryWrite(symbol.IndexGroup, symbol.IndexOffset, bytes);

            if (code != TwinCAT.Ads.AdsErrorCode.NoError)
            {
                throw new BeckhoffException(
                    code: ADS_WRITE_ERROR,
                    message: $"Failed to write string value '{value}' to PLC at indices {instancePath}",
                    details: $"ADS Error Code: {code} (0x{(uint)code:X8}) - {GetAdsErrorMessage(code)}. String length: {value.Length} chars, {bytes.Length} bytes"
                );
            }
        }

        public string ReadString(string instancePath)
        {
            var symbol = GetSymbol(instancePath);
            var result = this.ReadBytes(symbol.IndexGroup, symbol.IndexOffset, symbol.Size);

            if (result.ErrorCode != TwinCAT.Ads.AdsErrorCode.NoError)
            {
                throw new BeckhoffException(
                    code: ADS_READ_ERROR,
                    message: $"Failed to read string from PLC at address {instancePath}",
                    details: $"ADS Error Code: {result.ErrorCode} (0x{(uint)result.ErrorCode:X8}) - {GetAdsErrorMessage(result.ErrorCode)}. IndexGroup: 0x{symbol.IndexGroup:X8}, IndexOffset: 0x{symbol.IndexOffset:X8}, Requested size: {symbol.Size} bytes"
                );
            }

            try
            {
                return ByteArrayConverter<string>.Convert(result.Data, this.Encoding);
            }
            catch (Exception ex) when (ex is DecoderFallbackException || ex is ArgumentException)
            {
                throw new BeckhoffException(
                    code: ADS_STRING_DECODING_ERROR,
                    message: $"Failed to decode string from PLC at address {instancePath}",
                    details: $"Symbol size: {symbol.Size}, actual bytes read: {result.Data.Length}, inner exception: {ex.Message}"
                );
            }
        }

        public void Write(string instancePath, BeckhoffData.Date value)
        {
            this.Write(instancePath, value.Value);
        }

        public void Write(string instancePath, BeckhoffData.DateTime value)
        {
            this.Write(instancePath, value.Value);
        }

        public void Write(string instancePath, BeckhoffData.DateTimeOfDay value)
        {
            this.Write(instancePath, value.Value);
        }

        public void Write(string instancePath, BeckhoffData.LongDate value)
        {
            this.Write(instancePath, value.Value);
        }

        public void Write(string instancePath, BeckhoffData.LongDateTime value)
        {
            this.Write(instancePath, value.Value);
        }

        public void Write(string instancePath, BeckhoffData.LongDateTimeOfDay value)
        {
            this.Write(instancePath, value.Value);
        }

        public void Write(string instancePath, BeckhoffData.LongTime value)
        {
            this.Write(instancePath, value.Value);
        }

        public void Write(string instancePath, BeckhoffData.Time value)
        {
            this.Write(instancePath, value.Value);
        }

        public TValue Read<TValue>(string instancePath)
        {
            var symbol = GetSymbol(instancePath);
            var size = SizeOf<TValue>.Value;

            if (size != symbol.Size)
            {
                throw new BeckhoffException(
                    code: ADS_DATA_SIZE_MISMATCH,
                    message: $"Size mismatch when reading {typeof(TValue).Name} from PLC at address {instancePath}",
                    details: $"Expected size: {size} bytes, Symbol size: {symbol.Size} bytes. " +
                             "This may indicate a type mismatch between the requested type and the PLC symbol type."
                );
            }

            var result = this.ReadBytes(symbol.IndexGroup, symbol.IndexOffset, symbol.Size);

            if (result.ErrorCode != TwinCAT.Ads.AdsErrorCode.NoError)
            {
                throw new BeckhoffException(
                    code: ADS_READ_ERROR,
                    message: $"Failed to read {typeof(TValue).Name} from PLC at address {instancePath}",
                    details: $"ADS Error Code: {result.ErrorCode} (0x{(uint)result.ErrorCode:X8}) - {GetAdsErrorMessage(result.ErrorCode)}. IndexGroup: 0x{symbol.IndexGroup:X8}, IndexOffset: 0x{symbol.IndexOffset:X8}, Requested size: {size} bytes"
                );
            }

            try
            {
                return ByteArrayConverter<TValue>.Convert(result.Data, this.Encoding);
            }
            catch (Exception ex) when (ex is InvalidCastException || ex is ArgumentException || ex is FormatException)
            {
                throw new BeckhoffException(
                    code: ADS_DATA_CONVERSION_ERROR,
                    message: $"Failed to convert byte array to {typeof(TValue).Name} for address {instancePath}",
                    details: $"Byte array length: {result.Data.Length}, expected size: {size}, exception: {ex.Message}"
                );
            }
        }
    }
}
