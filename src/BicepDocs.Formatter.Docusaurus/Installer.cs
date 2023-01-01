using LandingZones.Tools.BicepDocs.Core.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace LandingZones.Tools.BicepDocs.Formatter.Docusaurus;

public static class Installer
{
    public static IServiceCollection AddDocusaurusDocsFormatter(this IServiceCollection services)
    {
        return services.AddTransient<IDocsFormatter, DocusaurusDocsFormatter>();
    }
}