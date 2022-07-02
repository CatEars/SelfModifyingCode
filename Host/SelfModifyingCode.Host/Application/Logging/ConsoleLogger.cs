namespace SelfModifyingCode.Host.Application.Logging;

public class ConsoleLogger : ILogger
{
    public void Info(string message)
    {
        Console.WriteLine(message);
    }
}