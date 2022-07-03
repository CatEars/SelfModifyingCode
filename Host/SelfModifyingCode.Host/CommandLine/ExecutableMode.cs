namespace SelfModifyingCode.Host.CommandLine;

public record ExecutableMode
{
    private ExecutableMode() {}

    public record PrintHelp() : ExecutableMode;

    public record PrintDetailedHelp(string Pattern) : ExecutableMode;
    
    public record RunExe() : ExecutableMode;
}