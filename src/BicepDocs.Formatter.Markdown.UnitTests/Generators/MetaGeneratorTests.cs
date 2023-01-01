using LandingZones.Tools.BicepDocs.Core.Models.Formatting;
using LandingZones.Tools.BicepDocs.Formatter.Markdown.Elements;
using LandingZones.Tools.BicepDocs.Formatter.Markdown.Generators;

namespace LandingZones.Tools.BicepDocs.Formatter.Markdown.UnitTests.Generators;

[TestClass]
public class MetaGeneratorTests
{
    [TestMethod]
    public void BuildTitle_TitleSet_Generates()
    {
        const string expected = "# My title";
        var md = new MarkdownDocument();
        MetaGenerator.BuildTitle(md, new MetadataModel(
            Title: "My title"
        ), TestConstants.DefaultFormatterContext);

        Assert.AreEqual(expected, md.ToMarkdown());
    }

    [TestMethod]
    public void BuildTitle_TitleNotSet_UsesFileNameNot()
    {
        const string expected = "# vault";
        var md = new MarkdownDocument();
        MetaGenerator.BuildTitle(md, new MetadataModel(), TestConstants.DefaultFormatterContext);

        Assert.AreEqual(expected, md.ToMarkdown());
    }


    [TestMethod]
    public void BuildDescription_DescriptionSet_Generates()
    {
        const string expected = "> My Description";
        var md = new MarkdownDocument();
        MetaGenerator.BuildDescription(md, new MetadataModel
        {
            Description = "My Description"
        });

        Assert.AreEqual(expected, md.ToMarkdown());
    }

    [TestMethod]
    public void BuildDescription_DescriptionNotSet_DoesNotGenerate()
    {
        var md = new MarkdownDocument();
        MetaGenerator.BuildDescription(md, new MetadataModel());

        Assert.AreEqual(0, md.Count);
    }
}