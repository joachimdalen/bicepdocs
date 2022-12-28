namespace LandingZones.Tools.BicepDocs.Core.Models;

public class ParsedResource
{
    public ParsedResource(string identifier, string provider, string resource)
    {
        Identifier = identifier;
        Provider = provider;
        Resource = resource;
    }

    public string Identifier { get; set; }
    public string Provider { get; set; }
    public string Resource { get; set; }
    public string? ApiVersion { get; set; }
    public string? DocUrl { get; set; }
    public bool IsExisting { get; set; }
    public string? Name { get; set; }
    public string? Scope { get; set; }
}