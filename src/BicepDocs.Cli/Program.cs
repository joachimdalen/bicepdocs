using System.CommandLine.Builder;
using System.CommandLine.Hosting;
using System.CommandLine.NamingConventionBinder;
using System.CommandLine.Parsing;
using LandingZones.Tools.BicepDocs.Cli.Commands;
using LandingZones.Tools.BicepDocs.Cli.Commands.Generate;
using LandingZones.Tools.BicepDocs.Core;
using LandingZones.Tools.BicepDocs.Formatter.Docusaurus;
using LandingZones.Tools.BicepDocs.Formatter.Markdown;
using LandingZones.Tools.BicepDocs.Source.FileSystem;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;

namespace LandingZones.Tools.BicepDocs.Cli;

public static class Program
{
    public static Task<int> Main(string[] args) => CreateParser().InvokeAsync(args);

    private static Parser CreateParser()
    {
        var rootCommand = new BicepDocsRootCmd();
        rootCommand.AddCommand(new GenerateCommand());

        var parser = new CommandLineBuilder(rootCommand)
            .UseHost(Host.CreateDefaultBuilder, ConfigureHost)
            .UseDefaults()
            .Build();

        rootCommand.Handler = CommandHandler.Create(() => parser.Invoke("-h"));
        return parser;
    }

    private static void ConfigureHost(IHostBuilder builder) => builder
        .ConfigureServices(services => services
            .AddStaticFileSystem()
            .AddConfigurationLoader()
            .AddBicepCore()
            .AddBicepDecompiler()
            .AddBicepFileService()
            .AddFileSystemSource()
            .AddMarkdownDocProvider()
            .AddDocusaurusDocsProvider()
            .AddLogging()
        )
        .UseSerilog((context, logging) => logging
            .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
            .MinimumLevel.Override("System", LogEventLevel.Warning)
            .WriteTo.Console())
        .AddFileSystemCommands();
}