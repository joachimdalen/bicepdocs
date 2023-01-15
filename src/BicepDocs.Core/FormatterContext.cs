using Bicep.Core.Semantics;
using LandingZones.Tools.BicepDocs.Core.Models.Formatting;

namespace LandingZones.Tools.BicepDocs.Core;

public class FormatterContext
{
    // Module path is relative to the input, so <inputFolder>/module/path/is/here
    public FormatterContext(SemanticModel template, string modulePath, FormatterOptions? options = null)
    {
        Template = template;
        ModulePath = modulePath;
        FormatterOptions = options ?? new FormatterOptions();
    }

    public SemanticModel Template { get; }
    public string ModulePath { get; }

    public FormatterOptions FormatterOptions { get; }
}