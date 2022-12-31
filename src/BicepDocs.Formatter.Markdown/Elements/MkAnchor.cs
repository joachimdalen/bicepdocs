namespace LandingZones.Tools.BicepDocs.Formatter.Markdown.Elements;

public class MkAnchor : MkBlockBase
{
    private readonly string _text;
    private readonly string _link;

    public MkAnchor(string text, string link)
    {
        _text = text;
        _link = link;
    }

    public override string ToMarkdown()
    {
        return $"[{_text}]({_link})";
    }
}