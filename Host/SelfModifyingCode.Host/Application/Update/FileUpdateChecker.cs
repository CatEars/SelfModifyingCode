using SelfModifyingCode.Common;

namespace SelfModifyingCode.Host.Application.Update;

public class FileUpdateChecker : IUpdateChecker
{
    public Task<bool> UpdateExists(ApplicationRunInfo appRunInfo, TimeSpan _)
    {
        var manifest = appRunInfo.Manifest;
        var location = GetProgramLocation(manifest);
        if (!File.Exists(location))
        {
            return Task.FromResult(false);
        }
        var identity = SHA256Helper.GetFileSHA256Hex(location);
        return Task.FromResult(identity != appRunInfo.Identity);
    }

    private static string GetProgramLocation(ISelfModifyingCodeManifest manifest)
    {
        var fileLocation = manifest.GloballyKnownDownloadLocation;
        if (fileLocation.Scheme != "file")
        {
            throw new ArgumentException(nameof(FileUpdateChecker) + " cannot check any URI that is not a file://");
        }

        return fileLocation.ToString().Replace("file:///", "");
    }

    public Task DownloadLatestProgram(ISelfModifyingCodeManifest manifest, string destination)
    {
        var location = GetProgramLocation(manifest);
        File.Copy(location, destination, true);
        return Task.CompletedTask;
    }
}