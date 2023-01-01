using System.Collections.Immutable;
using LandingZones.Tools.BicepDocs.Core.Models.Formatting;

namespace LandingZones.Tools.BicepDocs.Core.Abstractions;

public interface IDocsFormatter
{
    DocFormatter Formatter { get; }
    Task<IImmutableList<GenerationFile>> GenerateModuleDocs(GeneratorContext context);
}