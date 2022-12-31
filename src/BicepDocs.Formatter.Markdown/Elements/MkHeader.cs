namespace LandingZones.Tools.BicepDocs.Formatter.Markdown.Elements;

public class MkHeader : MkBlockBase
{
    private readonly string _text;
    private readonly MkHeaderLevel _level;

    public MkHeader(string text, MkHeaderLevel level)
    {
        _text = text;
        _level = level;
    }

    public override string ToMarkdown()
    {
        return $"{new string('#', (int)_level)} {_text}";
    }
}