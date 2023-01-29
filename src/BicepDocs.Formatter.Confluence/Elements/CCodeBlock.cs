namespace LandingZones.Tools.BicepDocs.Formatter.Confluence.Elements;

public class CCodeBlock : CBlockBase
{
    private readonly string _content;
    private readonly string? _language;

    public CCodeBlock(string content, string? language = null)
    {
        _content = content;
        _language = language;
    }

    public override string ToMarkup()
    {
        return @$"<ac:structured-macro ac:name=""code"" ac:schema-version=""1"">
    <ac:parameter ac:name=""language"">{_language}</ac:parameter>
    <ac:plain-text-body>
        <![CDATA[{_content}]]>
    </ac:plain-text-body>
</ac:structured-macro>";
    }
}