using LandingZones.Tools.BicepDocs.Core.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace LandingZones.Tools.BicepDocs.Formatter.Markdown;

public static class Installer
{
    public static IServiceCollection AddMarkdownDocProvider(this IServiceCollection services)
    {
        return services.AddTransient<IDocsProvider, MarkdownDocsProvider>().AddTransient<MarkdownDocsProvider>();
    }
}