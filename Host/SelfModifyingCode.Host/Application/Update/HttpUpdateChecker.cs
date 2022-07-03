namespace SelfModifyingCode.Host.Application.Update;

public class HttpUpdateChecker : IUpdateChecker
{
    private HttpClient HttpClient { get; } = new ();
    
    public Task<bool> UpdateExists(ApplicationRunInfo applicationRunInfo, TimeSpan timeout)
    {
        throw new NotImplementedException();
    }

    public Task DownloadLatestProgram(ISelfModifyingCodeManifest manifest, string destination)
    {
        throw new NotImplementedException();
    }
}