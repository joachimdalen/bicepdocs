using System.CommandLine;
using System.CommandLine.Binding;
using LandingZones.Tools.BicepDocs.Core.Models.Source;

namespace LandingZones.Tools.BicepDocs.Core.Commands;

public abstract class SourceCommand : Command
{
    public DocSource Source { get; }

    public virtual IValueDescriptor BinderDescriptor { get; } = null;

    protected SourceCommand(DocSource source, string name, string? description = null) : base(name, description)
    {
        Source = source;
    }
}