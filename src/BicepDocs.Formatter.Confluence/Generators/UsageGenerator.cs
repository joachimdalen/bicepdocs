using System.Collections.Immutable;
using LandingZones.Tools.BicepDocs.Core;
using LandingZones.Tools.BicepDocs.Core.Models.Formatting;
using LandingZones.Tools.BicepDocs.Core.Models.Parsing;
using LandingZones.Tools.BicepDocs.Formatter.Confluence.Elements;

namespace LandingZones.Tools.BicepDocs.Formatter.Confluence.Generators;

internal static class UsageGenerator
{
    internal static void BuildUsage(
        ConfluenceDocument document,
        FormatterOptions options,
        ImmutableList<ParsedParameter> parameters,
        string modulePath,
        string moduleVersion)
    {
        if (!options.IncludeUsage) return;
        document.Append(new CHeader("Usage", CHeaderLevel.H2));
        var usage = CodeGenerator.GetBicepExample(
            moduleName: "exampleInstance",
            moduleAlias: options.Usage.ModuleAlias,
            moduleType: options.Usage.ModuleType == ModuleUsageType.Local ? "ts" : "br",
            path: modulePath.TrimStart('/'),
            moduleVersion,
            parameters);
        document.Append(new CCodeBlock(
            usage, "bicep"));
    }
}