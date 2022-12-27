namespace LandingZones.Tools.BicepDocs.Core.Models;

public class ModulePaths
{
    /// <summary>
    /// Input paths to read modules from 
    /// </summary>
    public string InputFolder { get; set; }

    /// <summary>
    /// Output folder on the file system to write
    /// converted files to
    /// </summary>
    public string OutputBaseFolder { get; set; }

    /// <summary>
    /// Virtual path to use in virtual file system
    /// </summary>
    public string VirtualPath { get; set; }

    /// <summary>
    /// Input file name (with .bicep)
    /// </summary>
    public string InputFileName { get; set; }

    /// <summary>
    /// Output file name
    /// </summary>
    public string OutputFileName { get; set; }

    /// <summary>
    /// Filename without extension
    /// </summary>
    public string BaseFileName { get; set; }

    /// <summary>
    /// File path to write to
    /// </summary>
    public string OutputPath { get; set; }

    public string RelativeInputPath { get; set; }
    public string VirtualFolder { get; set; }
    public string OutputFolder { get; set; }
}