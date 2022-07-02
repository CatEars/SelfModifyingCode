namespace SelfModifyingCode.Host;

public record ConfigVariant
{

    private ConfigVariant()
    {
    }

    public record UsePackagedConfiguration() : ConfigVariant;

};