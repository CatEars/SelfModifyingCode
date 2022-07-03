namespace SelfModifyingCode.Host.Verify;

public static class RegisteredVerifiers
{

    public static IReadOnlyList<IVerifier> AllVerifiers = new List<IVerifier>()
    {
        new HasManifest(),
        new ManifestPointsToValidExecutable()
    };

}