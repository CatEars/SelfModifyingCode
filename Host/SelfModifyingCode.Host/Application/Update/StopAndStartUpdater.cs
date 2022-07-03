using SelfModifyingCode.Host.Application.Logging;

namespace SelfModifyingCode.Host.Application.Update;

public class StopAndStartUpdater : IUpdateStrategy
{

    private AppDeployer Deployer { get; }
    
    private ILogger Logger { get; } = new ConsoleLogger();

    public StopAndStartUpdater(AppDeployer deployer)
    {
        Deployer = deployer;
    }
    
    public Task OnStartup(ISelfModifyingCodeManifest manifest)
    {
        return Task.CompletedTask;
    }

    public async Task<ApplicationRunInfo> OnNewUpdateFound(ApplicationRunInfo applicationRunInfo, IUpdateChecker checker)
    {
        Logger.Info("Downloading update while current app is still running");
        await checker.DownloadLatestProgram(applicationRunInfo.Manifest, Deployer.ProgramLocation);
        Logger.Info("Update exists, closing and updating");
        applicationRunInfo.Runner.Stop();
        await applicationRunInfo.Runner.ActiveTask;
                
        Logger.Info("Inner thread stopped");
        var newApplicationRunInfo = Deployer.RedeployProgram();
        newApplicationRunInfo.Runner.StartAsync();
                
        Logger.Info("New program up and running!");
        var manifest = newApplicationRunInfo.Manifest;
        Logger.Info($"Manifest: {manifest.DisplayName} - {manifest.ProgramId}");

        return newApplicationRunInfo;
    }
}