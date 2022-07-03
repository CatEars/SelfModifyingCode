using SelfModifyingCode.Server.DataContract;
using SelfModifyingCode.WebClient;

namespace SelfModifyingCode.Host.Application.Update;

public class HttpUpdateChecker : IUpdateChecker
{
    private SelfModifyingCodeWebClient WebClient { get; }

    public HttpUpdateChecker(Uri serverLocation)
    {
        WebClient = new SelfModifyingCodeWebClient(serverLocation);
    }
    
    public async Task<bool> UpdateExists(ApplicationRunInfo applicationRunInfo, TimeSpan _)
    {
        var directory = await WebClient.GetDirectory();
        var latestVersion = GetLatestVersion(directory.AvailablePrograms, applicationRunInfo.Manifest.ProgramId.FullName);
        if (latestVersion == null)
        {
            return false;
        }

        return latestVersion > applicationRunInfo.Manifest.ProgramId.Version;
    }

    private static Version? GetLatestVersion(IReadOnlyList<ApiProgramInformation> programs, string name)
    {
        var latestVersion = programs
            .Where(program => program.Name == name)
            .Max(program => Version.Parse(program.Version));
        return latestVersion;
    }

    public async Task DownloadLatestProgram(ISelfModifyingCodeManifest manifest, string destination)
    {
        var directory = await WebClient.GetDirectory();
        var latestVersion = GetLatestVersion(directory.AvailablePrograms, manifest.ProgramId.FullName);
        if (latestVersion == null)
        {
            throw new SmcException($"Could not find latest version of program for {manifest.DisplayName}");
        }

        var latestProgram = directory.AvailablePrograms.First(
            program => program.Name == manifest.ProgramId.FullName && 
                       Version.Parse(program.Version) == latestVersion);

        var tempPath = Path.GetTempFileName();
        await WebClient.DownloadProgram(latestProgram.Identity.Identity, tempPath);
        File.Replace(tempPath, destination, null);
    }
}