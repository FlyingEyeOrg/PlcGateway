using System;

namespace PlcGateway.Core.Exceptions
{
    public class UserFriendlyException : BusinessException, IUserFriendlyException
    {
        public UserFriendlyException(
            string message,
            string? code = null,
            string? details = null,
            Exception? innerException = null)
            : base(message, code, details, innerException)
        {
        }

        string IUserFriendlyException.Message => Message;

        string? IUserFriendlyException.Code => Code;

        string? IUserFriendlyException.Details => Details;
    }
}
