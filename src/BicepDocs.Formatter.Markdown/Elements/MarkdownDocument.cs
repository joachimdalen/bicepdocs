using System.Text;

namespace LandingZones.Tools.BicepDocs.Formatter.Markdown.Elements;

public class MarkdownDocument
{
    private List<MkBlockBase> _elements = new();

    public int Count => _elements.Count;

    public void Append(MkBlockBase blockBase)
    {
        _elements.Add(blockBase);
    }

    public void Prepend(MkBlockBase blockBase)
    {
        _elements = _elements.Prepend(blockBase).ToList();
    }

    public MkBlockBase this[int index] => _elements[index];

    public string ToMarkdown()
    {
        var sb = new StringBuilder();
        foreach (var mkElement in _elements)
        {
            sb.AppendLine(mkElement.ToMarkdown());
            sb.AppendLine();
        }

        var markdownContent = sb.ToString();

        var normalized = Markdig.Markdown.Normalize(markdownContent);
        if (!normalized.EndsWith(Environment.NewLine))
        {
            normalized += Environment.NewLine;
        }

        return normalized;
    }
}