using Bicep.Core.Parsing;
using Bicep.Core.PrettyPrint;
using Bicep.Core.PrettyPrint.Options;

namespace LandingZones.Tools.BicepDocs.Core.Services;

public static class BicepFormatter
{
    public static string FormatBicepCode(string input)
    {
        var parser = new Parser(input);
        var programSyntax = parser.Program();
        var options = new PrettyPrintOptions(NewlineOption.Auto, IndentKindOption.Space, 2, false);
        return PrettyPrinter.PrintProgram(programSyntax, options, parser.LexingErrorLookup, parser.ParsingErrorLookup);
    }
}