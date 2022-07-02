using SelfModifyingCode.Interface;
using SelfModifyingCode.Interface.Helpers;

namespace BasicWebApi.SMCManifest;

public class Manifest : ISelfModifyingCodeManifest
{

    private Version Version { get; } = new(1, 1);
    
    public ProgramId ProgramId => ProgramId.FromFullNameAndVersion("xyz.catears.BasicWebServer", Version);
    
    public string DisplayName => "Echo Server";

    public IExeFileLocator GetExeLocator() => SingleExeLocator<Manifest>.FromRelativePath("BasicWebApi");

    public Uri GloballyKnownDownloadLocation => new("http://localhost:7111/Program/BasicWebApi");
    
}