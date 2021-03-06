namespace SelfModifyingCode.Common.ProgramRoot;

public class ExecutionRoot : IProgramRoot
{
    private string RootPath { get; }
    
    private ProgramId ProgramId { get; }
    
    public ExecutionRoot(string rootPath, ProgramId programId)
    {
        RootPath = Path.GetFullPath(rootPath);
        ProgramId = programId;
    }

    public string GetProgramRootFolder()
    {
        var programVersion = ProgramId.Version;
        var version = $"v{programVersion.Major}.{programVersion.Minor}";
        return Path.Combine(RootPath, ProgramId.FullName, version);
    }

    public string GetProgramName()
    {
        return ProgramId.FullName;
    }
}