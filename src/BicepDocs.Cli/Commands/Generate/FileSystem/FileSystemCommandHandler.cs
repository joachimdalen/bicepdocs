using System.CommandLine.Invocation;
using System.Diagnostics.CodeAnalysis;
using System.IO.Abstractions;
using Bicep.Core;
using LandingZones.Tools.BicepDocs.Core;
using LandingZones.Tools.BicepDocs.Core.Abstractions;
using LandingZones.Tools.BicepDocs.Core.Models;
using LandingZones.Tools.BicepDocs.Provider.Markdown.Models;
using Microsoft.Extensions.Logging;

namespace LandingZones.Tools.BicepDocs.Cli.Commands.Generate.FileSystem;

[SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
public sealed class FileSystemCommandHandler : ICommandHandler
{
    private readonly ILogger<FileSystemCommandHandler> _logger;
    private readonly BicepCompiler _compiler;
    private readonly IFileSystem _fileSystem;
    private readonly IEnumerable<IDocsProvider> _generators;
    private readonly IStaticFileSystem _staticFileSystem;
    private readonly ConfigurationLoader _configurationLoader;
    private readonly IMatcher _matcher;
    public string FolderPath { get; set; } = default!;
    public string Out { get; set; } = default!;
    public DocProvider Provider { get; set; } = default!;
    public string? Config { get; set; } = null;
    public string[]? Exclude { get; set; }

    public bool DryRun { get; set; }

    public FileSystemCommandHandler(ILogger<FileSystemCommandHandler> logger, BicepCompiler compiler,
        IFileSystem fileSystem, IEnumerable<IDocsProvider> generators, IStaticFileSystem staticFileSystem,
        ConfigurationLoader configurationLoader, IMatcher matcher)
    {
        _logger = logger;
        _compiler = compiler;
        _fileSystem = fileSystem;
        _generators = generators;
        _staticFileSystem = staticFileSystem;
        _configurationLoader = configurationLoader;
        _matcher = matcher;
    }

    public int Invoke(InvocationContext context)
    {
        throw new NotImplementedException();
    }

    public async Task<int> InvokeAsync(InvocationContext context)
    {
        var generator = _generators.First(x => x.Provider == Provider);

        var includePaths = new[]
        {
            "**/*.bicep"
        };


        _matcher.AddIncludePatterns(includePaths);
        if (Exclude != null) _matcher.AddExcludePatterns(Exclude);


        _logger.LogInformation("Scanning for bicep files...this make take a minute or two");
        var bicepFiles = _matcher.GetResultsInFullPath(FolderPath)?.ToList();

        if (bicepFiles == null)
        {
            _logger.LogWarning("Failed to find any bicep files");
            return 0;
        }

        if (bicepFiles.Count == 0)
        {
            _logger.LogWarning("Failed to find any bicep files");
            return 0;
        }

        _logger.LogInformation("Found {FileCount} bicep files", bicepFiles.Count);

        if (!_staticFileSystem.Directory.Exists(Out) && !DryRun)
        {
            _staticFileSystem.Directory.CreateDirectory(Out);
        }

        foreach (var bicepFile in bicepFiles)
        {
            var paths = PathResolver.ResolveModulePaths(bicepFile, FolderPath, Out);
            if (DryRun)
            {
                _logger.LogInformation("[DRY-RUN]: Processing file {FileName}", paths.VirtualPath);
            }
            else
            {
                _logger.LogInformation("Processing file {FileName}", paths.VirtualPath);
            }


            var fileContent = await _staticFileSystem.File.ReadAllTextAsync(bicepFile);
            _fileSystem.Directory.CreateDirectory(paths.VirtualFolder);
            await _fileSystem.File.WriteAllTextAsync(paths.VirtualPath, fileContent);
            var compilation = await _compiler.CreateCompilation(new Uri(paths.VirtualPath));
            var sourceFile = compilation.GetEntrypointSemanticModel();

            GeneratorOptions? generatorOptions = null;
            if (!string.IsNullOrEmpty(Config))
            {
                generatorOptions = await _configurationLoader.GetOptions(Config);
            }

            var generatorContext = new GeneratorContext(sourceFile, paths, generatorOptions);
            var f = await generator.GenerateModuleDocs(generatorContext);

            foreach (var outFile in f)
            {
                _logger.LogInformation("Processed file  {FileName} ==> {OutPath}",
                    paths.VirtualPath, Path.GetRelativePath(Out, outFile.FilePath));

                if (!DryRun)
                    switch (outFile)
                    {
                        case MarkdownGenerationFile mdFile:
                        {
                            await WriteFile(mdFile.FolderPath, mdFile.FilePath, mdFile.Document.ToMarkdown());
                            if (!string.IsNullOrEmpty(mdFile.VersionFilePath) &&
                                !string.IsNullOrEmpty(mdFile.VersionFolderPath))
                            {
                                await WriteFile(mdFile.VersionFolderPath, mdFile.VersionFilePath,
                                    mdFile.Document.ToMarkdown());
                            }

                            break;
                        }

                        case TextGenerationFile txtFile:
                        {
                            await WriteFile(txtFile.FolderPath, txtFile.FilePath, txtFile.Content);
                            if (!string.IsNullOrEmpty(txtFile.VersionFilePath) &&
                                !string.IsNullOrEmpty(txtFile.VersionFolderPath))
                            {
                                await WriteFile(txtFile.VersionFolderPath, txtFile.VersionFilePath,
                                    txtFile.Content);
                            }

                            break;
                        }
                    }
            }
        }

        return 1;
    }

    private async Task WriteFile(string folderPath, string filePath, string content)
    {
        if (!_staticFileSystem.Directory.Exists(folderPath))
        {
            _staticFileSystem.Directory.CreateDirectory(folderPath);
        }

        await _staticFileSystem.File.WriteAllTextAsync(filePath, content);
    }
}