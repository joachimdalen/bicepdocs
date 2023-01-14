using System.CommandLine;
using System.CommandLine.Binding;
using System.CommandLine.NamingConventionBinder;
using LandingZones.Tools.BicepDocs.Core.Commands;
using LandingZones.Tools.BicepDocs.Core.Models.Source;
using LandingZones.Tools.BicepDocs.Core.Validators;
using LandingZones.Tools.BicepDocs.Source.FileSystem.Models;

namespace LandingZones.Tools.BicepDocs.Source.FileSystem.Commands;

public class FileSystemSourceCommand : SourceCommand
{
    public override IValueDescriptor BinderDescriptor => new ModelBinder<FileSystemSourceOptions>().ValueDescriptor;

    private static readonly Option<string> FolderPath =
        new(name: "--folderPath", description: "Path to folder container bicep definitions")
        {
            IsRequired = true
        };

    // private static readonly Option<string> Out = new(name: "--out",
    //     description: "Root folder containing existing docs or empty folder")
    // {
    //     IsRequired = true
    // };

    private static readonly Option<string[]> Exclude =
        new(name: "--exclude", description: "Glob patterns to exclude generation from");

    public FileSystemSourceCommand() : base(DocSource.FileSystem, "filesystem",
        "Generate documentation for modules from a folder in the filesystem")
    {
        FolderPath.ValidateFolderPath();
        //Out.ValidateFolderPath(false);

        AddGlobalOption(FolderPath);
        //AddGlobalOption(Out);
        AddGlobalOption(Exclude);
    }
}