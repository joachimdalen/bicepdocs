using LandingZones.Tools.BicepDocs.Core;
using LandingZones.Tools.BicepDocs.Core.Models;
using LandingZones.Tools.BicepDocs.Provider.Markdown.Elements;

namespace LandingZones.Tools.BicepDocs.Provider.Markdown.Generators;

internal static class MetaGenerator
{
    internal static void BuildTitle(MarkdownDocument document, MetadataModel? metadata, GeneratorContext generatorContext)
    {
        document.Append(!string.IsNullOrEmpty(metadata?.Title) ? new MkHeader(metadata.Title, MkHeaderLevel.H1) : new MkHeader(generatorContext.Paths.BaseFileName, MkHeaderLevel.H1));
    }

    internal static void BuildDescription(MarkdownDocument document, MetadataModel? metadata)
    {
        if (!string.IsNullOrEmpty(metadata?.Description))
            document.Append(new MkBlockQuote(metadata.Description));
    }
}