using LandingZones.Tools.BicepDocs.Core;
using LandingZones.Tools.BicepDocs.Core.Abstractions;
using LandingZones.Tools.BicepDocs.Core.Models.Formatting;
using LandingZones.Tools.BicepDocs.Core.UnitTests;
using LandingZones.Tools.BicepDocs.Formatter.Markdown;
using LandingZones.Tools.BicepDocs.Formatter.Markdown.Models;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace LandingZones.Tools.BicepDocs.Formatter.Docusaurus.UnitTests;

[TestClass]
public class DocusaurusDocsProviderTests : BicepFileTestBase
{
    private readonly Mock<IStaticFileSystem> _staticFileSystem;
    private readonly ConfigurationLoader _configurationLoader;

    public DocusaurusDocsProviderTests()
    {
        _staticFileSystem = new Mock<IStaticFileSystem>(MockBehavior.Strict);
        _configurationLoader = new ConfigurationLoader(_staticFileSystem.Object);
    }

    [TestMethod]
    public async Task GenerateModuleDocs_NonVersionedModule_ReturnsSingleFile()
    {
        const string template = @"
param resourceGroupName string
param resourceGroupLocation string = subscription().location
param tags object = {}

metadata moduleDocs = {
  title: 'Resource Group'
  owner: 'Demo User <demo.user@local.test>'
  description: 'Deploy a new resource group with tags'
}

resource resourceGroup 'Microsoft.Resources/resourceGroups@2021-01-01' = {
  name: resourceGroupName
  location: resourceGroupLocation
  tags: tags
}";
        var semanticModel = await GetModel(template);
        var ctx = new GeneratorContext(semanticModel, GetPaths());
        var mdProvider = new MarkdownDocsProvider();
        var sut = new DocusaurusDocsProvider(NullLogger<DocusaurusDocsProvider>.Instance, mdProvider, _configurationLoader);

        var files = await sut.GenerateModuleDocs(ctx);
        Assert.AreEqual(2, files.Count);
        var otp = files[0];
        Assert.AreEqual("/output-folder/some-dir/docs/resources/resourceGroups.md", otp.FilePath);
        Assert.IsInstanceOfType(otp, typeof(MarkdownGenerationFile));
        Assert.IsNull(otp.VersionFilePath);
        Assert.IsNull(otp.VersionFolderPath);

        var meta = files[1];
        Assert.IsInstanceOfType(meta, typeof(TextGenerationFile));
        Assert.AreEqual(@"{
  ""label"": ""Resources""
}", (meta as TextGenerationFile)?.Content);
    }

    private ModulePaths GetPaths()
    {
        var inputFolder = "/input/folder/modules";
        var outputBaseFolder = "/output-folder/some-dir/docs";
        var inputFile = "/input/folder/modules/resources/resourceGroups.bicep";
        return PathResolver.ResolveModulePaths(
            bicepFilePath: inputFile,
            baseInputFolder: inputFolder,
            outputFolder: outputBaseFolder);
    }
}