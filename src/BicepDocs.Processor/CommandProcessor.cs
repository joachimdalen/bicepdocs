using System.Collections.Immutable;
using System.CommandLine.Invocation;
using LandingZones.Tools.BicepDocs.Core;
using LandingZones.Tools.BicepDocs.Core.Abstractions;
using LandingZones.Tools.BicepDocs.Core.Commands;
using LandingZones.Tools.BicepDocs.Core.Extensions;
using LandingZones.Tools.BicepDocs.Core.Models.Destination;
using LandingZones.Tools.BicepDocs.Core.Models.Formatting;
using LandingZones.Tools.BicepDocs.Core.Models.Source;
using LandingZones.Tools.BicepDocs.Formatter.Markdown.Extensions;
using LandingZones.Tools.BicepDocs.Formatter.Markdown.Models;
using Microsoft.Extensions.Logging;

namespace LandingZones.Tools.BicepDocs.Processor;

public class CommandProcessor : ICommandHandler
{
    private readonly IEnumerable<IDocsFormatter> _formatters;
    private readonly IEnumerable<IDocsDestination> _destinations;
    private readonly IEnumerable<IBicepSource> _sources;
    private readonly IBicepFileService _bicepFileService;
    private readonly ConfigurationLoader _configurationLoader;
    private readonly ILogger<CommandProcessor> _logger;

    public CommandProcessor(
        IEnumerable<IDocsFormatter> formatters,
        IEnumerable<IDocsDestination> destinations,
        IEnumerable<IBicepSource> sources,
        IBicepFileService bicepFileService,
        ConfigurationLoader configurationLoader,
        ILogger<CommandProcessor> logger)
    {
        _formatters = formatters;
        _destinations = destinations;
        _sources = sources;
        _bicepFileService = bicepFileService;
        _configurationLoader = configurationLoader;
        _logger = logger;
    }


    private IDocsDestination GetDestinationHandler(DocDestination destination)
    {
        var destinationHandler = _destinations.FirstOrDefault(x => x.Destination == destination);
        if (destinationHandler == null)
        {
            throw new ArgumentNullException($"Failed to find destination provider for {destination}");
        }

        return destinationHandler;
    }

    private IBicepSource GetSourceHandler(DocSource source)
    {
        var sourceHandler = _sources.FirstOrDefault(x => x.Source == source);
        if (sourceHandler == null)
        {
            throw new ArgumentNullException($"Failed to find source provider for {source}");
        }

        return sourceHandler;
    }

    public int Invoke(InvocationContext context)
    {
        throw new NotImplementedException();
    }

    public async Task<int> InvokeAsync(InvocationContext context)
    {
        var configPath = context.BindingContext.ParseResult.GetValueForOption(GlobalOptions.ConfigPath);
        // Sources
        var sourceCommand = context.GetSourceCommand();
        if (sourceCommand == null) throw new Exception("Failed to resolve source");
        var source = context.GetSourceFromCommand(sourceCommand);
        var sourceHandler = GetSourceHandler(source);

        SourceOptions? sourceOptions = null;
        if (sourceHandler.RequiresInput)
        {
            sourceOptions = context.GetSourceOptions<SourceOptions>(sourceCommand);
            if (sourceOptions == null)
                throw new Exception("Failed to resolve source options");
        }

        // Formatter
        var formatter = context.BindingContext.ParseResult.GetValueForOption(DestinationCommand.Formatter);
        var formatProvider = _formatters.FirstOrDefault(x => x.Formatter == formatter);
        if (formatProvider == null)
        {
            throw new ArgumentNullException($"Failed to find generation provider for {formatter}");
        }

        // Destinations
        var destinationCommand = context.GetDestinationCommand();
        if (destinationCommand == null) throw new Exception("Failed to resolve source");
        var destination = context.GetDestinationFromCommand(destinationCommand);
        var destinationHandler = GetDestinationHandler(destination);

        DestinationOptions? destinationOptions = null;
        if (destinationHandler.RequiresInput)
        {
            destinationOptions = context.GetDestinationOptions<DestinationOptions>(destinationCommand);
            if (destinationOptions == null)
                throw new Exception("Failed to resolve destination options");
        }


        var sourceFiles = await sourceHandler.GetSourceFiles(sourceOptions ?? null);
        if (!sourceFiles.IsSuccess)
        {
            return sourceFiles.ReturnCode;
        }


        foreach (var bicepFile in sourceFiles.Files)
        {
            //var paths = PathResolver.ResolveModulePaths(bicepFile.Name, inputPath, outputPath);
            //_logger.LogInformation("Processing file {FileName}", paths.VirtualPath);

            var fileContent = await sourceHandler.GetSourceContent(bicepFile);
            var sourceFile = await _bicepFileService.GetSemanticModelFromContent(fileContent);

            FormatterOptions? formatterOptions = null;
            if (!string.IsNullOrEmpty(configPath))
            {
                formatterOptions = await _configurationLoader.GetOptions(configPath);
            }

            var formatterContext = new FormatterContext(sourceFile, paths, formatterOptions);
            var generationFiles = await formatProvider.GenerateModuleDocs(formatterContext);
            var convertedFiles = generationFiles.Select(x =>
            {
                if (x is MarkdownGenerationFile markdownGenerationFile)
                {
                    return markdownGenerationFile.ToTextGenerationFile();
                }

                return x;
            }).ToImmutableList();


            await destinationHandler.Write(convertedFiles, destinationOptions);
        }


        return 1;
    }
}