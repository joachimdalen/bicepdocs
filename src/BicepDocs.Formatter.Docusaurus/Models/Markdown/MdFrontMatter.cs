using LandingZones.Tools.BicepDocs.Formatter.Markdown.Elements;
using YamlDotNet.Serialization;

namespace LandingZones.Tools.BicepDocs.Formatter.Docusaurus.Models.Markdown;

public class MdFrontMatter : MkBlockBase
{
    private readonly object _properties;

    public MdFrontMatter(object properties)
    {
        _properties = properties;
    }

    public override string ToMarkdown()
    {
        var deserializer = new SerializerBuilder()
            .ConfigureDefaultValuesHandling(DefaultValuesHandling.OmitNull)
            .Build();
        return @$"---
{deserializer.Serialize(_properties)}
---
";
    }
}