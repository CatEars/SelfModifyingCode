using System.Diagnostics;

namespace SelfModifyingCode.Host.Application;

public class ProgramRunner
{
    private ProcessStartInfo ProcessStartInfo { get; }

    private Process? Process { get; set; }

    public bool HasStopped { get; set; }

    public Task ActiveTask { get; private set; } = Task.CompletedTask;
    
    private bool HasStartedOnce { get; set; }
    
    public ProgramRunner(ProcessStartInfo processStartInfo)
    {
        ProcessStartInfo = processStartInfo;
    }

    public void StartAsync()
    {
        if (HasStartedOnce)
        {
            throw new SmcException($"Tried to run program {ProcessStartInfo.FileName} for a second time. Aborting.");
        }

        HasStartedOnce = true;
        ActiveTask = Task.Run(RunUntilQuit);
    }
    
    public void RunUntilQuit()
    {
        Process = Process.Start(ProcessStartInfo);
        AppDomain.CurrentDomain.ProcessExit += (_, _) =>
        {
            Stop();
        };

        while (Process is { HasExited: false })
        {
            Thread.Sleep(500);            
        }
    }

    public void Stop()
    {
        Process?.Kill();
        HasStopped = true;
    }
    
    public static ProgramRunner CreateRunnerFromManifest(ISelfModifyingCodeManifest realManifest)
    {
        var exeLocation = realManifest.GetExeLocator().GetExeFileLocation();
        var exeDirectory = Path.GetDirectoryName(exeLocation);
        var workingDirectory = exeDirectory!;
        var arguments = string.Join(" ", realManifest.ProgramArguments);
        var processOptions = new ProcessStartInfo()
        {
            FileName = exeLocation,
            WorkingDirectory = workingDirectory,
            Arguments = arguments,
            UseShellExecute = false
        };
        return new ProgramRunner(processOptions);
    }

    
}