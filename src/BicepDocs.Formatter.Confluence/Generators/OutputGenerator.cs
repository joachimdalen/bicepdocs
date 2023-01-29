using System.Collections.Immutable;
using LandingZones.Tools.BicepDocs.Core;
using LandingZones.Tools.BicepDocs.Core.Models.Parsing;
using LandingZones.Tools.BicepDocs.Core.Parsers;
using LandingZones.Tools.BicepDocs.Formatter.Confluence.Elements;
using LandingZones.Tools.BicepDocs.Formatter.Confluence.Extensions;

namespace LandingZones.Tools.BicepDocs.Formatter.Confluence.Generators;

internal static class OutputGenerator
{
    internal static void BuildOutputs(ConfluenceDocument document, IImmutableList<ParsedOutput> outputs)
    {
        document.Append(new CHeader("Outputs", CHeaderLevel.H2));
        var outPutTable = new CTable().AddColumn("Name").AddColumn("Type").AddColumn("Description");
        foreach (var output in outputs)
        {
            outPutTable.AddRow(output.Name.WrapInCodeBlock(), output.Type, output.Description ?? "");
        }

        document.Append(outPutTable);
    }

    internal static void BuildOutputs(ConfluenceDocument document, FormatterContext context)
    {
        if (!context.FormatterOptions.IncludeOutputs) return;
        var outputs = OutputParser.ParseOutputs(context.Template);
        if (!outputs.Any()) return;
        BuildOutputs(document, outputs);
    }
}