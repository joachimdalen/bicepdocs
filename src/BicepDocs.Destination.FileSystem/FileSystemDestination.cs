using System.Collections.Immutable;
using LandingZones.Tools.BicepDocs.Core.Abstractions;
using LandingZones.Tools.BicepDocs.Core.Models.Destination;
using LandingZones.Tools.BicepDocs.Core.Models.Formatting;
using Microsoft.Extensions.Logging;

namespace LandingZones.Tools.BicepDocs.Destination.FileSystem;

public class FileSystemDestination : IDocsDestination
{
    private readonly ILogger<FileSystemDestination> _logger;
    private readonly IStaticFileSystem _staticFileSystem;

    public FileSystemDestination(ILogger<FileSystemDestination> logger, IStaticFileSystem staticFileSystem)
    {
        _logger = logger;
        _staticFileSystem = staticFileSystem;
    }

    public DocDestination Destination => DocDestination.FileSystem;

    public async Task Write(IImmutableList<GenerationFile> generationFiles)
    {
        /*
         if (!_staticFileSystem.Directory.Exists(Out) && !DryRun)
        {
            _staticFileSystem.Directory.CreateDirectory(Out);
        }
         */
        
        foreach (var outFile in generationFiles)
        {
            //_logger.LogInformation("Processed file  {FileName} ==> {OutPath}", paths.VirtualPath, Path.GetRelativePath(Out, outFile.FilePath));

            switch (outFile)
            {
                case TextGenerationFile txtFile:
                {
                    await WriteFile(txtFile.FolderPath, txtFile.FilePath, txtFile.Content);
                    if (!string.IsNullOrEmpty(txtFile.VersionFilePath) &&
                        !string.IsNullOrEmpty(txtFile.VersionFolderPath))
                    {
                        await WriteFile(txtFile.VersionFolderPath, txtFile.VersionFilePath,
                            txtFile.Content);
                    }

                    break;
                }
                default:
                    throw new ArgumentException($"{outFile.GetType()} is not supported. Convert the type");
            }
        }
    }

    private async Task WriteFile(string folderPath, string filePath, string content)
    {
        if (!_staticFileSystem.Directory.Exists(folderPath))
        {
            _staticFileSystem.Directory.CreateDirectory(folderPath);
        }

        await _staticFileSystem.File.WriteAllTextAsync(filePath, content);
    }
}