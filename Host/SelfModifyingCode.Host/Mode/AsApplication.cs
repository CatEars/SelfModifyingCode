using SelfModifyingCode.Host.CommandLine;
using SelfModifyingCode.Host.ProgramDirectory;

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
        var manifestReader = new ManifestReader(Options.ProgramPath);
        var manifest = manifestReader.ReadProgramManifest();
        Console.WriteLine($"Manifest for {manifest.DisplayName} - {manifest.ProgramId}:");
    }
}