using SelfModifyingCode.Common;
using SelfModifyingCode.Common.Manifest;
using SelfModifyingCode.Common.ProgramRoot;
using SelfModifyingCode.Common.Unpacking;
using SelfModifyingCode.Server.Directory;

namespace SelfModifyingCode.Server.Infrastructure.LocalFiles;

public class DirectoryWatcherRegistrator : BackgroundService
{
    private WatchDirectory WatchDirectory { get; }

    private InMemoryPathRepository PathRepository { get; }
    
    private InMemoryProgramRepository ProgramRepository { get; }

    private HashSet<string> RegisteredProgramIds { get; } = new();

    private record FileCombo(string FileName, string HashIdentity);
    
    public DirectoryWatcherRegistrator(WatchDirectory watchDirectory, 
        InMemoryPathRepository pathRepository,
        InMemoryProgramRepository programRepository)
    {
        WatchDirectory = watchDirectory;
        PathRepository = pathRepository;
        ProgramRepository = programRepository;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            if (!System.IO.Directory.Exists(WatchDirectory.Value))
            {
                System.IO.Directory.CreateDirectory(WatchDirectory.Value);
            }
            var files = System.IO.Directory.EnumerateFiles(WatchDirectory.Value)
                .ToList();
            var unregisteredFiles = files.Select(file => new FileCombo(file, SHA256Helper.GetFileSHA256Hex(file)))
                .Where(combo => !RegisteredProgramIds.Contains(combo.HashIdentity))
                .ToList();
            foreach (var unregisteredFile in unregisteredFiles)
            {
                PathRepository.RegisterLocalFile(unregisteredFile.HashIdentity, unregisteredFile.FileName);
                var temporaryRoot = new TemporaryRoot(unregisteredFile.FileName);
                var unpacker = new Unpacker(unregisteredFile.FileName, temporaryRoot);
                unpacker.Unpack();
                var manifestReader = new ManifestReader(unregisteredFile.FileName, temporaryRoot);
                var manifest = manifestReader.ReadProgramManifest();
                var program = new Directory.Program(
                    manifest.ProgramId,
                    ProgramIdentity.NewSha256Identity(unregisteredFile.HashIdentity), 
                    unregisteredFile.FileName,
                    $"api/Download/{unregisteredFile.HashIdentity}");
                ProgramRepository.RegisterProgram(program);
                RegisteredProgramIds.Add(unregisteredFile.HashIdentity);
            }
            await Task.Delay(2000, stoppingToken);
        }
    }
}