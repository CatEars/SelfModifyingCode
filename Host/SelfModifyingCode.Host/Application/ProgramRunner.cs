using System.Diagnostics;
using SelfModifyingCode.Interface;

namespace SelfModifyingCode.Host.Application;

public class ProgramRunner
{
    private ProcessStartInfo ProcessStartInfo { get; }

    private Process? Process { get; set; }
    
    public ProgramRunner(ProcessStartInfo processStartInfo)
    {
        ProcessStartInfo = processStartInfo;
    }

    public void RunUntilQuit()
    {
        Process = Process.Start(ProcessStartInfo);
        AppDomain.CurrentDomain.ProcessExit += (_, _) =>
        {
            Process?.Close();
        };

        while (Process is { HasExited: false })
        {
            Thread.Sleep(500);            
        }
    }

    public void Stop()
    {
        Process?.Kill();
    }
    
}