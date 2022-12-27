using System.Collections.Immutable;
using LandingZones.Tools.BicepDocs.Core.Models;
using LandingZones.Tools.BicepDocs.Provider.Markdown.Elements;
using LandingZones.Tools.BicepDocs.Provider.Markdown.Generators;

namespace LandingZones.Tools.BicepDocs.Provider.Markdown.UnitTests.Generators;

[TestClass]
public class ParameterGeneratorTests
{
    #region Build Parameters

    [TestMethod]
    public void BuildParameters_DisabledInOptions_DoesNotGenerate()
    {
        var parameters = new List<ParsedParameter>
        {
            new("location", "string")
            {
                Description = "The location of the resource"
            }
        }.ToImmutableList();
        var document = new MarkdownDocument();

        ParameterGenerator.BuildParameters(document, new GeneratorOptions
        {
            IncludeParameters = false
        }, parameters);

        Assert.AreEqual(0, document.Count);
    }

    [TestMethod]
    public void BuildParameters_NoParameters_DoesNotGenerate()
    {
        var parameters = new List<ParsedParameter>().ToImmutableList();
        var document = new MarkdownDocument();

        ParameterGenerator.BuildParameters(document, new GeneratorOptions(), parameters);

        Assert.AreEqual(0, document.Count);
    }

    [TestMethod]
    public void BuildParameters_SimpleParameterType_BuildsCorrectly()
    {
        const string expected = @"## Parameters

| Parameter | Description | Type | Default |
| --- | --- | --- | --- |
| `location` | The location of the resource | string |  |";

        var parameters = new List<ParsedParameter>
        {
            new("location", "string")
            {
                Description = "The location of the resource"
            }
        }.ToImmutableList();
        var document = new MarkdownDocument();

        ParameterGenerator.BuildParameters(document, new GeneratorOptions(), parameters);

        Assert.AreEqual(2, document.Count);

        var md = document.ToMarkdown();

        Assert.AreEqual(expected, md);
    }

    [TestMethod]
    public void BuildParameters_SimpleParameterTypeWithDefault_BuildsCorrectly()
    {
        const string expected = @"## Parameters

| Parameter | Description | Type | Default |
| --- | --- | --- | --- |
| `location` | The location of the resource | string | northeurope |";

        var parameters = new List<ParsedParameter>
        {
            new("location", "string")
            {
                Description = "The location of the resource",
                DefaultValue = "northeurope"
            }
        }.ToImmutableList();
        var document = new MarkdownDocument();

        ParameterGenerator.BuildParameters(document, new GeneratorOptions(), parameters);

        Assert.AreEqual(2, document.Count);

        var md = document.ToMarkdown();

        Assert.AreEqual(expected, md);
    }

    [TestMethod]
    public void BuildParameters_ComplexParameterType_BuildsCorrectly()
    {
        const string expected = @"## Parameters

| Parameter | Description | Type | Default |
| --- | --- | --- | --- |
| `location` | The location of the resource | [locationAllow](#locationallow) |  |";

        var parameters = new List<ParsedParameter>
        {
            new("location", "'one' | 'two' | 'three' | 'four' ")
            {
                Description = "The location of the resource",
                IsComplexAllow = true
            }
        }.ToImmutableList();
        var document = new MarkdownDocument();

        ParameterGenerator.BuildParameters(document, new GeneratorOptions(), parameters);

        Assert.AreEqual(2, document.Count);

        var md = document.ToMarkdown();

        Assert.AreEqual(expected, md);
    }

    [TestMethod]
    public void BuildParameters_ComplexParameterTypeWithDefault_BuildsCorrectly()
    {
        const string expected = @"## Parameters

| Parameter | Description | Type | Default |
| --- | --- | --- | --- |
| `location` | The location of the resource | [locationAllow](#locationallow) | one |";

        var parameters = new List<ParsedParameter>
        {
            new("location", "'one' | 'two' | 'three' | 'four' ")
            {
                Description = "The location of the resource",
                IsComplexAllow = true,
                DefaultValue = "one"
            }
        }.ToImmutableList();
        var document = new MarkdownDocument();

        ParameterGenerator.BuildParameters(document, new GeneratorOptions(), parameters);

        Assert.AreEqual(2, document.Count);

        var md = document.ToMarkdown();

        Assert.AreEqual(expected, md);
    }

    #endregion

    #region Build Parameter References

    [TestMethod]
    public void BuildParameterReferences_DisabledInOptions_DoesNotGenerate()
    {
        var parameters = new List<ParsedParameter>
        {
            new("location", "string")
            {
                Description = "The location of the resource"
            }
        }.ToImmutableList();
        var document = new MarkdownDocument();

        ParameterGenerator.BuildParameterReferences(document, new GeneratorOptions
        {
            IncludeParameters = false
        }, parameters);

        Assert.AreEqual(0, document.Count);
    }


    [TestMethod]
    public void BuildParameterReferences_NoComplexOrAllow_DoesNotGenerate()
    {
        var parameters = new List<ParsedParameter>
        {
            new("location", "string")
            {
                Description = "The location of the resource"
            }
        }.ToImmutableList();

        var document = new MarkdownDocument();
        ParameterGenerator.BuildParameterReferences(document, new GeneratorOptions(), parameters);

        Assert.AreEqual(0, document.Count);
    }

    [TestMethod]
    public void BuildParameterReferences_ComplexDefaultValue_BuildsCorrectly()
    {
        const string expected = @"## References

### locationValue

```bicep
{
    one: 'something'
}
```";

        var parameters = new List<ParsedParameter>
        {
            new("location", "object")
            {
                Description = "The location of the resource",
                IsComplexDefault = true,
                DefaultValue = @"{
    one: 'something'
}"
            }
        }.ToImmutableList();

        var document = new MarkdownDocument();
        ParameterGenerator.BuildParameterReferences(document, new GeneratorOptions(), parameters);

        Assert.AreEqual(3, document.Count);

        var md = document.ToMarkdown();

        Assert.AreEqual(expected, md);
    }

    [TestMethod]
    public void BuildParameterReferences_ComplexParameterType_BuildsCorrectly()
    {
        const string expected = @"## References

### locationAllow

- one
- two
- three
- four";

        var parameters = new List<ParsedParameter>
        {
            new("location", "'one' | 'two' | 'three' | 'four' ")
            {
                Description = "The location of the resource",
                IsComplexAllow = true,
                DefaultValue = "one",
                AllowedValues = new List<string>
                {
                    "one",
                    "two",
                    "three",
                    "four"
                }
            }
        }.ToImmutableList();
        var document = new MarkdownDocument();

        ParameterGenerator.BuildParameterReferences(document, new GeneratorOptions(), parameters);

        Assert.AreEqual(3, document.Count);

        var md = document.ToMarkdown();

        Assert.AreEqual(expected, md);
    }

    #endregion

    // TODO:
    // Simple parameter type
    // Complex paramter type
    // Simple default value
    // Complex default value
}