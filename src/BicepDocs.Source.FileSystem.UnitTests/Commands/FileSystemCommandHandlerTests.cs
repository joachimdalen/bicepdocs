using System.CommandLine.Invocation;
using System.IO.Abstractions;
using LandingZones.Tools.BicepDocs.Core;
using LandingZones.Tools.BicepDocs.Core.Abstractions;
using LandingZones.Tools.BicepDocs.Core.Models.Formatting;
using LandingZones.Tools.BicepDocs.Core.UnitTests;
using LandingZones.Tools.BicepDocs.Destination.FileSystem;
using LandingZones.Tools.BicepDocs.Formatter.Markdown;
using LandingZones.Tools.BicepDocs.Source.FileSystem.Commands;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace LandingZones.Tools.BicepDocs.Source.FileSystem.UnitTests.Commands;

[TestClass]
public class FileSystemCommandHandlerTests : BicepFileTestBase
{
    private readonly ILogger<FileSystemCommandHandler> _logger;
    private readonly Mock<IStaticFileSystem> _staticFileSystem;
    private readonly ConfigurationLoader _configurationLoader;
    private readonly Mock<IMatcher> _matcher;
    private readonly IBicepFileService _bicepFileService;

    public FileSystemCommandHandlerTests()
    {
        _logger = new NullLogger<FileSystemCommandHandler>();
        _staticFileSystem = new Mock<IStaticFileSystem>(MockBehavior.Strict);
        _configurationLoader = new ConfigurationLoader(_staticFileSystem.Object);
        _matcher = new Mock<IMatcher>(MockBehavior.Strict);
        _bicepFileService = ServiceProvider.GetRequiredService<IBicepFileService>();
    }

    [TestMethod]
    public async Task Invoke_GetResultsInFullPath_Null_Returns()
    {
        var sut = GetSut(
            new List<IBicepSource>
            {
                GetFileSystemSource()
            },
            new List<IDocsFormatter>
            {
                new MarkdownDocsFormatter()
            }, new List<IDocsDestination>
            {
                GetFileSystemDestination()
            }
        );


        _matcher.Setup(x => x.AddIncludePatterns(It.IsAny<IEnumerable<string>>()));
        _matcher.Setup(x => x.GetResultsInFullPath(sut.FolderPath)).Returns((List<string>)null!);

        var result = await sut.InvokeAsync(new InvocationContext(null!));
        Assert.AreEqual(-1, result);
    }

    [TestMethod]
    public async Task Invoke_GetResultsInFullPath_Empty_Returns()
    {
        var sut = GetSut(
            new List<IBicepSource>
            {
                GetFileSystemSource()
            },
            new List<IDocsFormatter>
            {
                new MarkdownDocsFormatter()
            }, new List<IDocsDestination>
            {
                GetFileSystemDestination()
            }
        );


        _matcher.Setup(x => x.AddIncludePatterns(It.IsAny<IEnumerable<string>>()));
        _matcher.Setup(x => x.GetResultsInFullPath(sut.FolderPath)).Returns(new List<string>());

        var result = await sut.InvokeAsync(new InvocationContext(null!));
        Assert.AreEqual(-1, result);
    }

    [TestMethod]
    public async Task Invoke_Defaults_GeneratesAndWrites()
    {
        var sut = GetSut(
            new List<IBicepSource>
            {
                GetFileSystemSource()
            },
            new List<IDocsFormatter>
            {
                new MarkdownDocsFormatter()
            }, new List<IDocsDestination>
            {
                GetFileSystemDestination()
            }
        );

        const string template = @"
resource resourceGroup 'Microsoft.Resources/resourceGroups@2021-01-01' = {
  name: resourceGroupName
  location: resourceGroupLocation
  tags: tags
}";

        const string modulePath = "/input/modules/rg/resourceGroup.bicep";
        const string moduleOut = "/output/modules/rg";
        const string moduleOutFile = "/output/modules/rg/resourceGroup.md";

        _matcher.Setup(x => x.AddIncludePatterns(It.IsAny<IEnumerable<string>>()));
        _matcher.Setup(x => x.GetResultsInFullPath(sut.FolderPath)).Returns(
            new List<string>
            {
                modulePath
            });

        // Loading
        _staticFileSystem.Setup(x => x.Directory.Exists(sut.Out)).Returns(false);
        _staticFileSystem.Setup(x => x.Directory.CreateDirectory(sut.Out)).Returns((IDirectoryInfo)null!);
        _staticFileSystem.Setup(x => x.File.ReadAllTextAsync(modulePath, It.IsAny<CancellationToken>()))
            .ReturnsAsync(template);


        //Writing
        _staticFileSystem.Setup(x => x.Directory.Exists(moduleOut)).Returns(false);
        _staticFileSystem.Setup(x => x.Directory.CreateDirectory(moduleOut)).Returns((IDirectoryInfo)null!);
        _staticFileSystem
            .Setup(x => x.File.WriteAllTextAsync(moduleOutFile, It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);


        var result = await sut.InvokeAsync(new InvocationContext(null!));
        Assert.AreEqual(1, result);
    }


    private FileSystemCommandHandler GetSut(
        IEnumerable<IBicepSource> sources,
        IEnumerable<IDocsFormatter> generators,
        IEnumerable<IDocsDestination> destinations)
    {
        var sut = new FileSystemCommandHandler(
            _logger,
            generators,
            destinations,
            sources,
            _configurationLoader,
            _bicepFileService
        )
        {
            FolderPath = "/input/modules",
            Out = "/output/modules",
            Formatter = DocFormatter.Markdown
        };

        return sut;
    }

    private FileSystemSource GetFileSystemSource() =>
        new(_staticFileSystem.Object, _matcher.Object, new NullLogger<FileSystemSource>());

    private FileSystemDestination GetFileSystemDestination() =>
        new(NullLogger<FileSystemDestination>.Instance, _staticFileSystem.Object);
}