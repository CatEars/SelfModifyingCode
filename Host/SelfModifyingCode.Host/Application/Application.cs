using SelfModifyingCode.Host.Application.Logging;
using SelfModifyingCode.Host.Application.Update;
using SelfModifyingCode.Host.CommandLine;
using SelfModifyingCode.Host.ProgramDirectory;
using SelfModifyingCode.Interface;

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
        var applicationRunInfo = RedeployProgram();
        var manifest = applicationRunInfo.Manifest;
        Logger.Info($"Manifest for {manifest.DisplayName} - {manifest.ProgramId}");
        applicationRunInfo.Runner.StartAsync();
        
        var updateChecker = SelectUpdateChecker.Get(manifest.GloballyKnownDownloadLocation);
        await Task.Delay(3000);
        while (!applicationRunInfo.Runner.HasStopped)
        {
            var timeout = TimeSpan.FromSeconds(10);
            var updateExists = await updateChecker.UpdateExists(applicationRunInfo, timeout);
            if (updateExists)
            {
                Logger.Info("Update exists, closing and updating");
                applicationRunInfo.Runner.Stop();
                await applicationRunInfo.Runner.ActiveTask;
                
                Logger.Info("Inner thread stopped");
                updateChecker.DownloadLatestProgram(manifest, Options.ProgramPath).Wait();
                applicationRunInfo = RedeployProgram();
                manifest = applicationRunInfo.Manifest;
                applicationRunInfo.Runner.StartAsync();
                
                Logger.Info("New program up and running!");
                Logger.Info($"Manifest: {manifest.DisplayName} - {manifest.ProgramId}");
            }

            await Task.Delay(5000);
        }
    }

    private ApplicationRunInfo RedeployProgram()
    {
        var tempManifest = ExtractAndReadTemporaryManifest();
        var realManifest = UnpackProgramIntoExecutionEnvironment(tempManifest);
        var identity = SHA256Helper.GetFileSHA256Hex(Options.ProgramPath);
        var runner = ProgramRunner.CreateRunnerFromManifest(realManifest);
        return new ApplicationRunInfo(realManifest, identity, runner);
    }

    private ISelfModifyingCodeManifest UnpackProgramIntoExecutionEnvironment(ISelfModifyingCodeManifest tempManifest)
    {
        var root = new ExecutionRoot(Options.ExecutingDirectory, tempManifest.ProgramId);
        var unpacker = new Unpacker(Options.ProgramPath, root);
        unpacker.Unpack();
        var realManifestReader = new ManifestReader(Options.GetProgramFileName(), root);
        return realManifestReader.ReadProgramManifest();
    }

    private ISelfModifyingCodeManifest ExtractAndReadTemporaryManifest()
    {
        var temporaryRoot = new TemporaryRoot(Options.ProgramPath);
        var temporaryUnpacker = new Unpacker(Options.ProgramPath, temporaryRoot);
        temporaryUnpacker.Unpack();
        var programFileName = Options.GetProgramFileName();
        var temporaryManifestReader = new ManifestReader(programFileName, temporaryRoot);
        var manifest = temporaryManifestReader.ReadProgramManifest();
        return manifest;
    }
}