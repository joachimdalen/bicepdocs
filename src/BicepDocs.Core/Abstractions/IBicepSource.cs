using LandingZones.Tools.BicepDocs.Core.Models.Source;

namespace LandingZones.Tools.BicepDocs.Core.Abstractions;

public interface IBicepSource
{
    bool RequiresInput { get; }
    DocSource Source { get; }
    Task<SourceResult> GetSourceFiles(SourceOptions? options);
    Task<string> GetSourceContent(SourceFile source);
}