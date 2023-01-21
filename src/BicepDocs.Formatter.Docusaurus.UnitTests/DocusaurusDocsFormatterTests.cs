using LandingZones.Tools.BicepDocs.Core;
using LandingZones.Tools.BicepDocs.Core.Abstractions;
using LandingZones.Tools.BicepDocs.Core.Extensions;
using LandingZones.Tools.BicepDocs.Core.Models.Formatting;
using LandingZones.Tools.BicepDocs.Core.UnitTests;
using LandingZones.Tools.BicepDocs.Formatter.Docusaurus.Models.Markdown;
using LandingZones.Tools.BicepDocs.Formatter.Markdown;
using LandingZones.Tools.BicepDocs.Formatter.Markdown.Models;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace LandingZones.Tools.BicepDocs.Formatter.Docusaurus.UnitTests;

[TestClass]
public class DocusaurusDocsFormatterTests : BicepFileTestBase
{
    private readonly ConfigurationLoader _configurationLoader;

    public DocusaurusDocsFormatterTests()
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
        var ctx = new FormatterContext(semanticModel, "resources/resourceGroups.bicep".ToPlatformPath());
        var mdFormatter = new MarkdownDocsFormatter(_configurationLoader);
        var sut = new DocusaurusDocsFormatter(NullLogger<DocusaurusDocsFormatter>.Instance, mdFormatter,
            _configurationLoader);

        var files = await sut.GenerateModuleDocs(ctx);
        Assert.AreEqual(2, files.Count);
        var otp = files[0];
        Assert.AreEqual("resources/resourceGroups.md".ToPlatformPath(), otp.FilePath);
        Assert.IsNull(otp.VersionFilePath);
        Assert.IsNull(otp.VersionFolderPath);

        var mdFile = otp as MarkdownGenerationFile;
        Assert.IsNotNull(mdFile);
        var firstElement = mdFile.Document[0];
        Assert.IsInstanceOfType<MdFrontMatter>(firstElement);

        var meta = files[1];
        Assert.IsInstanceOfType(meta, typeof(TextGenerationFile));
        Assert.AreEqual(@"{
  ""label"": ""Resources""
}".ReplaceLineEndings(), (meta as TextGenerationFile)?.Content);
    }

    [TestMethod]
    public async Task GenerateModuleDocs_AddTagsDisabled_DoesNotAdd()
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
        var ctx = new FormatterContext(semanticModel, "/resources/resourceGroups.bicep", new FormatterOptions
        {
            Formatters = new Dictionary<DocFormatter, object>
            {
                {
                    DocFormatter.Docusaurus, new { addPageTags = false }
                }
            }
        });
        var mdFormatter = new MarkdownDocsFormatter(_configurationLoader);
        var sut = new DocusaurusDocsFormatter(NullLogger<DocusaurusDocsFormatter>.Instance, mdFormatter,
            _configurationLoader);

        var files = await sut.GenerateModuleDocs(ctx);
        Assert.AreEqual(2, files.Count);
        var otp = files[0];
        Assert.AreEqual("/resources/resourceGroups.md".ToPlatformPath(), otp.FilePath);
        var mdFile = otp as MarkdownGenerationFile;
        Assert.IsNotNull(mdFile);
        var firstElement = mdFile.Document[0];
        Assert.IsNotInstanceOfType<MdFrontMatter>(firstElement);

        Assert.AreEqual("resources/_category_.json", files[1].FilePath);
    }
}