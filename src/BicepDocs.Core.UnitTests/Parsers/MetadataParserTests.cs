using LandingZones.Tools.BicepDocs.Core.Parsers;

namespace LandingZones.Tools.BicepDocs.Core.UnitTests.Parsers;

[TestClass]
public class MetadataParserTests : BicepFileTestBase
{
    [TestMethod]
    public async Task MetadataParser_GetMetadata_NoMetaReturnsNull()
    {
        const string template = @"
resource resourceGroup 'Microsoft.Resources/resourceGroups@2021-01-01' = {
  name: resourceGroupName
  location: resourceGroupLocation
  tags: tags
}";
        var semanticModel = await GetModel(template);
        var metadata = MetadataParser.GetMetadata(semanticModel);

        Assert.IsNull(metadata);
    }

    [TestMethod]
    public async Task MetadataParser_GetMetadata_ParsesDefault()
    {
        const string template = @"metadata moduleDocs = {
  title: 'Resource Group'
  owner: 'Demo User <demo.user@local.test>'
  description: 'Deploy a new resource group with tags'
}

resource resourceGroup 'Microsoft.Resources/resourceGroups@2021-01-01' = {
  name: resourceGroupName
  location: resourceGroupLocation
  tags: tags
}";
        var semanticModel = await GetModel(template);
        var metadata = MetadataParser.GetMetadata(semanticModel);


        Assert.IsNotNull(metadata);
        Assert.AreEqual("Resource Group", metadata.Title);
        Assert.AreEqual("Demo User <demo.user@local.test>", metadata.Owner);
        Assert.AreEqual("Deploy a new resource group with tags", metadata.Description);
    }

    [TestMethod]
    public async Task MetadataParser_GetMetadata_UserKeyParsesDefault()
    {
        const string template = @"metadata myModuleDocs = {
  title: 'Resource Group'
  owner: 'Demo User <demo.user@local.test>'
  description: 'Deploy a new resource group with tags'
}

resource resourceGroup 'Microsoft.Resources/resourceGroups@2021-01-01' = {
  name: resourceGroupName
  location: resourceGroupLocation
  tags: tags
}";
        var semanticModel = await GetModel(template);
        var metadata = MetadataParser.GetMetadata(semanticModel, "myModuleDocs");


        Assert.IsNotNull(metadata);
        Assert.AreEqual("Resource Group", metadata.Title);
        Assert.AreEqual("Demo User <demo.user@local.test>", metadata.Owner);
        Assert.AreEqual("Deploy a new resource group with tags", metadata.Description);
    }

    [TestMethod]
    public async Task MetadataParser_GetMetadata_UnknownProperties_Null()
    {
        const string template = @"metadata moduleDocs = {
  boo: 'Resource Group'
}

resource resourceGroup 'Microsoft.Resources/resourceGroups@2021-01-01' = {
  name: resourceGroupName
  location: resourceGroupLocation
  tags: tags
}";
        var semanticModel = await GetModel(template);
        var metadata = MetadataParser.GetMetadata(semanticModel);

        Assert.IsNull(metadata);
    }
}