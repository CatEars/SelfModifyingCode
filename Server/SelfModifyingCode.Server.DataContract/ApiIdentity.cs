namespace SelfModifyingCode.Server.DataContract;

public enum ApiIdentityVersion
{
    Sha256 = 1
}

public record ApiIdentity(ApiIdentityVersion Version, string Identity);
