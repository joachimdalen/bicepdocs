using System.CommandLine;

namespace LandingZones.Tools.BicepDocs.Cli.Commands;

public class BicepDocsRootCmd : RootCommand
{
    public BicepDocsRootCmd()
    {
        AddGlobalOption(GlobalOptions.RegistryName);
        AddGlobalOption(GlobalOptions.ConfigPath);
    }
}