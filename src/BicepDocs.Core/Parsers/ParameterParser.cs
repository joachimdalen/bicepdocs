using System.Collections.Immutable;
using Bicep.Core.Navigation;
using Bicep.Core.Semantics;
using Bicep.Core.Syntax;
using LandingZones.Tools.BicepDocs.Core.Models.Parsing;

namespace LandingZones.Tools.BicepDocs.Core.Parsers;

public static class ParameterParser
{
    public static ImmutableList<ParsedParameter> ParseParameters(SemanticModel model)
    {
        var defaultValueSyntaxes = model.Root.ParameterDeclarations
            .ToImmutableDictionary(x => x.Name, y => y.DeclaringParameter.Modifier as ParameterDefaultValueSyntax)
            .ToDictionary(x => x.Key, y => y.Value);

        var parameters = new List<ParsedParameter>();
        foreach (var templateParameter in model.Parameters.OrderBy(x => x.Key))
        {
            var parameter = new ParsedParameter
            (
                Name: templateParameter.Key,
                Type: templateParameter.Value.TypeReference.Type.Name
            )
            {
                Description = templateParameter.Value.Description
            };
            if (defaultValueSyntaxes.TryGetValue(templateParameter.Key, out var syntaxBase) && syntaxBase != null)
            {
                parameter.DefaultValue = syntaxBase.DefaultValue.ToTextPreserveFormatting();
                parameter.IsComplexDefault = syntaxBase.DefaultValue switch
                {
                    ObjectSyntax objectSyntax when objectSyntax.ToNamedPropertyDictionary().IsEmpty => false,
                    ObjectSyntax => true,
                    ArraySyntax arraySyntax => IsComplexArray(arraySyntax),
                    _ => false
                };

                parameter.IsInterpolated = syntaxBase.DefaultValue switch
                {
                    StringSyntax stringSyntax when stringSyntax.IsInterpolated() => true,
                    PropertyAccessSyntax => true,
                    _ => false
                };
            }

            var paramType = templateParameter.Value.TypeReference.Type.Name;
            var allowList = paramType.Split('\'').Select(x => x.Trim()).Where(x => x.Length > 1).ToArray();
            var allowValues = allowList.Select(x => x.Replace("'", "")).Where(x => !string.IsNullOrEmpty(x)).ToList();
            parameter.IsComplexAllow = allowList.Length > 2;
            parameter.AllowedValues = allowValues;

            parameters.Add(parameter);
        }

        return parameters.ToImmutableList();
    }

    private static bool IsComplexArray(ArraySyntax syntax)
    {
        if (!syntax.Items.Any())
            return false;

        if (syntax.Items.Count() > 2)
            return true;

        return syntax.Items.First().Value switch
        {
            BooleanLiteralSyntax => false,
            IntegerLiteralSyntax => false,
            StringSyntax => false,
            ArraySyntax => true,
            ObjectSyntax => true,
            _ => true
        };
    }
}