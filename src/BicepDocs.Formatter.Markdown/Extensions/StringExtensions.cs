namespace LandingZones.Tools.BicepDocs.Formatter.Markdown.Extensions;

public static class StringExtensions
{
    public static string WrapInBackticks(this string input) => $"`{input}`";
}