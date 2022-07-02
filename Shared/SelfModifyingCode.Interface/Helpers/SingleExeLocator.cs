using System.Reflection;

namespace SelfModifyingCode.Interface.Helpers;

public class SingleExeLocator<TManifest> : IExeFileLocator
    where TManifest : ISelfModifyingCodeManifest
{
    private ExeDirectory RelativePathFromAssembly { get; }
    
    private Assembly BaseAssembly { get; }

    public SingleExeLocator(ExeDirectory relativePathFromAssembly)
    {
        BaseAssembly = typeof(TManifest).Assembly;
        RelativePathFromAssembly = relativePathFromAssembly;
    }
    
    public string GetExeFileLocation()
    {
        var basePath = Path.GetDirectoryName(BaseAssembly.Location)!;
        var exeFolder = Path.Combine(basePath, RelativePathFromAssembly.RelativePath);
        var entries = Directory.EnumerateFiles(exeFolder);
        var exeFiles = entries
            .Where(entry => entry.ToLower().EndsWith(".exe"))
            .ToList();
        if (exeFiles.Count == 0)
        {
            var manifestName = typeof(TManifest).FullName ?? typeof(TManifest).Name;
            throw new SmcException($"No executable for '{manifestName}' found in directory '{exeFolder}'");
        }

        if (exeFiles.Count > 1)
        {
            var manifestName = typeof(TManifest).FullName ?? typeof(TManifest).Name;
            var names = string.Join(", ", exeFiles);
            throw new SmcException($"Found multiple executables for '{manifestName}' in '{exeFolder}': {names}");
        }

        return exeFiles[0];
    }

    public static SingleExeLocator<TManifest> FromRelativePath(string relativePath)
    {
        return new SingleExeLocator<TManifest>(new ExeDirectory(relativePath));
    }
}