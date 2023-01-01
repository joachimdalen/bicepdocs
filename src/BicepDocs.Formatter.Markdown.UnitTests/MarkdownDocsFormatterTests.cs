using LandingZones.Tools.BicepDocs.Core;
using LandingZones.Tools.BicepDocs.Core.Models.Formatting;
using LandingZones.Tools.BicepDocs.Core.UnitTests;
using LandingZones.Tools.BicepDocs.Formatter.Markdown.Models;

namespace LandingZones.Tools.BicepDocs.Formatter.Markdown.UnitTests;

[TestClass]
public class MarkdownDocsFormatterTests : BicepFileTestBase
{
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
        var sut = new MarkdownDocsFormatter();

        var files = await sut.GenerateModuleDocs(ctx);
        Assert.AreEqual(1, files.Count);
        var otp = files[0];
        Assert.AreEqual("/output-folder/some-dir/docs/resources/resourceGroups.md", otp.FilePath);
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
        var sut = new MarkdownDocsFormatter();

        var files = await sut.GenerateModuleDocs(ctx);
        Assert.AreEqual(1, files.Count);
        var otp = files[0];
        Assert.AreEqual("/output-folder/some-dir/docs/resources/resourceGroups.md", otp.FilePath);
        Assert.IsInstanceOfType(otp, typeof(MarkdownGenerationFile));
        Assert.AreEqual("/output-folder/some-dir/docs/resources/versions/2022-12-26/resourceGroups.md", otp.VersionFilePath);
        Assert.AreEqual("/output-folder/some-dir/docs/resources/resourceGroups.md", otp.FilePath);

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