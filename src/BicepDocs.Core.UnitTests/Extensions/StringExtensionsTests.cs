using LandingZones.Tools.BicepDocs.Core.Extensions;

namespace LandingZones.Tools.BicepDocs.Core.UnitTests.Extensions;

[TestClass]
public class StringExtensionsTests
{
    [TestMethod]
    public void Repeat_Input_Repeats()
    {
        var f = "A".Repeat(10);
        Assert.AreEqual("AAAAAAAAAA", f);
    }

    [TestMethod]
    public void FirstCharToUpper_IsNull_Throws()
    {
        Assert.ThrowsException<ArgumentNullException>(() => StringExtensions.FirstCharToUpper(null));
    }

    [TestMethod]
    public void FirstCharToUpper_IsEmpty_Throws()
    {
        Assert.ThrowsException<ArgumentException>(() => "".FirstCharToUpper());
    }

    [DataTestMethod]
    [DataRow("mystring", "Mystring")]
    [DataRow("mYstring", "MYstring")]
    [DataRow("hello world", "Hello world")]
    public void FirstCharToUpper_Input_Converts(string input, string expected)
    {
        Assert.AreEqual(expected, input.FirstCharToUpper());
    }
}