using System.Collections.Immutable;

namespace LandingZones.Tools.BicepDocs.Core.Models.Source;

public sealed record SourceResult(bool IsSuccess, int ReturnCode, IImmutableList<SourceFile>? Files = null)
{
    public IImmutableList<SourceFile> Files { get; } = Files ?? Enumerable.Empty<SourceFile>().ToImmutableList();
}