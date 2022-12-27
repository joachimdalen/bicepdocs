using System.Collections.Immutable;
using Bicep.Core.Semantics;
using LandingZones.Tools.BicepDocs.Core.Models;

namespace LandingZones.Tools.BicepDocs.Core.Parsers;

public class ResourceParser
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
                IsExisting = resource.IsExistingResource,
                ApiVersion = resource.TypeReference.ApiVersion,
                DocUrl = ResourceLinkBuilder.GetResourceUrl(resource.TypeReference)
            };

            if (!resources.Any(x => x.Name == parsedResource.Name && x.ApiVersion == parsedResource.ApiVersion))
            {
                resources.Add(parsedResource);
            }
        }

        return resources.ToImmutableList();
    }
}