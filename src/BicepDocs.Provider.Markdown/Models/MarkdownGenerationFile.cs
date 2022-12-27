using Bicep.Core.Semantics;
using LandingZones.Tools.BicepDocs.Core.Models;
using LandingZones.Tools.BicepDocs.Provider.Markdown.Elements;

namespace LandingZones.Tools.BicepDocs.Provider.Markdown.Models;

public class MarkdownGenerationFile : GenerationFile
{
    /// <summary>
    /// Markdown document containing the elements that should be
    /// written to the file
    /// </summary>
    public MarkdownDocument Document { get; }

    /// <summary>
    /// The template model the document was generated from
    /// </summary>
    public SemanticModel Model { get; }

    public MarkdownGenerationFile(string filePath, MarkdownDocument document, SemanticModel model,
        string? versionFilePath = null) : base(filePath, versionFilePath)
    {
        Document = document;
        Model = model;
    }
}