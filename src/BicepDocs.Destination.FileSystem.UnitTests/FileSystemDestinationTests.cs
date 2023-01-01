using System.Collections.Immutable;
using System.IO.Abstractions;
using LandingZones.Tools.BicepDocs.Core.Abstractions;
using LandingZones.Tools.BicepDocs.Core.Models.Formatting;
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

    [TestMethod]
    public void Write_InvalidGenerationFileType_Throws()
    {
        var sut = new FileSystemDestination(new NullLogger<FileSystemDestination>(), _fileSystemMock.Object);
        var files = new List<GenerationFile>()
        {
            new DummyGenFile("/somewhere/folder/deploy.md")
        }.ToImmutableList();

        Assert.ThrowsExceptionAsync<ArgumentException>(() => sut.Write(files));
    }

    [TestMethod]
    public async Task Write_NoVersion_WritesSingleFile()
    {
        _fileSystemMock.Setup(x => x.Directory.Exists("/somewhere/folder")).Returns(true);
        _fileSystemMock
            .Setup(x => x.File.WriteAllTextAsync("/somewhere/folder/deploy.md", It.IsAny<string>(),
                It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

        var sut = new FileSystemDestination(new NullLogger<FileSystemDestination>(), _fileSystemMock.Object);
        var files = new List<GenerationFile>()
        {
            new TextGenerationFile("/somewhere/folder/deploy.md", "hello-there")
        }.ToImmutableList();

        await sut.Write(files);

        _fileSystemMock.Verify(
            x => x.File.WriteAllTextAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [TestMethod]
    public async Task Write_WithVersion_WritesMultipleFiles()
    {
        _fileSystemMock.Setup(x => x.Directory.Exists(It.IsAny<string>())).Returns(true);
        _fileSystemMock
            .Setup(x => x.File.WriteAllTextAsync("/somewhere/folder/deploy.md", It.IsAny<string>(),
                It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
        _fileSystemMock
            .Setup(x => x.File.WriteAllTextAsync("/somewhere/v1/folder/deploy.md", It.IsAny<string>(),
                It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

        var sut = new FileSystemDestination(new NullLogger<FileSystemDestination>(), _fileSystemMock.Object);
        var files = new List<GenerationFile>()
        {
            new TextGenerationFile("/somewhere/folder/deploy.md", "hello-there", "/somewhere/v1/folder/deploy.md")
        }.ToImmutableList();

        await sut.Write(files);

        _fileSystemMock.Verify(
            x => x.File.WriteAllTextAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()),
            Times.Exactly(2));
    }

    [TestMethod]
    public async Task Write_OutDirectoryDoesNotExist_CreatesDirectory()
    {
        _fileSystemMock.Setup(x => x.Directory.Exists("/somewhere/folder")).Returns(false);
        _fileSystemMock.Setup(x => x.Directory.CreateDirectory("/somewhere/folder")).Returns((IDirectoryInfo)null!);
        _fileSystemMock
            .Setup(x => x.File.WriteAllTextAsync("/somewhere/folder/deploy.md", It.IsAny<string>(),
                It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

        var sut = new FileSystemDestination(new NullLogger<FileSystemDestination>(), _fileSystemMock.Object);
        var files = new List<GenerationFile>()
        {
            new TextGenerationFile("/somewhere/folder/deploy.md", "hello-there")
        }.ToImmutableList();

        await sut.Write(files);

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