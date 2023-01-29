using System.Collections.Immutable;
using LandingZones.Tools.BicepDocs.Core;
using LandingZones.Tools.BicepDocs.Core.Models;
using LandingZones.Tools.BicepDocs.Core.Models.Formatting;
using LandingZones.Tools.BicepDocs.Core.Models.Parsing;
using LandingZones.Tools.BicepDocs.Formatter.Markdown.Elements;
using LandingZones.Tools.BicepDocs.Formatter.Markdown.Models;

namespace LandingZones.Tools.BicepDocs.Formatter.Markdown.Generators;

internal static class UsageGenerator
{
    internal static void BuildUsage(
        MarkdownDocument document,
        FormatterOptions options,
        ImmutableList<ParsedParameter> parameters,
        string modulePath,
        string moduleVersion)
    {
        if (!options.IncludeUsage) return;
        document.Append(new MkHeader("Usage", MkHeaderLevel.H2));
        var usage = CodeGenerator.GetBicepExample(
            moduleName: "exampleInstance",
            moduleAlias: options.Usage.ModuleAlias,
            moduleType: options.Usage.ModuleType == ModuleUsageType.Local ? "ts" : "br",
            path: modulePath.TrimStart('/'),
            moduleVersion,
            parameters);
        document.Append(new MkCodeBlock(
            usage, "bicep"));
    }
}