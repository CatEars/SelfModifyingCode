using SelfModifyingCode.Common.ProgramDirectory;

namespace SelfModifyingCode.Host.Verify;

public interface IVerifier
{

    VerificationIssue Verify(IProgramRoot programRoot);

}