namespace SelfModifyingCode.Host.CommandLine.Options;

public class ProgramLocationOption : ICommandLineOption
{
    public string? ShortFlag => "-p";
    public string LongFlag => "--program";
    public string? Description => "Location of SMC program";
    public string? SampleUsage => "--program path/to/program.smc";
    public string Name => "Program";
    public bool IsMandatory => true;
    public CommandLineOptions Apply(CommandLineOptions current, string? argumentValue)
    {
        if (argumentValue == null)
        {
            throw new MissingArgumentException("No `--program` specifying program location");
        }

        return current with { ProgramPath = argumentValue };
    }
}