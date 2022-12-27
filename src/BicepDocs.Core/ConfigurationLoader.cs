using LandingZones.Tools.BicepDocs.Core.Abstractions;
using LandingZones.Tools.BicepDocs.Core.Models;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace LandingZones.Tools.BicepDocs.Core;

public sealed class ConfigurationLoader
{
    private readonly IStaticFileSystem _fileSystem;

    public ConfigurationLoader(IStaticFileSystem fileSystem)
    {
        _fileSystem = fileSystem;
    }

    /// <summary>
    /// Get the configuration file from path given
    /// </summary>
    /// <param name="filePath">Path to the yaml configuration file</param>
    /// <returns>Instance of <see cref="GeneratorOptions"/></returns>
    public async Task<GeneratorOptions> GetOptions(string filePath)
    {
        var deserializer = new DeserializerBuilder()
            .WithNamingConvention(CamelCaseNamingConvention.Instance)
            .IgnoreUnmatchedProperties()
            .Build();
        var content = await _fileSystem.File.ReadAllTextAsync(filePath);
        return deserializer.Deserialize<GeneratorOptions>(content);
    }

    public T? GetProviderOptions<T>(GeneratorOptions options, DocProvider providerKey) where T : class, new()
    {
        if (!options.Providers.TryGetValue(providerKey, out var providerString))
        {
            return null;
        }

        var ser = new SerializerBuilder().Build();
        var s = ser.Serialize(providerString);
        var deserializer = new DeserializerBuilder()
            .WithNamingConvention(CamelCaseNamingConvention.Instance)
            .Build();
        return deserializer.Deserialize<T>(s);
    }

    public T GetProviderOptionsOrDefault<T>(GeneratorOptions options, DocProvider providerKey) where T : class, new()
    {
        var opts = GetProviderOptions<T>(options, providerKey);
        return opts ?? new T();
    }
}