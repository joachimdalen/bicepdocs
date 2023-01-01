using System.Collections.Immutable;

namespace LandingZones.Tools.BicepDocs.Source.AzureContainerRegistry.Abstractions;

public interface IRegistryService
{
    Task<IImmutableList<string>> GetRepositories(string registryName);
}