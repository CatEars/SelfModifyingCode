namespace SelfModifyingCode;

public record RestartType
{
    private RestartType() {}

    public record StopThenStart() : RestartType;
}