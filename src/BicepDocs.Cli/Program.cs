using System.CommandLine;
using System.CommandLine.Builder;
using System.CommandLine.Hosting;
using System.CommandLine.NamingConventionBinder;
using System.CommandLine.Parsing;
using Bicep.Core.Extensions;
using LandingZones.Tools.BicepDocs.Core;
using LandingZones.Tools.BicepDocs.Core.Commands;
using LandingZones.Tools.BicepDocs.Destination.Confluence;
using LandingZones.Tools.BicepDocs.Destination.Confluence.Commands;
using LandingZones.Tools.BicepDocs.Destination.Folder;
using LandingZones.Tools.BicepDocs.Destination.Folder.Commands;
using LandingZones.Tools.BicepDocs.Formatter.Docusaurus;
using LandingZones.Tools.BicepDocs.Formatter.Markdown;
using LandingZones.Tools.BicepDocs.Processor;
using LandingZones.Tools.BicepDocs.Source.FileSystem;
using LandingZones.Tools.BicepDocs.Source.FileSystem.Commands;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;

namespace LandingZones.Tools.BicepDocs.Cli;

public static class Program
{
    private static readonly Type[] Sources =
    {
        typeof(FileSystemSourceCommand)
    };

    private static readonly Type[] Destinations =
    {
        typeof(ConfluenceDestinationCommand),
        typeof(FolderDestinationCommand)
    };

    public static Task<int> Main(string[] args) => CreateParser().InvokeAsync(args);

    public static RootCommand BuildCommandTree()
    {
        var rootCommand = new BicepDocsRootCmd();

        var sources = Sources.Select(x => Activator.CreateInstance(x) as Command).WhereNotNull().ToList();
        var destinations = Destinations.Select(x => Activator.CreateInstance(x) as Command).WhereNotNull().ToList();

        foreach (var source in sources)
        {
            foreach (var destination in destinations)
            {
                source.AddCommand(destination);
            }

            rootCommand.AddCommand(source);
        }

        return rootCommand;
    }

    private static Parser CreateParser()
    {
        // var rootCommand = new BicepDocsRootCmd();
        // rootCommand.AddCommand(new GenerateCommand());
        var rootCommand = BuildCommandTree();

        var parser = new CommandLineBuilder(rootCommand)
            .UseHost(Host.CreateDefaultBuilder, ConfigureHost)
            .UseDefaults()
            .Build();

        rootCommand.Handler = CommandHandler.Create(() => parser.Invoke("-h"));
        return parser;
    }

    private static void ConfigureHost(IHostBuilder builder)
    {
        builder
            .ConfigureServices(services => services
                .AddHttpClient()
                .AddStaticFileSystem()
                .AddConfigurationLoader()
                .AddBicepCore()
                .AddBicepDecompiler()
                .AddBicepFileService()
                .AddFileSystemSource()
                .AddMarkdownDocFormatter()
                .AddDocusaurusDocsFormatter()
                .AddFileSystemDestination()
                .AddConfluenceDestination()
                .AddLogging()
            )
            .UseSerilog((context, logging) => logging
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .MinimumLevel.Override("System", LogEventLevel.Warning)
                .WriteTo.Console());

        foreach (var destination in Destinations)
        {
            builder.UseCommandHandler(destination, typeof(CommandProcessor));
        }
    }
    //.AddFileSystemCommands();
}