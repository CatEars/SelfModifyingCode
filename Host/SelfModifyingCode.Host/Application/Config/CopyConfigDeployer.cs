using SelfModifyingCode.Interface;

namespace SelfModifyingCode.Host.Application.Config;

public class CopyConfigDeployer : IConfigDeployer
{
    
    private ConfigVariant.CopyLocalAppSettings AppSettings { get; }

    public CopyConfigDeployer(ConfigVariant.CopyLocalAppSettings appSettings)
    {
        AppSettings = appSettings;
    }
    
    public void SaveConfig(ISelfModifyingCodeManifest manifest)
    {
        var exeLocator = manifest.GetExeLocator();
        var exeLocation = exeLocator.GetExeFileLocation();
        var exeDirectory = Path.GetDirectoryName(exeLocation)!;
        var fileName = Path.GetFileName(AppSettings.AppSettingsPath);
        var sourcePath = AppSettings.AppSettingsPath;
        var targetPath = Path.Combine(exeDirectory, fileName);
        
        File.Copy(sourcePath, targetPath, overwrite: true);
    }
}