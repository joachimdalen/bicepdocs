using System.Collections.Immutable;
using Bicep.Core.Resources;

namespace LandingZones.Tools.BicepDocs.Core.UnitTests;

[TestClass]
public class ResourceLinkBuilderTests
{
    private const string Base = "https://learn.microsoft.com/en-us/azure/templates/";

    [TestMethod]
    [DataRow(
        "Microsoft.Web", "microsoft.web,sites", "2018-11-01",
        "microsoft.web/2018-11-01/sites",
        DisplayName = "Parses one level resource"
    )]
    public void TestMethod1(string provider, string resource, string apiVersion, string expected)
    {
        var resourceParts = resource.Split(",").ToImmutableArray();
        var resourceRef = new ResourceTypeReference(resourceParts, apiVersion);
        var generatedLink = ResourceLinkBuilder.GetResourceUrl(resourceRef);
        Assert.AreEqual($"{Base}{expected}", generatedLink);
    }
}