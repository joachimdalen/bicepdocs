namespace LandingZones.Tools.BicepDocs.Core.Models;

public abstract class GenerationFile
{
    protected GenerationFile(string filePath, string? versionFilePath = null)
    {
        FilePath = filePath;
        FolderPath = Path.GetDirectoryName(filePath) ?? throw new ArgumentException("Failed to resolve folder path");

        if (!string.IsNullOrEmpty(versionFilePath))
        {
            VersionFilePath = versionFilePath;
            VersionFolderPath = Path.GetDirectoryName(versionFilePath) ??
                                throw new ArgumentException("Failed to resolve folder path");
        }
    }

    /// <summary>
    /// File path where the file should be written
    /// </summary>
    public string FilePath { get; }
    
    /// <summary>
    /// File path where the versioned file should be written
    /// </summary>
    public string? VersionFilePath { get; }

    
    /// <summary>
    /// Base folder of <see cref="FilePath"/>
    /// </summary>
    public string FolderPath { get; }
    
    /// <summary>
    /// Base folder of <see cref="VersionFilePath"/>
    /// </summary>
    public string? VersionFolderPath { get; }
}