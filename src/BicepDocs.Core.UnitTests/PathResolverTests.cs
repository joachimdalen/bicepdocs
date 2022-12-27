namespace LandingZones.Tools.BicepDocs.Core.UnitTests;

[TestClass]
public class PathResolverTests
{
    [TestMethod]
    public void PathResolver_ResolveModulePaths_Resolves()
    {
        var inputFolder = "/input/folder/modules";
        var outputBaseFolder = "/output-folder/some-dir/docs";
        var outputFolder = "/output-folder/some-dir/docs/vaults";
        var inputFile = "/input/folder/modules/vaults/vault.bicep";
        var relativeInputPath = "/vaults/vault.bicep";
        var outputPath = "/output-folder/some-dir/docs/vaults/vault.md";
        var resolved = PathResolver.ResolveModulePaths(
            bicepFilePath: inputFile,
            baseInputFolder: inputFolder,
            outputFolder: outputBaseFolder);

        Assert.AreEqual("vault.bicep", resolved.InputFileName);
        Assert.AreEqual("vault.md", resolved.OutputFileName);
        Assert.AreEqual("vault", resolved.BaseFileName);
        Assert.AreEqual(inputFolder, resolved.InputFolder);
        Assert.AreEqual(outputFolder, resolved.OutputFolder);
        Assert.AreEqual(outputBaseFolder, resolved.OutputBaseFolder);
        Assert.AreEqual(relativeInputPath, resolved.RelativeInputPath);
        Assert.AreEqual("/modules/vaults/vault.bicep", resolved.VirtualPath);
        Assert.AreEqual("/modules/vaults", resolved.VirtualFolder);
        Assert.AreEqual(outputPath, resolved.OutputPath);
    }

    [TestMethod]
    public void PathResolver_ResolveVersionPath_Resolves()
    {
        var inputFolder = "/input/folder/modules";
        var outputBaseFolder = "/output-folder/some-dir/docs";
        var inputFile = "/input/folder/modules/vaults/role-assignments/vault-rbac.bicep";
        var modulePaths = PathResolver.ResolveModulePaths(
            bicepFilePath: inputFile,
            baseInputFolder: inputFolder,
            outputFolder: outputBaseFolder);

        var filePaths = PathResolver.ResolveVersionPath(modulePaths, "2022-12-18");

        Assert.AreEqual("/output-folder/some-dir/docs/vaults/versions/2022-12-18/role-assignments/vault-rbac.md", filePaths.OutputPath);
        Assert.AreEqual("/output-folder/some-dir/docs/vaults/versions/2022-12-18/role-assignments", filePaths.OutputFolder);
    }
}