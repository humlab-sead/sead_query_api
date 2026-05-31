using System;
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
        var relativePath = "sead.query.composer/Templates/route_v1.yaml";

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
}

public class FacetRouteConfigurationValidationCommandTests
{
    [Fact]
    public void Run_WithRelativePath_ValidatesUsingAbsolutePath()
    {
        var importer = new Mock<IFacetRouteConfigurationImporter>();
        var command = new FacetRouteConfigurationValidationCommand(importer.Object);
        var relativePath = "sead.query.composer/Templates/route_v1.yaml";

        command.Run(relativePath);

        importer.Verify(service => service.ValidateFile(System.IO.Path.GetFullPath(relativePath)), Times.Once);
    }

    [Fact]
    public void Run_WithEmptyPath_ThrowsArgumentException()
    {
        var importer = new Mock<IFacetRouteConfigurationImporter>();
        var command = new FacetRouteConfigurationValidationCommand(importer.Object);

        var action = () => command.Run(" ");

        action.Should().Throw<ArgumentException>();
        importer.Verify(service => service.ValidateFile(It.IsAny<string>()), Times.Never);
    }
}
