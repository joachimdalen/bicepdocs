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

    public async Task<SemanticModel> GetSemanticModelFromContent(string fileContent, string fileName = "deploy.bicep")
    {
        const string basePath = "/modules";
        var filePath = Path.Join(basePath, fileName);
        if (!_fileSystem.Directory.Exists(basePath))
        {
            _fileSystem.Directory.CreateDirectory(basePath);
        }

        await _fileSystem.File.WriteAllTextAsync(filePath, fileContent);
        var compilation = await _compiler.CreateCompilation(new Uri(filePath));
        var sourceFile = compilation.GetEntrypointSemanticModel();
        return sourceFile;
    }


    public async Task<SemanticModel> GetSemanticModelFromPath(string bicepFilePath)
    {
        var fileName = Path.GetFileName(bicepFilePath);
        var fileContent = await File.ReadAllTextAsync(bicepFilePath);

        return await GetSemanticModelFromContent(fileContent, fileName);
    }
}