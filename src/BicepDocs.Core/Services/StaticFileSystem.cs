using System.IO.Abstractions;
using LandingZones.Tools.BicepDocs.Core.Abstractions;

namespace LandingZones.Tools.BicepDocs.Core.Services;

public class StaticFileSystem : FileSystem, IStaticFileSystem
{
}