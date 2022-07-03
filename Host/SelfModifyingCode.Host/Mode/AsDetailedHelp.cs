using SelfModifyingCode.Host.Application.Logging;
using SelfModifyingCode.Host.CommandLine.Options;

namespace SelfModifyingCode.Host.Mode;

public class AsDetailedHelp : IExecution
{
    
    private string Pattern { get; }

    private ILogger Logger { get; } = new ConsoleLogger();

    public AsDetailedHelp(string pattern)
    {
        Pattern = pattern;
    }

    public void Run()
    {
        if (string.IsNullOrWhiteSpace(Pattern))
        {
            Logger.Info("Looked for detailed help, but only got empty pattern. " +
                        "Specify more clearly what option to print help message for");
            return;
        }

        var matchingOption = OptionsRegistry.AllOptions
            .FirstOrDefault(option => option.Name.ToLower().Contains(Pattern.ToLower()));
        if (matchingOption == null)
        {
            Logger.Info($"No option found matching pattern '{Pattern}'");
            return;
        }

        var flag = AsHelpPrinter.GetFlagDescription(matchingOption);
        
        Logger.Info($"SelfModifyingCode.Host - Info for option '{matchingOption.Name}'");
        if (matchingOption.Description != null)
        {
            Logger.Info($"  {matchingOption.Description}");
        }
        Logger.Info("");
        Logger.Info($"  flag:       {flag}");
        Logger.Info($"  mandatory:  {matchingOption.IsMandatory}");

        if (matchingOption.SampleUsage != null)
        {
            Logger.Info("");
            Logger.Info($"Usage: {matchingOption.SampleUsage}");
        }
    }
}