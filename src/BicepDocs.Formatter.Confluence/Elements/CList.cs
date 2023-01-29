using System.Text;

namespace LandingZones.Tools.BicepDocs.Formatter.Confluence.Elements;

public class CList : CBlockBase
{
    public CList() : this(new List<string>())
    {
    }

    public CList(List<string> items)
    {
        _items = items;
    }

    private readonly List<string> _items;

    public CList AddListElement(string item)
    {
        _items.Add(item);
        return this;
    }

    public override string ToMarkup()
    {
        var sb = new StringBuilder();
        sb.AppendLine("<ul>");

        foreach (var listItem in _items)
        {
            sb.AppendLine($"<li><p>{listItem}</p></li>");
        }

        sb.AppendLine("</ul>");
        return sb.ToString();
    }
}