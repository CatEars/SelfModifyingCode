using SelfModifyingCode.Interface;

namespace SelfModifyingCode.Host.Application;

public interface IUpdateChecker
{

    Task<bool> UpdateExists(ISelfModifyingCodeManifest manifest, string currentVersionHash, TimeSpan timeout);

    Task DownloadLatestProgram(ISelfModifyingCodeManifest manifest, string destination);

}