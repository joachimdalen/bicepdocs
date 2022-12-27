using System.Text;

namespace LandingZones.Tools.BicepDocs.Provider.Markdown.Elements;

public class MkList : MkBlockBase
{
    public MkList() : this(new List<string>())
    {
    }

    public MkList(List<string> items)
    {
        _items = items;
    }

    private readonly List<string> _items;

    public MkList AddListElement(string item)
    {
        _items.Add(item);
        return this;
    }

    public override string ToMarkdown()
    {
        var sb = new StringBuilder();

        foreach (var listItem in _items)
        {
            sb.AppendLine($"- {listItem}");
        }

        return sb.ToString();
    }
}