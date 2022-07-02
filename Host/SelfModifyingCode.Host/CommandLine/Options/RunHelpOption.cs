namespace SelfModifyingCode.Host.CommandLine.Options;

public class RunHelpOption : ICommandLineOption
{
    public static string CommandName => "Display Help";
    
    public string? ShortFlag => "-h";
    public string LongFlag => "--help";
    public string? Description => "Displays this help message";
    public string Name => CommandName;
    
    public bool IsMandatory => false;
    
    public CommandLineOptions Apply(CommandLineOptions current, string? _)
    {
        return current with { ExecutableMode = new ExecutableMode.PrintHelp() };
    }
}