using SelfModifyingCode.Server.Directory;

namespace SelfModifyingCode.Server.Infrastructure;

public class FakeProgramPathRepository : IProgramPathRepository
{
    public PathType? GetPathByProgramId(string id)
    {
        if (id != "abc")
        {
            return null;
        }
        return new PathType.WebUrl(new Uri("http://localhost:8123/BasicWebApi.Bundle.smc"));
    }
}