using SelfModifyingCode.Host.CommandLine;

namespace SelfModifyingCode.Host;

public class Entrypoint
{

    public static void Main(string[] args)
    {
        var commandLineParser = new CommandLineParser(args);
        try
        {
            var options = commandLineParser.ParseArgs();
            if (options.ExecutableMode is ExecutableMode.PrintHelp)
            {
                HelpPrinter.PrintHelp();
            }
        }
        catch (MissingArgumentException ex)
        {
            Console.WriteLine(ex.Message);
            Environment.Exit(1);
        }
    }
    
}