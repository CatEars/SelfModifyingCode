using System.Diagnostics;
using SelfModifyingCode.Host.Application;
using SelfModifyingCode.Host.CommandLine;
using SelfModifyingCode.Host.ProgramDirectory;
using SelfModifyingCode.Interface;

namespace SelfModifyingCode.Host.Mode;

public class AsApplication : IExecution
{
    private CommandLineOptions Options { get; }
 
    public AsApplication(CommandLineOptions options)
    {
        Options = options;
    }
    
    public void Run()
    {
        Console.WriteLine("Running program: " + Options.ProgramPath);
        var manifest = RedeployProgram();
        Console.WriteLine($"Manifest for {manifest.DisplayName} - {manifest.ProgramId}");
        var currentIdentity = SHA256Helper.GetFileSHA256Hex(Options.ProgramPath);
        var runner = CreateProgramRunner(manifest);
        var task = Task.Run(() =>
        {
            runner.RunUntilQuit();
        });
        var updateChecker = SelectUpdateChecker.Get(manifest.GloballyKnownDownloadLocation);
        Thread.Sleep(2000);
        while (!(task.IsCanceled || task.IsCompleted || task.IsFaulted))
        {
            var updateExistsTask = updateChecker.UpdateExists(manifest, currentIdentity, TimeSpan.FromSeconds(10));
            var updateExists = updateExistsTask.Result;
            if (updateExists)
            {
                Console.WriteLine("Update exists, closing and updating");
                runner.Stop();
                task.Wait();
                Console.WriteLine("Inner thread stopped");
                updateChecker.DownloadLatestProgram(manifest, Options.ProgramPath).Wait();
                manifest = RedeployProgram();
                currentIdentity = SHA256Helper.GetFileSHA256Hex(Options.ProgramPath);
                runner = CreateProgramRunner(manifest);
                task = Task.Run(() =>
                {
                    runner.RunUntilQuit();
                });
                Console.WriteLine("New program up and running!");
                Console.WriteLine($"Manifest: {manifest.DisplayName} - {manifest.ProgramId}");
            }
            Thread.Sleep(5000);
        }
    }

    private ISelfModifyingCodeManifest RedeployProgram()
    {
        var tempManifest = ExtractAndReadTemporaryManifest();
        return UnpackProgramIntoExecutionEnvironment(tempManifest);
    }

    private static ProgramRunner CreateProgramRunner(ISelfModifyingCodeManifest realManifest)
    {
        var exeLocation = realManifest.GetExeLocator().GetExeFileLocation();
        var exeDirectory = Path.GetDirectoryName(exeLocation);
        var workingDirectory = exeDirectory!;
        var arguments = string.Join(" ", realManifest.ProgramArguments);
        var processOptions = new ProcessStartInfo()
        {
            FileName = exeLocation,
            WorkingDirectory = workingDirectory,
            Arguments = arguments,
            UseShellExecute = false
        };
        return new ProgramRunner(processOptions);
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