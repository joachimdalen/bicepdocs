namespace LandingZones.Tools.BicepDocs.Core.Models.Parsing;

public record ParsedResource(string Identifier, string Provider, string Resource)
{
    public string Identifier { get; set; } = Identifier;
    public string Provider { get; set; } = Provider;
    public string Resource { get; set; } = Resource;
    public string? ApiVersion { get; set; }
    public string? DocUrl { get; set; }
    public bool IsExisting { get; set; }
    public string? Name { get; set; }
    public string? Scope { get; set; }
}