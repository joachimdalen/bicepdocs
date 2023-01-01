using System.CommandLine;
using LandingZones.Tools.BicepDocs.Core.Models.Formatting;
using LandingZones.Tools.BicepDocs.Core.Validators;

namespace LandingZones.Tools.BicepDocs.Source.FileSystem.Commands;

public class FileSystemCommand : Command
{
    private static readonly Option<string> FolderPath =
        new(name: "--folderPath", description: "Path to folder container bicep definitions")
        {
            IsRequired = true
        };

    private static readonly Option<string> Out = new(name: "--out",
        description: "Root folder containing existing docs or empty folder")
    {
        IsRequired = true
    };

    private static readonly Option<DocFormatter> Formatter =
        new(name: "--formatter", description: "The formatter used to format the docs")
        {
            IsRequired = true
        };

    private static readonly Option<string[]> Exclude =
        new(name: "--exclude", description: "Glob patterns to exclude generation from");

    private static readonly Option<bool> DryRun = new(name: "--dryrun",
        description: "Only output what files would be written without generating them", getDefaultValue: () => false);

    public FileSystemCommand() : base("filesystem",
        "Generate documentation for modules from a folder in the filesystem")
    {
        FolderPath.ValidateFolderPath();
        Out.ValidateFolderPath(false);

        AddOption(FolderPath);
        AddOption(Out);
        AddOption(Formatter);
        AddOption(Exclude);
        AddOption(DryRun);
    }
}