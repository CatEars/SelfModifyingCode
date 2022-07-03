namespace SelfModifyingCode.Host.Application;

public record ApplicationRunInfo(
    ISelfModifyingCodeManifest Manifest, 
    string Identity,
    ProgramRunner Runner);