namespace SelfModifyingCode.Host.CommandLine.Options;

public static class OptionsRegistry
{
    public static IReadOnlyList<ICommandLineOption> AllOptions = new List<ICommandLineOption>()
    {
        new RunHelpOption(),
        new ExecutingDirectoryOption(),
        new ProgramLocationOption(),
        new AppSettingsConfigOption()
    };

    public static Dictionary<string, ICommandLineOption> Options { get; } = new();

    static OptionsRegistry()
    {
        foreach (var option in AllOptions)
        {
            if (option.ShortFlag != null)
            {
                Options[option.ShortFlag] = option;
            }

            Options[option.LongFlag] = option;
        }
    }

    public static bool IsRegisteredOption(string flagName) => Options.ContainsKey(flagName);

    public static bool AllMandatoryOptionsAreRun(HashSet<string> optionsThatHaveRun)
    {
        var mandatory = GetMandatoryArguments();
        return optionsThatHaveRun.IsSupersetOf(mandatory);
    }

    public static HashSet<string> GetMandatoryArguments()
    {
        var mandatory = AllOptions
            .Where(option => option.IsMandatory)
            .Select(option => option.Name)
            .ToHashSet();
        return mandatory;
    }
}