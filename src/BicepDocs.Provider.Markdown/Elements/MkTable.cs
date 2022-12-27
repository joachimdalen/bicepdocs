using System.Text;
using LandingZones.Tools.BicepDocs.Core.Extensions;

namespace LandingZones.Tools.BicepDocs.Provider.Markdown.Elements;

public class MkTable : MkBlockBase
{
    private readonly List<string> _columns = new();
    private readonly List<List<string>> _rows = new();

    public MkTable AddColumn(string text)
    {
        _columns.Add(text);
        return this;
    }

    public MkTable AddRow(params string[] values)
    {
        _rows.Add(values.ToList());
        return this;
    }


    public override string ToMarkdown()
    {
        var headerRow = $"| {string.Join(" | ", _columns)} |";
        var sb = new StringBuilder();

        sb.AppendLine(headerRow);
        sb.AppendLine("|" + StringExtensions.Repeat(" --- |", _columns.Count));

        foreach (var row in _rows)
        {
            sb.AppendLine($"| {string.Join(" | ", row)} |");
        }

        return sb.ToString().Replace("<", "\\<").Replace(">", "\\>");
    }
}