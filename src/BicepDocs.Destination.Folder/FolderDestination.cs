using System.Collections.Immutable;
using LandingZones.Tools.BicepDocs.Core.Abstractions;
using LandingZones.Tools.BicepDocs.Core.Models.Destination;
using LandingZones.Tools.BicepDocs.Core.Models.Formatting;
using Microsoft.Extensions.Logging;

namespace LandingZones.Tools.BicepDocs.Destination.Folder;

public class FolderDestination : IDocsDestination
{
    private readonly ILogger<FolderDestination> _logger;
    private readonly IStaticFileSystem _staticFileSystem;

    public FolderDestination(ILogger<FolderDestination> logger, IStaticFileSystem staticFileSystem)
    {
        _logger = logger;
        _staticFileSystem = staticFileSystem;
    }

    public DocDestination Destination => DocDestination.Folder;
    public bool RequiresInput => true;

    public async Task Write(IImmutableList<GenerationFile> generationFiles, DestinationOptions? options = null)
    {
        if (options is not FolderDestinationOptions folderDestinationOptions)
        {
            throw new Exception("Failed to resolve options");
        }

        foreach (var outFile in generationFiles)
        {
            switch (outFile)
            {
                case TextGenerationFile txtFile:
                {
                    await WriteFile(txtFile.FolderPath, txtFile.FilePath, txtFile.Content, folderDestinationOptions);
                    _logger.LogInformation("Processed file  {FileName}", outFile.FilePath);
                    if (!string.IsNullOrEmpty(txtFile.VersionFilePath) &&
                        !string.IsNullOrEmpty(txtFile.VersionFolderPath))
                    {
                        await WriteFile(txtFile.VersionFolderPath, txtFile.VersionFilePath,
                            txtFile.Content, folderDestinationOptions);
                        _logger.LogInformation("Processed file  {FileName}", outFile.VersionFilePath);
                    }

                    break;
                }
                default:
                    throw new ArgumentException($"{outFile.GetType()} is not supported. Convert the type");
            }
        }
    }

    private async Task WriteFile(string folderPath, string filePath, string content,
        FolderDestinationOptions folderDestinationOptions)
    {
        var intFolderPath = Path.Join(folderDestinationOptions.OutPath, folderPath);
        var intFilePath = Path.Join(folderDestinationOptions.OutPath, filePath);
        if (!_staticFileSystem.Directory.Exists(intFolderPath))
        {
            _staticFileSystem.Directory.CreateDirectory(intFolderPath);
        }

        await _staticFileSystem.File.WriteAllTextAsync(intFilePath, content);
    }
}