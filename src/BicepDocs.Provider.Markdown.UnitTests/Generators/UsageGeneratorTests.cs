using System.Collections.Immutable;
using LandingZones.Tools.BicepDocs.Core.Models;
using LandingZones.Tools.BicepDocs.Provider.Markdown.Elements;
using LandingZones.Tools.BicepDocs.Provider.Markdown.Generators;

namespace LandingZones.Tools.BicepDocs.Provider.Markdown.UnitTests.Generators;

[TestClass]
public class UsageGeneratorTests
{
    [TestMethod]
    public void BuildUsage_Input_BuildsCorrectly()
    {
        const string expected = @"## Usage

```bicep
module exampleInstance 'br/MyRegistry:bicep/modules/customModule:2022-10-29' = {
  name: 'exampleInstance'
  params: {
    location: 'location'
}

```";

        var parameters = new List<ParsedParameter>()
        {
            new("location", "string")
            {
                Description = "The location of the resource"
            }
        }.ToImmutableList();
        var document = new MarkdownDocument();

        UsageGenerator.BuildUsage(document, new GeneratorOptions(), parameters);

        Assert.AreEqual(2, document.Count);

        var md = document.ToMarkdown();

        Assert.AreEqual(expected, md);
    }

    [TestMethod]
    public void BuildUsage_WithDefaults_BuildsCorrectly()
    {
        const string expected = @"## Usage

```bicep
module exampleInstance 'br/MyRegistry:bicep/modules/customModule:2022-10-29' = {
  name: 'exampleInstance'
  params: {
    location: 'northeurope'
    count: 10
}

```";

        var parameters = new List<ParsedParameter>()
        {
            new("location", "string")
            {
                Description = "The location of the resource",
                DefaultValue = "northeurope"
            },
            new("count", "integer")
            {
                DefaultValue = "10"
            }
        }.ToImmutableList();
        var document = new MarkdownDocument();

        UsageGenerator.BuildUsage(document, new GeneratorOptions(), parameters);

        Assert.AreEqual(2, document.Count);

        var md = document.ToMarkdown();

        Assert.AreEqual(expected, md);
    }
    
    [TestMethod]
    public void BuildResources_DisabledInOptions_DoesNotGenerate()
    {
        var parameters = new List<ParsedParameter>()
        {
            new("location", "string")
            {
                Description = "The location of the resource",
                DefaultValue = "northeurope"
            }
        }.ToImmutableList();
        var document = new MarkdownDocument();
        UsageGenerator.BuildUsage(document, new GeneratorOptions(){IncludeUsage = false}, parameters);

        Assert.AreEqual(0, document.Count);
    }
}