using System.IO.Compression;
using SelfModifyingCode.Common.Manifest;
using SelfModifyingCode.Common.ProgramRoot;
using SelfModifyingCode.Host.Application.Logging;
using SelfModifyingCode.Host.Verify;

namespace SelfModifyingCode.Host.Mode;

public class AsVerifierAndPacker : IExecution
{
    private string PackTarget { get; }

    private ILogger Logger { get; } = new ConsoleLogger();
    
    public AsVerifierAndPacker(string packTarget)
    {
        PackTarget = Path.GetFullPath(packTarget);
    }
    
    public void Run()
    {
        Logger.Info("Verifying pack target: " + PackTarget);
        var target = new PlainProgramRoot(PackTarget);
        var failedVerifications = RegisteredVerifiers.AllVerifiers
            .Select(verifier => verifier.Verify(target))
            .Where(result => result.IsError)
            .ToList();
        if (failedVerifications.Any())
        {
            Logger.Info("Verification of packing target failed:");
            foreach (var verification in failedVerifications)
            {
                Logger.Info($" - {verification.ErrorMessage}");
            }
            Environment.Exit(1);
        }

        var manifestReader = new ManifestReader("<none>", target);
        var manifest = manifestReader.ReadProgramManifest();
        var resolvedDirectory = Path.GetDirectoryName(PackTarget)!;
        var packDirectory = Directory.GetParent(resolvedDirectory)!.FullName;
        var packFilename = manifest.ProgramId.FullName + $".v{manifest.ProgramId.Version}.smc";
        var smcOutputPath = Path.Combine(packDirectory, packFilename);
        ZipFile.CreateFromDirectory(PackTarget, smcOutputPath);
        Logger.Info($"Finished packing application into '{smcOutputPath}'");
    }
}