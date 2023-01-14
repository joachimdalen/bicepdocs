using System.CommandLine.NamingConventionBinder;

namespace LandingZones.Tools.BicepDocs.Source.FileSystem;

public class SourceOptionsDemo
{
    public string FolderPath { get; set; }
}

public class SourceOptionsBinder : ModelBinder<SourceOptionsDemo>
{
}