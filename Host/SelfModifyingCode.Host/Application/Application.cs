using SelfModifyingCode.Host.Application.Logging;
using SelfModifyingCode.Host.Application.Update;
using SelfModifyingCode.Host.CommandLine;

namespace SelfModifyingCode.Host.Application;

public class Application
{
    private CommandLineOptions Options { get; }

    private ILogger Logger { get; } = new ConsoleLogger();
    
    public Application(CommandLineOptions options)
    {
        Options = options;
    }

    public async Task Run()
    {
        Logger.Info("Running program: " + Options.ProgramPath);
        var deployer = new AppDeployer(Options);
        var applicationRunInfo = deployer.RedeployProgram();
        var manifest = applicationRunInfo.Manifest;
        Logger.Info($"Manifest for {manifest.DisplayName} - {manifest.ProgramId}");
        applicationRunInfo.Runner.StartAsync();
        
        var updateChecker = SelectUpdateChecker.Get(manifest.GloballyKnownDownloadLocation);
        var updater = SelectUpdateStrategy.Get(manifest.RestartType, deployer);
        await AppOrDelay(applicationRunInfo, 3000);
        while (!applicationRunInfo.Runner.HasStopped)
        {
            var timeout = TimeSpan.FromSeconds(10);
            var updateExists = await updateChecker.UpdateExists(applicationRunInfo, timeout);
            if (updateExists)
            {
                applicationRunInfo = await updater.OnNewUpdateFound(applicationRunInfo, updateChecker);
            }

            await AppOrDelay(applicationRunInfo, 5000);
        }
    }

    private static async Task AppOrDelay(ApplicationRunInfo applicationRunInfo, int timeoutMs)
    {
        await Task.WhenAny(applicationRunInfo.Runner.ActiveTask, Task.Delay(timeoutMs));
    }

}