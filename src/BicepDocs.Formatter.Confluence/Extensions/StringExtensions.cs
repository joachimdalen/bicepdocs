namespace LandingZones.Tools.BicepDocs.Formatter.Confluence.Extensions;

public static class StringExtensions
{
    public static string WrapInCodeBlock(this string input) => $"<code>{input}</code>";
}