using System.Collections.Immutable;
using LandingZones.Tools.BicepDocs.Core.Abstractions;
using LandingZones.Tools.BicepDocs.Core.Models.Source;
using Microsoft.Extensions.Logging;

namespace LandingZones.Tools.BicepDocs.Source.AzureContainerRegistry;

public record AcrSourceOptions(
    string FolderPath,
    string OutFolder,
    string[]? Exclude) : SourceOptions;

public class AcrSource : IBicepSource
{
    private readonly ILogger<AcrSource> _logger;

    public AcrSource(ILogger<AcrSource> logger)
    {
        _logger = logger;
    }

    public DocSource Source => DocSource.AzureContainerRegistry;

    public Task<SourceResult> GetSourceFiles(SourceOptions options)
    {
        if (options is not AcrSourceOptions acrOptions)
        {
            throw new Exception("Failed to resolve options");
        }

        return Task.FromResult(new SourceResult(true, 1));
    }

    public async Task<string> GetSourceContent(SourceFile source)
    {
        return "";
    }
}