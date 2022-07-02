namespace SelfModifyingCode.Host.CommandLine.Options;

public class ExecutingDirectoryOption : ICommandLineOption
{
    public string? ShortFlag => "-e";
    public string LongFlag => "--executing-directory";
    public string? Description => "Directory where program is packed up and executed";
    public string Name => "Executing Directory";
    public bool IsMandatory => false;
    public CommandLineOptions Apply(CommandLineOptions current, string? argumentValue)
    {
        if (argumentValue == null)
        {
            throw new MissingArgumentException("--execution-directory not specifying directory");
        }

        return current with { ExecutingDirectory = argumentValue };
    }
}