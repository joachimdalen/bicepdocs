using Bicep.Core.Semantics;
using LandingZones.Tools.BicepDocs.Core.Models.Formatting;

namespace LandingZones.Tools.BicepDocs.Core;

public class GeneratorContext
{
    public GeneratorContext(SemanticModel template, ModulePaths paths, FormatterOptions? options = null)
    {
        Template = template;
        Paths = paths;
        FormatterOptions = options ?? new FormatterOptions();
    }

    public SemanticModel Template { get; }
    public ModulePaths Paths { get; }

    public FormatterOptions FormatterOptions { get; }
}