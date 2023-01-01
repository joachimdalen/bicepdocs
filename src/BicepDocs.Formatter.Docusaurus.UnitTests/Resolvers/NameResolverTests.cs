using LandingZones.Tools.BicepDocs.Formatter.Docusaurus.Resolvers;

namespace LandingZones.Tools.BicepDocs.Formatter.Docusaurus.UnitTests.Resolvers;

[TestClass]
public class NameResolverTests
{
    [DataTestMethod]
    [DataRow("appConfiguration", "App Configuration")]
    [DataRow("app_configuration", "App Configuration")]
    [DataRow("app-configuration", "App Configuration")]
    [DataRow("app-configuration_rbac", "App Configuration Rbac")]
    [DataRow("AppConfiguration", "App Configuration")]
    public void NameResolver_ResolveName_Resolves(string input, string expected)
    {
        Assert.AreEqual(expected, NameResolver.ResolveName(input));
    }
}