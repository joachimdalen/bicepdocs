namespace LandingZones.Tools.BicepDocs.Core.Models.Formatting;

public class TextGenerationFile : GenerationFile
{
    public string Content { get; }

    public TextGenerationFile(string filePath, string content, string? versionPath = null) : base(filePath, versionPath)
    {
        Content = content;
    }
}