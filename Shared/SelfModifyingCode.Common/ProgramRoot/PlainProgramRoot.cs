namespace SelfModifyingCode.Common.ProgramRoot;

public class PlainProgramRoot : IProgramRoot
{
    private string ProgramRoot { get; }
    
    public PlainProgramRoot(string programRoot)
    {
        ProgramRoot = programRoot;
    }    
    
    public string GetProgramRootFolder()
    {
        return ProgramRoot;
    }

    public string GetProgramName()
    {
        return Path.GetDirectoryName(ProgramRoot) ?? "<unknown>";
    }
}