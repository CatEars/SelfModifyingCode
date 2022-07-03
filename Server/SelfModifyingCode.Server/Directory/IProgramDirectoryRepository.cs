namespace SelfModifyingCode.Server.Directory;

public interface IProgramDirectoryRepository
{

    ProgramDirectory GetProgramDirectory();

    Program GetProgramByName(string name);

}