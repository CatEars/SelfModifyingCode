namespace SelfModifyingCode.Server.Directory;

public record PathType
{

    private PathType() {}

    public record LocalFile(string Path) : PathType;

    public record WebUrl(Uri Uri) : PathType;

}