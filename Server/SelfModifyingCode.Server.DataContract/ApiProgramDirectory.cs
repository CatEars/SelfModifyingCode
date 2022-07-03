namespace SelfModifyingCode.Server.DataContract;

public record ApiProgramDirectory(
    List<ApiProgramInformation> AvailablePrograms
    );