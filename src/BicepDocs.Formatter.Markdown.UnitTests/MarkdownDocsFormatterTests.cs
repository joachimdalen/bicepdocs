using LandingZones.Tools.BicepDocs.Core;
using LandingZones.Tools.BicepDocs.Core.Abstractions;
using LandingZones.Tools.BicepDocs.Core.Extensions;
using LandingZones.Tools.BicepDocs.Core.Models.Formatting;
using LandingZones.Tools.BicepDocs.Core.UnitTests;
using LandingZones.Tools.BicepDocs.Formatter.Markdown.Models;
using Moq;

namespace LandingZones.Tools.BicepDocs.Formatter.Markdown.UnitTests;

[TestClass]
public class MarkdownDocsFormatterTests : BicepFileTestBase
{
    private readonly ConfigurationLoader _configurationLoader;

    public MarkdownDocsFormatterTests()
    {
        Mock<IStaticFileSystem> staticFileSystem = new(MockBehavior.Strict);
        _configurationLoader = new ConfigurationLoader(staticFileSystem.Object);
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
        var ctx = new FormatterContext(semanticModel, GetPaths());
        var sut = new MarkdownDocsFormatter(_configurationLoader);

        var files = await sut.GenerateModuleDocs(ctx);
        Assert.AreEqual(1, files.Count);
        var otp = files[0];
        Assert.AreEqual("/output-folder/some-dir/docs/resources/resourceGroups.md".ToPlatformPath(), otp.FilePath);
        Assert.IsInstanceOfType(otp, typeof(MarkdownGenerationFile));
        Assert.IsNull(otp.VersionFilePath);
        Assert.IsNull(otp.VersionFolderPath);
    }
    
    [TestMethod]
    public async Task GenerateModuleDocs_VersionedModule_ReturnsSingleFile()
    {
        const string template = @"
param resourceGroupName string
param resourceGroupLocation string = subscription().location
param tags object = {}

metadata moduleDocs = {
  title: 'Resource Group'
  owner: 'Demo User <demo.user@local.test>'
  description: 'Deploy a new resource group with tags'
  version: '2022-12-26'
}

resource resourceGroup 'Microsoft.Resources/resourceGroups@2021-01-01' = {
  name: resourceGroupName
  location: resourceGroupLocation
  tags: tags
}";
        var semanticModel = await GetModel(template);
        var ctx = new FormatterContext(semanticModel, GetPaths());
        var sut = new MarkdownDocsFormatter(_configurationLoader);

        var files = await sut.GenerateModuleDocs(ctx);
        Assert.AreEqual(1, files.Count);
        var otp = files[0];
        Assert.AreEqual("/output-folder/some-dir/docs/resources/resourceGroups.md".ToPlatformPath(), otp.FilePath);
        Assert.IsInstanceOfType(otp, typeof(MarkdownGenerationFile));
        Assert.AreEqual("/output-folder/some-dir/docs/resources/versions/2022-12-26/resourceGroups.md".ToPlatformPath(), otp.VersionFilePath);
        Assert.AreEqual("/output-folder/some-dir/docs/resources/resourceGroups.md".ToPlatformPath(), otp.FilePath);

    }
    private ModulePaths GetPaths()
    {
        var inputFolder = "/input/folder/modules".ToPlatformPath();
        var outputBaseFolder = "/output-folder/some-dir/docs".ToPlatformPath();
        var inputFile = "/input/folder/modules/resources/resourceGroups.bicep".ToPlatformPath();
        return PathResolver.ResolveModulePaths(
            bicepFilePath: inputFile,
            baseInputFolder: inputFolder,
            outputFolder: outputBaseFolder);
    }
}
