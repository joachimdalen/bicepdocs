namespace LandingZones.Tools.BicepDocs.Core.Models;

public class ParsedOutput
{
    public ParsedOutput(string name, string type, string? description)
    {
        Name = name;
        Type = type;
        Description = description;
    }

    public string Name { get; }
    public string Type { get; }
    public string? Description { get; }
}