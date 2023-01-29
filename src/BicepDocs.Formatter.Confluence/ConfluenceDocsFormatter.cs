using System.Collections.Immutable;
using LandingZones.Tools.BicepDocs.Core;
using LandingZones.Tools.BicepDocs.Core.Abstractions;
using LandingZones.Tools.BicepDocs.Core.Models.Formatting;
using LandingZones.Tools.BicepDocs.Core.Parsers;
using LandingZones.Tools.BicepDocs.Formatter.Confluence.Elements;
using LandingZones.Tools.BicepDocs.Formatter.Confluence.Generators;
using LandingZones.Tools.BicepDocs.Formatter.Confluence.Models;

namespace LandingZones.Tools.BicepDocs.Formatter.Confluence;

public class ConfluenceDocsFormatter : IDocsFormatter
{
    public DocFormatter Formatter => DocFormatter.Confluence;

    public Task<IImmutableList<GenerationFile>> GenerateModuleDocs(FormatterContext context)
    {
        var outputFiles = new List<GenerationFile>();
        var confluenceDocument = new ConfluenceDocument();
        var metadata = MetadataParser.GetMetadata(context.Template, context.FormatterOptions.MetaKeyword);
        var parameters = ParameterParser.ParseParameters(context.Template);

        var title = MetaGenerator.BuildTitle(metadata, context);
        
        foreach (var docSection in context.FormatterOptions.SectionOrder)
        {
            switch (docSection)
            {
                case DocSection.Title:
                    //MetaGenerator.BuildTitle(markdownDocument, metadata, context);
                    break;
                case DocSection.Description:
                    MetaGenerator.BuildDescription(confluenceDocument, metadata);
                    break;
                case DocSection.Parameters:
                    ParameterGenerator.BuildParameters(confluenceDocument, context.FormatterOptions, parameters);
                    break;
                case DocSection.Usage:
                    UsageGenerator.BuildUsage(
                        confluenceDocument,
                        context.FormatterOptions,
                        parameters,
                        Path.ChangeExtension(context.ModulePath, null),
                        metadata?.Version ?? "<version>");
                    break;
                case DocSection.Resources:
                    ResourceGenerator.BuildResources(confluenceDocument, context);
                    break;
                case DocSection.Outputs:
                    OutputGenerator.BuildOutputs(confluenceDocument, context);
                    break;
                case DocSection.ParameterReferences:
                    ParameterGenerator.BuildParameterReferences(confluenceDocument, context.FormatterOptions,
                        parameters);
                    break;
                case DocSection.ReferencedResources:
                    ResourceGenerator.BuildReferencedResources(confluenceDocument, context);
                    break;
            }
        }

        if (!string.IsNullOrEmpty(metadata?.Version) && context.FormatterOptions.DisableVersioning == false)
        {
            var versionPaths = PathResolver.ResolveVersionPath(context.ModulePath, metadata.Version);
            outputFiles.Add(new ConfluenceGenerationFile(metadata.Title, Path.ChangeExtension(context.ModulePath, "md"),
                versionPaths));
        }
        else
        {
            outputFiles.Add(
                new ConfluenceGenerationFile(metadata.Title, Path.ChangeExtension(context.ModulePath, "md")));
        }

        return Task.FromResult<IImmutableList<GenerationFile>>(outputFiles.ToImmutableArray());
    }
}