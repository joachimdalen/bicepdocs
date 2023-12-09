using LandingZones.Tools.BicepDocs.Formatter.Markdown.Elements;
using LandingZones.Tools.BicepDocs.Formatter.Markdown.Extensions;
using LandingZones.Tools.BicepDocs.Formatter.Markdown.Models;

namespace LandingZones.Tools.BicepDocs.Formatter.Markdown.UnitTests.Extensions;

[TestClass]
public class GenerationFileExtensionsTests
{
    [TestMethod]
    public void GenerationFileExtensions_ToTextGenerationFile_Converts()
    {
        var mdDoc = new MarkdownDocument();
        mdDoc.Append(new MkHeader("Hello", MkHeaderLevel.H1));
        var mdGenFile = new MarkdownGenerationFile("/path/output.md", mdDoc, null!);
        var txtGenFile = mdGenFile.ToTextGenerationFile();

        Assert.AreEqual(mdGenFile.FilePath, txtGenFile.FilePath);
        Assert.AreEqual(mdGenFile.FolderPath, txtGenFile.FolderPath);
        Assert.AreEqual(mdGenFile.VersionFilePath, txtGenFile.VersionFilePath);
        Assert.AreEqual(mdGenFile.VersionFolderPath, txtGenFile.VersionFolderPath);
        Assert.AreEqual("# Hello" + Environment.NewLine, txtGenFile.Content);
    }
}