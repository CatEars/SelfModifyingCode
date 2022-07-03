namespace SelfModifyingCode.Server.Directory;

public record Program(ProgramId ProgramId, ProgramIdentity Identity, string ProgramPath, string DownloadPath);