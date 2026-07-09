using System;
using System.IO;
using FluentAssertions;
using Moq;
using SeadQueryAPI.Services;
using SeadQueryCore;
using Xunit;

namespace SQT.UnitTests.Services;

public class FacetRouteConfigurationImportCommandTests
{
    [Fact]
    public void Run_WithRelativePath_ImportsUsingAbsolutePath()
    {
        var importer = new Mock<IFacetRouteConfigurationImporter>();
        var command = new FacetRouteConfigurationImportCommand(importer.Object);
        var relativePath = "sead.query.composer/Templates/facet_configuration.yml";

        command.Run(relativePath);

        importer.Verify(service => service.ImportFromFile(System.IO.Path.GetFullPath(relativePath)), Times.Once);
    }

    [Fact]
    public void Run_WithEmptyPath_ThrowsArgumentException()
    {
        var importer = new Mock<IFacetRouteConfigurationImporter>();
        var command = new FacetRouteConfigurationImportCommand(importer.Object);

        var action = () => command.Run(" ");

        action.Should().Throw<ArgumentException>();
        importer.Verify(service => service.ImportFromFile(It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public void Run_WithMissingCurrentDirectoryFile_ResolvesParentRelativePath()
    {
        var importer = new Mock<IFacetRouteConfigurationImporter>();
        var command = new FacetRouteConfigurationImportCommand(importer.Object);

        var originalDirectory = Directory.GetCurrentDirectory();
        var root = Path.Combine(Path.GetTempPath(), $"sead-import-test-{Guid.NewGuid():N}");
        var projectDirectory = Path.Combine(root, "project");
        Directory.CreateDirectory(projectDirectory);

        var relativePath = Path.Combine("configs", "route.yaml");
        var filePath = Path.Combine(root, relativePath);
        Directory.CreateDirectory(Path.GetDirectoryName(filePath)!);
        File.WriteAllText(filePath, "schema_version: 1");

        try
        {
            Directory.SetCurrentDirectory(projectDirectory);

            command.Run(relativePath);
        }
        finally
        {
            Directory.SetCurrentDirectory(originalDirectory);
            Directory.Delete(root, recursive: true);
        }

        importer.Verify(service => service.ImportFromFile(Path.GetFullPath(filePath)), Times.Once);
    }
}

public class FacetRouteConfigurationValidationCommandTests
{
    [Fact]
    public void Run_WithRelativePath_ValidatesUsingAbsolutePath()
    {
        var importer = new Mock<IFacetRouteConfigurationImporter>();
        var command = new FacetRouteConfigurationValidationCommand(importer.Object);
        var relativePath = "sead.query.composer/Templates/facet_configuration.yml";

        command.Run(relativePath);

        importer.Verify(service => service.ValidateFile(System.IO.Path.GetFullPath(relativePath)), Times.Once);
    }

    [Fact]
    public void Run_WithOfflineFlag_ValidatesUsingSchemaOnlyPath()
    {
        var importer = new Mock<IFacetRouteConfigurationImporter>();
        var command = new FacetRouteConfigurationValidationCommand(importer.Object);
        var relativePath = "sead.query.composer/Templates/facet_configuration.yml";

        command.Run(relativePath, offline: true);

        importer.Verify(service => service.ValidateFileSchemaOnly(System.IO.Path.GetFullPath(relativePath)), Times.Once);
        importer.Verify(service => service.ValidateFile(It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public void Run_WithEmptyPath_ThrowsArgumentException()
    {
        var importer = new Mock<IFacetRouteConfigurationImporter>();
        var command = new FacetRouteConfigurationValidationCommand(importer.Object);

        var action = () => command.Run(" ");

        action.Should().Throw<ArgumentException>();
        importer.Verify(service => service.ValidateFile(It.IsAny<string>()), Times.Never);
        importer.Verify(service => service.ValidateFileSchemaOnly(It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public void Run_WithMissingCurrentDirectoryFile_ResolvesParentRelativePath()
    {
        var importer = new Mock<IFacetRouteConfigurationImporter>();
        var command = new FacetRouteConfigurationValidationCommand(importer.Object);

        var originalDirectory = Directory.GetCurrentDirectory();
        var root = Path.Combine(Path.GetTempPath(), $"sead-validate-test-{Guid.NewGuid():N}");
        var projectDirectory = Path.Combine(root, "project");
        Directory.CreateDirectory(projectDirectory);

        var relativePath = Path.Combine("configs", "route.yaml");
        var filePath = Path.Combine(root, relativePath);
        Directory.CreateDirectory(Path.GetDirectoryName(filePath)!);
        File.WriteAllText(filePath, "schema_version: 1");

        try
        {
            Directory.SetCurrentDirectory(projectDirectory);

            command.Run(relativePath);
        }
        finally
        {
            Directory.SetCurrentDirectory(originalDirectory);
            Directory.Delete(root, recursive: true);
        }

        importer.Verify(service => service.ValidateFile(Path.GetFullPath(filePath)), Times.Once);
    }
}
