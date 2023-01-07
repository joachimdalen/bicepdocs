using LandingZones.Tools.BicepDocs.Core.Parsers;

namespace LandingZones.Tools.BicepDocs.Core.UnitTests.Parsers;

[TestClass]
public class ParameterParserTests : BicepFileTestBase
{
    [TestMethod]
    public async Task Parameter_Basic_Parses()
    {
        const string template = @"targetScope = 'subscription'

@description('Name of the resource group')
param resourceGroupName string

param resourceGroupLocation string

@description('Tags to append to resource group')
param tags object = {}

resource resourceGroup 'Microsoft.Resources/resourceGroups@2021-01-01' = {
  name: resourceGroupName
  location: resourceGroupLocation
  tags: tags
}";
        var semanticModel = await GetModel(template);
        var parameters = ParameterParser.ParseParameters(semanticModel);

        Assert.AreEqual(3, parameters.Count);

        var tags = parameters.First(x => x.Name == "tags");
        Assert.IsFalse(tags.IsComplexAllow);
        Assert.IsFalse(tags.IsComplexDefault);
        Assert.AreEqual("tags", tags.Name);
        Assert.AreEqual("object", tags.Type);
        Assert.AreEqual("{}", tags.DefaultValue);
        Assert.AreEqual("Tags to append to resource group", tags.Description);
    }

    [TestMethod]
    public async Task Parameter_Interpolated_Parses()
    {
        const string template = @"targetScope = 'subscription'

@description('Name of the resource group')
param resourceGroupName string

param resourceGroupLocation string = resourceGroup().location

resource resourceGroup 'Microsoft.Resources/resourceGroups@2021-01-01' = {
  name: resourceGroupName
  location: resourceGroupLocation
}";
        var semanticModel = await GetModel(template);
        var parameters = ParameterParser.ParseParameters(semanticModel);

        var resourceGroupLocation = parameters.First(x => x.Name == "resourceGroupLocation");
        Assert.IsFalse(resourceGroupLocation.IsComplexAllow);
        Assert.IsTrue(resourceGroupLocation.IsInterpolated);
        Assert.AreEqual("resourceGroupLocation", resourceGroupLocation.Name);
        Assert.AreEqual("string", resourceGroupLocation.Type);
        Assert.AreEqual("resourceGroup().location", resourceGroupLocation.DefaultValue);
    }

    [TestMethod]
    public async Task Parameter_String_Parses()
    {
        const string template = @"param stringParam string = 'string-value'
resource resourceGroup 'Microsoft.Resources/resourceGroups@2021-01-01' = {
  name: resourceGroupName
  location: resourceGroupLocation
  tags: tags
}";
        var semanticModel = await GetModel(template);
        var parameters = ParameterParser.ParseParameters(semanticModel);

        var param = parameters.First(x => x.Name == "stringParam");
        Assert.IsFalse(param.IsComplexAllow);
        Assert.IsFalse(param.IsComplexDefault);
        Assert.AreEqual("stringParam", param.Name);
        Assert.AreEqual("string", param.Type);
        Assert.AreEqual("'string-value'", param.DefaultValue);
    }

    [TestMethod]
    public async Task Parameter_Bool_Parses()
    {
        const string template = @"param boolParam bool = true
resource resourceGroup 'Microsoft.Resources/resourceGroups@2021-01-01' = {
  name: resourceGroupName
  location: resourceGroupLocation
  tags: tags
}";
        var semanticModel = await GetModel(template);
        var parameters = ParameterParser.ParseParameters(semanticModel);

        var param = parameters.First(x => x.Name == "boolParam");
        Assert.IsFalse(param.IsComplexAllow);
        Assert.IsFalse(param.IsComplexDefault);
        Assert.AreEqual("boolParam", param.Name);
        Assert.AreEqual("bool", param.Type);
        Assert.AreEqual("true", param.DefaultValue);
    }

    [TestMethod]
    public async Task Parameter_SimpleObject_Parses()
    {
        const string template = @"param objectParam object = {}
resource resourceGroup 'Microsoft.Resources/resourceGroups@2021-01-01' = {
  name: resourceGroupName
  location: resourceGroupLocation
  tags: tags
}";
        var semanticModel = await GetModel(template);
        var parameters = ParameterParser.ParseParameters(semanticModel);

        var param = parameters.First(x => x.Name == "objectParam");
        Assert.IsFalse(param.IsComplexAllow);
        Assert.IsFalse(param.IsComplexDefault);
        Assert.AreEqual("objectParam", param.Name);
        Assert.AreEqual("object", param.Type);
        Assert.AreEqual("{}", param.DefaultValue);
    }

    [TestMethod]
    public async Task Parameter_ComplexObject_Parses()
    {
        const string template = @"param objectParam object = {
name: 'hello'
}
resource resourceGroup 'Microsoft.Resources/resourceGroups@2021-01-01' = {
  name: resourceGroupName
  location: resourceGroupLocation
  tags: tags
}";
        var semanticModel = await GetModel(template);
        var parameters = ParameterParser.ParseParameters(semanticModel);

        var param = parameters.First(x => x.Name == "objectParam");
        Assert.IsFalse(param.IsComplexAllow);
        Assert.IsTrue(param.IsComplexDefault);
        Assert.AreEqual("objectParam", param.Name);
        Assert.AreEqual("object", param.Type);
        Assert.AreEqual(@"{
name: 'hello'
}", param.DefaultValue);
    }

    [TestMethod]
    public async Task Parameter_SimpleArray_Parses()
    {
        const string template = @"param arrayParam array = []
resource resourceGroup 'Microsoft.Resources/resourceGroups@2021-01-01' = {
  name: resourceGroupName
  location: resourceGroupLocation
  tags: tags
}";
        var semanticModel = await GetModel(template);
        var parameters = ParameterParser.ParseParameters(semanticModel);

        var param = parameters.First(x => x.Name == "arrayParam");
        Assert.IsFalse(param.IsComplexAllow);
        Assert.IsFalse(param.IsComplexDefault);
        Assert.AreEqual("arrayParam", param.Name);
        Assert.AreEqual("array", param.Type);
        Assert.AreEqual("[]", param.DefaultValue);
    }

    [TestMethod]
    public async Task Parameter_ComplexArray_Parses()
    {
        const string template = @"param arrayParam array = [
'one'
'two'
'three'
]
resource resourceGroup 'Microsoft.Resources/resourceGroups@2021-01-01' = {
  name: resourceGroupName
  location: resourceGroupLocation
  tags: tags
}";
        var semanticModel = await GetModel(template);
        var parameters = ParameterParser.ParseParameters(semanticModel);

        var param = parameters.First(x => x.Name == "arrayParam");
        Assert.IsFalse(param.IsComplexAllow);
        Assert.IsTrue(param.IsComplexDefault);
        Assert.AreEqual("arrayParam", param.Name);
        Assert.AreEqual("array", param.Type);
        Assert.AreEqual(@"[
'one'
'two'
'three'
]", param.DefaultValue);
    }

    [TestMethod]
    public async Task Parameter_AllowedValues_Parses()
    {
        const string template = @"
@allowed([
'one'
'two'
'three'
])
param stringParam string = 'string-value'

resource resourceGroup 'Microsoft.Resources/resourceGroups@2021-01-01' = {
  name: resourceGroupName
  location: resourceGroupLocation
  tags: tags
}";
        var semanticModel = await GetModel(template);
        var parameters = ParameterParser.ParseParameters(semanticModel);

        var param = parameters.First(x => x.Name == "stringParam");
        Assert.IsTrue(param.IsComplexAllow);
        Assert.IsFalse(param.IsComplexDefault);
        Assert.IsNotNull(param.AllowedValues);
        Assert.AreEqual(3, param.AllowedValues.Count);
        Assert.AreEqual("stringParam", param.Name);
        Assert.AreEqual("'one' | 'three' | 'two'", param.Type);
        Assert.AreEqual("'string-value'", param.DefaultValue);
    }
}