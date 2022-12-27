using System.Collections.Immutable;
using LandingZones.Tools.BicepDocs.Core;
using LandingZones.Tools.BicepDocs.Core.Models;
using LandingZones.Tools.BicepDocs.Core.Parsers;
using LandingZones.Tools.BicepDocs.Provider.Markdown.Elements;

namespace LandingZones.Tools.BicepDocs.Provider.Markdown.Generators;

internal static class ResourceGenerator
{
    internal static void BuildResources(MarkdownDocument document, GeneratorContext context, IImmutableList<ParsedResource> resources)
    {
        document.Append(new MkHeader("Resources", MkHeaderLevel.H2));
        var resourceList = new MkList();
        var processed = new List<string>();
        foreach (var resource in resources.Where(resource => !resource.IsExisting || context.GeneratorOptions.IncludeExistingResources))
        {
            if (processed.Contains(resource.Name))
            {
                continue;
            }

            resourceList.AddListElement(new MkAnchor(resource.Name, resource.DocUrl ?? "#").ToMarkdown());
            processed.Add(resource.Name);
        }

        document.Append(resourceList);
    }

    internal static void BuildResources(MarkdownDocument document, GeneratorContext context)
    {
        if (!context.GeneratorOptions.IncludeResources) return;
        var resources = ResourceParser.ParseResources(context.Template);
        if (!resources.Any()) return;
        BuildResources(document, context, resources);
    }
}