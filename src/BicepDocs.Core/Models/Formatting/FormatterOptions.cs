using LandingZones.Tools.BicepDocs.Core.Parsers;

namespace LandingZones.Tools.BicepDocs.Core.Models.Formatting;

public class FormatterOptions
{
    public bool IncludeExistingResources { get; set; } = true;
    public bool IncludeParameters { get; set; } = true;
    public bool IncludeUsage { get; set; } = true;
    public bool IncludeResources { get; set; } = true;
    public bool IncludeOutputs { get; set; } = true;
    public bool IncludeReferencedResources { get; set; } = true;
    public bool DisableVersioning { get; set; } = false;
    public string MetaKeyword { get; set; } = MetadataParser.MetadataKey;

    public UsageOptions Usage { get; set; } = new();
    public Dictionary<DocFormatter, object> Formatters { get; set; } = new();

    public HashSet<DocSection> SectionOrder { get; set; } = new()
    {
        DocSection.Title,
        DocSection.Description,
        DocSection.Usage,
        DocSection.Parameters,
        DocSection.ReferencedResources,
        DocSection.Resources,
        DocSection.Outputs,
        DocSection.ParameterReferences
    };
}