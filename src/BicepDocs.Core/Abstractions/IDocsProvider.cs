using System.Collections.Immutable;
using LandingZones.Tools.BicepDocs.Core.Models;

namespace LandingZones.Tools.BicepDocs.Core.Abstractions;

public interface IDocsProvider
{
    DocProvider Provider { get; }
    Task<IImmutableList<GenerationFile>> GenerateModuleDocs(GeneratorContext context);
}