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
            var type = tp.IsComplexAllow
                ? new MkAnchor($"{tp.Name}Allow", $"#{tp.Name}Allow".ToLower()).ToMarkdown()
                : tp.Type.Replace("|", "\\|");
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

        return (tp.IsInterpolated ? tp.DefaultValue.WrapInBackticks() : tp.DefaultValue).Replace(Environment.NewLine, "");
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
}