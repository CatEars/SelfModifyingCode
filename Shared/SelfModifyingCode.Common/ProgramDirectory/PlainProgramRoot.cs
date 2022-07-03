﻿namespace SelfModifyingCode.Common.ProgramDirectory;

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
}