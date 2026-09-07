using PlcGateway.Drivers.Beckhoff.Exceptions;
using System;
using System.Text;
using TwinCAT.Ads;
using static PlcGateway.Drivers.Beckhoff.AdsErrorCode;

namespace PlcGateway.Drivers.Beckhoff
{
    internal abstract class BeckhoffDriverImplBase : IDisposable
    {
        public AmsNetId AmsNetId { get; }

        public AmsPort Port { get; }

        protected readonly TcAdsClient AdsClient = new TcAdsClient();

        public Encoding Encoding { get; }

        public bool IsConnected => AdsClient.IsConnected;

        public BeckhoffDriverImplBase(AmsNetId amsNetId, AmsPort port, Encoding defaultEncoding)
        {
            AmsNetId = amsNetId ?? throw new BeckhoffException(
                code: ADS_INVALID_AMS_NET_ID,
                message: "AMS Net ID cannot be null",
                details: "Provide a valid AMS Net ID for PLC connection"
            );

            Port = port;
            Encoding = defaultEncoding ?? throw new BeckhoffException(
                code: ADS_INVALID_ENCODING,
                message: "Encoding cannot be null",
                details: "Provide a valid encoding for string operations"
            );
        }

        public BeckhoffDriverImplBase(AmsNetId amsNetId, AmsPort port)
            : this(amsNetId, port, Encoding.UTF8)
        {
        }

        public BeckhoffDriverImplBase(string amsNetId, int port, Encoding defaultEncoding)
            : this(ParseAmsNetId(amsNetId), ParseAmsPort(port), defaultEncoding)
        {
        }

        public BeckhoffDriverImplBase(string amsNetId, int port)
            : this(amsNetId, port, Encoding.UTF8)
        {
        }

        virtual public void Connect()
        {
            if (IsConnected)
            {
                return;
            }

            try
            {
                AdsClient.Connect(AmsNetId, Port);
            }
            catch (AdsErrorException ex)
            {
                var errorMessage = GetAdsErrorMessage(ex.ErrorCode);
                throw new BeckhoffException(
                    code: ADS_CONNECTION_ERROR,
                    message: $"Failed to connect to PLC at {AmsNetId}:{Port}",
                    details: $"ADS Error Code: {ex.ErrorCode} (0x{(uint)ex.ErrorCode:X8}) - {errorMessage}",
                    innerException: ex
                );
            }
            catch (ArgumentNullException ex)
            {
                throw new BeckhoffException(
                    code: ADS_INVALID_CONNECTION_PARAMETERS,
                    message: "Invalid connection parameters provided",
                    details: $"AMS Net ID: {AmsNetId}, Port: {Port}. Check that AMS Net ID is not null or empty.",
                    innerException: ex
                );
            }
            catch (InvalidOperationException ex)
            {
                throw new BeckhoffException(
                    code: ADS_INVALID_CLIENT_STATE,
                    message: "ADS client is in an invalid state for connection",
                    details: "The ADS client may already be connected or disposed.",
                    innerException: ex
                );
            }
            catch (Exception ex)
            {
                throw new BeckhoffException(
                    code: ADS_CONNECTION_FAILED,
                    message: $"Failed to establish connection to PLC at {AmsNetId}:{Port}",
                    details: $"Unexpected error occurred. AMS Net ID: {AmsNetId}, Port: {Port}, Exception: {ex.GetType().Name} - {ex.Message}",
                    innerException: ex
                );
            }

            if (!AdsClient.IsConnected)
            {
                throw new BeckhoffException(
                    code: ADS_CONNECTION_VERIFICATION_FAILED,
                    message: "Connection verification failed - client reports not connected",
                    details: $"ADS client Connect() method returned without error but IsConnected is false. AMS Net ID: {AmsNetId}, Port: {Port}"
                );
            }
        }

        virtual public void Disconnect()
        {
            if (!IsConnected)
            {
                return;
            }

            try
            {
                AdsClient.Disconnect();
            }
            catch (AdsErrorException ex)
            {
                var errorMessage = GetAdsErrorMessage(ex.ErrorCode);
                throw new BeckhoffException(
                    code: ADS_DISCONNECT_ERROR,
                    message: $"Failed to disconnect from PLC at {AmsNetId}:{Port}",
                    details: $"ADS Error Code: {ex.ErrorCode} (0x{(uint)ex.ErrorCode:X8}) - {errorMessage}",
                    innerException: ex
                );
            }
            catch (InvalidOperationException ex)
            {
                throw new BeckhoffException(
                    code: ADS_INVALID_DISCONNECT_STATE,
                    message: "ADS client is in an invalid state for disconnection",
                    details: "The ADS client may already be disconnected or disposed.",
                    innerException: ex
                );
            }
            catch (Exception ex)
            {
                throw new BeckhoffException(
                    code: ADS_DISCONNECTION_FAILED,
                    message: $"Failed to disconnect from PLC at {AmsNetId}:{Port}",
                    details: $"Unexpected error occurred during disconnection. Exception: {ex.GetType().Name} - {ex.Message}",
                    innerException: ex
                );
            }

            if (AdsClient.IsConnected)
            {
                throw new BeckhoffException(
                    code: ADS_DISCONNECTION_VERIFICATION_FAILED,
                    message: "Disconnection verification failed - client still reports as connected",
                    details: $"ADS client Disconnect() method returned without error but IsConnected is still true. AMS Net ID: {AmsNetId}, Port: {Port}"
                );
            }
        }

        public void Dispose()
        {
            try
            {
                if (IsConnected)
                {
                    Disconnect();
                }

                AdsClient.Dispose();
            }
            catch
            {
                // Dispose must remain best-effort and should not throw.
            }
        }

        public void VerifyConnection()
        {
            if (!IsConnected)
            {
                throw new BeckhoffException(
                    code: ADS_NOT_CONNECTED,
                    message: "PLC connection is not established",
                    details: $"Call Connect() method first. AMS Net ID: {AmsNetId}, Port: {Port}"
                );
            }

            try
            {
                // Try to read a small piece of data to verify connection is alive
                var state = AdsClient.ReadState();
                if (state.AdsState == AdsState.Invalid)
                {
                    throw new BeckhoffException(
                        code: ADS_INVALID_STATE,
                        message: "PLC is in an invalid state",
                        details: $"PLC ADS State: {state.AdsState}, Device State: {state.DeviceState}"
                    );
                }
            }
            catch (AdsErrorException ex)
            {
                var errorMessage = GetAdsErrorMessage(ex.ErrorCode);
                throw new BeckhoffException(
                    code: ADS_CONNECTION_VERIFICATION_ERROR,
                    message: "Failed to verify PLC connection",
                    details: $"ADS Error Code: {ex.ErrorCode} (0x{(uint)ex.ErrorCode:X8}) - {errorMessage}",
                    innerException: ex
                );
            }
            catch (Exception ex)
            {
                throw new BeckhoffException(
                    code: ADS_CONNECTION_VERIFICATION_FAILED,
                    message: "PLC connection verification failed",
                    details: $"Exception while verifying connection: {ex.GetType().Name} - {ex.Message}",
                    innerException: ex
                );
            }
        }

        protected string GetAdsErrorMessage(TwinCAT.Ads.AdsErrorCode errorCode)
        {
            return errorCode.ToMessage();
        }

        protected AdsReadBufferResult ReadBytes(uint indexGroup, uint indexOffset, int length)
        {
            var data = new byte[length];
            var errorCode = AdsClient.TryRead(indexGroup, indexOffset, data, 0, length, out var bytesRead);

            if (errorCode == TwinCAT.Ads.AdsErrorCode.NoError && bytesRead != length)
            {
                errorCode = TwinCAT.Ads.AdsErrorCode.DeviceInvalidSize;
            }

            return new AdsReadBufferResult(errorCode, data, bytesRead);
        }

        protected byte[] EncodePlcString(string value, int bufferLength)
        {
            if (bufferLength <= 0)
            {
                throw new BeckhoffException(
                    code: ADS_INVALID_STRING_LENGTH,
                    message: "String buffer length must be greater than zero",
                    details: $"Provided buffer length: {bufferLength}"
                );
            }

            var sourceBytes = Encoding.GetBytes(value ?? string.Empty);
            var buffer = new byte[bufferLength];
            var copyLength = Math.Min(sourceBytes.Length, Math.Max(bufferLength - 1, 0));

            if (copyLength > 0)
            {
                Buffer.BlockCopy(sourceBytes, 0, buffer, 0, copyLength);
            }

            return buffer;
        }

        private static AmsNetId ParseAmsNetId(string amsNetId)
        {
            if (string.IsNullOrWhiteSpace(amsNetId))
            {
                throw new ArgumentException("AMS Net ID cannot be null or empty.", nameof(amsNetId));
            }

            return new AmsNetId(amsNetId.Trim());
        }

        private static AmsPort ParseAmsPort(int port)
        {
            if (port < 1 || port > ushort.MaxValue)
            {
                throw new ArgumentOutOfRangeException(nameof(port), port, "AMS port must be between 1 and 65535.");
            }

            return (AmsPort)port;
        }

        protected readonly struct AdsReadBufferResult
        {
            public TwinCAT.Ads.AdsErrorCode ErrorCode { get; }

            public byte[] Data { get; }

            public int BytesRead { get; }

            public AdsReadBufferResult(TwinCAT.Ads.AdsErrorCode errorCode, byte[] data, int bytesRead)
            {
                ErrorCode = errorCode;
                Data = data;
                BytesRead = bytesRead;
            }
        }
    }

    internal static class TcAdsClientCompatibilityExtensions
    {
        public static TwinCAT.Ads.AdsErrorCode TryWrite(
            this TcAdsClient client,
            uint indexGroup,
            uint indexOffset,
            byte[] data)
        {
            return client.TryWrite(indexGroup, indexOffset, data, 0, data.Length);
        }

    }
}
