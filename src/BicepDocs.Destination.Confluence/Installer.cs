using Microsoft.Extensions.DependencyInjection;

namespace LandingZones.Tools.BicepDocs.Destination.Confluence;

// https://github.com/dotnet/command-line-api/issues/879
public static class Installer
{
    public static IServiceCollection AddConfluenceDestination(this IServiceCollection services)
    {
        
        return services;
    }
}

// bicepdocs filesystem filesystem 
// bicepdocs registry filesystem
// bicepdocs filesystem 

interface ISource
{
    // All required options for the source
    Type OptionType { get; set; }    
    
    // Get from source
}

interface IDest
{
    Type OptionType { get; set; }
    // Write to source
}


