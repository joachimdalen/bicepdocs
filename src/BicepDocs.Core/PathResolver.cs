using LandingZones.Tools.BicepDocs.Core.Models;

namespace LandingZones.Tools.BicepDocs.Core;

public static class PathResolver
{
    public const string VirtualBasePath = "/modules";
    public const string VersionsBasePath = "versions";

    public static ModuleVersionPaths ResolveVersionPath(ModulePaths modulePaths, string version)
    {
        var relativeOutput = Path.GetRelativePath(modulePaths.OutputBaseFolder, modulePaths.OutputFolder);
        var root = relativeOutput.Split(Path.DirectorySeparatorChar);
        var newPath = Path.Join(root.First(), VersionsBasePath, version,
            string.Join(Path.DirectorySeparatorChar, root.Skip(1)));

        return new ModuleVersionPaths(outputFolder: Path.Join(modulePaths.OutputBaseFolder, newPath),
            outputPath: Path.Join(modulePaths.OutputBaseFolder, newPath, modulePaths.OutputFileName));
    }

    public static ModulePaths ResolveModulePaths(string bicepFilePath, string baseInputFolder, string outputFolder)
    {
        var modulePath = bicepFilePath.Replace(baseInputFolder, "");
        var fileName = Path.GetFileName(bicepFilePath);
        var outputPath = Path.Join(outputFolder, modulePath);
        var outputPathMd = Path.ChangeExtension(outputPath, "md");
        return new ModulePaths
        {
            RelativeInputPath = modulePath,
            VirtualPath = Path.Join(VirtualBasePath, modulePath),
            VirtualFolder = Path.Join(VirtualBasePath, Path.GetDirectoryName(modulePath)),
            BaseFileName = Path.GetFileNameWithoutExtension(bicepFilePath),
            InputFolder = baseInputFolder,
            OutputBaseFolder = outputFolder,
            OutputFolder = Path.GetDirectoryName(outputPath),
            OutputFileName = Path.ChangeExtension(fileName, "md"),
            InputFileName = fileName,
            OutputPath = outputPathMd
        };
    }
}

public class ModuleVersionPaths
{
    public ModuleVersionPaths(string outputFolder, string outputPath)
    {
        OutputFolder = outputFolder;
        OutputPath = outputPath;
    }

    public string OutputFolder { get; set; }
    public string OutputPath { get; set; }
}