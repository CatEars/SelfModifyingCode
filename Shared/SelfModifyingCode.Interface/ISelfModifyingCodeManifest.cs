namespace SelfModifyingCode.Interface;

public interface ISelfModifyingCodeManifest
{
    
    string ProgramId { get; }
    
    string DisplayName { get; }

    Version GetVersion();

    IExeFileLocator GetExeLocator();

    RestartType RestartType => new RestartType.StopThenStart();
    
    string[] ProgramArguments => Array.Empty<string>();

    Uri GloballyKnownDownloadLocation { get; }
}