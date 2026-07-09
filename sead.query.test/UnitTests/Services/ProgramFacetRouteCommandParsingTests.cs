using System;
using FluentAssertions;
using SeadQueryAPI.Cli;
using Xunit;

namespace SQT.UnitTests.Services;

public class ProgramFacetRouteCommandParsingTests
{
    [Fact]
    public void Parse_WithImportArgument_ReturnsImportCommandAndPath()
    {
        var parser = new StartupCommandParser();
        var command = parser.Parse(["--import-facet-config", "sead.query.composer/Templates/facet_configuration.yml"]);

        command.Should().BeOfType<ImportFacetConfigCommand>();
        var importCommand = (ImportFacetConfigCommand)command;
        importCommand.ConfigurationFilePath.Should().Be("sead.query.composer/Templates/facet_configuration.yml");
    }

    [Fact]
    public void Parse_WithValidateArgument_ReturnsValidationCommandAndPath()
    {
        var parser = new StartupCommandParser();
        var command = parser.Parse(["--validate-facet-config", "sead.query.composer/Templates/facet_configuration.yml"]);

        command.Should().BeOfType<ValidateFacetConfigCommand>();
        var validateCommand = (ValidateFacetConfigCommand)command;
        validateCommand.ConfigurationFilePath.Should().Be("sead.query.composer/Templates/facet_configuration.yml");
        validateCommand.Offline.Should().BeFalse();
    }

    [Fact]
    public void Parse_WithValidateArgumentAndOfflineFlag_ReturnsValidationCommandWithOfflineMode()
    {
        var parser = new StartupCommandParser();
        var command = parser.Parse(
            ["--validate-facet-config", "sead.query.composer/Templates/facet_configuration.yml", "--validate-facet-config-offline"]
        );

        command.Should().BeOfType<ValidateFacetConfigCommand>();
        var validateCommand = (ValidateFacetConfigCommand)command;
        validateCommand.ConfigurationFilePath.Should().Be("sead.query.composer/Templates/facet_configuration.yml");
        validateCommand.Offline.Should().BeTrue();
    }

    [Fact]
    public void Parse_WithConflictingFacetRouteArguments_ThrowsArgumentException()
    {
        var parser = new StartupCommandParser();
        var action = () => parser.Parse(["--import-facet-config", "import.yaml", "--validate-facet-config", "validate.yaml"]);

        action.Should().Throw<ArgumentException>().WithMessage("--import-facet-config and --validate-facet-config cannot be used together.");
    }

    [Fact]
    public void Parse_WithMissingFacetRoutePath_ThrowsArgumentException()
    {
        var parser = new StartupCommandParser();
        var action = () => parser.Parse(["--validate-facet-config"]);

        action.Should().Throw<ArgumentException>().WithMessage("--validate-facet-config requires a configuration file path.");
    }

    [Fact]
    public void Parse_WithOfflineFlagWithoutValidate_ThrowsArgumentException()
    {
        var parser = new StartupCommandParser();
        var action = () => parser.Parse(["--validate-facet-config-offline"]);

        action
            .Should()
            .Throw<ArgumentException>()
            .WithMessage("--validate-facet-config-offline can only be used together with --validate-facet-config.");
    }

    [Fact]
    public void Parse_WithFacetSqlArgument_ReturnsFacetSqlCommand()
    {
        var parser = new StartupCommandParser();
        var command = parser.Parse(["--print-facet-sql", "family:family"]);

        command.Should().BeOfType<PrintFacetSqlCommand>();
        var facetSqlCommand = (PrintFacetSqlCommand)command;
        facetSqlCommand.FacetUrl.Should().Be("family:family");
    }

    [Fact]
    public void Parse_WithMissingFacetSqlFacetUrl_ThrowsArgumentException()
    {
        var parser = new StartupCommandParser();
        var action = () => parser.Parse(["--print-facet-sql"]);

        action.Should().Throw<ArgumentException>().WithMessage("--print-facet-sql requires a facet URL.");
    }

    [Fact]
    public void Parse_WithResultSqlArgumentAndOptions_ReturnsResultSqlCommand()
    {
        var parser = new StartupCommandParser();
        var command = parser.Parse(["--print-result-sql", "family:family", "--view-type", "map", "--result-code", "map_result"]);

        command.Should().BeOfType<PrintResultSqlCommand>();
        var resultSqlCommand = (PrintResultSqlCommand)command;
        resultSqlCommand.FacetUrl.Should().Be("family:family");
        resultSqlCommand.ViewTypeId.Should().Be("map");
        resultSqlCommand.ResultCode.Should().Be("map_result");
    }

    [Fact]
    public void Parse_WithMissingResultSqlFacetUrl_ThrowsArgumentException()
    {
        var parser = new StartupCommandParser();
        var action = () => parser.Parse(["--print-result-sql"]);

        action.Should().Throw<ArgumentException>().WithMessage("--print-result-sql requires a facet URL.");
    }

    [Fact]
    public void Parse_WithConflictingSqlCommands_ThrowsArgumentException()
    {
        var parser = new StartupCommandParser();
        var action = () => parser.Parse(["--print-facet-sql", "family:family", "--print-result-sql", "family:family"]);

        action.Should().Throw<ArgumentException>().WithMessage("--print-facet-sql and --print-result-sql cannot be used together.");
    }

    [Fact]
    public void Parse_WithNoCommandArguments_ReturnsRunWebHostCommand()
    {
        var parser = new StartupCommandParser();
        var command = parser.Parse(["--urls", "http://localhost:5000"]);

        command.Should().BeOfType<RunWebHostCommand>();
        command.HostArgs.Should().Equal("--urls", "http://localhost:5000");
    }

    [Fact]
    public void Parse_WithMissingViewTypeValue_ThrowsArgumentException()
    {
        var parser = new StartupCommandParser();
        var action = () => parser.Parse(["--print-result-sql", "family:family", "--view-type"]);

        action.Should().Throw<ArgumentException>().WithMessage("--view-type requires a non-empty view type when provided.");
    }

    [Fact]
    public void Parse_WithMissingResultCodeValue_ThrowsArgumentException()
    {
        var parser = new StartupCommandParser();
        var action = () => parser.Parse(["--print-result-sql", "family:family", "--result-code"]);

        action.Should().Throw<ArgumentException>().WithMessage("--result-code requires a non-empty result code when provided.");
    }
}
