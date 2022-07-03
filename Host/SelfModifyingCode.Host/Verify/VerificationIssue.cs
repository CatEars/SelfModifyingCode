namespace SelfModifyingCode.Host.Verify;

public record VerificationIssue(bool IsOk, string ErrorMessage)
{

    public bool IsError => !IsOk;

    public static VerificationIssue Ok() => new VerificationIssue(true, "");

    public static VerificationIssue Failed(string message) => new(false, message);

}