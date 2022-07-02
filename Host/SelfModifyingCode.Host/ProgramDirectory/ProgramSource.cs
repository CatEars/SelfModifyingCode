using SelfModifyingCode.Interface;

namespace SelfModifyingCode.Host.ProgramDirectory;

public record ProgramSource(string ProgramPath, ProgramId Id)
{

    public static ProgramSource FromRelativePath(string relativePath, ProgramId id) => new(relativePath, id);

}