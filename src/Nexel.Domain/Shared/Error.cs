namespace Nexel.Domain.Shared;

public class Error
{
    public static readonly Error None = new(string.Empty);
    public static readonly Error DefaultError = new("ERROR");

    public Error(string message)
    {
        Message = message;
    }

    public string Message { get; }
}