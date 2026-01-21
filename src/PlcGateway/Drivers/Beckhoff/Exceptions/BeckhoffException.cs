using PlcGateway.Core.Exceptions;
using System;
using System.Runtime.Serialization;

namespace PlcGateway.Drivers.Beckhoff.Exceptions
{
    /// <summary>
    /// 汇川PLC驱动异常
    /// </summary>
    [Serializable]
    public class BeckhoffException : BusinessException
    {
        public BeckhoffException()
        {
        }

        public BeckhoffException(string message)
            : base(message)
        {
        }

        public BeckhoffException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public BeckhoffException(string code, string message)
            : base(code, message)
        {
        }

        public BeckhoffException(string code, string message, string details)
            : base(code, message, details)
        {
        }

        public BeckhoffException(string code, string message, string details, Exception innerException)
            : base(code, message, details, innerException)
        {
        }

        public BeckhoffException(string code, string message, string details, string localizationKey, params object[] localizationParameters)
            : base(code, message, details, localizationKey, localizationParameters)
        {
        }

        protected BeckhoffException(SerializationInfo serializationInfo, StreamingContext context)
            : base(serializationInfo, context)
        {
        }
    }
}