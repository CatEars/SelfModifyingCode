using SelfModifyingCode;
using SelfModifyingCode.Helpers;

namespace BasicWebApi.SMCManifest;

public class Manifest : ISelfModifyingCodeManifest
{

    private Version Version { get; } = new(1, 6);
    
    public ProgramId ProgramId => ProgramId.FromFullNameAndVersion("xyz.catears.BasicWebServer", Version);
    
    public string DisplayName => "Echo Server";

    public IExeFileLocator GetExeLocator() => SingleExeLocator<Manifest>.FromRelativePath("BasicWebApi");

    public Uri GloballyKnownDownloadLocation => new("http://localhost:5049/");
    
}