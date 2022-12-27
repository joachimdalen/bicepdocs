using Bicep.Core.Semantics;
using LandingZones.Tools.BicepDocs.Core.Models;

namespace LandingZones.Tools.BicepDocs.Core;

public class GeneratorContext
{
    public GeneratorContext(SemanticModel template, ModulePaths paths, GeneratorOptions? options = null)
    {
        Template = template;
        Paths = paths;
        GeneratorOptions = options ?? new GeneratorOptions();
    }

    public SemanticModel Template { get; }
    public ModulePaths Paths { get; }

    public GeneratorOptions GeneratorOptions { get; }
}