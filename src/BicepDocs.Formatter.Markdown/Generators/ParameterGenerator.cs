using System.Collections.Immutable;
using LandingZones.Tools.BicepDocs.Core.Models.Formatting;
using LandingZones.Tools.BicepDocs.Core.Models.Parsing;
using LandingZones.Tools.BicepDocs.Formatter.Markdown.Elements;

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
        foreach (var templateParameter in parameters.OrderBy(x => x.Name))
        {
            var defaultValue = templateParameter.DefaultValue;

            var type = templateParameter.IsComplexAllow
                ? new MkAnchor($"{templateParameter.Name}Allow", $"#{templateParameter.Name}Allow".ToLower())
                    .ToMarkdown()
                : templateParameter.Type.Replace("|", "\\|");
            var dfValue = templateParameter.IsComplexDefault
                ? new MkAnchor($"{templateParameter.Name}Value", $"#{templateParameter.Name}Value".ToLower())
                    .ToMarkdown()
                : defaultValue?.Replace(Environment.NewLine, string.Empty) ?? string.Empty;

            paramOverviewTable.AddRow($"`{templateParameter.Name}`", templateParameter.Description ?? "",
                type,
                dfValue);
        }

        document.Append(paramOverviewTable);
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