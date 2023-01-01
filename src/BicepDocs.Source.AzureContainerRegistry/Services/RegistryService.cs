using System.Collections.Immutable;
using Azure.Containers.ContainerRegistry;
using Azure.Containers.ContainerRegistry.Specialized;
using Azure.Identity;
using LandingZones.Tools.BicepDocs.Source.AzureContainerRegistry.Abstractions;
using Newtonsoft.Json;

namespace LandingZones.Tools.BicepDocs.Source.AzureContainerRegistry.Services;

public class RegistryService : IRegistryService
{
    // https://github.com/Azure/bicep/blob/0e1aee25c9afee3e4d287b95864f40bd4d48305b/src/Bicep.Core/Registry/AzureContainerRegistryManager.cs#L94
    public async Task<IImmutableList<string>> GetRepositories(string registryName)
    {
        var endpoint = new Uri($"https://{registryName}.azurecr.io");
        var client = new ContainerRegistryClient(endpoint, new DefaultAzureCredential(),
            new ContainerRegistryClientOptions()
            {
                Audience = ContainerRegistryAudience.AzureResourceManagerPublicCloud
            });

        var s = await client.GetRepositoryNamesAsync().ToListAsync();

        var firstRepo = s.First();

        var repoDetails = client.GetRepository(firstRepo);


        var mani = await repoDetails.GetAllManifestPropertiesAsync().ToListAsync();

        var artifact = repoDetails.GetArtifact(mani.OrderBy(x => x.CreatedOn.Date).First().Digest);

        return s.ToImmutableList();
    }
}