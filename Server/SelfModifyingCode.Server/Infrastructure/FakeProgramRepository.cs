using SelfModifyingCode.Server.Directory;

namespace SelfModifyingCode.Server.Infrastructure;

public class FakeProgramRepository : IProgramRepository
{

    private Directory.Program BaseProgram => new(
        ProgramId.FromFullNameAndVersion("xyz.catears.BasicWebServer", Version.Parse("1.4")),
        ProgramIdentity.NewSha256Identity("abc"),
        "C:/temp/Bundles/BasicWebApi.Bundle.smc",
        "/api/Download/abc"
        );
    
    public ProgramDirectory GetProgramDirectory()
    {
        return ProgramDirectory.Empty().With(BaseProgram);
    }

    public Directory.Program? GetProgramByName(string name)
    {
        if (name != "xyz.catears.BasicWebServer")
        {
            return null;
        }

        return BaseProgram;
    }

    public void RegisterProgram(Directory.Program program)
    {
        throw new NotImplementedException();
    }
}