using SelfModifyingCode.Host.CommandLine.Options;

namespace SelfModifyingCode.Host.Mode;

public class AsHelpPrinter : IExecution
{
    
    public static string GetFlagDescription(ICommandLineOption option)
    {
        return (option.ShortFlag == null ? "" : option.ShortFlag + "|") + option.LongFlag;
    }
    
    public void Run()
    {
        Console.WriteLine("SelfModifyingCode.Host - Host program for an SMC self-updating process");
        Console.WriteLine();
        Console.WriteLine("Arguments:");
        var maxFlagLength = OptionsRegistry.AllOptions.Max(x => GetFlagDescription(x).Length);
        var options = OptionsRegistry.AllOptions
            .OrderBy(x => x.Name);
        
        foreach (var option in options)
        {
            var flag = GetFlagDescription(option);
            var name = option.Name;
            var message = "  " + flag.PadRight(maxFlagLength) + " - " + name;
            Console.WriteLine(message);
        }

        Console.WriteLine();
        Console.WriteLine("Usage: ./SelfModifyingCode.Host.exe -p ./my-program.smc -e ./tmp");
        Console.WriteLine();
        Console.WriteLine("To inspect a specific argument, write a pattern matching the name:");
        Console.WriteLine("   ./SelfModifyingCode.Host.exe --help prog");
        Console.WriteLine("Will print description of option 'Program'");
    }
}