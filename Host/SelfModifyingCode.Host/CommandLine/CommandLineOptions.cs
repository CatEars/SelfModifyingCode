namespace SelfModifyingCode.Host.CommandLine;

public record CommandLineOptions(
    // The path to the program (in zip-file) format
    string ProgramPath,

    // Directory where the program is packed up and executed
    string ExecutingDirectory,

    // How the configuration should be accessed
    ConfigVariant ConfigVariant,

    // The requested execution mode (e.g. print help text, run SMC program)
    ExecutableMode ExecutableMode
)
{
    public static CommandLineOptions RunHelp() => new(
        "",
        "",
        new ConfigVariant.UsePackagedConfiguration(),
        new ExecutableMode.PrintHelp()
        );

    public static CommandLineOptions Default() => new(
        "",
        Path.GetTempPath(),
        new ConfigVariant.UsePackagedConfiguration(),
        new ExecutableMode.RunExe()
        );
}