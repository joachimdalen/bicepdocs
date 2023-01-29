using LandingZones.Tools.BicepDocs.Core;
using LandingZones.Tools.BicepDocs.Core.Models.Formatting;
using LandingZones.Tools.BicepDocs.Formatter.Confluence.Elements;

namespace LandingZones.Tools.BicepDocs.Formatter.Confluence.Generators;

internal static class MetaGenerator
{
    internal static string BuildTitle(MetadataModel? metadata,
        FormatterContext formatterContext)
    {
        return !string.IsNullOrEmpty(metadata?.Title)
            ? metadata.Title
            : Path.GetFileNameWithoutExtension(formatterContext.ModulePath);
    }

    internal static void BuildDescription(ConfluenceDocument document, MetadataModel? metadata)
    {
        if (!string.IsNullOrEmpty(metadata?.Description))
            document.Append(new CBlockQuote(metadata.Description));
    }
}