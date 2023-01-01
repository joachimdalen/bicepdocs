using LandingZones.Tools.BicepDocs.Core.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace LandingZones.Tools.BicepDocs.Formatter.Markdown;

public static class Installer
{
    public static IServiceCollection AddMarkdownDocFormatter(this IServiceCollection services)
    {
        return services.AddTransient<IDocsFormatter, MarkdownDocsFormatter>().AddTransient<MarkdownDocsFormatter>();
    }
}