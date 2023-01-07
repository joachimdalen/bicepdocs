using System.Collections.Immutable;
using LandingZones.Tools.BicepDocs.Core;
using LandingZones.Tools.BicepDocs.Core.Models.Parsing;
using LandingZones.Tools.BicepDocs.Core.Parsers;
using LandingZones.Tools.BicepDocs.Formatter.Markdown.Elements;
using LandingZones.Tools.BicepDocs.Formatter.Markdown.Extensions;

namespace LandingZones.Tools.BicepDocs.Formatter.Markdown.Generators;

internal static class OutputGenerator
{
    internal static void BuildOutputs(MarkdownDocument document, IImmutableList<ParsedOutput> outputs)
    {
        document.Append(new MkHeader("Outputs", MkHeaderLevel.H2));
        var outPutTable = new MkTable().AddColumn("Name").AddColumn("Type").AddColumn("Description");
        foreach (var output in outputs)
        {
            outPutTable.AddRow(output.Name.WrapInBackticks(), output.Type, output.Description ?? "");
        }

        document.Append(outPutTable);
    }

    internal static void BuildOutputs(MarkdownDocument document, FormatterContext context)
    {
        if (!context.FormatterOptions.IncludeOutputs) return;
        var outputs = OutputParser.ParseOutputs(context.Template);
        if (!outputs.Any()) return;
        BuildOutputs(document, outputs);
    }
}