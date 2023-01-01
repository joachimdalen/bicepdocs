using System.IO.Abstractions;
using System.IO.Abstractions.TestingHelpers;
using Bicep.Core;
using Bicep.Core.Analyzers.Interfaces;
using Bicep.Core.Analyzers.Linter;
using Bicep.Core.Analyzers.Linter.ApiVersions;
using Bicep.Core.Configuration;
using Bicep.Core.Features;
using Bicep.Core.FileSystem;
using Bicep.Core.Registry;
using Bicep.Core.Registry.Auth;
using Bicep.Core.Semantics.Namespaces;
using Bicep.Core.TypeSystem.Az;
using Bicep.Decompiler;
using LandingZones.Tools.BicepDocs.Core.Abstractions;
using LandingZones.Tools.BicepDocs.Core.Services;
using Microsoft.Extensions.DependencyInjection;
using BicepConfigurationManager = Bicep.Core.Configuration.ConfigurationManager;

namespace LandingZones.Tools.BicepDocs.Core;

public static class Installer
{
    public static IServiceCollection AddBicepCore(this IServiceCollection services) => services
        .AddScoped<INamespaceProvider, DefaultNamespaceProvider>()
        .AddScoped<IAzResourceTypeLoader, AzResourceTypeLoader>()
        .AddScoped<IContainerRegistryClientFactory, ContainerRegistryClientFactory>()
        .AddScoped<ITemplateSpecRepositoryFactory, TemplateSpecRepositoryFactory>()
        .AddScoped<IModuleDispatcher, ModuleDispatcher>()
        .AddScoped<IModuleRegistryProvider, DefaultModuleRegistryProvider>()
        .AddScoped<ITokenCredentialFactory, TokenCredentialFactory>()
        .AddScoped<IFileResolver, FileResolver>()
        .AddScoped<IFileSystem, MockFileSystem>()
        .AddScoped<IConfigurationManager, BicepConfigurationManager>()
        .AddScoped<IApiVersionProviderFactory, ApiVersionProviderFactory>()
        .AddScoped<IBicepAnalyzer, LinterAnalyzer>()
        .AddScoped<IFeatureProviderFactory, FeatureProviderFactory>()
        .AddScoped<ILinterRulesProvider, LinterRulesProvider>()
        .AddScoped<BicepCompiler>();

    public static IServiceCollection AddBicepDecompiler(this IServiceCollection services) => services
        .AddScoped<BicepDecompiler>();

    public static IServiceCollection AddStaticFileSystem(this IServiceCollection services)
    {
        return services.AddSingleton<IStaticFileSystem, StaticFileSystem>().AddTransient<IMatcher, MatcherWrapper>();
    }

    public static IServiceCollection AddConfigurationLoader(this IServiceCollection services)
    {
        return services.AddSingleton<ConfigurationLoader>();
    }

    public static IServiceCollection AddBicepFileService(this IServiceCollection services)
    {
        return services.AddTransient<IBicepFileService, BicepFileService>();
    }
}