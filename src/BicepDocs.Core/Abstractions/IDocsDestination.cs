using System.Collections.Immutable;
using LandingZones.Tools.BicepDocs.Core.Models.Destination;
using LandingZones.Tools.BicepDocs.Core.Models.Formatting;

namespace LandingZones.Tools.BicepDocs.Core.Abstractions;

public interface IDocsDestination
{
    DocDestination Destination { get; }
    bool RequiresInput { get; }
    Task Write(IImmutableList<GenerationFile> generationFiles, DestinationOptions? options = null);
}