using PlcGateway.Core.Exceptions;
using System;
using System.Runtime.Serialization;

namespace PlcGateway.Drivers.Inovance.Exceptions
{
    /// <summary>
    /// 汇川PLC驱动异常
    /// </summary>
    [Serializable]
    public class InovanceException : BusinessException
    {
        public InovanceException()
        {
        }

        public InovanceException(string message)
            : base(message)
        {
        }

        public InovanceException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public InovanceException(string code, string message)
            : base(code, message)
        {
        }

        public InovanceException(string code, string message, string details)
            : base(code, message, details)
        {
        }

        public InovanceException(string code, string message, string details, Exception innerException)
            : base(code, message, details, innerException)
        {
        }

        public InovanceException(string code, string message, string details, string localizationKey, params object[] localizationParameters)
            : base(code, message, details, localizationKey, localizationParameters)
        {
        }

        protected InovanceException(SerializationInfo serializationInfo, StreamingContext context)
            : base(serializationInfo, context)
        {
        }
    }
}