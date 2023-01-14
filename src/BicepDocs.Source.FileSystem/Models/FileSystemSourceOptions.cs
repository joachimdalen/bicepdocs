using LandingZones.Tools.BicepDocs.Core.Models.Source;

namespace LandingZones.Tools.BicepDocs.Source.FileSystem.Models;

public record FileSystemSourceOptions(string FolderPath, string[] Exclude) : SourceOptions;