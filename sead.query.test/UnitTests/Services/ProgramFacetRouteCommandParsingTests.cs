using FluentAssertions;
using SeadQueryAPI;
using Xunit;

namespace SQT.UnitTests.Services;

public class ProgramFacetRouteCommandParsingTests
{
    [Fact]
    public void TryGetFacetRouteCommand_WithImportArgument_ReturnsImportCommandAndPath()
    {
        var result = Program.TryGetFacetRouteCommand(
            ["--import-facet-config", "sead.query.composer/Templates/route_v1.yaml"],
            out var commandType,
            out var configurationFilePath,
            out var parseError
        );

        result.Should().BeTrue();
        commandType.Should().Be(Program.FacetRouteConfigurationCommandType.Import);
        configurationFilePath.Should().Be("sead.query.composer/Templates/route_v1.yaml");
        parseError.Should().BeNull();
    }

    [Fact]
    public void TryGetFacetRouteCommand_WithValidateArgument_ReturnsValidationCommandAndPath()
    {
        var result = Program.TryGetFacetRouteCommand(
            ["--validate-facet-config", "sead.query.composer/Templates/route_v1.yaml"],
            out var commandType,
            out var configurationFilePath,
            out var parseError
        );

        result.Should().BeTrue();
        commandType.Should().Be(Program.FacetRouteConfigurationCommandType.Validate);
        configurationFilePath.Should().Be("sead.query.composer/Templates/route_v1.yaml");
        parseError.Should().BeNull();
    }

    [Fact]
    public void TryGetFacetRouteCommand_WithConflictingArguments_ReturnsParseError()
    {
        var result = Program.TryGetFacetRouteCommand(
            ["--import-facet-config", "import.yaml", "--validate-facet-config", "validate.yaml"],
            out var commandType,
            out var configurationFilePath,
            out var parseError
        );

        result.Should().BeFalse();
        commandType.Should().Be(Program.FacetRouteConfigurationCommandType.None);
        configurationFilePath.Should().BeNull();
        parseError.Should().Be("--import-facet-config and --validate-facet-config cannot be used together.");
    }

    [Fact]
    public void TryGetFacetRouteCommand_WithMissingPath_ReturnsParseError()
    {
        var result = Program.TryGetFacetRouteCommand(
            ["--validate-facet-config"],
            out var commandType,
            out var configurationFilePath,
            out var parseError
        );

        result.Should().BeFalse();
        commandType.Should().Be(Program.FacetRouteConfigurationCommandType.Validate);
        configurationFilePath.Should().BeNull();
        parseError.Should().Be("--validate-facet-config requires a configuration file path.");
    }
}
