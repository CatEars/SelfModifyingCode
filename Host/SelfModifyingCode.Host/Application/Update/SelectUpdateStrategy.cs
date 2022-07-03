using SelfModifyingCode.Interface;

namespace SelfModifyingCode.Host.Application.Update;

public static class SelectUpdateStrategy
{

    public static IUpdateStrategy Get(RestartType restartType, AppDeployer deployer)
    {
        return restartType switch
        {
            RestartType.StopThenStart => new StopAndStartUpdater(deployer),
            _ => throw new ArgumentException($"Update strategy of type {restartType.GetType().Name} is unknown")
        };
    }
    
}