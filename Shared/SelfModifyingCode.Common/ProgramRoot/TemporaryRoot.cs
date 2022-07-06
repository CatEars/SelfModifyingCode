namespace SelfModifyingCode.Common.ProgramRoot;

public class TemporaryRoot : IProgramRoot
{
    private string ProgramPath { get; }
    
    public TemporaryRoot(string programPath)
    {
        ProgramPath = programPath;
    }
    
    public string GetProgramRootFolder()
    {
        var hash = SHA256Helper.GetFileSHA256Hex(ProgramPath);
        var tempDirectory = Path.GetTempPath();
        return Path.Combine(tempDirectory, hash);
    }
}