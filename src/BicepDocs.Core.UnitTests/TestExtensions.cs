using LandingZones.Tools.BicepDocs.Core.Extensions;

namespace LandingZones.Tools.BicepDocs.Core.UnitTests
{
    public static class TestExtensions
    {
        public static string ToPlatformLineEndings(this string text)
        {
            return text.Contains("\r\n") ? text.Replace("\r\n", "\n") : text.Replace("\n", Environment.NewLine).Replace("\r", Environment.NewLine);
        }

        public static string WithPlatformRootPath(this string text) => Path.Combine(Path.GetPathRoot(Environment.CurrentDirectory)!, text.TrimStart('/').TrimStart('\\').ToPlatformPath());
    }
}
