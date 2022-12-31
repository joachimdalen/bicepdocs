using LandingZones.Tools.BicepDocs.Core.Abstractions;
using LandingZones.Tools.BicepDocs.Core.Models.Formatting;
using Moq;

namespace LandingZones.Tools.BicepDocs.Core.UnitTests;

[TestClass]
public class ConfigurationLoaderTests
{
    private readonly Mock<IStaticFileSystem> _staticFileSystemMock;

    public ConfigurationLoaderTests()
    {
        _staticFileSystemMock = new Mock<IStaticFileSystem>(MockBehavior.Strict);
    }

    [TestMethod]
    public async Task GetOptions_UnknownOptions_Loads()
    {
        const string configFile = @"
includeExistingResources: false
provider:
    - john
";
        _staticFileSystemMock.Setup(x => x.File.ReadAllTextAsync("config.yml", It.IsAny<CancellationToken>())).ReturnsAsync(configFile);

        var sut = new ConfigurationLoader(_staticFileSystemMock.Object);
        var opt = await sut.GetOptions("config.yml");
        Assert.IsFalse(opt.IncludeExistingResources);
    }

    [TestMethod]
    public async Task GetProviderOptions_Configured_ReturnsOptions()
    {
        const string configFile = @"
includeExistingResources: false
providers:
  docusaurus:
    addPageTags: true
";
        _staticFileSystemMock.Setup(x => x.File.ReadAllTextAsync("config.yml", It.IsAny<CancellationToken>())).ReturnsAsync(configFile);

        var sut = new ConfigurationLoader(_staticFileSystemMock.Object);
        var opt = await sut.GetOptions("config.yml");
        var provOpt = sut.GetProviderOptions<ProviderOptions>(opt, DocFormatter.Docusaurus);
        Assert.IsNotNull(provOpt);
        Assert.IsTrue(provOpt.AddPageTags);
    }

    [TestMethod]
    public async Task GetProviderOptions_NotConfigured_ReturnsNull()
    {
        const string configFile = @"
includeExistingResources: false
providers:
  docusaurus:
    addPageTags: true
";
        _staticFileSystemMock.Setup(x => x.File.ReadAllTextAsync("config.yml", It.IsAny<CancellationToken>())).ReturnsAsync(configFile);

        var sut = new ConfigurationLoader(_staticFileSystemMock.Object);
        var opt = await sut.GetOptions("config.yml");
        var provOpt = sut.GetProviderOptions<ProviderOptions>(opt, DocFormatter.Markdown);
        Assert.IsNull(provOpt);
    }
    
    [TestMethod]
    public async Task GetProviderOptionsOrDefault_Configured_ReturnsOptions()
    {
        const string configFile = @"
includeExistingResources: false
providers:
  docusaurus:
    addPageTags: true
";
        _staticFileSystemMock.Setup(x => x.File.ReadAllTextAsync("config.yml", It.IsAny<CancellationToken>())).ReturnsAsync(configFile);

        var sut = new ConfigurationLoader(_staticFileSystemMock.Object);
        var opt = await sut.GetOptions("config.yml");
        var provOpt = sut.GetProviderOptionsOrDefault<ProviderOptions>(opt, DocFormatter.Docusaurus);
        Assert.IsNotNull(provOpt);
        Assert.IsTrue(provOpt.AddPageTags);
    }

    [TestMethod]
    public async Task GetProviderOptionsOrDefault_NotConfigured_ReturnsDefault()
    {
        const string configFile = @"
includeExistingResources: false
providers:
  docusaurus:
    addPageTags: true
";
        _staticFileSystemMock.Setup(x => x.File.ReadAllTextAsync("config.yml", It.IsAny<CancellationToken>())).ReturnsAsync(configFile);

        var sut = new ConfigurationLoader(_staticFileSystemMock.Object);
        var opt = await sut.GetOptions("config.yml");
        var provOpt = sut.GetProviderOptionsOrDefault<ProviderOptions>(opt, DocFormatter.Markdown);
        Assert.IsNotNull(provOpt);
        Assert.IsFalse(provOpt.AddPageTags);
    }

    class ProviderOptions
    {
        public bool AddPageTags { get; set; }
    }
}