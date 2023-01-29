using System.CommandLine;
using System.CommandLine.Binding;
using System.CommandLine.NamingConventionBinder;
using LandingZones.Tools.BicepDocs.Core.Commands;
using LandingZones.Tools.BicepDocs.Core.Models.Destination;

namespace LandingZones.Tools.BicepDocs.Destination.Confluence.Commands;

public class ConfluenceDestinationCommand : DestinationCommand
{
    public override IValueDescriptor BinderDescriptor => new ModelBinder<ConfluenceOptions>().ValueDescriptor;

    private static readonly Option<string> InstanceName =
        new(name: "--instanceName", description: "Path to folder container bicep definitions")
        {
            IsRequired = true
        };

    private static readonly Option<string> User =
        new(name: "--user", description: "The email for the current user")
        {
            IsRequired = true
        };

    private static readonly Option<string> Token =
        new(name: "--token", description: "The token for the API",
            getDefaultValue: () => Environment.GetEnvironmentVariable("BD_CONFLUENCE_TOKEN"));

    private static readonly Option<string> SpaceKey =
        new(name: "--spaceKey", description: "The key of the confluence space")
        {
            IsRequired = true
        };

    private static readonly Option<int> RootPageId =
        new(name: "--rootPageId", description: "The ID of the page to serve as root page")
        {
            IsRequired = true
        };


    public ConfluenceDestinationCommand() : base(DocDestination.Confluence, "confluence",
        "Upload generated files to confluence")
    {
        AddOption(InstanceName);
        AddOption(User);
        AddOption(Token);
        AddOption(SpaceKey);
        AddOption(RootPageId);
    }
}