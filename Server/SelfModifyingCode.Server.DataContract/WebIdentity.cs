namespace SelfModifyingCode.Server.DataContract;

public enum WebIdentityVersion
{
    Sha256 = 1
}

public record WebIdentity(WebIdentityVersion Version, string Identity);
