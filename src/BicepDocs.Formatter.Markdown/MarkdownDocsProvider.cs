using System.Collections.Immutable;
using LandingZones.Tools.BicepDocs.Core;
using LandingZones.Tools.BicepDocs.Core.Abstractions;
using LandingZones.Tools.BicepDocs.Core.Models.Formatting;
using LandingZones.Tools.BicepDocs.Core.Parsers;
using LandingZones.Tools.BicepDocs.Formatter.Markdown.Elements;
using LandingZones.Tools.BicepDocs.Formatter.Markdown.Generators;
using LandingZones.Tools.BicepDocs.Formatter.Markdown.Models;

namespace LandingZones.Tools.BicepDocs.Formatter.Markdown;

public class MarkdownDocsProvider : IDocsProvider
{
    public DocFormatter Formatter => DocFormatter.Markdown;

    public Task<IImmutableList<GenerationFile>> GenerateModuleDocs(GeneratorContext context)
    {
        var outputFiles = new List<GenerationFile>();
        var markdownDocument = new MarkdownDocument();
        var metadata = MetadataParser.GetMetadata(context.Template, context.FormatterOptions.MetaKeyword);
        var parameters = ParameterParser.ParseParameters(context.Template);

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
                    UsageGenerator.BuildUsage(markdownDocument, context.FormatterOptions, parameters);
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
            var versionPaths = PathResolver.ResolveVersionPath(context.Paths, metadata.Version);
            outputFiles.Add(new MarkdownGenerationFile(context.Paths.OutputPath, markdownDocument, context.Template, versionPaths.OutputPath));
        }
        else
        {
            outputFiles.Add(new MarkdownGenerationFile(context.Paths.OutputPath, markdownDocument, context.Template));
        }

        return Task.FromResult<IImmutableList<GenerationFile>>(outputFiles.ToImmutableArray());
    }
}
