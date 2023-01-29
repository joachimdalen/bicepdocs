using LandingZones.Tools.BicepDocs.Core.Models.Formatting;

namespace LandingZones.Tools.BicepDocs.Formatter.Confluence.Models;

public class ConfluenceGenerationFile : GenerationFile
{
    public string Title { get; set; }

    public ConfluenceGenerationFile(string title, string filePath, string? versionFilePath = null) : base(filePath,
        versionFilePath)
    {
        Title = title;
    }
}