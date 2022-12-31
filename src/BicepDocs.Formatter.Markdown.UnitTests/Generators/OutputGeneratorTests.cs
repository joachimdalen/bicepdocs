using System.Collections.Immutable;
using LandingZones.Tools.BicepDocs.Core;
using LandingZones.Tools.BicepDocs.Core.Models.Formatting;
using LandingZones.Tools.BicepDocs.Core.Models.Parsing;
using LandingZones.Tools.BicepDocs.Core.UnitTests;
using LandingZones.Tools.BicepDocs.Formatter.Markdown.Elements;
using LandingZones.Tools.BicepDocs.Formatter.Markdown.Generators;

namespace LandingZones.Tools.BicepDocs.Formatter.Markdown.UnitTests.Generators;

[TestClass]
public class OutputGeneratorTests : BicepFileTestBase
{
    [TestMethod]
    public void BuildOutputs_Input_BuildsCorrectly()
    {
        const string expected = @"## Outputs

| Name | Type | Description |
| --- | --- | --- |
| `resourceId` | string | The resource id of the resource |";
        var outputs = new List<ParsedOutput>
        {
            new("resourceId", "string", "The resource id of the resource")
        }.ToImmutableList();

        var document = new MarkdownDocument();
        OutputGenerator.BuildOutputs(document, outputs);

        Assert.AreEqual(2, document.Count);

        var md = document.ToMarkdown();

        Assert.AreEqual(expected, md);
    }

    [TestMethod]
    public async Task BuildOutputs_DisabledInOptions_DoesNotGenerate()
    {
        const string template = @"resource resourceGroup 'Microsoft.Resources/resourceGroups@2021-01-01' = {
  name: resourceGroupName
  location: resourceGroupLocation
  tags: tags
}

output resourceId string = resourceGroup.id";
        var semanticModel = await GetModel(template);
        var document = new MarkdownDocument();
        var ctx = new GeneratorContext(semanticModel, TestConstants.GetMockModulePaths(), new FormatterOptions
        {
            IncludeOutputs = false
        });
        OutputGenerator.BuildOutputs(document, ctx);

        Assert.AreEqual(0, document.Count);
    }

    [TestMethod]
    public async Task BuildOutputs_TemplateInput_Generates()
    {
        const string expected = @"## Outputs

| Name | Type | Description |
| --- | --- | --- |
| `resourceId` | string | The resource id of the resource |";
        const string template = @"resource resourceGroup 'Microsoft.Resources/resourceGroups@2021-01-01' = {
  name: resourceGroupName
  location: resourceGroupLocation
  tags: tags
}

@description('The resource id of the resource')
output resourceId string = resourceGroup.id";
        var semanticModel = await GetModel(template);
        var document = new MarkdownDocument();
        var ctx = new GeneratorContext(semanticModel, TestConstants.GetMockModulePaths(), new FormatterOptions());
        OutputGenerator.BuildOutputs(document, ctx);

        Assert.AreEqual(2, document.Count);

        var md = document.ToMarkdown();

        Assert.AreEqual(expected, md);
    }

    [TestMethod]
    public async Task BuildOutputs_NoOutputs_DoesNotGenerate()
    {
        const string template = @"resource resourceGroup 'Microsoft.Resources/resourceGroups@2021-01-01' = {
  name: resourceGroupName
  location: resourceGroupLocation
  tags: tags
}";
        var semanticModel = await GetModel(template);
        var document = new MarkdownDocument();
        var ctx = new GeneratorContext(semanticModel, TestConstants.GetMockModulePaths(), new FormatterOptions());
        OutputGenerator.BuildOutputs(document, ctx);

        Assert.AreEqual(0, document.Count);
    }
}
