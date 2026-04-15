using System;
using PlcGateway.Core.Exceptions;

namespace PlcGateway.Drivers.Beckhoff.Exceptions
{
    public class BeckhoffException : BusinessException
    {
        public BeckhoffException(
            string message,
            string? code = null,
            string? details = null,
            Exception? innerException = null)
            : base(message, code, details, innerException)
        {
        }

        public BeckhoffException(string code, string message)
            : base(message, code)
        {
        }

        public BeckhoffException(string code, string message, string details)
            : base(message, code, details)
        {
        }

    }
}