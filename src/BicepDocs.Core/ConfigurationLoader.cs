using LandingZones.Tools.BicepDocs.Core.Abstractions;
using LandingZones.Tools.BicepDocs.Core.Models.Formatting;
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
    /// <returns>Instance of <see cref="FormatterOptions"/></returns>
    public async Task<FormatterOptions> GetOptions(string filePath)
    {
        var deserializer = new DeserializerBuilder()
            .WithNamingConvention(CamelCaseNamingConvention.Instance)
            .IgnoreUnmatchedProperties()
            .Build();
        var content = await _fileSystem.File.ReadAllTextAsync(filePath);
        return deserializer.Deserialize<FormatterOptions>(content);
    }

    public T? GetFormatterOptions<T>(FormatterOptions options, DocFormatter formatterKey) where T : class, new()
    {
        if (!options.Formatters.TryGetValue(formatterKey, out var providerString))
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

    public T GetFormatterOptionsOrDefault<T>(FormatterOptions options, DocFormatter formatterKey) where T : class, new()
    {
        var opts = GetFormatterOptions<T>(options, formatterKey);
        return opts ?? new T();
    }
}