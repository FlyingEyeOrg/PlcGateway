namespace PlcGateway.Core.Exceptions
{
    public interface IUserFriendlyException
    {
        string Message { get; }

        string? Code { get; }

        string? Details { get; }
    }
}