using System.IO.Compression;
using System.Reflection;
using System.Security.Cryptography;
using SelfModifyingCode.Interface;

namespace SelfModifyingCode.Host.ProgramDirectory;

public class ManifestReader
{

    private string ProgramPath { get; }
    
    public ManifestReader(string programPath)
    {
        ProgramPath = programPath;
    }

    public ISelfModifyingCodeManifest ReadProgramManifest()
    {
        var hash = SHA256Helper.GetFileSHA256Hex(ProgramPath);
        var tempDirectory = Path.GetTempPath();
        var tempUnpackDirectory = Path.Combine(tempDirectory, hash);
        
        if (!Directory.Exists(tempUnpackDirectory))
        {
            Directory.CreateDirectory(tempUnpackDirectory);
            UnpackManifestInto(tempUnpackDirectory);
        }

        return InitializeManifestFromTempDirectory(tempUnpackDirectory);
    }

    private ISelfModifyingCodeManifest InitializeManifestFromTempDirectory(string tempUnpackDirectory)
    {
        var files = Directory.GetFiles(tempUnpackDirectory);
        var manifestFiles = files
            .Select(Path.GetFileName)
            .Where(x => x != null)
            .Select(f => f!) // convert away null string
            .Where(file => file.EndsWith(".dll") && file.Contains("SMCManifest"))
            .ToList();
        ThrowIfNoManifestFile(tempUnpackDirectory, manifestFiles);
        ThrowIfMultipleManifestFiles(tempUnpackDirectory, manifestFiles);
        var fullPath = Path.Combine(tempUnpackDirectory, manifestFiles[0]);
        return LoadManifestFromDll(fullPath);
    }

    private ISelfModifyingCodeManifest LoadManifestFromDll(string manifestFile)
    {
        var assembly = Assembly.LoadFile(manifestFile);
        var types = assembly.GetTypes();
        var expectedManifestType = typeof(ISelfModifyingCodeManifest);
        var manifestTypes = types
            .Where(type => expectedManifestType.IsAssignableFrom(type))
            .ToList();
        ThrowIfNoTypesImplementingManifest(manifestTypes);
        ThrowIfMultipleTypesImplementingManifest(manifestTypes);
        return (ISelfModifyingCodeManifest)Activator.CreateInstance(manifestTypes.First())!;
    }

    private void ThrowIfMultipleTypesImplementingManifest(List<Type> manifestTypes)
    {
        if (manifestTypes.Count > 1)
        {
            var typeNames = manifestTypes.Select(type => type.Name);
            var names = string.Join(", ", typeNames);
            throw new SmcException($"Found multiple manifests for program {ProgramPath}: {names}");
        }
    }

    private void ThrowIfNoTypesImplementingManifest(List<Type> manifestTypes)
    {
        if (manifestTypes.Count == 0)
        {
            var manifestName = nameof(ISelfModifyingCodeManifest);
            throw new SmcException($"Found no manifests implementing `{manifestName}` in program {ProgramPath}");
        }
    }

    private static void ThrowIfMultipleManifestFiles(string tempUnpackDirectory, List<string> manifestFiles)
    {
        if (manifestFiles.Count > 1)
        {
            var manifests = string.Join(", ", manifestFiles);
            throw new SmcException($"Expected only one manifest file but found {manifestFiles.Count} of them" +
                                   $"in temporary unpack folder '{tempUnpackDirectory}': {manifests}");
        }
    }

    private void ThrowIfNoManifestFile(string tempUnpackDirectory, List<string> manifestFile)
    {
        if (manifestFile.Count == 0)
        {
            throw new SmcException($"Tried to find manifest file for '{ProgramPath}', but none were" +
                                   $" found in temporary unpacked folder '{tempUnpackDirectory}'");
        }
    }

    private void UnpackManifestInto(string tempUnpackDirectory)
    {
        ZipFile.ExtractToDirectory(ProgramPath, tempUnpackDirectory);
    }
}