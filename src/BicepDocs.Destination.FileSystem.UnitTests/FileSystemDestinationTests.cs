using System.Collections.Immutable;
using System.IO.Abstractions;
using LandingZones.Tools.BicepDocs.Core.Abstractions;
using LandingZones.Tools.BicepDocs.Core.Extensions;
using LandingZones.Tools.BicepDocs.Core.Models.Formatting;
using LandingZones.Tools.BicepDocs.Destination.Folder;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace LandingZones.Tools.BicepDocs.Destination.FileSystem.UnitTests;

[TestClass]
public class FileSystemDestinationTests
{
    private readonly Mock<IStaticFileSystem> _fileSystemMock;

    public FileSystemDestinationTests()
    {
        _fileSystemMock = new Mock<IStaticFileSystem>(MockBehavior.Strict);
    }

    private readonly FolderDestinationOptions _folderDestinationOptions = new("/somewhere");


    [TestMethod]
    public void Write_InvalidGenerationFileType_Throws()
    {
        var sut = new FolderDestination(new NullLogger<FolderDestination>(), _fileSystemMock.Object);
        var files = new List<GenerationFile>()
        {
            new DummyGenFile("folder/deploy.md".ToPlatformPath())
        }.ToImmutableList();

        Assert.ThrowsExceptionAsync<ArgumentException>(() => sut.Write(files));
    }

    [TestMethod]
    public async Task Write_NoVersion_WritesSingleFile()
    {
        _fileSystemMock.Setup(x => x.Directory.Exists("/somewhere/folder".ToPlatformPath())).Returns(true);
        _fileSystemMock
            .Setup(x => x.File.WriteAllTextAsync("/somewhere/folder/deploy.md".ToPlatformPath(), It.IsAny<string>(),
                It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

        var sut = new FolderDestination(new NullLogger<FolderDestination>(), _fileSystemMock.Object);
        var files = new List<GenerationFile>()
        {
            new TextGenerationFile("/folder/deploy.md".ToPlatformPath(), "hello-there")
        }.ToImmutableList();

        await sut.Write(files, _folderDestinationOptions);

        _fileSystemMock.Verify(
            x => x.File.WriteAllTextAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [TestMethod]
    public async Task Write_WithVersion_WritesMultipleFiles()
    {
        _fileSystemMock.Setup(x => x.Directory.Exists(It.IsAny<string>())).Returns(true);
        _fileSystemMock
            .Setup(x => x.File.WriteAllTextAsync("/somewhere/folder/deploy.md".ToPlatformPath(), It.IsAny<string>(),
                It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
        _fileSystemMock
            .Setup(x => x.File.WriteAllTextAsync("/somewhere/v1/folder/deploy.md".ToPlatformPath(), It.IsAny<string>(),
                It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

        var sut = new FolderDestination(new NullLogger<FolderDestination>(), _fileSystemMock.Object);
        var files = new List<GenerationFile>()
        {
            new TextGenerationFile("/folder/deploy.md".ToPlatformPath(), "hello-there",
                "v1/folder/deploy.md".ToPlatformPath())
        }.ToImmutableList();

        await sut.Write(files, _folderDestinationOptions);

        _fileSystemMock.Verify(
            x => x.File.WriteAllTextAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()),
            Times.Exactly(2));
    }

    [TestMethod]
    public async Task Write_OutDirectoryDoesNotExist_CreatesDirectory()
    {
        _fileSystemMock.Setup(x => x.Directory.Exists("/somewhere/folder".ToPlatformPath())).Returns(false);
        _fileSystemMock.Setup(x => x.Directory.CreateDirectory("/somewhere/folder".ToPlatformPath()))
            .Returns((IDirectoryInfo)null!);
        _fileSystemMock
            .Setup(x => x.File.WriteAllTextAsync("/somewhere/folder/deploy.md".ToPlatformPath(), It.IsAny<string>(),
                It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

        var sut = new FolderDestination(new NullLogger<FolderDestination>(), _fileSystemMock.Object);
        var files = new List<GenerationFile>()
        {
            new TextGenerationFile("folder/deploy.md".ToPlatformPath(), "hello-there")
        }.ToImmutableList();

        await sut.Write(files, _folderDestinationOptions);

        _fileSystemMock.Verify(
            x => x.File.WriteAllTextAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()),
            Times.Once);
    }

    class DummyGenFile : GenerationFile
    {
        public DummyGenFile(string filePath, string? versionFilePath = null) : base(filePath, versionFilePath)
        {
        }
    }
}