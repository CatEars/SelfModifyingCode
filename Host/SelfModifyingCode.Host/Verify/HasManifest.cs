using SelfModifyingCode.Common.ProgramDirectory;

namespace SelfModifyingCode.Host.Verify;

public class HasManifest : IVerifier
{
    public VerificationIssue Verify(IProgramRoot programRoot)
    {
        var manifestReader = new ManifestReader("<none>", programRoot);
        try
        {
            manifestReader.ReadProgramManifest();
            return VerificationIssue.Ok();
        }
        catch (Exception)
        {
            return VerificationIssue.Failed($"Could not find manifest in '{programRoot.GetProgramRootFolder()}'");            
        }
    }
}