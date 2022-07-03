using System.Collections.Immutable;

namespace SelfModifyingCode.Host;

public record ConfigVariant
{

    private ConfigVariant()
    {
    }

    public record UsePackagedConfiguration() : ConfigVariant;

    public record CopyLocalAppSettings(ImmutableList<string> AppSettingsPaths) : ConfigVariant
    {

        public static CopyLocalAppSettings Empty() => new(ImmutableList<string>.Empty);

        public CopyLocalAppSettings With(string appSettingPath) => this with
        {
            AppSettingsPaths = AppSettingsPaths.Add(appSettingPath)
        };

    };

};