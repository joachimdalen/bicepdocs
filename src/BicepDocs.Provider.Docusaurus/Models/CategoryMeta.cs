namespace LandingZones.Tools.BicepDocs.Provider.Docusaurus.Models;

public class CategoryMeta
{
    public CategoryMeta(string label)
    {
        Label = label;
    }

    public string Label { get; set; }
    public float? Position { get; set; }
    public bool? Collapsible { get; set; }
    public bool? Collapsed { get; set; }
}