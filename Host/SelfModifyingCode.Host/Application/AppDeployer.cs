using SelfModifyingCode.Common;
using SelfModifyingCode.Common.Manifest;
using SelfModifyingCode.Common.ProgramRoot;
using SelfModifyingCode.Common.Unpacking;
using SelfModifyingCode.Host.Application.Config;
using SelfModifyingCode.Host.CommandLine;

namespace SelfModifyingCode.Host.Application;

public class AppDeployer
{
    private CommandLineOptions Options { get; }

    private IConfigDeployer ConfigDeployer { get; }
    
    public AppDeployer(CommandLineOptions options)
    {
        Options = options;
        ConfigDeployer = options.ConfigVariant switch
        {
            ConfigVariant.CopyLocalAppSettings appSettings => new CopyConfigDeployer(appSettings),
            _ => new NullDeployer()
        };
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
        var realManifestReader = new ManifestReader(root);
        var manifest = realManifestReader.ReadProgramManifest();
        ConfigDeployer.SaveConfig(manifest);
        return manifest;
    }

    private ISelfModifyingCodeManifest ExtractAndReadTemporaryManifest()
    {
        var temporaryRoot = new TemporaryRoot(Options.ProgramPath);
        var temporaryUnpacker = new Unpacker(Options.ProgramPath, temporaryRoot);
        temporaryUnpacker.Unpack();
        var temporaryManifestReader = new ManifestReader(temporaryRoot);
        var manifest = temporaryManifestReader.ReadProgramManifest();
        return manifest;
    }
    
}