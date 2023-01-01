using System.Collections.Immutable;
using LandingZones.Tools.BicepDocs.Core.Models.Formatting;
using LandingZones.Tools.BicepDocs.Core.Models.Parsing;
using LandingZones.Tools.BicepDocs.Formatter.Markdown.Elements;

namespace LandingZones.Tools.BicepDocs.Formatter.Markdown.Generators;

internal static class UsageGenerator
{
    internal static void BuildUsage(MarkdownDocument document, FormatterOptions options,
        ImmutableList<ParsedParameter> parameters)
    {
        if (!options.IncludeUsage) return;
        document.Append(new MkHeader("Usage", MkHeaderLevel.H2));
        document.Append(new MkCodeBlock(
            CodeGenerator.GetExample("exampleInstance", "br/MyRegistry:bicep/modules/customModule", "2022-10-29",
                parameters), "bicep"));
    }
}