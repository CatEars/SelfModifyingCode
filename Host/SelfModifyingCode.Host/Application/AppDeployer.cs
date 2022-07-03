using SelfModifyingCode.Host.CommandLine;
using SelfModifyingCode.Host.ProgramDirectory;
using SelfModifyingCode.Interface;

namespace SelfModifyingCode.Host.Application;

public class AppDeployer
{
    private CommandLineOptions Options { get; }
    
    public AppDeployer(CommandLineOptions options)
    {
        Options = options;
    }

    public string ProgramLocation => Options.ProgramPath;
    
    public ApplicationRunInfo RedeployProgram()
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