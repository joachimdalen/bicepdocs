using System.IO.Abstractions;
using Bicep.Core;
using Bicep.Core.Semantics;
using LandingZones.Tools.BicepDocs.Core.Abstractions;

namespace LandingZones.Tools.BicepDocs.Core.Services;

public class BicepFileService : IBicepFileService
{
    private readonly IFileSystem _fileSystem;
    private readonly BicepCompiler _compiler;

    public BicepFileService(IFileSystem fileSystem, BicepCompiler compiler)
    {
        _fileSystem = fileSystem;
        _compiler = compiler;
    }

    public async Task<SemanticModel> GetSemanticModelFromContent(string folder, string path, string content)
    {
        _fileSystem.Directory.CreateDirectory(folder);
        await _fileSystem.File.WriteAllTextAsync(path, content);
        var compilation = await _compiler.CreateCompilation(new Uri(path));
        var sourceFile = compilation.GetEntrypointSemanticModel();
        return sourceFile;
    }
}