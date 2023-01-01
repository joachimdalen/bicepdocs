using LandingZones.Tools.BicepDocs.Core.Models.Source;

namespace LandingZones.Tools.BicepDocs.Source.AzureContainerRegistry.Models;

public record AcrSourceFile(string Name, string RepoPath, AcrSourceVersion[] Versions) : SourceFile(Name);