using LandingZones.Tools.BicepDocs.Core.Models.Destination;

namespace LandingZones.Tools.BicepDocs.Destination.Confluence;

public record ConfluenceOptions(
    string InstanceName,
    string User,
    string Token,
    string SpaceKey,
    int RootPageId
) : DestinationOptions;