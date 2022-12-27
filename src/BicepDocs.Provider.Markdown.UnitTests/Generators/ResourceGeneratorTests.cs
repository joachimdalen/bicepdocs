using System.Collections.Immutable;
using Bicep.Core.Resources;
using LandingZones.Tools.BicepDocs.Core;
using LandingZones.Tools.BicepDocs.Core.Models;
using LandingZones.Tools.BicepDocs.Core.UnitTests;
using LandingZones.Tools.BicepDocs.Provider.Markdown.Elements;
using LandingZones.Tools.BicepDocs.Provider.Markdown.Generators;

namespace LandingZones.Tools.BicepDocs.Provider.Markdown.UnitTests.Generators;

[TestClass]
public class ResourceGeneratorTests : BicepFileTestBase
{
    [TestMethod]
    public void BuildResources_Input_BuildsCorrectly()
    {
        const string expected = @"## Resources

- [Microsoft.Web/sites/2022-12-18](https://learn.microsoft.com/en-us/azure/templates/microsoft.web/2022-12-18/sites)";
        var resources = new List<ParsedResource>
        {
            new("Microsoft.Web/sites/2022-12-18", "Microsoft.Web", "sites")
            {
                DocUrl = ResourceLinkBuilder.GetResourceUrl(new ResourceTypeReference(new List<string>()
                {
                    "microsoft.web",
                    "sites"
                }.ToImmutableArray(), "2022-12-18"))
            }
        }.ToImmutableList();

        var document = new MarkdownDocument();
        var ctx = new GeneratorContext(null, new ModulePaths());
        ResourceGenerator.BuildResources(document, ctx, resources);

        Assert.AreEqual(2, document.Count);

        var md = document.ToMarkdown();

        Assert.AreEqual(expected, md);
    }

    [TestMethod]
    public void BuildResources_MultipleOfSame_BuildsSingle()
    {
        const string expected = @"## Resources

- [Microsoft.Web/sites/2022-12-18](https://learn.microsoft.com/en-us/azure/templates/microsoft.web/2022-12-18/sites)";
        var resources = new List<ParsedResource>
        {
            new("Microsoft.Web/sites/2022-12-18", "Microsoft.Web", "sites")
            {
                DocUrl = ResourceLinkBuilder.GetResourceUrl(new ResourceTypeReference(new List<string>()
                {
                    "microsoft.web",
                    "sites"
                }.ToImmutableArray(), "2022-12-18"))
            },
            new("Microsoft.Web/sites/2022-12-18", "Microsoft.Web", "sites")
            {
                DocUrl = ResourceLinkBuilder.GetResourceUrl(new ResourceTypeReference(new List<string>()
                {
                    "microsoft.web",
                    "sites"
                }.ToImmutableArray(), "2022-12-18"))
            }
        }.ToImmutableList();

        var document = new MarkdownDocument();
        var ctx = new GeneratorContext(null, new ModulePaths());
        ResourceGenerator.BuildResources(document, ctx, resources);

        Assert.AreEqual(2, document.Count);

        var md = document.ToMarkdown();

        Assert.AreEqual(expected, md);
    }


    [TestMethod]
    public async Task BuildResources_InputTemplate_BuildsSingle()
    {
        const string expected = @"## Resources

- [Microsoft.Resources/resourceGroups@2021-01-01](https://learn.microsoft.com/en-us/azure/templates/microsoft.resources/2021-01-01/resourcegroups)";

        const string template = @"resource resourceGroup 'Microsoft.Resources/resourceGroups@2021-01-01' = {
  name: resourceGroupName
  location: resourceGroupLocation
  tags: tags
}

output resourceId string = resourceGroup.id";

        var document = new MarkdownDocument();

        var semanticModel = await GetModel(template);
        var ctx = new GeneratorContext(semanticModel, new ModulePaths(), new GeneratorOptions()
        );
        ResourceGenerator.BuildResources(document, ctx);

        Assert.AreEqual(2, document.Count);

        var md = document.ToMarkdown();

        Assert.AreEqual(expected, md);
    }

    [TestMethod]
    public async Task BuildResources_DisabledInOptions_DoesNotGenerate()
    {
        const string template = @"resource resourceGroup 'Microsoft.Resources/resourceGroups@2021-01-01' = {
  name: resourceGroupName
  location: resourceGroupLocation
  tags: tags
}

output resourceId string = resourceGroup.id";
        var semanticModel = await GetModel(template);
        var document = new MarkdownDocument();
        var ctx = new GeneratorContext(semanticModel, new ModulePaths(), new GeneratorOptions()
        {
            IncludeResources = false
        });
        ResourceGenerator.BuildResources(document, ctx);

        Assert.AreEqual(0, document.Count);
    }

    [TestMethod]
    public async Task BuildResources_NoResources_DoesNotGenerate()
    {
        const string template = @"
param something string = 'nothing'";
        var semanticModel = await GetModel(template);
        var document = new MarkdownDocument();
        var ctx = new GeneratorContext(semanticModel, new ModulePaths(), new GeneratorOptions());
        ResourceGenerator.BuildResources(document, ctx);

        Assert.AreEqual(0, document.Count);
    }
}