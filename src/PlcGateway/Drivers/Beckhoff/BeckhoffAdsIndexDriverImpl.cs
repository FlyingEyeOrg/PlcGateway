using System;
using System.Text;
using PlcGateway.Core.Exceptions;
using PlcGateway.Drivers.Converter;
using TwinCAT.Ads;

using BeckhoffData = PlcGateway.Drivers.Beckhoff.Data;
using static PlcGateway.Drivers.Beckhoff.BeckhoffErrorCode;

namespace PlcGateway.Drivers.Beckhoff
{
    internal class BeckhoffAdsIndexDriverImpl : BeckhoffDriverImplBase
    {
        public BeckhoffAdsIndexDriverImpl(AmsNetId amsNetId, AmsPort port, Encoding encoding) : base(amsNetId, port, encoding)
        {
        }

        public BeckhoffAdsIndexDriverImpl(AmsNetId amsNetId, AmsPort port) : base(amsNetId, port)
        {
        }

        public void Write(AdsIndexAddress address, sbyte value)
        {
            var code = this.AdsClient.TryWrite(address.IndexGroup, address.IndexOffset, BitConverter.GetBytes(value));

            if (code != AdsErrorCode.NoError)
            {
                throw new BusinessException(
                    code: ADS_WRITE_ERROR,
                    message: $"Failed to write sbyte value '{value}' to PLC at address {address}",
                    details: $"ADS Error Code: {code} (0x{(uint)code:X8}) - {GetAdsErrorMessage(code)}"
                );
            }
        }

        public void Write(AdsIndexAddress address, byte value)
        {
            var code = this.AdsClient.TryWrite(address.IndexGroup, address.IndexOffset, BitConverter.GetBytes(value));

            if (code != AdsErrorCode.NoError)
            {
                throw new BusinessException(
                    code: ADS_WRITE_ERROR,
                    message: $"Failed to write byte value '{value}' to PLC at address {address}",
                    details: $"ADS Error Code: {code} (0x{(uint)code:X8}) - {GetAdsErrorMessage(code)}"
                );
            }
        }

        public void Write(AdsIndexAddress address, bool value)
        {
            var code = this.AdsClient.TryWrite(address.IndexGroup, address.IndexOffset, BitConverter.GetBytes(value));

            if (code != AdsErrorCode.NoError)
            {
                throw new BusinessException(
                    code: ADS_WRITE_ERROR,
                    message: $"Failed to write bool value '{value}' to PLC at address {address}",
                    details: $"ADS Error Code: {code} (0x{(uint)code:X8}) - {GetAdsErrorMessage(code)}"
                );
            }
        }

        public void Write(AdsIndexAddress address, short value)
        {
            var code = this.AdsClient.TryWrite(address.IndexGroup, address.IndexOffset, BitConverter.GetBytes(value));

            if (code != AdsErrorCode.NoError)
            {
                throw new BusinessException(
                    code: ADS_WRITE_ERROR,
                    message: $"Failed to write short value '{value}' to PLC at address {address}",
                    details: $"ADS Error Code: {code} (0x{(uint)code:X8}) - {GetAdsErrorMessage(code)}"
                );
            }
        }

        public void Write(AdsIndexAddress address, ushort value)
        {
            var code = this.AdsClient.TryWrite(address.IndexGroup, address.IndexOffset, BitConverter.GetBytes(value));

            if (code != AdsErrorCode.NoError)
            {
                throw new BusinessException(
                    code: ADS_WRITE_ERROR,
                    message: $"Failed to write ushort value '{value}' to PLC at address {address}",
                    details: $"ADS Error Code: {code} (0x{(uint)code:X8}) - {GetAdsErrorMessage(code)}"
                );
            }
        }

        public void Write(AdsIndexAddress address, int value)
        {
            var code = this.AdsClient.TryWrite(address.IndexGroup, address.IndexOffset, BitConverter.GetBytes(value));

            if (code != AdsErrorCode.NoError)
            {
                throw new BusinessException(
                    code: ADS_WRITE_ERROR,
                    message: $"Failed to write int value '{value}' to PLC at address {address}",
                    details: $"ADS Error Code: {code} (0x{(uint)code:X8}) - {GetAdsErrorMessage(code)}"
                );
            }
        }

        public void Write(AdsIndexAddress address, uint value)
        {
            var code = this.AdsClient.TryWrite(address.IndexGroup, address.IndexOffset, BitConverter.GetBytes(value));

            if (code != AdsErrorCode.NoError)
            {
                throw new BusinessException(
                    code: ADS_WRITE_ERROR,
                    message: $"Failed to write uint value '{value}' to PLC at address {address}",
                    details: $"ADS Error Code: {code} (0x{(uint)code:X8}) - {GetAdsErrorMessage(code)}"
                );
            }
        }

        public void Write(AdsIndexAddress address, long value)
        {
            var code = this.AdsClient.TryWrite(address.IndexGroup, address.IndexOffset, BitConverter.GetBytes(value));

            if (code != AdsErrorCode.NoError)
            {
                throw new BusinessException(
                    code: ADS_WRITE_ERROR,
                    message: $"Failed to write long value '{value}' to PLC at address {address}",
                    details: $"ADS Error Code: {code} (0x{(uint)code:X8}) - {GetAdsErrorMessage(code)}"
                );
            }
        }

        public void Write(AdsIndexAddress address, ulong value)
        {
            var code = this.AdsClient.TryWrite(address.IndexGroup, address.IndexOffset, BitConverter.GetBytes(value));

            if (code != AdsErrorCode.NoError)
            {
                throw new BusinessException(
                    code: ADS_WRITE_ERROR,
                    message: $"Failed to write ulong value '{value}' to PLC at address {address}",
                    details: $"ADS Error Code: {code} (0x{(uint)code:X8}) - {GetAdsErrorMessage(code)}"
                );
            }
        }

        public void Write(AdsIndexAddress address, float value)
        {
            var code = this.AdsClient.TryWrite(address.IndexGroup, address.IndexOffset, BitConverter.GetBytes(value));

            if (code != AdsErrorCode.NoError)
            {
                throw new BusinessException(
                    code: ADS_WRITE_ERROR,
                    message: $"Failed to write float value '{value}' to PLC at address {address}",
                    details: $"ADS Error Code: {code} (0x{(uint)code:X8}) - {GetAdsErrorMessage(code)}"
                );
            }
        }

        public void Write(AdsIndexAddress address, double value)
        {
            var code = this.AdsClient.TryWrite(address.IndexGroup, address.IndexOffset, BitConverter.GetBytes(value));

            if (code != AdsErrorCode.NoError)
            {
                throw new BusinessException(
                    code: ADS_WRITE_ERROR,
                    message: $"Failed to write double value '{value}' to PLC at address {address}",
                    details: $"ADS Error Code: {code} (0x{(uint)code:X8}) - {GetAdsErrorMessage(code)}"
                );
            }
        }

        public void Write(AdsIndexAddress address, string value)
        {
            byte[] bytes = this.Encoding.GetBytes(value);

            var code = this.AdsClient.TryWrite(address.IndexGroup, address.IndexOffset, bytes);

            if (code != AdsErrorCode.NoError)
            {
                throw new BusinessException(
                    code: ADS_WRITE_ERROR,
                    message: $"Failed to write string value '{value}' to PLC at address {address}",
                    details: $"ADS Error Code: {code} (0x{(uint)code:X8}) - {GetAdsErrorMessage(code)}. String length: {value.Length} chars, {bytes.Length} bytes"
                );
            }
        }

        public void Write(AdsIndexAddress address, BeckhoffData.Date value)
        {
            this.Write(address, value.Value);
        }

        public void Write(AdsIndexAddress address, BeckhoffData.DateTime value)
        {
            this.Write(address, value.Value);
        }

        public void Write(AdsIndexAddress address, BeckhoffData.DateTimeOfDay value)
        {
            this.Write(address, value.Value);
        }

        public void Write(AdsIndexAddress address, BeckhoffData.LongDate value)
        {
            this.Write(address, value.Value);
        }

        public void Write(AdsIndexAddress address, BeckhoffData.LongDateTime value)
        {
            this.Write(address, value.Value);
        }

        public void Write(AdsIndexAddress address, BeckhoffData.LongDateTimeOfDay value)
        {
            this.Write(address, value.Value);
        }

        public void Write(AdsIndexAddress address, BeckhoffData.LongTime value)
        {
            this.Write(address, value.Value);
        }

        public void Write(AdsIndexAddress address, BeckhoffData.Time value)
        {
            this.Write(address, value.Value);
        }

        public TValue Read<TValue>(AdsIndexAddress address)
        {
            var size = SizeOf<TValue>.Value;
            var result = this.AdsClient.ReadAsResult(address.IndexGroup, address.IndexOffset, size);

            if (result.ErrorCode != AdsErrorCode.NoError)
            {
                throw new BusinessException(
                    code: ADS_READ_ERROR,
                    message: $"Failed to read {typeof(TValue).Name} from PLC at address {address}",
                    details: $"ADS Error Code: {result.ErrorCode} (0x{(uint)result.ErrorCode:X8}) - {GetAdsErrorMessage(result.ErrorCode)}. IndexGroup: 0x{address.IndexGroup:X8}, IndexOffset: 0x{address.IndexOffset:X8}, Requested size: {size} bytes"
                );
            }

            try
            {
                return ByteArrayConverter<TValue>.Convert(result.Data.ToArray(), this.Encoding);
            }
            catch (Exception ex) when (ex is InvalidCastException || ex is ArgumentException || ex is FormatException)
            {
                throw new BusinessException(
                    code: ADS_DATA_CONVERSION_ERROR,
                    message: $"Failed to convert byte array to {typeof(TValue).Name} for address {address}",
                    details: $"Byte array length: {result.Data.Length}, expected size: {size}, exception: {ex.Message}"
                );
            }
        }

        public string Read(StringAdsIndexAddress address)
        {
            var result = this.AdsClient.ReadAsResult(address.IndexGroup, address.IndexOffset, address.DataLength);

            if (result.ErrorCode != AdsErrorCode.NoError)
            {
                throw new BusinessException(
                    code: ADS_READ_ERROR,
                    message: $"Failed to read string from PLC at address {address}",
                    details: $"ADS Error Code: {result.ErrorCode} (0x{(uint)result.ErrorCode:X8}) - {GetAdsErrorMessage(result.ErrorCode)}. IndexGroup: 0x{address.IndexGroup:X8}, IndexOffset: 0x{address.IndexOffset:X8}, Requested length: {address.DataLength} bytes"
                );
            }

            try
            {
                var bytes = result.Data.ToArray();
                string resultString = this.Encoding.GetString(bytes);

                // Trim null terminators
                int nullIndex = resultString.IndexOf('\0');
                if (nullIndex >= 0)
                {
                    resultString = resultString.Substring(0, nullIndex);
                }

                return resultString;
            }
            catch (Exception ex) when (ex is DecoderFallbackException || ex is ArgumentException)
            {
                throw new BusinessException(
                    code: ADS_STRING_DECODING_ERROR,
                    message: $"Failed to decode string from byte array using {this.Encoding.EncodingName} encoding",
                    details: $"Address: {address}, requested length: {address.DataLength}, actual bytes read: {result.Data.Length}, inner exception: {ex.Message}"
                );
            }
        }
    }
}