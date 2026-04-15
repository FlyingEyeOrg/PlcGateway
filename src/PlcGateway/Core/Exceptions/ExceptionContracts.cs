namespace PlcGateway.Core.Exceptions
{
    public interface IHasErrorCode
    {
        string? Code { get; }
    }

    public interface IHasErrorDetails
    {
        string? Details { get; }
    }
}