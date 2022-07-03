namespace SelfModifyingCode.Server.Directory;

public enum IdentityVersion
{
    Sha256 = 1
}

public record ProgramIdentity(IdentityVersion Version, string Identity)
{

    public static ProgramIdentity NewSha256Identity(string identity) => new(IdentityVersion.Sha256, identity);

}