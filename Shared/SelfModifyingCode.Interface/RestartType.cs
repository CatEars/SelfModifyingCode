namespace SelfModifyingCode.Interface;

public record RestartType
{
    private RestartType() {}

    public record StopThenStart() : RestartType;
}