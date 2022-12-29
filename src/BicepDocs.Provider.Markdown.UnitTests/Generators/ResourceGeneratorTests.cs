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
    #region BuildResources

    [TestMethod]
    public void BuildResources_Input_BuildsCorrectly()
    {
        const string expected = @"## Resources

- [Microsoft.Web/sites/2022-12-18](https://learn.microsoft.com/en-us/azure/templates/microsoft.web/2022-12-18/sites)";
        var resources = new List<ParsedResource>
        {
            new("Microsoft.Web/sites/2022-12-18", "Microsoft.Web", "sites")
            {
                DocUrl = ResourceLinkBuilder.GetResourceUrl(new ResourceTypeReference(new List<string>
                {
                    "microsoft.web",
                    "sites"
                }.ToImmutableArray(), "2022-12-18"))
            }
        }.ToImmutableList();

        var document = new MarkdownDocument();
        ResourceGenerator.BuildResources(document, TestConstants.DefaultContext, resources);

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
                DocUrl = ResourceLinkBuilder.GetResourceUrl(new ResourceTypeReference(new List<string>
                {
                    "microsoft.web",
                    "sites"
                }.ToImmutableArray(), "2022-12-18"))
            },
            new("Microsoft.Web/sites/2022-12-18", "Microsoft.Web", "sites")
            {
                DocUrl = ResourceLinkBuilder.GetResourceUrl(new ResourceTypeReference(new List<string>
                {
                    "microsoft.web",
                    "sites"
                }.ToImmutableArray(), "2022-12-18"))
            }
        }.ToImmutableList();

        var document = new MarkdownDocument();
        ResourceGenerator.BuildResources(document, TestConstants.DefaultContext, resources);

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
        var ctx = new GeneratorContext(semanticModel, TestConstants.GetMockModulePaths(), new GeneratorOptions()
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
        var ctx = new GeneratorContext(semanticModel, TestConstants.GetMockModulePaths(), new GeneratorOptions
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
        var ctx = new GeneratorContext(semanticModel, TestConstants.GetMockModulePaths(), new GeneratorOptions());
        ResourceGenerator.BuildResources(document, ctx);

        Assert.AreEqual(0, document.Count);
    }

    #endregion

    #region Referenced Resources

    [TestMethod]
    public void BuildReferencedResources_Input_BuildsCorrectly()
    {
        const string expected = @"## Referenced Resources

| Provider | Name | Scope |
| --- | --- | --- |
| Microsoft.Web/sites/2022-12-18 | siteOne | `subscription()` |";
        var resources = new List<ParsedResource>
        {
            new("Microsoft.Web/sites/2022-12-18", "Microsoft.Web", "sites")
            {
                Name = "siteOne",
                Scope = "subscription()",
                IsExisting = true,
                DocUrl = ResourceLinkBuilder.GetResourceUrl(new ResourceTypeReference(new List<string>
                {
                    "microsoft.web",
                    "sites"
                }.ToImmutableArray(), "2022-12-18"))
            }
        }.ToImmutableList();

        var document = new MarkdownDocument();
        ResourceGenerator.BuildReferencedResources(document, resources);

        Assert.AreEqual(2, document.Count);

        var md = document.ToMarkdown();

        Assert.AreEqual(expected, md);
    }

    [TestMethod]
    public void BuildReferencedResources_MultipleOfSame_BuildsAll()
    {
        const string expected = @"## Referenced Resources

| Provider | Name | Scope |
| --- | --- | --- |
| Microsoft.Web/sites/2022-12-18 | siteOne | `subscription()` |
| Microsoft.Web/sites/2022-12-18 | siteTwo | `subscription()` |";
        var resources = new List<ParsedResource>
        {
            new("Microsoft.Web/sites/2022-12-18", "Microsoft.Web", "sites")
            {
                Name = "siteOne",
                Scope = "subscription()",
                IsExisting = true,
                DocUrl = ResourceLinkBuilder.GetResourceUrl(new ResourceTypeReference(new List<string>
                {
                    "microsoft.web",
                    "sites"
                }.ToImmutableArray(), "2022-12-18"))
            },
            new("Microsoft.Web/sites/2022-12-18", "Microsoft.Web", "sites")
            {
                Name = "siteTwo",
                Scope = "subscription()",
                IsExisting = true,
                DocUrl = ResourceLinkBuilder.GetResourceUrl(new ResourceTypeReference(new List<string>
                {
                    "microsoft.web",
                    "sites"
                }.ToImmutableArray(), "2022-12-18"))
            }
        }.ToImmutableList();

        var document = new MarkdownDocument();
        ResourceGenerator.BuildReferencedResources(document, resources);

        Assert.AreEqual(2, document.Count);

        var md = document.ToMarkdown();

        Assert.AreEqual(expected, md);
    }

    [TestMethod]
    public async Task BuildReferencedResources_InputTemplate_BuildsSingle()
    {
        const string expected = @"## Referenced Resources

| Provider | Name | Scope |
| --- | --- | --- |
| Microsoft.Resources/resourceGroups@2021-01-01 | resourceGroupName | - |";

        const string template = @"resource resourceGroup 'Microsoft.Resources/resourceGroups@2021-01-01' existing = {
  name: resourceGroupName
}

output resourceId string = resourceGroup.id";

        var document = new MarkdownDocument();

        var semanticModel = await GetModel(template);
        var ctx = new GeneratorContext(semanticModel, TestConstants.GetMockModulePaths(), new GeneratorOptions()
        );
        ResourceGenerator.BuildReferencedResources(document, ctx);

        Assert.AreEqual(2, document.Count);

        var md = document.ToMarkdown();

        Assert.AreEqual(expected, md);
    }

    [TestMethod]
    public async Task BuildReferencedResources_DisabledInOptions_DoesNotGenerate()
    {
        const string template = @"resource resourceGroup 'Microsoft.Resources/resourceGroups@2021-01-01' existing = {
  name: resourceGroupName
}

output resourceId string = resourceGroup.id";
        var semanticModel = await GetModel(template);
        var document = new MarkdownDocument();
        var ctx = new GeneratorContext(semanticModel, TestConstants.GetMockModulePaths(), new GeneratorOptions
        {
            IncludeReferencedResources = false
        });
        ResourceGenerator.BuildReferencedResources(document, ctx);

        Assert.AreEqual(0, document.Count);
    }

    [TestMethod]
    public async Task BuildReferencedResources_NoResources_DoesNotGenerate()
    {
        const string template = @"
param something string = 'nothing'";
        var semanticModel = await GetModel(template);
        var document = new MarkdownDocument();
        var ctx = new GeneratorContext(semanticModel, TestConstants.GetMockModulePaths(), new GeneratorOptions());
        ResourceGenerator.BuildReferencedResources(document, ctx);

        Assert.AreEqual(0, document.Count);
    }

    [TestMethod]
    public async Task BuildReferencedResources_NoExistingResources_DoesNotGenerate()
    {
        const string template = @"resource resourceGroup 'Microsoft.Resources/resourceGroups@2021-01-01' = {
  name: resourceGroupName
  location: resourceGroupLocation
  tags: tags
}

output resourceId string = resourceGroup.id";
        var semanticModel = await GetModel(template);
        var document = new MarkdownDocument();
        var ctx = new GeneratorContext(semanticModel, TestConstants.GetMockModulePaths(), new GeneratorOptions());
        ResourceGenerator.BuildReferencedResources(document, ctx);

        Assert.AreEqual(0, document.Count);
    }

    #endregion
}
