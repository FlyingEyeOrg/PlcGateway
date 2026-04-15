using System;
using PlcGateway.Core.Exceptions;

namespace PlcGateway.Drivers.Inovance.Exceptions
{
    public class InovanceException : BusinessException
    {
        public InovanceException(
            string message,
            string? code = null,
            string? details = null,
            Exception? innerException = null)
            : base(message, code, details, innerException)
        {
        }

        public InovanceException(string code, string message)
            : base(message, code)
        {
        }

        public InovanceException(string code, string message, string details)
            : base(message, code, details)
        {
        }

    }
}