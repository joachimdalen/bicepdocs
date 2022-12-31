using YamlDotNet.Serialization;

namespace LandingZones.Tools.BicepDocs.Formatter.Docusaurus.Models;

public class PageMeta
{
    [YamlMember(Alias = "sidebar_position")]
    public int? SidebarPosition { get; set; }

    [YamlMember(Alias = "sidebar_label")]
    public string? SidebarLabel { get; set; }

    [YamlMember(Alias = "tags")]
    public string[]? Tags { get; set; } = null;
}