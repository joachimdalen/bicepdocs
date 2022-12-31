using LandingZones.Tools.BicepDocs.Core.Models.Source;

namespace LandingZones.Tools.BicepDocs.Core.Abstractions;

public interface IBicepSource
{
    DocSource Source { get; }
    public Task<SourceResult> GetSourceFiles(SourceOptions options);
    public Task<string> GetSourceContent(SourceFile source);
}