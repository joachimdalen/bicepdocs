using System.Collections.Immutable;
using System.CommandLine.Invocation;
using System.Diagnostics.CodeAnalysis;
using LandingZones.Tools.BicepDocs.Core;
using LandingZones.Tools.BicepDocs.Core.Abstractions;
using LandingZones.Tools.BicepDocs.Core.Models.Destination;
using LandingZones.Tools.BicepDocs.Core.Models.Formatting;
using LandingZones.Tools.BicepDocs.Core.Models.Source;
using LandingZones.Tools.BicepDocs.Formatter.Markdown.Extensions;
using LandingZones.Tools.BicepDocs.Formatter.Markdown.Models;
using Microsoft.Extensions.Logging;

namespace LandingZones.Tools.BicepDocs.Source.FileSystem.Commands;

[SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
public sealed class FileSystemCommandHandler : ICommandHandler
{
    private readonly ILogger<FileSystemCommandHandler> _logger;
    private readonly IEnumerable<IDocsFormatter> _generators;
    private readonly IEnumerable<IDocsDestination> _destinations;
    private readonly IEnumerable<IBicepSource> _sources;
    private readonly IStaticFileSystem _staticFileSystem;
    private readonly ConfigurationLoader _configurationLoader;
    private readonly IBicepFileService _bicepFileService;
    public string FolderPath { get; set; } = default!;
    public string Out { get; set; } = default!;
    public DocFormatter Formatter { get; set; } = default!;
    public string? Config { get; set; } = null;
    public string[]? Exclude { get; set; }

    public bool DryRun { get; set; }

    public FileSystemCommandHandler(
        ILogger<FileSystemCommandHandler> logger,
        IEnumerable<IDocsFormatter> generators,
        IEnumerable<IDocsDestination> destinations,
        IEnumerable<IBicepSource> sources,
        IStaticFileSystem staticFileSystem,
        ConfigurationLoader configurationLoader,
        IBicepFileService bicepFileService
    )
    {
        _logger = logger;
        _generators = generators;
        _destinations = destinations;
        _sources = sources;
        _staticFileSystem = staticFileSystem;
        _configurationLoader = configurationLoader;
        _bicepFileService = bicepFileService;
    }

    public int Invoke(InvocationContext context)
    {
        throw new NotImplementedException();
    }

    public async Task<int> InvokeAsync(InvocationContext context)
    {
        var formatProvider = _generators.FirstOrDefault(x => x.Formatter == Formatter);
        if (formatProvider == null)
        {
            throw new ArgumentNullException($"Failed to find generation provider for {Formatter}");
        }

        var destinationProvider = _destinations.FirstOrDefault(x => x.Destination == DocDestination.FileSystem);
        if (destinationProvider == null)
        {
            throw new ArgumentNullException($"Failed to find destination provider for {DocDestination.FileSystem}");
        }

        var fileSystemSource = _sources.FirstOrDefault(x => x.Source == DocSource.FileSystem);
        if (fileSystemSource == null)
        {
            throw new ArgumentNullException($"Failed to find source provider for {DocSource.FileSystem}");
        }

        var bicepFiles = await fileSystemSource.GetSourceFiles(new FileSystemSourceOptions(FolderPath, Out, Exclude));

        if (!bicepFiles.IsSuccess)
        {
            return bicepFiles.ReturnCode;
        }


        foreach (var bicepFile in bicepFiles.Files)
        {
            var paths = PathResolver.ResolveModulePaths(bicepFile.Name, FolderPath, Out);
            if (DryRun)
            {
                _logger.LogInformation("[DRY-RUN]: Processing file {FileName}", paths.VirtualPath);
            }
            else
            {
                _logger.LogInformation("Processing file {FileName}", paths.VirtualPath);
            }

            var fileContent = await fileSystemSource.GetSourceContent(bicepFile);
            var sourceFile =
                await _bicepFileService.GetSemanticModelFromContent(paths.VirtualFolder, paths.VirtualPath,
                    fileContent);

            FormatterOptions? generatorOptions = null;
            if (!string.IsNullOrEmpty(Config))
            {
                generatorOptions = await _configurationLoader.GetOptions(Config);
            }

            var generatorContext = new GeneratorContext(sourceFile, paths, generatorOptions);
            var generationFiles = await formatProvider.GenerateModuleDocs(generatorContext);
            var convertedFiles = generationFiles.Select(x =>
            {
                if (x is MarkdownGenerationFile markdownGenerationFile)
                {
                    return markdownGenerationFile.ToTextGenerationFile();
                }

                return x;
            }).ToImmutableList();


            await destinationProvider.Write(convertedFiles);
        }

        return 1;
    }
}