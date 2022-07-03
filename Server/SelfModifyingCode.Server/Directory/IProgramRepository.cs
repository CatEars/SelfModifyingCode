namespace SelfModifyingCode.Server.Directory;

public interface IProgramRepository
{

    ProgramDirectory GetProgramDirectory();

    Program? GetProgramByName(string name);

    void RegisterProgram(Program program);

}