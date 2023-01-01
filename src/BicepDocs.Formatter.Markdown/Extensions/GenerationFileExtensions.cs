using LandingZones.Tools.BicepDocs.Core.Models.Formatting;
using LandingZones.Tools.BicepDocs.Formatter.Markdown.Models;

namespace LandingZones.Tools.BicepDocs.Formatter.Markdown.Extensions;

public static class GenerationFileExtensions
{
    public static TextGenerationFile ToTextGenerationFile(this MarkdownGenerationFile generationFile)
    {
        return new TextGenerationFile(generationFile.FilePath, generationFile.Document.ToMarkdown(),
            generationFile.VersionFilePath);
    }
}