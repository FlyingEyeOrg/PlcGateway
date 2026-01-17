namespace PlcGateway.Core.Exceptions
{
    using System;
    using System.Runtime.Serialization;

    namespace YourNamespace.Exceptions
    {
        /// <summary>
        /// 用户友好异常（显示给最终用户）
        /// </summary>
        [Serializable]
        public class UserFriendlyException : BusinessException
        {
            public override bool IsUserFriendly => true;

            public UserFriendlyException()
            {
            }

            public UserFriendlyException(string message)
                : base(message)
            {
            }

            public UserFriendlyException(string message, Exception innerException)
                : base(message, innerException)
            {
            }

            public UserFriendlyException(string code, string message)
                : base(code, message)
            {
            }

            public UserFriendlyException(string code, string message, string details)
                : base(code, message, details)
            {
            }

            public UserFriendlyException(string code, string message, string details, Exception innerException)
                : base(code, message, details, innerException)
            {
            }

            protected UserFriendlyException(SerializationInfo serializationInfo, StreamingContext context)
                : base(serializationInfo, context)
            {
            }
        }
    }
}
