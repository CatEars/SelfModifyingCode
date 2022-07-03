namespace SelfModifyingCode;

public interface ISelfModifyingCodeManifest
{
    
    ProgramId ProgramId { get; }
    
    string DisplayName { get; }

    IExeFileLocator GetExeLocator();

    RestartType RestartType => new RestartType.StopThenStart();
    
    string[] ProgramArguments => Array.Empty<string>();

    Uri GloballyKnownDownloadLocation { get; }
}