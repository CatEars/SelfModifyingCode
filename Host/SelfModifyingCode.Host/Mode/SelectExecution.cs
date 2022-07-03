using SelfModifyingCode.Host.CommandLine;

namespace SelfModifyingCode.Host.Mode;

public static class SelectExecution
{
    public static IExecution Get(CommandLineOptions options)
    {
        return options.ExecutableMode switch
        {
            ExecutableMode.PrintHelp => new AsHelpPrinter(),
            ExecutableMode.RunExe => new AsApplication(options),
            _ => throw new ArgumentException($"Options of type {options.ExecutableMode.GetType().Name} is not a valid execution mode")
        };
    }
}