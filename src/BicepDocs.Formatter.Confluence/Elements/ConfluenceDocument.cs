using System.Text;

namespace LandingZones.Tools.BicepDocs.Formatter.Confluence.Elements;

public class ConfluenceDocument
{
    private List<CBlockBase> _elements = new();

    public int Count => _elements.Count;

    public void Append(CBlockBase blockBase)
    {
        _elements.Add(blockBase);
    }

    public void Prepend(CBlockBase blockBase)
    {
        _elements = _elements.Prepend(blockBase).ToList();
    }

    public CBlockBase this[int index] => _elements[index];

    public string ToMarkdown()
    {
        var sb = new StringBuilder();
        foreach (var mkElement in _elements)
        {
            sb.AppendLine(mkElement.ToMarkup());
            sb.AppendLine();
        }

        var markdownContent = sb.ToString();
        return markdownContent;
    }
}