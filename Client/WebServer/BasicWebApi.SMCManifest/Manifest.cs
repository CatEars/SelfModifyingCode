using SelfModifyingCode.Interface;
using SelfModifyingCode.Interface.Helpers;

namespace BasicWebApi.SMCManifest;

public class Manifest : ISelfModifyingCodeManifest
{

    public string ProgramId => "xyz.catears.BasicWebServer";
    
    public string DisplayName => "Echo Server";

    public Version GetVersion() => new (1, 1);

    public IExeFileLocator GetExeLocator() => SingleExeLocator<Manifest>.FromRelativePath("BasicWebApi");

    public Uri GloballyKnownDownloadLocation => new("http://localhost:7111/Program/BasicWebApi");
    
}