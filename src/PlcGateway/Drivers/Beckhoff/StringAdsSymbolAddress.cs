using PlcGateway.Core.Exceptions;

namespace PlcGateway.Drivers.Beckhoff
{
    public readonly struct StringAdsSymbolAddress
    {
        /// <summary>
        /// The name of the string to be read
        /// </summary>
        public string SymbolName { get; }

        /// <summary>
        /// The length of the string to be read
        /// </summary>
        public int DataLength { get; }

        public StringAdsSymbolAddress(string symbolName, int dataLength)
        {
            if (string.IsNullOrEmpty(symbolName))
            {
                throw new BusinessException(
                    code: "INVALID_SYMBOL_NAME",
                    message: "String name cannot be null or empty",
                    details: $"Parameter 'symbolName' is null or empty. A valid string identifier is required."
                );
            }

            if (dataLength <= 0)
            {
                throw new BusinessException(
                    code: "INVALID_STRING_LENGTH",
                    message: $"String length must be greater than zero. Current value: {dataLength}",
                    details: $"String '{symbolName}' specified an invalid length. Length must be a positive integer, but received: {dataLength}"
                );
            }

            // Optional: Add maximum length constraint if needed
            const int MAX_ALLOWED_LENGTH = 1024 * 1024; // Example: 1MB
            if (dataLength > MAX_ALLOWED_LENGTH)
            {
                throw new BusinessException(
                    code: "STRING_LENGTH_EXCEEDS_MAX",
                    message: $"String length exceeds maximum allowed limit of {MAX_ALLOWED_LENGTH}",
                    details: $"String '{symbolName}' length {dataLength} exceeds the maximum allowed value of {MAX_ALLOWED_LENGTH}"
                );
            }

            SymbolName = symbolName;
            DataLength = dataLength;
        }
    }
}
