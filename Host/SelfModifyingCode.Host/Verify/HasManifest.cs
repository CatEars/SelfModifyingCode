using SelfModifyingCode.Common.Manifest;
using SelfModifyingCode.Common.ProgramRoot;

namespace SelfModifyingCode.Host.Verify;

public class HasManifest : IVerifier
{
    public VerificationIssue Verify(IProgramRoot programRoot)
    {
        var manifestReader = new ManifestReader(programRoot);
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