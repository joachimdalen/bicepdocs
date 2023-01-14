using System.CommandLine.Invocation;
using System.CommandLine.NamingConventionBinder;
using System.CommandLine.Parsing;
using LandingZones.Tools.BicepDocs.Core.Commands;
using LandingZones.Tools.BicepDocs.Core.Models.Destination;
using LandingZones.Tools.BicepDocs.Core.Models.Source;

namespace LandingZones.Tools.BicepDocs.Core.Extensions;

public static class InvocationContextExtension
{
    public static SourceCommand? GetSourceCommand(this InvocationContext context)
    {
        var command = (context.ParseResult.CommandResult.Parent as CommandResult)?.Command as SourceCommand;
        return command;
    }

    public static DestinationCommand? GetDestinationCommand(this InvocationContext context)
    {
        var command = context.ParseResult.CommandResult.Command as DestinationCommand;
        return command;
    }


    public static T? GetSourceOptions<T>(this InvocationContext context, SourceCommand command) where T : class
    {
        var mb = context.BindingContext.GetOrCreateModelBinder(command.BinderDescriptor);
        var inst = mb.CreateInstance(context.BindingContext) as T;
        return inst;
    }
    
    public static DocSource GetSourceFromCommand(this InvocationContext context, SourceCommand? command)
    {
        var sourceCommand = (command ?? context.GetSourceCommand())?.Source;
        if (sourceCommand == null)
            throw new Exception("Failed to resolve source");
        return (DocSource)sourceCommand;
    }

    public static DocDestination GetDestinationFromCommand(this InvocationContext context)
    {
        var parent = context.GetDestinationCommand()?.Destination;
        if (parent == null)
            throw new Exception("Failed to resolve destination");
        return (DocDestination)parent;
    }
}