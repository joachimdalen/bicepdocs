using System.CommandLine.Hosting;
using LandingZones.Tools.BicepDocs.Core.Abstractions;
using LandingZones.Tools.BicepDocs.Source.AzureContainerRegistry.Abstractions;
using LandingZones.Tools.BicepDocs.Source.AzureContainerRegistry.Commands;
using LandingZones.Tools.BicepDocs.Source.AzureContainerRegistry.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace LandingZones.Tools.BicepDocs.Source.AzureContainerRegistry;

public static class Installer
{
    public static IHostBuilder AddAzureContainerRegistryCommands(this IHostBuilder builder)
    {
        return builder.UseCommandHandler<AcrCommand, AcrCommandHandler>();
    }

    public static IServiceCollection AddAzureContainerRegistrySource(this IServiceCollection services)
    {
        return services.AddTransient<IBicepSource, AcrSource>().AddTransient<IRegistryService, RegistryService>();
    }
}