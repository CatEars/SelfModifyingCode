namespace SelfModifyingCode.Host.CommandLine.Options;

public class PackOption : ICommandLineOption
{
    public string Description => "Verifies and packs a published application. Argument is the folder the application was published to.";
    public string LongFlag => "--pack";
    public string Name => "Pack Application";
    public bool IsMandatory => false;
    public CommandLineOptions Apply(CommandLineOptions current, string? argumentValue)
    {
        if (argumentValue == null)
        {
            throw new MissingArgumentException("--pack expects folder of the application to pack but none was found");
        }

        return current with { ExecutableMode = new ExecutableMode.PackApplication(argumentValue) };
    }
}