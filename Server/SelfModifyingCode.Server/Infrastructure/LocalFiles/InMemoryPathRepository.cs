using SelfModifyingCode.Server.Directory;

namespace SelfModifyingCode.Server.Infrastructure.LocalFiles;

public class InMemoryPathRepository : IProgramPathRepository
{

    private Dictionary<string, PathType.LocalFile> RegisteredFiles { get; } = new();

    public PathType? GetPathByProgramId(string id)
    {
        return RegisteredFiles.TryGetValue(id, out var val) ? val : null;
    }

    public void RegisterLocalFile(string id, string location)
    {
        RegisteredFiles[id] = new PathType.LocalFile(location);
    }
}