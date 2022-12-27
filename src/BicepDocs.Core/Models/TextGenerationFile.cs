namespace LandingZones.Tools.BicepDocs.Core.Models;

public class TextGenerationFile : GenerationFile
{
    public string Content { get; set; }

    public TextGenerationFile(string filePath, string content) : base(filePath)
    {
        Content = content;
    }
}