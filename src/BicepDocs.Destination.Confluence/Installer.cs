using LandingZones.Tools.BicepDocs.Core.Abstractions;
using LandingZones.Tools.BicepDocs.Destination.Confluence.Abstractions;
using LandingZones.Tools.BicepDocs.Destination.Confluence.Services;
using Microsoft.Extensions.DependencyInjection;

namespace LandingZones.Tools.BicepDocs.Destination.Confluence;

// https://github.com/dotnet/command-line-api/issues/879
public static class Installer
{
    public static IServiceCollection AddConfluenceDestination(this IServiceCollection services)
    {
        services.AddSingleton<IConfluenceServiceFactory, ConfluenceServiceFactory>();
        services.AddSingleton<IDocsDestination, ConfluenceDestination>();
        return services;
    }
}