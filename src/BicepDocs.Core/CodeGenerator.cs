using System.Collections.Immutable;
using System.Text;
using LandingZones.Tools.BicepDocs.Core.Models.Parsing;
using LandingZones.Tools.BicepDocs.Core.Services;

namespace LandingZones.Tools.BicepDocs.Core;

public static class CodeGenerator
{
    public static string GetBicepExample(string moduleName, string moduleAlias, string moduleType, string path,
        string moduleVersion,
        IImmutableList<ParsedParameter> parameters)
    {
        var sb = new StringBuilder();
        sb.AppendLine();

        foreach (var defaultParameter in parameters)
        {
            sb.AppendLine($"    {defaultParameter.Name}: {GetDefaultValue(defaultParameter)}");
        }

        var s = sb.ToString();
        var example = $@"module {moduleName} '{moduleType}/{moduleAlias}:{path}:{moduleVersion}' = {{
  name: '{moduleName}'
  params: {{{s}}}
}}";

        return BicepFormatter.FormatBicepCode(example);
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

        if (parameter.DefaultValue.StartsWith('\'') && parameter.DefaultValue.EndsWith('\'') ||
            parameter.IsInterpolated)
        {
            return parameter.DefaultValue;
        }

        return $"'{parameter.DefaultValue}'";
    }
}