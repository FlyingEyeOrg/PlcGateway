using System;

namespace PlcGateway.Core.Exceptions
{
    public class BusinessException : Exception, IHasErrorCode, IHasErrorDetails
    {
        public BusinessException(
            string message,
            string? code = null,
            string? details = null,
            Exception? innerException = null)
            : base(message, innerException)
        {
            Code = code;
            Details = details;
        }

        public string? Code { get; }

        public string? Details { get; }

        public BusinessException WithData(string name, object? value)
        {
            Data[name] = value;
            return this;
        }
    }
}
