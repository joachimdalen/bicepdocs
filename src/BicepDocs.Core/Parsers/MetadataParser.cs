using Bicep.Core.Semantics;
using Bicep.Core.Syntax;
using LandingZones.Tools.BicepDocs.Core.Models.Formatting;

namespace LandingZones.Tools.BicepDocs.Core.Parsers;

public static class MetadataParser
{
    public const string MetadataKey = "moduleDocs";

    public static MetadataModel? GetMetadata(SemanticModel model, string metaKey = MetadataKey)
    {
        var mod = model.Root.MetadataDeclarations.FirstOrDefault(x => x.Name == metaKey);
        if (mod == null) return null;

        var title = GetMetadataField(nameof(MetadataModel.Title).ToLower(), mod);
        var description = GetMetadataField(nameof(MetadataModel.Description).ToLower(), mod);
        var author = GetMetadataField(nameof(MetadataModel.Owner).ToLower(), mod);
        var version = GetMetadataField(nameof(MetadataModel.Version).ToLower(), mod);

        if (string.IsNullOrEmpty(title) && string.IsNullOrEmpty(description) && string.IsNullOrEmpty(author) &&
            string.IsNullOrEmpty(version))
            return null;

        return new MetadataModel
        {
            Title = title,
            Description = description,
            Owner = author,
            Version = version
        };
    }

    private static string? GetMetadataField(string fieldName, MetadataSymbol symbol)
    {
        if (symbol.Value is not ObjectSyntax objectSyntax) return null;
        var objectField = objectSyntax.Properties.ToList()
            .FirstOrDefault(x => (x.Key as IdentifierSyntax)?.IdentifierName == fieldName);
        if (objectField?.Value is not StringSyntax stringToken) return null;
        if (!stringToken.StringTokens.Any()) return null;
        var token = stringToken.StringTokens.First().Text;
        return token.Trim('\'');
    }
}