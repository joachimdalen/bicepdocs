namespace LandingZones.Tools.BicepDocs.Core.Models;

public class ParsedParameter
{
    public ParsedParameter(string name, string type)
    {
        Name = name;
        Type = type;
    }

    public string Name { get; set; }
    public string Type { get; set; }
    public string? Description { get; set; }
    public string? DefaultValue { get; set; }
    public bool IsComplexDefault { get; set; }
    public List<string>? AllowedValues { get; set; }
    public bool IsComplexAllow { get; set; }
}