namespace SelfModifyingCode.Host.Application.Config;

public interface IConfigDeployer
{

    void SaveConfig(ISelfModifyingCodeManifest manifest);

}