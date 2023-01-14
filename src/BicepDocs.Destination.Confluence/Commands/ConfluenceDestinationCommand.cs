using System.CommandLine;
using LandingZones.Tools.BicepDocs.Core.Commands;
using LandingZones.Tools.BicepDocs.Core.Models.Destination;

namespace LandingZones.Tools.BicepDocs.Destination.Confluence.Commands;

public class ConfluenceDestinationCommand : DestinationCommand
{
    private static readonly Option<string> InstanceName =
        new(name: "--instanceName", description: "Path to folder container bicep definitions")
        {
            IsRequired = true
        };


    public ConfluenceDestinationCommand() : base(DocDestination.Confluence, "confluence",
        "Upload generated files to confluence")
    {
        AddOption(InstanceName);
    }
}