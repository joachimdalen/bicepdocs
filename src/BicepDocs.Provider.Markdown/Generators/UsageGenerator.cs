using System.Collections.Immutable;
using LandingZones.Tools.BicepDocs.Core.Models;
using LandingZones.Tools.BicepDocs.Provider.Markdown.Elements;

namespace LandingZones.Tools.BicepDocs.Provider.Markdown.Generators;

internal static class UsageGenerator
{
    internal static void BuildUsage(MarkdownDocument document, GeneratorOptions options,
        ImmutableList<ParsedParameter> parameters)
    {
        if (!options.IncludeUsage) return;
        document.Append(new MkHeader("Usage", MkHeaderLevel.H2));
        document.Append(new MkCodeBlock(
            CodeGenerator.GetExample("exampleInstance", "br/MyRegistry:bicep/modules/customModule", "2022-10-29",
                parameters), "bicep"));
    }
}