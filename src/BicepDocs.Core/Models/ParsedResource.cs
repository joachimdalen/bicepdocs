namespace LandingZones.Tools.BicepDocs.Core.Models;

public class ParsedResource
{
    public ParsedResource(string name, string provider, string resource)
    {
        Name = name;
        Provider = provider;
        Resource = resource;
    }

    public string Name { get; set; }
    public string Provider { get; set; }
    public string Resource { get; set; }
    public string? ApiVersion { get; set; }
    public string? DocUrl { get; set; }
    public bool IsExisting { get; set; }
}