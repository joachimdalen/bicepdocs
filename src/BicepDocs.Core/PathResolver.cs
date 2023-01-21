using LandingZones.Tools.BicepDocs.Core.Extensions;

namespace LandingZones.Tools.BicepDocs.Core;

public static class PathResolver
{
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
    
    public static string ResolveVersionPath(string path, string version)
    {
        var intPath = Path.ChangeExtension(path, "md");
        var root = intPath.Split(Path.DirectorySeparatorChar);
        return Path.Join(root.First(), Path.DirectorySeparatorChar.ToString(), VersionsBasePath, version,
            string.Join(Path.DirectorySeparatorChar, root.Skip(1)));
    }

    public static Uri FilePathToUri(string path) => new UriBuilder
    {
        Scheme = "file",
        Host = null,
        Path = path.ToPlatformPath(),
    }.Uri;
}