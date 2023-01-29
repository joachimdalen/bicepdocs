using System.Collections.Immutable;
using LandingZones.Tools.BicepDocs.Core;
using LandingZones.Tools.BicepDocs.Core.Models.Parsing;
using LandingZones.Tools.BicepDocs.Core.Parsers;
using LandingZones.Tools.BicepDocs.Formatter.Confluence.Elements;
using LandingZones.Tools.BicepDocs.Formatter.Confluence.Extensions;

namespace LandingZones.Tools.BicepDocs.Formatter.Confluence.Generators;

internal static class ResourceGenerator
{
    internal static void BuildResources(ConfluenceDocument document, FormatterContext context,
        IImmutableList<ParsedResource> resources)
    {
        document.Append(new CHeader("Resources", CHeaderLevel.H2));
        var resourceList = new CList();
        var processed = new List<string>();
        foreach (var resource in resources.Where(resource =>
                     !resource.IsExisting || context.FormatterOptions.IncludeExistingResources))
        {
            if (processed.Contains(resource.Identifier))
            {
                continue;
            }

            resourceList.AddListElement(new CAnchor(resource.Identifier, resource.DocUrl ?? "#").ToMarkup());
            processed.Add(resource.Identifier);
        }

        document.Append(resourceList);
    }

    internal static void BuildResources(ConfluenceDocument document, FormatterContext context)
    {
        if (!context.FormatterOptions.IncludeResources) return;
        var resources = ResourceParser.ParseResources(context.Template);
        if (!resources.Any()) return;
        BuildResources(document, context, resources);
    }

    internal static void BuildReferencedResources(ConfluenceDocument document, FormatterContext context)
    {
        if (!context.FormatterOptions.IncludeReferencedResources) return;
        var resources = ResourceParser.ParseResources(context.Template);
        if (!resources.Any(x => x.IsExisting)) return;
        BuildReferencedResources(document, resources);
    }

    internal static void BuildReferencedResources(ConfluenceDocument document, IImmutableList<ParsedResource> resources)
    {
        document.Append(new CHeader("Referenced Resources", CHeaderLevel.H2));
        var resourceTable = new CTable().AddColumn("Provider").AddColumn("Name").AddColumn("Scope");
        foreach (var resource in resources.Where(resource => resource.IsExisting))
        {
            resourceTable.AddRow(resource.Identifier, resource.Name?.WrapInCodeBlock() ?? "-",
                !string.IsNullOrEmpty(resource.Scope) ? resource.Scope.WrapInCodeBlock() : "-");
        }

        document.Append(resourceTable);
    }
}