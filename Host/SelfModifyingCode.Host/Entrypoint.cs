using SelfModifyingCode.Host.CommandLine;
using SelfModifyingCode.Host.Mode;

namespace SelfModifyingCode.Host;

public class Entrypoint
{

    public static void Main(string[] args)
    {
        var commandLineParser = new CommandLineParser(args);
        try
        {
            var options = commandLineParser.ParseArgs();
            var execution = SelectExecution.Get(options);
            execution.Run();
        }
        catch (MissingArgumentException ex)
        {
            Console.WriteLine(ex.Message);
            Environment.Exit(1);
        }
    }
    
}