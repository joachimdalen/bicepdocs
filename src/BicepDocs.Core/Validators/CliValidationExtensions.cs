using System.CommandLine;

namespace LandingZones.Tools.BicepDocs.Core.Validators;

public static class CliValidationExtensions
{
    public static void ValidateFolderPath(this Option<string> option, bool folderMustExist = true)
    {
        option.AddValidator(validationResult =>
        {
            var value = validationResult.GetValueForOption(option);
            if (string.IsNullOrEmpty(value))
            {
                validationResult.ErrorMessage = "File path is required";
                return;
            }

            switch (folderMustExist)
            {
                case true when !File.GetAttributes(value).HasFlag(FileAttributes.Directory):
                    validationResult.ErrorMessage = "Path is not a valid directory";
                    return;
                case true when !Directory.Exists(value):
                    validationResult.ErrorMessage = "Path is not a directory";
                    break;
            }
        });
    }
}