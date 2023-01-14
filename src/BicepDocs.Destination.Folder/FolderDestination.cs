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

    public async Task Write(IImmutableList<GenerationFile> generationFiles)
    {
        foreach (var outFile in generationFiles)
        {
            switch (outFile)
            {
                case TextGenerationFile txtFile:
                {
                    await WriteFile(txtFile.FolderPath, txtFile.FilePath, txtFile.Content);
                    _logger.LogInformation("Processed file  {FileName}", outFile.FilePath);
                    if (!string.IsNullOrEmpty(txtFile.VersionFilePath) &&
                        !string.IsNullOrEmpty(txtFile.VersionFolderPath))
                    {
                        await WriteFile(txtFile.VersionFolderPath, txtFile.VersionFilePath,
                            txtFile.Content);
                        _logger.LogInformation("Processed file  {FileName}", outFile.VersionFilePath);
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