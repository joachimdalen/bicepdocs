using System.Collections.Immutable;
using LandingZones.Tools.BicepDocs.Core.Models.Formatting;
using LandingZones.Tools.BicepDocs.Core.Models.Parsing;
using LandingZones.Tools.BicepDocs.Formatter.Confluence.Elements;
using LandingZones.Tools.BicepDocs.Formatter.Confluence.Extensions;

namespace LandingZones.Tools.BicepDocs.Formatter.Confluence.Generators;

internal static class ParameterGenerator
{
    internal static void BuildParameters(ConfluenceDocument document, FormatterOptions options,
        ImmutableList<ParsedParameter> parameters)
    {
        if (!parameters.Any() || !options.IncludeParameters) return;

        document.Append(new CHeader("Parameters", CHeaderLevel.H2));
        var paramOverviewTable = new CTable().AddColumn("Parameter").AddColumn("Description").AddColumn("Type")
            .AddColumn("Default");
        foreach (var tp in parameters.OrderBy(x => x.Name))
        {
            var type = BuildType(tp);
            var dfValue = GetParameterDefault(tp);

            paramOverviewTable.AddRow(tp.Name.WrapInCodeBlock(), tp.Description ?? "", type, dfValue);
        }

        document.Append(paramOverviewTable);
    }

    private static string GetParameterDefault(ParsedParameter tp)
    {
        if (string.IsNullOrEmpty(tp.DefaultValue))
            return string.Empty;

        if (tp.IsComplexDefault)
            return new CAnchor($"{tp.Name}Value", $"#{tp.Name}Value".ToLower()).ToMarkup();

        return (tp.IsInterpolated ? tp.DefaultValue.WrapInCodeBlock() : tp.DefaultValue).Replace(Environment.NewLine,
            "");
    }

    internal static void BuildParameterReferences(ConfluenceDocument document, FormatterOptions options,
        ImmutableList<ParsedParameter> parameters)
    {
        if (!options.IncludeParameters) return;
        var complexParameters = parameters.Where(x => x.IsComplexDefault).ToList();
        var complexAllow = parameters.Where(x => x.IsComplexAllow).ToList();

        var hasComplexParameters = complexParameters.Any();
        var hasComplexAllow = complexAllow.Any();
        if (!hasComplexParameters && !hasComplexAllow)
        {
            return;
        }

        document.Append(new CHeader("References", CHeaderLevel.H2));

        if (hasComplexParameters)
        {
            foreach (var parameterDefault in complexParameters)
            {
                document.Append(new CHeader($"{parameterDefault.Name}Value", CHeaderLevel.H3));
                if (!string.IsNullOrEmpty(parameterDefault.DefaultValue))
                    document.Append(new CCodeBlock(parameterDefault.DefaultValue, "bicep"));
            }
        }

        if (hasComplexAllow)
        {
            foreach (var parameterDefault in complexAllow)
            {
                document.Append(new CHeader($"{parameterDefault.Name}Allow", CHeaderLevel.H3));
                if (parameterDefault.AllowedValues != null)
                    document.Append(new CList(parameterDefault.AllowedValues));
            }
        }
    }

    private static string? BuildType(ParsedParameter parameter)
    {
        string type = string.Empty;

        if (parameter.IsComplexAllow)
        {
            type += new CAnchor($"{parameter.Name}Allow", $"#{parameter.Name}Allow".ToLower()).ToMarkup();
            if (parameter.Secure)
            {
                type += " (secure)";
            }
        }
        else
        {
            type += parameter.Type.Replace("|", "\\|");
            if (parameter.Secure) type += " (secure)";
        }


        var minMax = GetCharacterLimit(parameter);
        if (minMax != null)
        {
            type += " <br/> <br/>";
            type += $"Character limit: {minMax}";
        }

        var value = GetAcceptedValues(parameter);
        if (value != null)
        {
            type += " <br/> <br/>";
            type += $"Accepted values: {value}";;
        }

        return type;
    }

    public static string? GetAcceptedValues(ParsedParameter parameter)
    {
        if (parameter.MinValue == null && parameter.MaxValue == null)
            return null;

        if (parameter is { MinValue: { }, MaxValue: { } })
        {
            return $"from {parameter.MinValue} to {parameter.MaxValue}.";
        }

        if (parameter.MinValue != null)
        {
            return $"from {parameter.MinValue}.";
        }

        if (parameter.MaxValue != null)
        {
            return $"to {parameter.MaxValue}.";
        }

        return null;
    }


    private static string? GetCharacterLimit(ParsedParameter parameter)
    {
        if (parameter.MinLength == null && parameter.MaxLength == null)
        {
            return null;
        }

        if (parameter is { MinLength: { }, MaxLength: { } })
        {
            return $"{parameter.MinLength}-{parameter.MaxLength}";
        }

        if (parameter.MinLength != null)
        {
            return $"{parameter.MinLength}-X";
        }

        if (parameter.MaxLength != null)
        {
            return $"X-{parameter.MaxLength}";
        }

        return null;
    }
}