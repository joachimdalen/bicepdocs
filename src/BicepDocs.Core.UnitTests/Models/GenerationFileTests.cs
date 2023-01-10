using LandingZones.Tools.BicepDocs.Core.Extensions;
using LandingZones.Tools.BicepDocs.Core.Models.Formatting;

namespace LandingZones.Tools.BicepDocs.Core.UnitTests.Models;

[TestClass]
public class GenerationFileTests
{
    [TestMethod]
    public void GenerationFile_ValidPath_ReturnsFolder()
    {
        var ctx = new TextGenerationFile("/some/path/here/deploy.bicep".ToPlatformPath(), "");
        Assert.AreEqual("/some/path/here".ToPlatformPath(), ctx.FolderPath);
    }

    [TestMethod]
    public void GenerationFile_VersionString_SetsProperties()
    {
        var ctx = new DemoGenFile("/some/path/here/deploy.bicep".ToPlatformPath(), "/some/path/here/version/deploy.bicep"
            .ToPlatformPath());
        Assert.AreEqual("/some/path/here".ToPlatformPath(), ctx.FolderPath);
        Assert.AreEqual("/some/path/here/deploy.bicep".ToPlatformPath(), ctx.FilePath);

        Assert.AreEqual("/some/path/here/version".ToPlatformPath(), ctx.VersionFolderPath);
        Assert.AreEqual("/some/path/here/version/deploy.bicep".ToPlatformPath(), ctx.VersionFilePath);
    }

    class DemoGenFile : GenerationFile
    {
        public DemoGenFile(string filePath, string? versionFilePath = null) : base(filePath, versionFilePath)
        {
        }
    }
}
