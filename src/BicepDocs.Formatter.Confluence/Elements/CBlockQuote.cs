namespace LandingZones.Tools.BicepDocs.Formatter.Confluence.Elements;

public class CBlockQuote : CBlockBase
{
    private readonly string _text;

    public CBlockQuote(string text)
    {
        _text = text;
    }

    public override string ToMarkup()
    {
        return $"<blockquote><p>{_text}</p></blockquote>";
    }
}