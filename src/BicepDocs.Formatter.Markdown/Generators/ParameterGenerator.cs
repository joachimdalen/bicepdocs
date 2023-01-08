using System.Collections.Immutable;
using LandingZones.Tools.BicepDocs.Core.Models.Formatting;
using LandingZones.Tools.BicepDocs.Core.Models.Parsing;
using LandingZones.Tools.BicepDocs.Formatter.Markdown.Elements;
using LandingZones.Tools.BicepDocs.Formatter.Markdown.Extensions;

namespace LandingZones.Tools.BicepDocs.Formatter.Markdown.Generators;

internal static class ParameterGenerator
{
    internal static void BuildParameters(MarkdownDocument document, FormatterOptions options,
        ImmutableList<ParsedParameter> parameters)
    {
        if (!parameters.Any() || !options.IncludeParameters) return;

        document.Append(new MkHeader("Parameters", MkHeaderLevel.H2));
        var paramOverviewTable = new MkTable().AddColumn("Parameter").AddColumn("Description").AddColumn("Type")
            .AddColumn("Default");
        foreach (var tp in parameters.OrderBy(x => x.Name))
        {
            var type = BuildType(tp);
            var dfValue = GetParameterDefault(tp);

            paramOverviewTable.AddRow(tp.Name.WrapInBackticks(), tp.Description ?? "", type, dfValue);
        }

        document.Append(paramOverviewTable);
    }

    private static string GetParameterDefault(ParsedParameter tp)
    {
        if (string.IsNullOrEmpty(tp.DefaultValue))
            return string.Empty;

        if (tp.IsComplexDefault)
            return new MkAnchor($"{tp.Name}Value", $"#{tp.Name}Value".ToLower()).ToMarkdown();

        return (tp.IsInterpolated ? tp.DefaultValue.WrapInBackticks() : tp.DefaultValue).Replace(Environment.NewLine,
            "");
    }

    internal static void BuildParameterReferences(MarkdownDocument document, FormatterOptions options,
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

        document.Append(new MkHeader("References", MkHeaderLevel.H2));

        if (hasComplexParameters)
        {
            foreach (var parameterDefault in complexParameters)
            {
                document.Append(new MkHeader($"{parameterDefault.Name}Value", MkHeaderLevel.H3));
                if (!string.IsNullOrEmpty(parameterDefault.DefaultValue))
                    document.Append(new MkCodeBlock(parameterDefault.DefaultValue, "bicep"));
            }
        }

        if (hasComplexAllow)
        {
            foreach (var parameterDefault in complexAllow)
            {
                document.Append(new MkHeader($"{parameterDefault.Name}Allow", MkHeaderLevel.H3));
                if (parameterDefault.AllowedValues != null)
                    document.Append(new MkList(parameterDefault.AllowedValues));
            }
        }
    }

    private static string? BuildType(ParsedParameter parameter)
    {
        string type = string.Empty;

        if (parameter.IsComplexAllow)
        {
            type += new MkAnchor($"{parameter.Name}Allow", $"#{parameter.Name}Allow".ToLower()).ToMarkdown();
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
            return $"from {parameter.MinValue} to {parameter.MaxValue}";
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