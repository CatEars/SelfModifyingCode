using System.Text.Json;
using System.Web;
using SelfModifyingCode.Server.DataContract;

namespace SelfModifyingCode.WebClient;

public class SelfModifyingCodeWebClient
{

    private Uri ServerLocation { get; }

    private HttpClient HttpClient { get; } = new();
    
    public SelfModifyingCodeWebClient(Uri serverLocation)
    {
        ServerLocation = serverLocation;
    }

    private Uri GetUriForPath(string path)
    {
        var uriBuilder = new UriBuilder();
        uriBuilder.Scheme = ServerLocation.Scheme;
        uriBuilder.Host = ServerLocation.Host;
        uriBuilder.Port = ServerLocation.Port;
        uriBuilder.Path = path;
        return uriBuilder.Uri;
    }
    
    public Task<ApiProgramDirectory> GetDirectory()
    {
        var uri = GetUriForPath("api/Program");
        return Get<ApiProgramDirectory>(uri);
    }

    public Task<ApiProgramInformation> GetProgram(string programName)
    {
        var encoded = HttpUtility.UrlEncode(programName);
        var path = GetUriForPath($"api/Program/{encoded}");
        return Get<ApiProgramInformation>(path);
    }

    public async Task DownloadProgram(string programId, string targetPath)
    {
        var path = GetUriForPath($"api/Download/{programId}");
        var response = await HttpClient.GetAsync(path);
        response.EnsureSuccessStatusCode();
        
        await using var fileStream = await response.Content.ReadAsStreamAsync();
        await using var fileHandle = File.Open(targetPath, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None);

        fileStream.Seek(0, SeekOrigin.Begin);
        await fileStream.CopyToAsync(fileHandle);
    }
    
    private async Task<TReturnType> Get<TReturnType>(Uri uri)
    {
        using var response = await HttpClient.GetAsync(uri);
        if (response.IsSuccessStatusCode &&
            response.Content.Headers.ContentType?.MediaType == "application/json")
        {
            var result = await response.Content.ReadAsStringAsync();
            var obj = JsonSerializer.Deserialize<TReturnType>(result, new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase 
            });
            if (obj == null)
            {
                throw new SmcException($"Could not deserialize response from server at '{uri}'");
            }

            return obj;
        }
        else
        {
            throw new SmcException($"Got response from server at '{uri}' that did not " +
                                   "match what was expected");
        }
    }
    
}