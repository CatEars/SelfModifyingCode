namespace SelfModifyingCode.Host;

public record ConfigVariant
{

    private ConfigVariant()
    {
    }

    public record UsePackagedConfiguration() : ConfigVariant;

    public record CopyLocalAppSettings(string AppSettingsPath) : ConfigVariant;

};