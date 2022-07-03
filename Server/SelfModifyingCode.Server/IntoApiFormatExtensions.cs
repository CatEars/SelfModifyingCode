using SelfModifyingCode.Server.DataContract;
using SelfModifyingCode.Server.Directory;

namespace SelfModifyingCode.Server;

public static class IntoApiFormatExtensions
{

    public static ApiIdentityVersion IntoApiFormat(this IdentityVersion version) => version switch
    {
        IdentityVersion.Sha256 => ApiIdentityVersion.Sha256,
        _ => throw new ArgumentException($"Unknown identity version {version}")
    };
    
    public static ApiIdentity IntoApiFormat(this ProgramIdentity identity)
    {
        return new ApiIdentity(identity.Version.IntoApiFormat(), identity.Identity);
    }

    public static ApiProgramInformation IntoApiFormat(this Directory.Program program)
    {
        return new ApiProgramInformation(
            program.ProgramId.FullName,
            program.ProgramId.Version.ToString(),
            program.Identity.IntoApiFormat(),
            Path.GetFileName(program.ProgramPath),
            program.DownloadPath
        );
    }

    public static ApiProgramDirectory IntoApiFormat(this ProgramDirectory directory)
    {
        return new ApiProgramDirectory(
            directory.Programs.Select(IntoApiFormat).ToList()
        );
    }
    
}