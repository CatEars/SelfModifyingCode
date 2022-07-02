namespace SelfModifyingCode.Host.CommandLine.Options;

public static class RegisteredOptions
{
    public static IReadOnlyList<ICommandLineOption> AllOptions = new List<ICommandLineOption>()
    {
        new RunHelpOption(),
        new ProgramLocationOption()
    };

    public static Dictionary<string, ICommandLineOption> Options { get; } = new();

    static RegisteredOptions()
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