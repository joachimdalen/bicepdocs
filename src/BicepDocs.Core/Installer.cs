using System.Collections.Immutable;
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
using Bicep.Core.TypeSystem;
using Bicep.Core.TypeSystem.Az;
using Bicep.Core.Utils;
using Bicep.Decompiler;
using LandingZones.Tools.BicepDocs.Core.Abstractions;
using LandingZones.Tools.BicepDocs.Core.Services;
using Microsoft.Extensions.DependencyInjection;


namespace LandingZones.Tools.BicepDocs.Core;

public class EmptyModuleRegistryProvider : IArtifactRegistryProvider
{
    public ImmutableArray<IArtifactRegistry> Registries(Uri _) => ImmutableArray<IArtifactRegistry>.Empty;
}

public static class Installer
{
    public static IServiceCollection AddBicepCore(this IServiceCollection services) => services
        .AddSingleton<IResourceTypeLoader, AzResourceTypeLoader>()
        .AddSingleton<IResourceTypeLoaderFactory, AzResourceTypeLoaderFactory>()
        .AddSingleton<INamespaceProvider, DefaultNamespaceProvider>()
        .AddSingleton<IModuleDispatcher, ModuleDispatcher>()
        .AddSingleton<IArtifactRegistryProvider, EmptyModuleRegistryProvider>()
        .AddSingleton<ITokenCredentialFactory, TokenCredentialFactory>()
        .AddSingleton<IFileResolver, FileResolver>()
        .AddSingleton<IEnvironment, Bicep.Core.Utils.Environment>()
        .AddSingleton<IFileSystem, MockFileSystem>()
        .AddSingleton<IConfigurationManager, ConfigurationManager>()
        .AddSingleton<IBicepAnalyzer, LinterAnalyzer>()
        .AddSingleton<IFeatureProviderFactory, FeatureProviderFactory>()
        .AddSingleton<ILinterRulesProvider, LinterRulesProvider>()
        .AddSingleton<BicepCompiler>();

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