using System.Text.RegularExpressions;
using LandingZones.Tools.BicepDocs.Core.Extensions;

namespace LandingZones.Tools.BicepDocs.Formatter.Docusaurus.Resolvers;

public abstract class NameResolver
{
    public static string ResolveName(string name)
    {
        var replaced = Regex.Replace(name, @"([A-Z]|_|-)", " $1", RegexOptions.Compiled).Trim().Split(' ');

        var trans = replaced.Select(x => x.Replace('-', ' ').Replace('_', ' ').Trim())
            .Select(StringExtensions.FirstCharToUpper);

        return string.Join(' ', trans);
    }
}