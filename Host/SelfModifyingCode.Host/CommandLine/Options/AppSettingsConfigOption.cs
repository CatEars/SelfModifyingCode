namespace SelfModifyingCode.Host.CommandLine.Options;

public class AppSettingsConfigOption : ICommandLineOption
{
    public string? ShortFlag => "-s";
    public string LongFlag => "--settings-file";
    public string? Description => "Location of settings file to use. Will be copied next to .exe file on startup. " +
                                  "Can be used multiple times to copy multiple files.";
    public string Name => "App Settings";
    public bool IsMandatory => false;
    
    public CommandLineOptions Apply(CommandLineOptions current, string? argumentValue)
    {
        if (argumentValue == null)
        {
            throw new MissingArgumentException("--settings-file specified, but with no arguments. Expected config file.");
        }

        if (current.ConfigVariant is ConfigVariant.CopyLocalAppSettings currentAppSettings)
        {
            return current with
            {
                ConfigVariant = currentAppSettings.With(argumentValue)
            };
        }

        var empty = ConfigVariant.CopyLocalAppSettings.Empty();
        return current with { ConfigVariant = empty.With(argumentValue) };
    }
}