using System.Collections.Immutable;
using LandingZones.Tools.BicepDocs.Core.Abstractions;
using LandingZones.Tools.BicepDocs.Core.Models.Source;
using Microsoft.Extensions.Logging;

namespace LandingZones.Tools.BicepDocs.Source.FileSystem;

public record FileSystemSourceOptions(
    string FolderPath,
    string OutFolder,
    string[]? Exclude) : SourceOptions;

public class FileSystemSource : IBicepSource
{
    private readonly IStaticFileSystem _staticFileSystem;
    private readonly IMatcher _matcher;
    private readonly ILogger<FileSystemSource> _logger;

    private readonly string[] _includePaths =
    {
        "**/*.bicep"
    };

    public FileSystemSource(
        IStaticFileSystem staticFileSystem,
        IMatcher matcher,
        ILogger<FileSystemSource> logger)
    {
        _staticFileSystem = staticFileSystem;
        _matcher = matcher;
        _logger = logger;
    }

    public DocSource Source => DocSource.FileSystem;

    public async Task<SourceResult> GetSourceFiles(SourceOptions options)
    {
        if (options is not FileSystemSourceOptions fileSystemSourceOptions)
        {
            throw new Exception("Failed to resolve options");
        }

        _matcher.AddIncludePatterns(_includePaths);
        if (fileSystemSourceOptions.Exclude != null) _matcher.AddExcludePatterns(fileSystemSourceOptions.Exclude);

        _logger.LogInformation("Scanning for bicep files...this make take a minute or two");
        var bicepFiles = _matcher.GetResultsInFullPath(fileSystemSourceOptions.FolderPath)?.ToList();

        if (bicepFiles == null)
        {
            _logger.LogWarning("Failed to find any bicep files");
            return new SourceResult(false, -1);
        }

        if (bicepFiles.Count == 0)
        {
            _logger.LogWarning("Failed to find any bicep files");
            return new SourceResult(false, -1);
        }

        _logger.LogInformation("Found {FileCount} bicep files", bicepFiles.Count);

        return new SourceResult(true, 1, bicepFiles.Select(x => new SourceFile(x)).ToImmutableList());
    }

    public async Task<string> GetSourceContent(SourceFile source)
    {
        var fileContent = await _staticFileSystem.File.ReadAllTextAsync(source.Name);
        return fileContent;
    }
}