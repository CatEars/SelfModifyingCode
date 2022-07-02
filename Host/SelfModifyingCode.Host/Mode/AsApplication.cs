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
        var tempManifest = ExtractAndReadTemporaryManifest();
        Console.WriteLine($"Manifest for {tempManifest.DisplayName} - {tempManifest.ProgramId}:");
        Console.WriteLine("ExecutionRoot: " + Options.ExecutingDirectory);
        var root = new ExecutionRoot(Options.ExecutingDirectory, tempManifest.ProgramId);
        var unpacker = new Unpacker(Options.ProgramPath, root);
        unpacker.Unpack();
        var realManifestReader = new ManifestReader(Options.GetProgramFileName(), root);
        var realManifest = realManifestReader.ReadProgramManifest();
        Console.WriteLine($"starting execution of {realManifest.GetExeLocator().GetExeFileLocation()}");
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