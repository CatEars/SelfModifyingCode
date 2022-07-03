namespace SelfModifyingCode.Host.Application.Update;

public static class SelectUpdateChecker
{

    public static IUpdateChecker Get(Uri location)
    {
        return location.Scheme switch
        {
            "file" => new FileUpdateChecker(),
            "http" or "https" => new HttpUpdateChecker(),
            _ => throw new ArgumentException($"Could not create update checker for URI {location}")
        };
    }
    
}