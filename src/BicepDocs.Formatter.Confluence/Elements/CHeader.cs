namespace LandingZones.Tools.BicepDocs.Formatter.Confluence.Elements;

public class CHeader : CBlockBase
{
    private readonly string _text;
    private readonly CHeaderLevel _level;

    public CHeader(string text, CHeaderLevel level)
    {
        _text = text;
        _level = level;
    }

    public override string ToMarkup()
    {
        return $"<h{_level}>{_text}</h{_level}";
    }
}