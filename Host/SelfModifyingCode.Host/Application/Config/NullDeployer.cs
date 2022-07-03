using SelfModifyingCode.Interface;

namespace SelfModifyingCode.Host.Application.Config;

public class NullDeployer : IConfigDeployer
{
    public void SaveConfig(ISelfModifyingCodeManifest _)
    {
        // Intentionally left empty
    }
}