using Bicep.Core.Semantics;

namespace LandingZones.Tools.BicepDocs.Core.Abstractions;

public interface IBicepFileService
{
    Task<SemanticModel> GetSemanticModelFromPath(string filePath);
    Task<SemanticModel> GetSemanticModelFromContent(string fileContent, string fileName = "deploy.bicep");
}