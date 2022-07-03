using SelfModifyingCode.Common.ProgramDirectory;

namespace SelfModifyingCode.Host.Verify;

public class ManifestPointsToValidExecutable : IVerifier
{
    public VerificationIssue Verify(IProgramRoot programRoot)
    {
        var manifestReader = new ManifestReader("<none>", programRoot);
        try
        {
            var manifest = manifestReader.ReadProgramManifest();
            var exeLocation = manifest.GetExeLocator().GetExeFileLocation();
            var fullLocation = Path.Combine(programRoot.GetProgramRootFolder(), exeLocation);
            var fileExists = File.Exists(fullLocation);
            if (!fileExists)
            {
                return VerificationIssue.Failed($"Expected exe-file to exist here: '{fullLocation}' but it did not");
            }
            return VerificationIssue.Ok();
        }
        catch (Exception)
        {
            return VerificationIssue.Failed($"Could not find manifest in '{programRoot.GetProgramRootFolder()}' and " +
                                            $"therefore could not determine exe-file location.");            
        }

    }
}