using System.CommandLine;
using LandingZones.Tools.BicepDocs.Destination.Confluence.Commands;
using LandingZones.Tools.BicepDocs.Source.FileSystem.Commands;

namespace LandingZones.Tools.BicepDocs.Cli.Commands.Generate;

public class GenerateCommand : Command
{
    public GenerateCommand() : base("generate", "Generate documentation for modules")
    {
        AddCommand(new FileSystemSourceCommand());
        AddCommand(new ConfluenceDestinationCommand());
    }
}