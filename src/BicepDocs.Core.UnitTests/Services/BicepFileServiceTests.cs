using Bicep.Core;
using LandingZones.Tools.BicepDocs.Core.Services;
using Microsoft.Extensions.DependencyInjection;

namespace LandingZones.Tools.BicepDocs.Core.UnitTests.Services;

[TestClass]
public class BicepFileServiceTests : BicepFileTestBase
{
    [TestMethod]
    public async Task GetSemanticModelFromContent_ValidContent_ReturnsModel()
    {
        const string template = @"targetScope = 'subscription'

@description('Name of the resource group')
param resourceGroupName string

resource resourceGroup 'Microsoft.Resources/resourceGroups@2021-01-01' = {
  name: resourceGroupName
  location: resourceGroupLocation
  tags: tags
}";
        var compiler = ServiceProvider.GetRequiredService<BicepCompiler>();
        var sut = new BicepFileService(FileSystem, compiler);

        var res = await sut.GetSemanticModelFromContent("/internal/path", "/internal/path/deploy.bicep", template);
        Assert.IsNotNull(res);
        Assert.AreEqual(1, res.AllResources.Length);
    }
}
