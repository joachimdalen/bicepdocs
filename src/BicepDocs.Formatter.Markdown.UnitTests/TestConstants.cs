using LandingZones.Tools.BicepDocs.Core;
using LandingZones.Tools.BicepDocs.Core.Models.Formatting;

namespace LandingZones.Tools.BicepDocs.Formatter.Markdown.UnitTests;

public static class TestConstants
{
    public static ModulePaths GetMockModulePaths()
    {
        const string inputFolder = "/input/folder/modules";
        const string outputBaseFolder = "/output-folder/some-dir/docs";
        const string inputFile = "/input/folder/modules/vaults/vault.bicep";
        return PathResolver.ResolveModulePaths(
            bicepFilePath: inputFile,
            baseInputFolder: inputFolder,
            outputFolder: outputBaseFolder);
    }

    public static FormatterContext DefaultFormatterContext => new(null!, "vaults/vault.bicep");
}