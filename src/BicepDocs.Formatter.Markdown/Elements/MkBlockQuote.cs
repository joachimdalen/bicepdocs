namespace LandingZones.Tools.BicepDocs.Formatter.Markdown.Elements;

public class MkBlockQuote : MkBlockBase
{
    private readonly string _text;

    public MkBlockQuote(string text)
    {
        _text = text;
    }

    public override string ToMarkdown()
    {
        return $"> {_text}";
    }
}