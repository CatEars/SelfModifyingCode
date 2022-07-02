namespace SelfModifyingCode.Host.CommandLine;

public class MissingArgumentException : Exception
{
    public MissingArgumentException(string? message) : base(message)
    {
    }
}