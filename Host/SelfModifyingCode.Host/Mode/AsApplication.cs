using SelfModifyingCode.Host.CommandLine;

namespace SelfModifyingCode.Host.Mode;

public class AsApplication : IExecution
{
    private CommandLineOptions Options { get; }
 
    public AsApplication(CommandLineOptions options)
    {
        Options = options;
    }

    public void Run()
    {
        new Application.Application(Options).Run().Wait();
    }
}