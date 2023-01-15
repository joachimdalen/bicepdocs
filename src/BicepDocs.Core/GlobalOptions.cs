using System.CommandLine;

namespace LandingZones.Tools.BicepDocs.Core;

public static class GlobalOptions
{
    static GlobalOptions()
    {
        // RegistryName = new Option<string>(name: "--registry", description: "The name of the container registry");
        // RegistryName.AddValidator((d) =>
        // {
        //     if (string.IsNullOrEmpty(d.GetValueForOption(RegistryName)))
        //     {
        //         d.ErrorMessage = "Registry name is required";
        //     }
        // });

        ConfigPath = new Option<string>(name: "--config", description: "Path to configuration file");
    }

    //public static Option<string> RegistryName { get; }
    public static Option<string> ConfigPath { get; }
}