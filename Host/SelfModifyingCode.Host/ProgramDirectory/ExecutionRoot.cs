using SelfModifyingCode.Interface;

namespace SelfModifyingCode.Host.ProgramDirectory;

public class ExecutionRoot
{
    private string RootPath { get; }
    
    public ExecutionRoot(string rootPath)
    {
        RootPath = Path.GetFullPath(rootPath);
    }

    public void EnsureProgramFolderExists(ProgramId programId)
    {
        Directory.CreateDirectory(GetProgramFolder(programId));
    }

    public string GetProgramFolder(ProgramId programId)
    {
        var programVersion = programId.Version;
        var version = $"v{programVersion.Major}.{programVersion.Minor}";
        return Path.Combine(RootPath, programId.FullName, version);
    }
}