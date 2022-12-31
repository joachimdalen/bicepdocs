using LandingZones.Tools.BicepDocs.Core.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace LandingZones.Tools.BicepDocs.Formatter.Docusaurus;

public static class Installer
{
    public static IServiceCollection AddDocusaurusDocsProvider(this IServiceCollection services)
    {
        return services.AddTransient<IDocsProvider, DocusaurusDocsProvider>();
    }
}