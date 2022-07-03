namespace SelfModifyingCode.Server.DataContract;

public record ProgramInformation(
    string Name, 
    string Version, 
    WebIdentity Identity,
    string FileName,
    string DownloadUri);
    
    