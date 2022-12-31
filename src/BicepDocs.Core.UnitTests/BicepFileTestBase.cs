using System.IO.Abstractions;
using Bicep.Core;
using Bicep.Core.Semantics;
using Microsoft.Extensions.DependencyInjection;

namespace LandingZones.Tools.BicepDocs.Core.UnitTests;

public abstract class BicepFileTestBase
{
    protected readonly IFileSystem FileSystem;
    protected readonly IServiceProvider ServiceProvider;

    protected BicepFileTestBase()
    {
        var sp = new ServiceCollection();
        sp.AddBicepCore().AddBicepFileService();
        ServiceProvider = new DefaultServiceProviderFactory().CreateBuilder(sp).BuildServiceProvider();
        FileSystem = ServiceProvider.GetRequiredService<IFileSystem>();
    }

    protected async Task<SemanticModel> GetModelFromPath(string filePath)
    {
        var fileName = Path.GetFileName(filePath);
        var fileContent = await File.ReadAllTextAsync(filePath);
        return await GetModel(fileContent, fileName);
    }

    protected async Task<SemanticModel> GetModel(string fileContent, string fileName = "deploy.bicep")
    {
        var vPath = Path.Join("/modules", fileName);
        FileSystem.Directory.CreateDirectory("/modules");
        await FileSystem.File.WriteAllTextAsync(vPath, fileContent);
        var compiler = ServiceProvider.GetRequiredService<BicepCompiler>();
        var compilation = await compiler.CreateCompilation(new Uri(vPath));
        return compilation.GetEntrypointSemanticModel();
    }
}