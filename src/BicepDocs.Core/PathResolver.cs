using LandingZones.Tools.BicepDocs.Core.Extensions;
using LandingZones.Tools.BicepDocs.Core.Models.Formatting;

namespace LandingZones.Tools.BicepDocs.Core;

public static class PathResolver
{
    private const string VirtualBasePath = "modules";
    private const string VersionsBasePath = "versions";

    public static string ResolvePath(string path, string? baseDirectory = null)
    {
        if (Path.IsPathFullyQualified(path))
        {
            return path;
        }

        var p = Path.Combine(baseDirectory ?? Environment.CurrentDirectory, path);
        return Path.GetFullPath(p);
    }

    public static ModuleVersionPaths ResolveVersionPath(ModulePaths modulePaths, string version)
    {
        var relativeOutput = Path.GetRelativePath(modulePaths.OutputBaseFolder, modulePaths.OutputFolder);
        var root = relativeOutput.Split(Path.DirectorySeparatorChar);
        var newPath = Path.Join(root.First(), Path.DirectorySeparatorChar.ToString(), VersionsBasePath, version,
            string.Join(Path.DirectorySeparatorChar, root.Skip(1)));

        return new ModuleVersionPaths(OutputFolder: Path.Join(modulePaths.OutputBaseFolder, newPath),
            OutputPath: Path.Join(modulePaths.OutputBaseFolder, newPath, modulePaths.OutputFileName));
    }

    public static string ResolveVersionPath(string path, string version)
    {
        var root = path.Split(Path.DirectorySeparatorChar);
        return Path.Join(root.First(), Path.DirectorySeparatorChar.ToString(), VersionsBasePath, version,
            string.Join(Path.DirectorySeparatorChar, root.Skip(1)));
    }

    public static ModulePaths ResolveModulePaths(string bicepFilePath, string baseInputFolder, string outputFolder)
    {
        var modulePath = bicepFilePath.Replace(baseInputFolder, "");
        var fileName = Path.GetFileName(bicepFilePath);
        var outputPath = Path.Join(outputFolder, modulePath);
        var outputPathMd = Path.ChangeExtension(outputPath, "md");
        return new ModulePaths(
            RelativeInputPath: modulePath,
            VirtualPath: Path.Join(Path.DirectorySeparatorChar.ToString(), VirtualBasePath, modulePath),
            VirtualFolder: Path.Join(Path.DirectorySeparatorChar.ToString(), VirtualBasePath,
                Path.GetDirectoryName(modulePath)),
            BaseFileName: Path.GetFileNameWithoutExtension(bicepFilePath),
            InputFolder: baseInputFolder,
            OutputBaseFolder: outputFolder,
            OutputFolder: Path.GetDirectoryName(outputPath) ??
                          throw new ArgumentException("Failed to resolve OutputFolder", nameof(outputFolder)),
            OutputFileName: Path.ChangeExtension(fileName, "md"),
            InputFileName: fileName,
            OutputPath: outputPathMd
        );
    }

    public static Uri FilePathToUri(string path) => new UriBuilder
    {
        Scheme = "file",
        Host = null,
        Path = path.ToPlatformPath(),
    }.Uri;
}