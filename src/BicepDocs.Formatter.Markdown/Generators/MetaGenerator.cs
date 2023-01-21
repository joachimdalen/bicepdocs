using LandingZones.Tools.BicepDocs.Core;
using LandingZones.Tools.BicepDocs.Core.Models.Formatting;
using LandingZones.Tools.BicepDocs.Formatter.Markdown.Elements;

namespace LandingZones.Tools.BicepDocs.Formatter.Markdown.Generators;

internal static class MetaGenerator
{
    internal static void BuildTitle(MarkdownDocument document, MetadataModel? metadata,
        FormatterContext formatterContext)
    {
        document.Append(!string.IsNullOrEmpty(metadata?.Title)
            ? new MkHeader(metadata.Title, MkHeaderLevel.H1)
            : new MkHeader(Path.GetFileNameWithoutExtension(formatterContext.ModulePath), MkHeaderLevel.H1));
    }

    internal static void BuildDescription(MarkdownDocument document, MetadataModel? metadata)
    {
        if (!string.IsNullOrEmpty(metadata?.Description))
            document.Append(new MkBlockQuote(metadata.Description));
    }
}