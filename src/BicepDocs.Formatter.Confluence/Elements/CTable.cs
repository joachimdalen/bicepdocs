using System.Text;
using LandingZones.Tools.BicepDocs.Core.Extensions;

namespace LandingZones.Tools.BicepDocs.Formatter.Confluence.Elements;

public class CTable : CBlockBase
{
    private readonly List<string> _columns = new();
    private readonly List<List<string>> _rows = new();

    public CTable AddColumn(string text)
    {
        _columns.Add(text);
        return this;
    }

    public CTable AddRow(params string[] values)
    {
        _rows.Add(values.ToList());
        return this;
    }


    public override string ToMarkup()
    {
        var sb = new StringBuilder();

        sb.AppendLine(@"<table data-layout=""default"">");
        sb.AppendLine(@"<colgroup>");
        sb.AppendLine(@"<col style=""width: 170.0px;"" />");
        sb.AppendLine(@"</colgroup>");
        sb.AppendLine(@"<tbody>");
        sb.AppendLine(@"<tr>");

        foreach (var column in _columns)
        {
            sb.AppendLine($@"<th><p>{column}</p></th>");
        }

        sb.AppendLine(@"</tr>");

        foreach (var row in _rows)
        {
            sb.AppendLine(@"<tr>");
            foreach (var rowCol in row)
            {
                sb.AppendLine($@"<td><p>{rowCol}</p></td>");
            }

            sb.AppendLine(@"</tr>");
        }

        sb.AppendLine(@"</tbody>");
        sb.AppendLine(@"</table>");
        return sb.ToString();
    }
}