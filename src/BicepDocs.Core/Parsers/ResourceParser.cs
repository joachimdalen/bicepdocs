using System.Collections.Immutable;
using Bicep.Core.Navigation;
using Bicep.Core.Semantics;
using Bicep.Core.Semantics.Metadata;
using LandingZones.Tools.BicepDocs.Core.Models;

namespace LandingZones.Tools.BicepDocs.Core.Parsers;

public static class ResourceParser
{
    public static ImmutableList<ParsedResource> ParseResources(SemanticModel model)
    {
        var resources = new List<ParsedResource>();

        foreach (var resource in model.AllResources)
        {
            var provider = resource.TypeReference.TypeSegments.First();
            var resourceType = string.Join("/", resource.TypeReference.TypeSegments.Skip(1));
            var parsedResource = new ParsedResource(resource.Type.Name, provider, resourceType)
            {
                Name = (resource as DeclaredResourceMetadata)?.TryGetNameSyntax()?.ToTextPreserveFormatting(),
                IsExisting = resource.IsExistingResource,
                ApiVersion = resource.TypeReference.ApiVersion,
                DocUrl = ResourceLinkBuilder.GetResourceUrl(resource.TypeReference)
            };

            if (!resources.Any(x => x.Identifier == parsedResource.Identifier && x.ApiVersion == parsedResource.ApiVersion && !x.IsExisting))
            {
                resources.Add(parsedResource);
            }
        }

        return resources.ToImmutableList();
    }
}