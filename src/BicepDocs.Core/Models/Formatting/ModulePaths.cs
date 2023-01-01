namespace LandingZones.Tools.BicepDocs.Core.Models.Formatting;

/// <summary>
/// Contains the file system paths for a module generation
/// </summary>
/// <param name="InputFolder">Input paths to read modules from </param>
/// <param name="OutputBaseFolder">Output folder on the file system to write converted files to</param>
/// <param name="VirtualPath">Virtual path to use in virtual file system</param>
/// <param name="InputFileName">Input file name (with .bicep)</param>
/// <param name="OutputFileName">Output file name</param>
/// <param name="BaseFileName">Filename without extension</param>
/// <param name="OutputPath">File path to write to</param>
/// <param name="RelativeInputPath"></param>
/// <param name="VirtualFolder"></param>
/// <param name="OutputFolder"></param>
public record ModulePaths(
    string InputFolder,
    string OutputBaseFolder,
    string VirtualPath,
    string InputFileName,
    string OutputFileName,
    string BaseFileName,
    string OutputPath,
    string RelativeInputPath,
    string VirtualFolder,
    string OutputFolder);
