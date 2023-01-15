using System.CommandLine;
using System.CommandLine.Binding;
using LandingZones.Tools.BicepDocs.Core.Models.Destination;
using LandingZones.Tools.BicepDocs.Core.Models.Formatting;

namespace LandingZones.Tools.BicepDocs.Core.Commands;

public abstract class DestinationCommand : Command
{
    public virtual IValueDescriptor BinderDescriptor { get; } = null;

    public static readonly Option<DocFormatter> Formatter =
        new(name: "--formatter", description: "The formatter used to format the docs")
        {
            IsRequired = true
        };

    public DocDestination Destination { get; }

    protected DestinationCommand(
        DocDestination destination,
        string name, string? description = null) :
        base(name, description)
    {
        Destination = destination;

        AddOption(Formatter);
    }
}