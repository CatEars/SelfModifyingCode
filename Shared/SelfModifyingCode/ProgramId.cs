namespace SelfModifyingCode;

public record ProgramId(string FullName, Version Version)
{

    public static ProgramId FromFullNameAndVersion(string val, Version version) => new(val, version);

}