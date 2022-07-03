namespace SelfModifyingCode.Host.Application.Update;

public interface IUpdateStrategy
{

    Task OnStartup(ISelfModifyingCodeManifest manifest);

    Task<ApplicationRunInfo> OnNewUpdateFound(ApplicationRunInfo runInfo, IUpdateChecker checker);

}