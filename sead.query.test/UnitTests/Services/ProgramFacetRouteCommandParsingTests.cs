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

    [Fact]
    public void TryGetFacetSqlCommand_WithArgument_ReturnsFacetUrl()
    {
        var result = Program.TryGetFacetSqlCommand(["--print-facet-sql", "family:family"], out var facetUrl, out var parseError);

        result.Should().BeTrue();
        facetUrl.Should().Be("family:family");
        parseError.Should().BeNull();
    }

    [Fact]
    public void TryGetFacetSqlCommand_WithMissingFacetUrl_ReturnsParseError()
    {
        var result = Program.TryGetFacetSqlCommand(["--print-facet-sql"], out var facetUrl, out var parseError);

        result.Should().BeFalse();
        facetUrl.Should().BeNull();
        parseError.Should().Be("--print-facet-sql requires a facet URL.");
    }

    [Fact]
    public void TryGetResultSqlCommand_WithArgumentAndOptions_ReturnsProbeConfiguration()
    {
        var result = Program.TryGetResultSqlCommand(
            ["--print-result-sql", "family:family", "--view-type", "map", "--result-code", "map_result"],
            out var facetUrl,
            out var viewTypeId,
            out var resultCode,
            out var parseError
        );

        result.Should().BeTrue();
        facetUrl.Should().Be("family:family");
        viewTypeId.Should().Be("map");
        resultCode.Should().Be("map_result");
        parseError.Should().BeNull();
    }

    [Fact]
    public void TryGetResultSqlCommand_WithMissingFacetUrl_ReturnsParseError()
    {
        var result = Program.TryGetResultSqlCommand(
            ["--print-result-sql"],
            out var facetUrl,
            out var viewTypeId,
            out var resultCode,
            out var parseError
        );

        result.Should().BeFalse();
        facetUrl.Should().BeNull();
        viewTypeId.Should().Be("tabular");
        resultCode.Should().BeNull();
        parseError.Should().Be("--print-result-sql requires a facet URL.");
    }
}
