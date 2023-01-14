using System.CommandLine.Hosting;
using LandingZones.Tools.BicepDocs.Core.Abstractions;
using LandingZones.Tools.BicepDocs.Source.FileSystem.Commands;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace LandingZones.Tools.BicepDocs.Source.FileSystem;

public static class Installer
{
    public static IHostBuilder AddFileSystemCommands(this IHostBuilder builder)
    {
        return builder.UseCommandHandler<FileSystemSourceCommand, FileSystemCommandHandler>();
    }

    public static IServiceCollection AddFileSystemSource(this IServiceCollection services)
    {
        return services.AddTransient<IBicepSource, FileSystemSource>();
    }
}