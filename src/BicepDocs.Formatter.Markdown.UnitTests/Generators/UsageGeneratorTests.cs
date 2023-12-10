using System.Collections.Immutable;
using LandingZones.Tools.BicepDocs.Core.Models.Formatting;
using LandingZones.Tools.BicepDocs.Core.Models.Parsing;
using LandingZones.Tools.BicepDocs.Core.UnitTests;
using LandingZones.Tools.BicepDocs.Formatter.Markdown.Elements;
using LandingZones.Tools.BicepDocs.Formatter.Markdown.Generators;
using LandingZones.Tools.BicepDocs.Formatter.Markdown.Models;

namespace LandingZones.Tools.BicepDocs.Formatter.Markdown.UnitTests.Generators;

[TestClass]
public class UsageGeneratorTests
{
    [TestMethod]
    public void BuildUsage_Input_BuildsCorrectly()
    {
        var usageOptions = new UsageOptions
        {
            ModuleAlias = "MyRegistry",
            ModuleType = ModuleUsageType.Registry
        };
        const string modulePath = "workloads/web/function-app";
        var expected = @"## Usage

```bicep
module exampleInstance 'br/MyRegistry:workloads/web/function-app:2022-10-29' = {
  name: 'exampleInstance'
  params: {
    location: 'location'
  }
}
```".ToPlatformLineEndings() + Environment.NewLine;

        var parameters = new List<ParsedParameter>
        {
            new("location", "string")
            {
                Description = "The location of the resource"
            }
        }.ToImmutableList();
        var document = new MarkdownDocument();

        UsageGenerator.BuildUsage(document, new FormatterOptions(), parameters, usageOptions, modulePath, "2022-10-29");

        Assert.AreEqual(2, document.Count);

        var md = document.ToMarkdown();

        Assert.AreEqual(expected, md);
    }

    [TestMethod]
    public void BuildUsage_WithDefaults_BuildsCorrectly()
    {
        var usageOptions = new UsageOptions
        {
            ModuleAlias = "MyRegistry",
            ModuleType = ModuleUsageType.Registry
        };
        const string modulePath = "workloads/web/function-app";
        var expected = @"## Usage

```bicep
module exampleInstance 'br/MyRegistry:workloads/web/function-app:2022-10-29' = {
  name: 'exampleInstance'
  params: {
    location: 'northeurope'
    count: 10
  }
}
```".ToPlatformLineEndings() + Environment.NewLine;

        var parameters = new List<ParsedParameter>
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

        UsageGenerator.BuildUsage(document, new FormatterOptions(), parameters, usageOptions, modulePath, "2022-10-29");

        Assert.AreEqual(2, document.Count);

        var md = document.ToMarkdown();

        Assert.AreEqual(expected, md);
    }

    [TestMethod]
    public void BuildResources_DisabledInOptions_DoesNotGenerate()
    {
        var usageOptions = new UsageOptions
        {
            ModuleAlias = "MyRegistry",
            ModuleType = ModuleUsageType.Registry
        };
        const string modulePath = "workloads/web/function-app";
        var parameters = new List<ParsedParameter>
        {
            new("location", "string")
            {
                Description = "The location of the resource",
                DefaultValue = "northeurope"
            }
        }.ToImmutableList();
        var document = new MarkdownDocument();
        UsageGenerator.BuildUsage(document, new FormatterOptions { IncludeUsage = false }, parameters, usageOptions,
            modulePath, "2022-10-29");

        Assert.AreEqual(0, document.Count);
    }
}