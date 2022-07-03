using SelfModifyingCode.Host.CommandLine.Options;

namespace SelfModifyingCode.Host.Mode;

public class AsHelpPrinter : IExecution
{
    
    private static string GetFlagDescription(ICommandLineOption option)
    {
        return (option.ShortFlag == null ? "" : option.ShortFlag + "|") + option.LongFlag;
    }
    
    public void Run()
    {
        Console.WriteLine("SelfModifyingCode.Host - Host program for an SMC self-updating process");
        Console.WriteLine();
        Console.WriteLine("Arguments:");
        var maxFlagLength = OptionsRegistry.AllOptions.Max(x => GetFlagDescription(x).Length);
        var maxNameLength = OptionsRegistry.AllOptions.Max(x => x.Name.Length);
        var options = OptionsRegistry.AllOptions
            .OrderBy(x => x.Name);
        
        foreach (var option in options)
        {
            var flag = GetFlagDescription(option);
            var name = option.Name;
            var description = option.Description ?? "";
            var message = "  " + flag.PadRight(maxFlagLength) + " - " 
                          + name.PadRight(maxNameLength + 5) + description;
            Console.WriteLine(message);
        }

        Console.WriteLine();
        Console.WriteLine("Usage: ./SelfModifyingCode.Host.exe -p ./my-program.smc -e ./tmp");
        Console.WriteLine();
    }
}