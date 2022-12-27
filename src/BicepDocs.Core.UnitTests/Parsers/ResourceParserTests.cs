using LandingZones.Tools.BicepDocs.Core.Parsers;

namespace LandingZones.Tools.BicepDocs.Core.UnitTests.Parsers;

[TestClass]
public class ResourceParserTests : BicepFileTestBase
{
    [TestMethod]
    public async Task Resources_Basic_Parses()
    {
        const string template = @"targetScope = 'subscription'

@description('Name of the resource group')
param resourceGroupName string

resource resourceGroup 'Microsoft.Resources/resourceGroups@2021-01-01' = {
  name: resourceGroupName
  location: resourceGroupLocation
  tags: tags
}";
        var semanticModel = await GetModel(template);
        var resources = ResourceParser.ParseResources(semanticModel);

        Assert.AreEqual(1, resources.Count);

        var resource = resources.First();

        Assert.AreEqual("resourceGroups", resource.Resource);
        Assert.AreEqual("Microsoft.Resources/resourceGroups@2021-01-01", resource.Name);
        Assert.AreEqual("Microsoft.Resources", resource.Provider);
        Assert.AreEqual("2021-01-01", resource.ApiVersion);
        Assert.IsFalse(resource.IsExisting);
    }

    [TestMethod]
    public async Task Resources_MultipleWithSameApiVersion_ReturnsOnlyOne()
    {
        const string template = @"targetScope = 'subscription'

@description('Name of the resource group')
param resourceGroupName string

resource resourceGroup 'Microsoft.Resources/resourceGroups@2021-01-01' = {
  name: resourceGroupName
  location: resourceGroupLocation
  tags: tags
}
resource resourceGroup2 'Microsoft.Resources/resourceGroups@2021-01-01' = {
  name: resourceGroupName
  location: resourceGroupLocation
  tags: tags
}
";
        var semanticModel = await GetModel(template);
        var resources = ResourceParser.ParseResources(semanticModel);

        Assert.AreEqual(1, resources.Count);
    }

    [TestMethod]
    public async Task Resources_MultipleWithDifferentApiVersion_ReturnsAll()
    {
        const string template = @"targetScope = 'subscription'

@description('Name of the resource group')
param resourceGroupName string

resource resourceGroup 'Microsoft.Resources/resourceGroups@2021-01-01' = {
  name: resourceGroupName
  location: resourceGroupLocation
  tags: tags
}
resource resourceGroup2 'Microsoft.Resources/resourceGroups@2021-01-02' = {
  name: resourceGroupName
  location: resourceGroupLocation
  tags: tags
}
";
        var semanticModel = await GetModel(template);
        var resources = ResourceParser.ParseResources(semanticModel);

        Assert.AreEqual(2, resources.Count);
    }
}