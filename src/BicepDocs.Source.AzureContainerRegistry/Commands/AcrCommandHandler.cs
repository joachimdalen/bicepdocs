using System.CommandLine.Invocation;
using LandingZones.Tools.BicepDocs.Source.AzureContainerRegistry.Abstractions;
using Microsoft.Extensions.Logging;

namespace LandingZones.Tools.BicepDocs.Source.AzureContainerRegistry.Commands;

public class AcrCommandHandler : ICommandHandler
{
    private readonly ILogger<AcrCommandHandler> _logger;
    private readonly IRegistryService _registryService;

    public string RegistryName { get; set; } = default!;

    public AcrCommandHandler(ILogger<AcrCommandHandler> logger, IRegistryService registryService)
    {
        _logger = logger;
        _registryService = registryService;
    }


    public int Invoke(InvocationContext context)
    {
        throw new NotImplementedException();
    }

    public async Task<int> InvokeAsync(InvocationContext context)
    {
        _logger.LogInformation("Loading repositories from registry {RegistryName}...", RegistryName);
        _logger.LogInformation("This might take a while...");
        var repos = await _registryService.GetRepositories(RegistryName);

        _logger.LogInformation("Loaded {RepositoryCount} repositories...", repos.Count);

        return 1;
    }
}