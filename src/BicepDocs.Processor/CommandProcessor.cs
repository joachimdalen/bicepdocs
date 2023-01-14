using System.CommandLine.Invocation;
using LandingZones.Tools.BicepDocs.Core.Abstractions;
using LandingZones.Tools.BicepDocs.Core.Extensions;
using LandingZones.Tools.BicepDocs.Core.Models.Destination;
using LandingZones.Tools.BicepDocs.Core.Models.Source;

namespace LandingZones.Tools.BicepDocs.Processor;

public class CommandProcessor : ICommandHandler
{
    private readonly IEnumerable<IDocsFormatter> _formatters;
    private readonly IEnumerable<IDocsDestination> _destinations;
    private readonly IEnumerable<IBicepSource> _sources;

    public CommandProcessor(
        IEnumerable<IDocsFormatter> formatters,
        IEnumerable<IDocsDestination> destinations,
        IEnumerable<IBicepSource> sources
    )
    {
        _formatters = formatters;
        _destinations = destinations;
        _sources = sources;
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
        // Sources
        var sourceCommand = context.GetSourceCommand();
        if (sourceCommand == null) throw new Exception("Failed to resolve source");
        var source = context.GetSourceFromCommand(sourceCommand);
        var sourceHandler = GetSourceHandler(source);
        var sourceOptions = context.GetSourceOptions<SourceOptions>(sourceCommand);
        var sourceFiles = sourceHandler.GetSourceFiles(sourceOptions);


        // Formatter

        // Destinations
        var destination = context.GetDestinationFromCommand();
        var destinationHandler = GetDestinationHandler(destination);


        return 1;
    }
}