namespace SelfModifyingCode.Server.DataContract;

public record ApiProgramInformation(
    string Name, 
    string Version, 
    ApiIdentity Identity,
    string FileName,
    string DownloadPath);
    
    