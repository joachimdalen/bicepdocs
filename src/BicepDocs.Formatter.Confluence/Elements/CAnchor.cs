namespace LandingZones.Tools.BicepDocs.Formatter.Confluence.Elements;

public class CAnchor : CBlockBase
{
    private readonly string _text;
    private readonly string _link;

    public CAnchor(string text, string link)
    {
        _text = text;
        _link = link;
    }

    public override string ToMarkup()
    {
        return $@"<a href=""{_link}"">{_text}</a>";
    }
}