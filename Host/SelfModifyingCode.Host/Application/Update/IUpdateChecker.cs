namespace SelfModifyingCode.Host.Application.Update;

public interface IUpdateChecker
{

    Task<bool> UpdateExists(ApplicationRunInfo applicationRunInfo, TimeSpan timeout);

    Task DownloadLatestProgram(ISelfModifyingCodeManifest manifest, string destination);

}