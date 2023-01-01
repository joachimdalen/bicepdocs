using System.CommandLine;

namespace LandingZones.Tools.BicepDocs.Source.AzureContainerRegistry.Commands;

public class AcrCommand : Command
{
    public readonly Option<string> RegistryName =
        new(name: "--registryName", description: "The name of the container registry")
        {
            IsRequired = true
        };


    public AcrCommand() : base("registry", "Generate documentation for modules from a Azure Container Registry")
    {
        AddOption(RegistryName);
    }
}