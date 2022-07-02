namespace SelfModifyingCode.Host.CommandLine;

public record ExecutableMode
{
    private ExecutableMode() {}

    public record PrintHelp() : ExecutableMode;

    public record RunExe() : ExecutableMode;
}