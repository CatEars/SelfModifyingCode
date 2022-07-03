namespace SelfModifyingCode.Server.Directory;

public interface IProgramPathRepository
{
    PathType? GetPathByProgramId(string id);
}