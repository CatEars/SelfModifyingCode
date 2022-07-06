using SelfModifyingCode.Common.ProgramRoot;

namespace SelfModifyingCode.Host.Verify;

public interface IVerifier
{

    VerificationIssue Verify(IProgramRoot programRoot);

}