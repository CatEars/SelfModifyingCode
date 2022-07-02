using System.IO.Compression;

namespace SelfModifyingCode.Host.ProgramDirectory;

public class Unpacker
{
    private ProgramSource ProgramSource { get; }
    
    private ExecutionRoot ExecutionRoot { get; }

    public Unpacker(ProgramSource source, ExecutionRoot executionRoot)
    {
        ProgramSource = source;
        ExecutionRoot = executionRoot;
    }

    public void Unpack()
    {
        ExecutionRoot.EnsureProgramFolderExists(ProgramSource.Id);
        var programFolder = ExecutionRoot.GetProgramFolder(ProgramSource.Id);
        ZipFile.ExtractToDirectory(ProgramSource.ProgramPath, programFolder);
    }
    
}