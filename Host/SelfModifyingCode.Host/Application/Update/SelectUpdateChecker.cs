namespace SelfModifyingCode.Host.Application.Update;

public static class SelectUpdateChecker
{

    public static IUpdateChecker Get(Uri location)
    {
        if (location.Scheme == "file")
        {
            return new FileUpdateChecker();
        }
        else
        {
            throw new ArgumentException($"Could not create update checker for URI {location}");
        }
    }
    
}