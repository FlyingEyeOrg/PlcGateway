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

        protected readonly AdsClient AdsClient = new AdsClient();

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

            // Validate encoding
            if (Encoding == null)
            {
                throw new BeckhoffException(
                    code: ADS_INVALID_ENCODING,
                    message: "Default encoding is null",
                    details: "Encoding must be properly initialized"
                );
            }
        }

        public BeckhoffDriverImplBase(AmsNetId amsNetId, AmsPort port)
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
                if (AdsClient != null)
                {
                    if (IsConnected)
                    {
                        Disconnect();
                    }
                    AdsClient.Dispose();
                }
            }
            catch (Exception ex)
            {
                // Log but don't throw in Dispose
                // Consider logging this exception
                throw new BeckhoffException(
                    code: ADS_DISPOSE_ERROR,
                    message: "Error occurred while disposing ADS client",
                    details: $"Exception during disposal: {ex.GetType().Name} - {ex.Message}",
                    innerException: ex
                );
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
    }
}