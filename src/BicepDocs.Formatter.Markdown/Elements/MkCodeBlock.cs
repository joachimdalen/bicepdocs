namespace LandingZones.Tools.BicepDocs.Formatter.Markdown.Elements;

public class MkCodeBlock : MkBlockBase
{
    private readonly string _content;
    private readonly string? _language;

    public MkCodeBlock(string content, string? language = null)
    {
        _content = content;
        _language = language;
    }

    public override string ToMarkdown()
    {
        return @$"```{_language}
{_content}
```";
    }
}