using System.Collections.Immutable;
using System.Text;
using LandingZones.Tools.BicepDocs.Core.Models.Parsing;

namespace LandingZones.Tools.BicepDocs.Formatter.Markdown;

public static class CodeGenerator
{
    public static string GetExample(string moduleName, string path, string apiVersion,
        IImmutableList<ParsedParameter> parameters)
    {
        var sb = new StringBuilder();
        sb.AppendLine();

        foreach (var defaultParameter in parameters)
        {
            sb.AppendLine($"    {defaultParameter.Name}: {GetDefaultValue(defaultParameter)}");
        }

        var s = sb.ToString();

        return $@"module {moduleName} '{path}:{apiVersion}' = {{
  name: '{moduleName}'
  params: {{{s}}}
";
    }

    private static string GetDefaultValue(ParsedParameter parameter)
    {
        if (string.IsNullOrEmpty(parameter.DefaultValue))
        {
            return parameter.Type == "string" ? $"'{parameter.Name}'" : parameter.Name;
        }

        if (parameter.Type != "string")
        {
            return parameter.DefaultValue;
        }

        if (parameter.DefaultValue.StartsWith('\'') && parameter.DefaultValue.EndsWith('\''))
        {
            return parameter.DefaultValue;
        }

        return $"'{parameter.DefaultValue}'";
    }
}