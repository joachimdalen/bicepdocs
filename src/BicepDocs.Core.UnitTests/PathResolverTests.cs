using LandingZones.Tools.BicepDocs.Core.Extensions;

namespace LandingZones.Tools.BicepDocs.Core.UnitTests;

[TestClass]
public class PathResolverTests
{
    [DataTestMethod]
    [DataRow("./docs/somefolder", "/root", "/root/docs/somefolder")]
    [DataRow("/root/docs/somefolder", "/root", "/root/docs/somefolder")]
    [DataRow("../docs/somefolder", "/root/mypath", "/root/docs/somefolder")]
    public void ResolvePath_Input_Resolves(string input, string baseDirectory, string expected)
    {
        var resolved = PathResolver.ResolvePath(input.ToPlatformPath(), baseDirectory.ToPlatformPath());
        Assert.AreEqual(Path.Combine(Path.GetPathRoot(resolved)!, expected.TrimStart('/').ToPlatformPath()), resolved);
    }

    // [TestMethod]
    // public void PathResolver_ResolveModulePaths_Resolves()
    // {
    //     var inputFolder = "/input/folder/modules".ToPlatformPath();
    //     var outputBaseFolder = "/output-folder/some-dir/docs".ToPlatformPath();
    //     var outputFolder = "/output-folder/some-dir/docs/vaults".ToPlatformPath();
    //     var inputFile = "/input/folder/modules/vaults/vault.bicep".ToPlatformPath();
    //     var relativeInputPath = "/vaults/vault.bicep".ToPlatformPath();
    //     var outputPath = "/output-folder/some-dir/docs/vaults/vault.md".ToPlatformPath();
    //     var resolved = PathResolver.ResolveModulePaths(
    //         bicepFilePath: inputFile,
    //         baseInputFolder: inputFolder,
    //         outputFolder: outputBaseFolder);
    //
    //     Assert.AreEqual("vault.bicep", resolved.InputFileName);
    //     Assert.AreEqual("vault.md", resolved.OutputFileName);
    //     Assert.AreEqual("vault", resolved.BaseFileName);
    //     Assert.AreEqual(inputFolder, resolved.InputFolder);
    //     Assert.AreEqual(outputFolder, resolved.OutputFolder);
    //     Assert.AreEqual(outputBaseFolder, resolved.OutputBaseFolder);
    //     Assert.AreEqual(relativeInputPath, resolved.RelativeInputPath);
    //     Assert.AreEqual("/modules/vaults/vault.bicep".ToPlatformPath(), resolved.VirtualPath);
    //     Assert.AreEqual("/modules/vaults".ToPlatformPath(), resolved.VirtualFolder);
    //     Assert.AreEqual(outputPath, resolved.OutputPath);
    // }
    //
    // [TestMethod]
    // public void PathResolver_ResolveVersionPath_Resolves()
    // {
    //     var inputFolder = "/input/folder/modules".ToPlatformPath();
    //     var outputBaseFolder = "/output-folder/some-dir/docs".ToPlatformPath();
    //     var inputFile = "/input/folder/modules/vaults/role-assignments/vault-rbac.bicep".ToPlatformPath();
    //     var modulePaths = PathResolver.ResolveModulePaths(
    //         bicepFilePath: inputFile,
    //         baseInputFolder: inputFolder,
    //         outputFolder: outputBaseFolder);
    //
    //     var filePaths = PathResolver.ResolveVersionPath(modulePaths, "2022-12-18");
    //
    //     Assert.AreEqual("/output-folder/some-dir/docs/vaults/versions/2022-12-18/role-assignments/vault-rbac.md".ToPlatformPath(), filePaths.OutputPath);
    //     Assert.AreEqual("/output-folder/some-dir/docs/vaults/versions/2022-12-18/role-assignments".ToPlatformPath(), filePaths.OutputFolder);
    // }
}
