using Bicep.Core.Resources;

namespace LandingZones.Tools.BicepDocs.Core;

public static class ResourceLinkBuilder
{
    private const string BaseUrl = "https://learn.microsoft.com/en-us/azure/templates/{0}/{1}/{2}";

    public static string GetResourceUrl(ResourceTypeReference resourceType)
    {
        var provider = resourceType.TypeSegments.First();
        var apiVersion = resourceType.ApiVersion;
        var pathElements = resourceType.TypeSegments.Skip(1);
        return string.Format(BaseUrl, provider, apiVersion, string.Join("/", pathElements)).ToLower();
    }
}