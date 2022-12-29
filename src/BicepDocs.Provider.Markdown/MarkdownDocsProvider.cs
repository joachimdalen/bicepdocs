using System.Collections.Immutable;
using LandingZones.Tools.BicepDocs.Core;
using LandingZones.Tools.BicepDocs.Core.Abstractions;
using LandingZones.Tools.BicepDocs.Core.Models;
using LandingZones.Tools.BicepDocs.Core.Parsers;
using LandingZones.Tools.BicepDocs.Provider.Markdown.Elements;
using LandingZones.Tools.BicepDocs.Provider.Markdown.Generators;
using LandingZones.Tools.BicepDocs.Provider.Markdown.Models;

namespace LandingZones.Tools.BicepDocs.Provider.Markdown;

public class MarkdownDocsProvider : IDocsProvider
{
    public DocProvider Provider => DocProvider.Markdown;

    public Task<IImmutableList<GenerationFile>> GenerateModuleDocs(GeneratorContext context)
    {
        var outputFiles = new List<GenerationFile>();
        var markdownDocument = new MarkdownDocument();
        var metadata = MetadataParser.GetMetadata(context.Template, context.GeneratorOptions.MetaKeyword);
        var parameters = ParameterParser.ParseParameters(context.Template);

        foreach (var docSection in context.GeneratorOptions.SectionOrder)
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
                    ParameterGenerator.BuildParameters(markdownDocument, context.GeneratorOptions, parameters);
                    break;
                case DocSection.Usage:
                    UsageGenerator.BuildUsage(markdownDocument, context.GeneratorOptions, parameters);
                    break;
                case DocSection.Resources:
                    ResourceGenerator.BuildResources(markdownDocument, context);
                    break;
                case DocSection.Outputs:
                    OutputGenerator.BuildOutputs(markdownDocument, context);
                    break;
                case DocSection.ParameterReferences:
                    ParameterGenerator.BuildParameterReferences(markdownDocument, context.GeneratorOptions, parameters);
                    break;
                case DocSection.ReferencedResources:
                    ResourceGenerator.BuildReferencedResources(markdownDocument, context);
                    break;
            }
        }

        if (!string.IsNullOrEmpty(metadata?.Version) && context.GeneratorOptions.DisableVersioning == false)
        {
            var versionPaths = PathResolver.ResolveVersionPath(context.Paths, metadata.Version);
            outputFiles.Add(new MarkdownGenerationFile(context.Paths.OutputPath, markdownDocument, context.Template,
                versionPaths.OutputPath));
        }
        else
        {
            outputFiles.Add(new MarkdownGenerationFile(context.Paths.OutputPath, markdownDocument, context.Template));
        }

        return Task.FromResult<IImmutableList<GenerationFile>>(outputFiles.ToImmutableArray());
    }
}
