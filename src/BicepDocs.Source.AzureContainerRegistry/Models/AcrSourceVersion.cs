namespace LandingZones.Tools.BicepDocs.Source.AzureContainerRegistry.Models;

public record AcrSourceVersion(string Digest, string[] Tags, DateTime Created);