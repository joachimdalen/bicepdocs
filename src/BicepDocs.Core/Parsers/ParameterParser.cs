using System.Collections.Immutable;
using System.Text;
using Bicep.Core.Semantics;
using Bicep.Core.Syntax;
using LandingZones.Tools.BicepDocs.Core.Models;

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
                name: templateParameter.Key,
                type: templateParameter.Value.TypeReference.Type.Name
            )
            {
                Description = templateParameter.Value.Description
            };
            if (defaultValueSyntaxes.TryGetValue(templateParameter.Key, out var syntaxBase) && syntaxBase != null)
            {
                switch (syntaxBase.DefaultValue)
                {
                    case BooleanLiteralSyntax booleanLiteralSyntax:
                    {
                        parameter.DefaultValue = GetValue(templateParameter.Key, booleanLiteralSyntax);
                        break;
                    }
                    case ObjectSyntax objectSyntax when objectSyntax.ToNamedPropertyDictionary().IsEmpty:
                    {
                        parameter.DefaultValue = "{}";
                        break;
                    }
                    case ObjectSyntax objectSyntax:
                        parameter.DefaultValue = BuildObject(objectSyntax);
                        parameter.IsComplexDefault = true;
                        break;
                    case ArraySyntax arraySyntax:
                        parameter.DefaultValue = !arraySyntax.Items.Any()
                            ? "[]"
                            : BuildArray(templateParameter.Key, arraySyntax);
                        parameter.IsComplexDefault = IsComplexArray(arraySyntax);
                        break;
                    case StringSyntax stringSyntax when stringSyntax.IsInterpolated():
                        parameter.DefaultValue = "TODO";
                        break;
                    case StringSyntax stringSyntax:
                        parameter.DefaultValue = stringSyntax.TryGetLiteralValue();
                        break;
                }
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


    private static string GetValue(string parameter, SyntaxBase syntaxBase,
        string? indentation = null)
    {
        return syntaxBase switch
        {
            BooleanLiteralSyntax booleanLiteralSyntax => booleanLiteralSyntax.Value.ToString(),
            IntegerLiteralSyntax integerLiteralSyntax => integerLiteralSyntax.Value.ToString(),
            StringSyntax stringSyntax when stringSyntax.IsInterpolated() => "TODO",
            StringSyntax stringSyntax => $"'{stringSyntax.TryGetLiteralValue()!}'",
            ArraySyntax arraySyntax => BuildArray(parameter, arraySyntax, indentation),
            ObjectSyntax objectSyntax => BuildObject(objectSyntax),
            _ => "TODO"
        };
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

    private static string BuildObject(ObjectSyntax objectSyntax, string objIndentation = "", bool close = true,
        bool closeLine = true)
    {
        var stringBuilder = new StringBuilder();
        stringBuilder.AppendLine(objectSyntax.OpenBrace.Text);

        var properties = objectSyntax.ToNamedPropertyValueDictionary();
        var indentation = objIndentation + objectSyntax.GetBodyIndentation();
        foreach (var (key, value) in properties)
        {
            stringBuilder.Append(indentation);
            if (value is ObjectSyntax objProp)
            {
                stringBuilder.Append($"{key}: {BuildObject(objProp, indentation, false)}");
                stringBuilder.Append(indentation);
                stringBuilder.AppendLine(objectSyntax.CloseBrace.Text);
                indentation = objProp.GetBodyIndentation();
            }
            else
            {
                stringBuilder.AppendLine($"{key}: {GetValue(key, value, indentation)}");
            }
        }

        if (close)
        {
            if (closeLine)
                stringBuilder.AppendLine(objectSyntax.CloseBrace.Text);
            else
                stringBuilder.Append(objectSyntax.CloseBrace.Text);
        }

        return stringBuilder.ToString();
    }

    private static string BuildArray(string parameter, ArraySyntax arraySyntax,
        string? indentation = "")
    {
        var stringBuilder = new StringBuilder();
        stringBuilder.Append(arraySyntax.OpenBracket.Text);
        var paramCount = 0;
        foreach (var item in arraySyntax.Items)
        {
            if (paramCount != 0)
                stringBuilder.Append(indentation);

            if (item.Value is ObjectSyntax objectSyntax)
            {
                stringBuilder.Append(BuildObject(objectSyntax, indentation, true, false));
            }
            else
            {
                if (paramCount == 0 || paramCount == arraySyntax.Items.Count() - 1)
                    stringBuilder.Append(GetValue(parameter, item.Value, indentation) + " ");
                else
                    stringBuilder.AppendLine(GetValue(parameter, item.Value, indentation));
            }

            paramCount++;
        }

        stringBuilder.Append(arraySyntax.CloseBracket.Text);
        return stringBuilder.ToString();
    }
}