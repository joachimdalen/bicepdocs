using Bicep.Core.Semantics;

namespace LandingZones.Tools.BicepDocs.Core.Abstractions;

public interface IBicepFileService
{
    Task<SemanticModel> GetSemanticModelFromContent(string folder, string path, string content);
}