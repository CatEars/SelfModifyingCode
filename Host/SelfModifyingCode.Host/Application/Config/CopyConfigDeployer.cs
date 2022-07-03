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
        foreach (var appSetting in AppSettings.AppSettingsPaths)
        {
            var fileName = Path.GetFileName(appSetting);
            var sourcePath = appSetting;
            var targetPath = Path.Combine(exeDirectory, fileName);
        
            File.Copy(sourcePath, targetPath, overwrite: true);
        }
    }
}