using LandingZones.Tools.BicepDocs.Core.Parsers;

namespace LandingZones.Tools.BicepDocs.Core.UnitTests.Parsers;

[TestClass]
public class OutputParserTests : BicepFileTestBase
{
    [TestMethod]
    public async Task Output_Basic_Parses()
    {
        const string template = @"resource resourceGroup 'Microsoft.Resources/resourceGroups@2021-01-01' = {
  name: resourceGroupName
  location: resourceGroupLocation
  tags: tags
}

output resourceId string = resourceGroup.id";
        var semanticModel = await GetModel(template);
        var outputs = OutputParser.ParseOutputs(semanticModel);
        var resourceId = outputs.First();

        Assert.AreEqual("resourceId", resourceId.Name);
        Assert.AreEqual("string", resourceId.Type);
        Assert.IsNull(resourceId.Description);
    }

    [TestMethod]
    public async Task Output_BasicWithDescription_Parses()
    {
        const string template = @"resource resourceGroup 'Microsoft.Resources/resourceGroups@2021-01-01' = {
  name: resourceGroupName
  location: resourceGroupLocation
  tags: tags
}

@description('The resource id of the resource group')
output resourceId string = resourceGroup.id";
        var semanticModel = await GetModel(template); //Path.Join(TestContext.DeploymentDirectory, "TestFiles/main.bicep")
        var outputs = OutputParser.ParseOutputs(semanticModel);
        var resourceId = outputs.First();

        Assert.AreEqual("resourceId", resourceId.Name);
        Assert.AreEqual("string", resourceId.Type);
        Assert.AreEqual("The resource id of the resource group", resourceId.Description);
    }
}