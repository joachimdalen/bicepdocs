using System.Collections.Immutable;
using Bicep.Core.Semantics;
using LandingZones.Tools.BicepDocs.Core.Models;

namespace LandingZones.Tools.BicepDocs.Core.Parsers;

public static class OutputParser
{
    public static ImmutableList<ParsedOutput> ParseOutputs(SemanticModel model)
    {
        return model.Outputs.Select(x => new ParsedOutput(x.Name, x.TypeReference.Type.Name, x.Description)).ToImmutableList();
    }
}