using System.Collections.Generic;
using System.Text;

namespace PlcGateway.Core.Exceptions
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    namespace YourNamespace.Exceptions
    {
        /// <summary>
        /// ABP 业务异常基类
        /// </summary>
        [Serializable]
        public class BusinessException : Exception
        {
            /// <summary>
            /// 错误代码
            /// </summary>
            public string? Code { get; set; }

            /// <summary>
            /// 错误详情
            /// </summary>
            public string? Details { get; set; }

            /// <summary>
            /// 本地化键
            /// </summary>
            public string? LocalizationKey { get; set; }

            /// <summary>
            /// 本地化参数
            /// </summary>
            public object[]? LocalizationParameters { get; set; }

            /// <summary>
            /// 错误数据
            /// </summary>
            public IDictionary<string, object> DataDictionary { get; } = new Dictionary<string, object>();

            /// <summary>
            /// 是否是用户友好异常
            /// </summary>
            public virtual bool IsUserFriendly => false;

            /// <summary>
            /// 日志级别
            /// </summary>
            public virtual LogLevel LogLevel { get; set; } = LogLevel.Error;

            public BusinessException()
            {
            }

            public BusinessException(string message)
                : base(message)
            {
            }

            public BusinessException(string message, Exception innerException)
                : base(message, innerException)
            {
            }

            public BusinessException(string code, string message)
                : base(message)
            {
                Code = code;
            }

            public BusinessException(string code, string message, string details)
                : base(message)
            {
                Code = code;
                Details = details;
            }

            public BusinessException(string code, string message, string details, Exception innerException)
                : base(message, innerException)
            {
                Code = code;
                Details = details;
            }

            public BusinessException(string code, string message, string details, string localizationKey, params object[] localizationParameters)
                : base(message)
            {
                Code = code;
                Details = details;
                LocalizationKey = localizationKey;
                LocalizationParameters = localizationParameters;
            }

            protected BusinessException(SerializationInfo serializationInfo, StreamingContext context)
                : base(serializationInfo, context)
            {
            }

            /// <summary>
            /// 设置本地化信息
            /// </summary>
            public BusinessException WithLocalization(string key, params object[] parameters)
            {
                LocalizationKey = key;
                LocalizationParameters = parameters;
                return this;
            }

            /// <summary>
            /// 设置错误代码
            /// </summary>
            public BusinessException WithCode(string code)
            {
                Code = code;
                return this;
            }

            /// <summary>
            /// 设置错误详情
            /// </summary>
            public BusinessException WithDetails(string details)
            {
                Details = details;
                return this;
            }

            /// <summary>
            /// 添加额外数据
            /// </summary>
            public BusinessException WithData(string key, object value)
            {
                DataDictionary[key] = value;
                return this;
            }

            /// <summary>
            /// 设置日志级别
            /// </summary>
            public BusinessException WithLogLevel(LogLevel logLevel)
            {
                LogLevel = logLevel;
                return this;
            }
        }

        /// <summary>
        /// 日志级别
        /// </summary>
        public enum LogLevel
        {
            Trace,
            Debug,
            Information,
            Warning,
            Error,
            Critical,
            None
        }
    }
}
