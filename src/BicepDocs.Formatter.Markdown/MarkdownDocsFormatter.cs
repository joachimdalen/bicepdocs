using System.Collections.Immutable;
using LandingZones.Tools.BicepDocs.Core;
using LandingZones.Tools.BicepDocs.Core.Abstractions;
using LandingZones.Tools.BicepDocs.Core.Models.Formatting;
using LandingZones.Tools.BicepDocs.Core.Parsers;
using LandingZones.Tools.BicepDocs.Formatter.Markdown.Elements;
using LandingZones.Tools.BicepDocs.Formatter.Markdown.Generators;
using LandingZones.Tools.BicepDocs.Formatter.Markdown.Models;

namespace LandingZones.Tools.BicepDocs.Formatter.Markdown;

public class MarkdownDocsFormatter : IDocsFormatter
{
    private readonly ConfigurationLoader _configurationLoader;

    public MarkdownDocsFormatter(ConfigurationLoader configurationLoader)
    {
        _configurationLoader = configurationLoader;
    }

    public DocFormatter Formatter => DocFormatter.Markdown;

    public Task<IImmutableList<GenerationFile>> GenerateModuleDocs(FormatterContext context)
    {
        var outputFiles = new List<GenerationFile>();
        var markdownDocument = new MarkdownDocument();
        var metadata = MetadataParser.GetMetadata(context.Template, context.FormatterOptions.MetaKeyword);
        var parameters = ParameterParser.ParseParameters(context.Template);
        var configuration =
            _configurationLoader.GetFormatterOptionsOrDefault<MarkdownOptions>(context.FormatterOptions, Formatter);

        foreach (var docSection in context.FormatterOptions.SectionOrder)
        {
            switch (docSection)
            {
                case DocSection.Title:
                    MetaGenerator.BuildTitle(markdownDocument, metadata, context);
                    break;
                case DocSection.Description:
                    MetaGenerator.BuildDescription(markdownDocument, metadata);
                    break;
                case DocSection.Parameters:
                    ParameterGenerator.BuildParameters(markdownDocument, context.FormatterOptions, parameters);
                    break;
                case DocSection.Usage:
                    UsageGenerator.BuildUsage(
                        markdownDocument,
                        context.FormatterOptions,
                        parameters,
                        configuration.Usage,
                        Path.ChangeExtension(context.ModulePath, null),
                        metadata?.Version ?? "<version>");
                    break;
                case DocSection.Resources:
                    ResourceGenerator.BuildResources(markdownDocument, context);
                    break;
                case DocSection.Outputs:
                    OutputGenerator.BuildOutputs(markdownDocument, context);
                    break;
                case DocSection.ParameterReferences:
                    ParameterGenerator.BuildParameterReferences(markdownDocument, context.FormatterOptions, parameters);
                    break;
                case DocSection.ReferencedResources:
                    ResourceGenerator.BuildReferencedResources(markdownDocument, context);
                    break;
            }
        }

        if (!string.IsNullOrEmpty(metadata?.Version) && context.FormatterOptions.DisableVersioning == false)
        {
            var versionPaths = PathResolver.ResolveVersionPath(context.ModulePath, metadata.Version);
            outputFiles.Add(new MarkdownGenerationFile(Path.ChangeExtension(context.ModulePath, "md"),
                markdownDocument, context.Template,
                versionPaths));
        }
        else
        {
            outputFiles.Add(new MarkdownGenerationFile(Path.ChangeExtension(context.ModulePath, "md"),
                markdownDocument, context.Template));
        }

        return Task.FromResult<IImmutableList<GenerationFile>>(outputFiles.ToImmutableArray());
    }
}