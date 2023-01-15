using System.CommandLine;

namespace LandingZones.Tools.BicepDocs.Core.Commands;

public class BicepDocsRootCmd : RootCommand
{
    public BicepDocsRootCmd()
    {
        AddGlobalOption(GlobalOptions.ConfigPath);
    }
}