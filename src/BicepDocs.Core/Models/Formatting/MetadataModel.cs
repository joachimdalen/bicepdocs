namespace LandingZones.Tools.BicepDocs.Core.Models.Formatting;

public record MetadataModel(
    string? Title = null,
    string? Description = null,
    string? Owner = null,
    string? Version = null
);