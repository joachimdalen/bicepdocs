using System.Collections.Immutable;
using LandingZones.Tools.BicepDocs.Core;
using LandingZones.Tools.BicepDocs.Core.Models;
using LandingZones.Tools.BicepDocs.Core.Parsers;
using LandingZones.Tools.BicepDocs.Provider.Markdown.Elements;

namespace LandingZones.Tools.BicepDocs.Provider.Markdown.Generators;

internal static class OutputGenerator
{
    internal static void BuildOutputs(MarkdownDocument document, IImmutableList<ParsedOutput> outputs)
    {
        document.Append(new MkHeader("Outputs", MkHeaderLevel.H2));
        var outPutTable = new MkTable().AddColumn("Name").AddColumn("Type").AddColumn("Description");
        foreach (var output in outputs)
        {
            outPutTable.AddRow($"`{output.Name}`", output.Type, output.Description ?? "");
        }

        document.Append(outPutTable);
    }

    internal static void BuildOutputs(MarkdownDocument document, GeneratorContext context)
    {
        if (!context.GeneratorOptions.IncludeOutputs) return;
        var outputs = OutputParser.ParseOutputs(context.Template);
        if (!outputs.Any()) return;
        BuildOutputs(document, outputs);
    }
}