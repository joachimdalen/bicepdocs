using LandingZones.Tools.BicepDocs.Core.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace LandingZones.Tools.BicepDocs.Destination.FileSystem;

public static class Installer
{
    public static IServiceCollection AddFileSystemDestination(this IServiceCollection services)
    {
        return services.AddTransient<IDocsDestination, FileSystemDestination>();
    }
}