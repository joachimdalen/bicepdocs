using System.CommandLine;
using LandingZones.Tools.BicepDocs.Core.Models;

namespace LandingZones.Tools.BicepDocs.Cli.Commands.Generate.FileSystem;

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

    private static readonly Option<DocProvider> Provider =
        new(name: "--provider", description: "The provided used to format the docs")
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
        FolderPath.AddValidator(validationResult =>
        {
            var value = validationResult.GetValueForOption(FolderPath);
            if (string.IsNullOrEmpty(value))
            {
                validationResult.ErrorMessage = "File path is required";
                return;
            }

            if (!File.GetAttributes(value).HasFlag(FileAttributes.Directory))
            {
                validationResult.ErrorMessage = "Path is not a valid directory";
                return;
            }

            if (!Directory.Exists(value))
            {
                validationResult.ErrorMessage = "Path is not a directory";
            }
        });

        Out.AddValidator(validationResult =>
        {
            var value = validationResult.GetValueForOption(FolderPath);
            if (string.IsNullOrEmpty(value))
            {
                validationResult.ErrorMessage = "Out path is required";
                return;
            }

            if (!File.GetAttributes(value).HasFlag(FileAttributes.Directory))
            {
                validationResult.ErrorMessage = "Path is not a valid directory";
                return;
            }
        });

        AddOption(FolderPath);
        AddOption(Out);
        AddOption(Provider);
        AddOption(Exclude);
        AddOption(DryRun);
    }
}