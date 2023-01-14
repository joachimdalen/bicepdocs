using System.CommandLine;
using LandingZones.Tools.BicepDocs.Core.Commands;
using LandingZones.Tools.BicepDocs.Core.Models.Destination;
using LandingZones.Tools.BicepDocs.Core.Validators;

namespace LandingZones.Tools.BicepDocs.Destination.Folder.Commands;

public class FolderDestinationCommand : DestinationCommand
{
    private static readonly Option<string> OutFolder =
        new(name: "--out", description: "Folder to write output to")
        {
            IsRequired = true
        };


    public FolderDestinationCommand() : base(DocDestination.Folder, "folder",
        "Write documentation files to a folder on the current machine")
    {
        OutFolder.ValidateFolderPath(false);
        AddOption(OutFolder);
    }
}