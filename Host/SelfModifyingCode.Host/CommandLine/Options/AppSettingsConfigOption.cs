namespace SelfModifyingCode.Host.CommandLine.Options;

public class AppSettingsConfigOption : ICommandLineOption
{
    public string? ShortFlag => "-s";
    public string LongFlag => "--settings-file";
    public string? Description => "Location of settings file to use. Will be copied next to .exe file on startup";
    public string Name => "App Settings";
    public bool IsMandatory => false;
    public CommandLineOptions Apply(CommandLineOptions current, string? argumentValue)
    {
        if (argumentValue == null)
        {
            throw new MissingArgumentException("--settings-file specified, but with no arguments. Expected config file.");
        }

        return current with { ConfigVariant = new ConfigVariant.CopyLocalAppSettings(argumentValue)};
    }
}