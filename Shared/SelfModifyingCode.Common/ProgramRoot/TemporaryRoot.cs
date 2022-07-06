namespace SelfModifyingCode.Common.ProgramRoot;

public class TemporaryRoot : IProgramRoot
{
    private string ProgramPath { get; }
    
    private string ProgramRoot { get; }
    
    public TemporaryRoot(string programPath)
    {
        ProgramPath = programPath;
        ProgramRoot = GenerateProgramRoot(programPath);
    }

    private static string GenerateProgramRoot(string programPath)
    {
        var hash = SHA256Helper.GetFileSHA256Hex(programPath);
        var tempDirectory = Path.GetTempPath();
        return Path.Combine(tempDirectory, hash);
    }
    
    public string GetProgramRootFolder()
    {
        return ProgramRoot;
    }

    public string GetProgramName()
    {
        return Path.GetFileName(ProgramPath);
    }
}