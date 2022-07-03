using System.IO.Compression;

namespace SelfModifyingCode.Common.ProgramDirectory;

public class Unpacker
{
    
    private string ProgramPath { get; }
    
    private IProgramRoot ProgramRoot { get; }

    public Unpacker(string programPath, IProgramRoot programRoot)
    {
        ProgramPath = programPath;
        ProgramRoot = programRoot;
    }

    public void Unpack()
    {
        var programIdentity = SHA256Helper.GetFileSHA256Hex(ProgramPath);
        var directory = ProgramRoot.GetProgramRootFolder();
        var hashFile = Path.Combine(directory, programIdentity + ".hash");
        if (File.Exists(hashFile))
        {
            // We assume that if the hashfile of the program has been written,
            // then the equivalent program already exists
            return;
        }

        if (Directory.Exists(directory))
        {
            Directory.Delete(directory, recursive: true);
        }
        
        Directory.CreateDirectory(directory);
        ZipFile.ExtractToDirectory(ProgramPath, directory);
        File.WriteAllText(hashFile, programIdentity);
    }
    
}