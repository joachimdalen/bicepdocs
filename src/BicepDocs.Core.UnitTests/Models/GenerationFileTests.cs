using LandingZones.Tools.BicepDocs.Core.Models.Formatting;

namespace LandingZones.Tools.BicepDocs.Core.UnitTests.Models;

[TestClass]
public class GenerationFileTests
{
    [TestMethod]
    public void GenerationFile_ValidPath_ReturnsFolder()
    {
        var ctx = new TextGenerationFile("/some/path/here/deploy.bicep", "");
        Assert.AreEqual("/some/path/here", ctx.FolderPath);
    }

    [TestMethod]
    public void GenerationFile_VersionString_SetsProperties()
    {
        var ctx = new DemoGenFile("/some/path/here/deploy.bicep", "/some/path/here/version/deploy.bicep");
        Assert.AreEqual("/some/path/here", ctx.FolderPath);
        Assert.AreEqual("/some/path/here/deploy.bicep", ctx.FilePath);

        Assert.AreEqual("/some/path/here/version", ctx.VersionFolderPath);
        Assert.AreEqual("/some/path/here/version/deploy.bicep", ctx.VersionFilePath);
    }

    class DemoGenFile : GenerationFile
    {
        public DemoGenFile(string filePath, string? versionFilePath = null) : base(filePath, versionFilePath)
        {
        }
    }
}