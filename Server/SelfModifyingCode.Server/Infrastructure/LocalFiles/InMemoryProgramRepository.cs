using SelfModifyingCode.Server.Directory;

namespace SelfModifyingCode.Server.Infrastructure.LocalFiles;

public class InMemoryProgramRepository : IProgramRepository
{
    
    private ProgramDirectory Directory { get; set; } = ProgramDirectory.Empty();

    public ProgramDirectory GetProgramDirectory()
    {
        return Directory;
    }

    public Directory.Program? GetProgramByName(string name)
    {
        return Directory.Programs.FirstOrDefault(program => program.ProgramId.FullName == name);
    }

    public void RegisterProgram(Directory.Program program)
    {
        Directory = Directory.With(program);
    }
}