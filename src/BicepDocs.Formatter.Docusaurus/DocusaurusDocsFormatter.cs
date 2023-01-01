using System.Collections.Immutable;
using System.Text.Json;
using System.Text.Json.Serialization;
using LandingZones.Tools.BicepDocs.Core;
using LandingZones.Tools.BicepDocs.Core.Abstractions;
using LandingZones.Tools.BicepDocs.Core.Models.Formatting;
using LandingZones.Tools.BicepDocs.Core.Parsers;
using LandingZones.Tools.BicepDocs.Formatter.Docusaurus.Models;
using LandingZones.Tools.BicepDocs.Formatter.Docusaurus.Models.Markdown;
using LandingZones.Tools.BicepDocs.Formatter.Docusaurus.Resolvers;
using LandingZones.Tools.BicepDocs.Formatter.Markdown;
using LandingZones.Tools.BicepDocs.Formatter.Markdown.Models;
using Microsoft.Extensions.Logging;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace LandingZones.Tools.BicepDocs.Formatter.Docusaurus;

public class DocusaurusDocsFormatter : IDocsFormatter
{
    private readonly ILogger<DocusaurusDocsFormatter> _logger;
    private readonly MarkdownDocsFormatter _markdownDocsFormatter;
    private readonly ConfigurationLoader _configurationLoader;

    private readonly JsonSerializerOptions _serializeOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = true,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
    };

    public DocusaurusDocsFormatter(
        ILogger<DocusaurusDocsFormatter> logger,
        MarkdownDocsFormatter markdownDocsFormatter,
        ConfigurationLoader configurationLoader)
    {
        _logger = logger;
        _markdownDocsFormatter = markdownDocsFormatter;
        _configurationLoader = configurationLoader;
    }

    public DocFormatter Formatter => DocFormatter.Docusaurus;

    public async Task<IImmutableList<GenerationFile>> GenerateModuleDocs(GeneratorContext context)
    {
        var files = await _markdownDocsFormatter.GenerateModuleDocs(context);
        var generationFiles = new List<GenerationFile>();
        var configuration = _configurationLoader.GetFormatterOptionsOrDefault<DocusaurusOptions>(context.FormatterOptions, Formatter);

        if (configuration.AddPageTags)
        {
            foreach (var generationFile in files)
            {
                if (generationFile is MarkdownGenerationFile mkFile)
                {
                    var resources = ResourceParser.ParseResources(mkFile.Model);
                    var pageTags = resources.Select(x => x.Provider).Distinct().ToArray();

                    var pageMeta = new PageMeta
                    {
                        Tags = pageTags
                    };

                    mkFile.Document.Prepend(new MdFrontMatter(pageMeta));
                    generationFiles.Add(mkFile);
                }
                else
                {
                    generationFiles.Add(generationFile);
                }
            }
        }
        else
        {
            generationFiles.AddRange(files);
        }


        // Build and add category files
        foreach (var markdownFile in files)
        {
            var prevPath = context.Paths.OutputBaseFolder;
            var relativePath = Path.GetRelativePath(context.Paths.OutputBaseFolder, markdownFile.FilePath);
            var folderPath = Path.GetDirectoryName(relativePath);

            if (string.IsNullOrEmpty(folderPath))
            {
                _logger.LogWarning("Failed to resolve folder path for: {FilePath}", markdownFile.FilePath);
                continue;
            }

            var pathElements = folderPath.Split('/');

            var prevElement = "";

            foreach (var element in pathElements)
            {
                if (prevElement == "versions")
                {
                    prevPath = Path.Join(prevPath, element);
                    continue;
                }

                var name = NameResolver.ResolveName(element);
                var category = new CategoryMeta(name);

                prevPath = Path.Join(prevPath, element);
                var output = Path.Join(prevPath, Constants.CategoryMetaFileName);

                generationFiles.Add(new TextGenerationFile(output, JsonSerializer.Serialize(category, _serializeOptions)));
                prevElement = element;
            }
        }

        return generationFiles.ToImmutableArray();
    }
}
