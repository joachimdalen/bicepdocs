using YamlDotNet.Serialization;

namespace LandingZones.Tools.BicepDocs.Formatter.Docusaurus.Models;

public class PageMeta
{
    [YamlMember(Alias = "tags")]
    public string[]? Tags { get; set; }
}