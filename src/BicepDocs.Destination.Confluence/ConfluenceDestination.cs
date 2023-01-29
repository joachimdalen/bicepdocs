using System.Collections.Immutable;
using LandingZones.Tools.BicepDocs.Core.Abstractions;
using LandingZones.Tools.BicepDocs.Core.Models.Destination;
using LandingZones.Tools.BicepDocs.Core.Models.Formatting;
using LandingZones.Tools.BicepDocs.Destination.Confluence.Abstractions;
using LandingZones.Tools.BicepDocs.Destination.Confluence.Services;

namespace LandingZones.Tools.BicepDocs.Destination.Confluence;

public class ConfluenceDestination : IDocsDestination
{
    private readonly IConfluenceServiceFactory _serviceFactory;
    private ConfluenceService? _client;

    public ConfluenceDestination(IConfluenceServiceFactory serviceFactory)
    {
        _serviceFactory = serviceFactory;
    }

    public DocDestination Destination => DocDestination.Confluence;
    public bool RequiresInput => true;

    public async Task Write(IImmutableList<GenerationFile> generationFiles, DestinationOptions? options = null)
    {
        if (options is not ConfluenceOptions confluenceOptions)
        {
            throw new Exception("Failed to resolve options");
        }

        //  "representation":"wiki" 
        if (_client == null)
        {
            _client = _serviceFactory.GetConfluenceClient(confluenceOptions.InstanceName, confluenceOptions.User,
                confluenceOptions.Token);
        }

        var rootPage = await _client.GetPageById(confluenceOptions.RootPageId);

        if (rootPage == null)
        {
            throw new Exception("Failed to resolve root page");
        }

        foreach (var file in generationFiles)
        {
        }


        throw new NotImplementedException();
    }
}