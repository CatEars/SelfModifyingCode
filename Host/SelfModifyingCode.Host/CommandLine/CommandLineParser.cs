using SelfModifyingCode.Host.CommandLine.Options;

namespace SelfModifyingCode.Host.CommandLine;

public class CommandLineParser
{
    
    private IReadOnlyList<string> Args { get; }

    public CommandLineParser(string[] args)
    {
        Args = args.ToList();
    }

    public static bool ArgumentIsFlag(string arg)
    {
        return arg.StartsWith("-");
    }
    
    public CommandLineOptions ParseArgs()
    {
        var parsedOptions = CommandLineOptions.Default();
        var commandsThatRan = new HashSet<string>();
        for (int idx = 0; idx < Args.Count; ++idx)
        {
            var currentArg = Args[idx];
            var nextArg = (idx + 1 < Args.Count) ? (Args[idx + 1]) : null;
            if (ArgumentIsFlag(currentArg) && RegisteredOptions.IsRegisteredOption(currentArg))
            {
                var command = RegisteredOptions.Options[currentArg];
                parsedOptions = command.Apply(parsedOptions, nextArg);
                commandsThatRan.Add(command.Name);
            }
        }
        
        // Create special case for -h / --help so it always runs, even if other flags/args are non-sensical
        if (HelpFlagIsPresent(commandsThatRan))
        {
            return CommandLineOptions.RunHelp();
        }

        ThrowIfNotAllMandatoryOptionsAreRun(commandsThatRan);
        return parsedOptions;
    }

    private static bool HelpFlagIsPresent(HashSet<string> commandsThatRan)
    {
        return commandsThatRan.Contains(RunHelpOption.CommandName);
    }

    private static void ThrowIfNotAllMandatoryOptionsAreRun(HashSet<string> commandsThatRan)
    {
        if (!RegisteredOptions.AllMandatoryOptionsAreRun(commandsThatRan))
        {
            var mandatoryOptions = RegisteredOptions.GetMandatoryArguments();
            var missing = mandatoryOptions
                .Except(commandsThatRan)
                .Select(commandName => $"'{commandName}'");
            var missingStr = string.Join(", ", missing);
            throw new MissingArgumentException($"The following mandatory arguments were not specified: {missingStr}");
        }
    }
}