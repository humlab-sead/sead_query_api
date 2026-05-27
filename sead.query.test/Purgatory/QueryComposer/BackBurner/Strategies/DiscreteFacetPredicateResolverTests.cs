using System.Collections.Generic;
using System.Threading.Tasks;
using FakeItEasy;
using SeadQueryCore.QueryComposer.Models;
using SeadQueryCore.QueryComposer.Strategies;
using Xunit;

namespace SeadQuery.Tests.QueryComposer.Strategies;

public class DiscreteFacetPredicateResolverTests
{
    private readonly IRouteParser _fakeRouteParser;
    private readonly ITableRelationRepository _fakeRelationRepository;
    private readonly DiscreteFacetPredicateResolver _resolver;

    public DiscreteFacetPredicateResolverTests()
    {
        _fakeRouteParser = A.Fake<IRouteParser>();
        _fakeRelationRepository = A.Fake<ITableRelationRepository>();
        _resolver = new DiscreteFacetPredicateResolver(_fakeRouteParser, _fakeRelationRepository);
    }

    [Fact]
    public async Task ResolveSqlAsync_WithExplicitSql_ReturnsExplicitSql()
    {
        // Arrange
        var userInput = new DiscreteFacetUserInput
        {
            Picks = new List<object> { 1, 2, 3 },
        };
        var configuration = new AnchorTemplate { ExplicitSql = "SELECT custom_query FROM custom_table WHERE id IN (@picks)" };

        // Act
        var result = await _resolver.ResolveSqlAsync(
            "test_facet",
            "tbl_test",
            "test_id",
            userInput,
            configuration,
            "tbl_anchor",
            "anchor_id"
        );

        // Assert
        Assert.Equal(configuration.ExplicitSql, result);
    }

    [Fact]
    public async Task ResolveSqlAsync_WithSimpleRoute_GeneratesCorrectSql()
    {
        // Arrange
        var userInput = new DiscreteFacetUserInput
        {
            Picks = new List<object> { 1, 2, 3 },
        };
        var configuration = new AnchorTemplate { Route = new List<string> { "tbl_intermediate" } };

        A.CallTo(() => _fakeRouteParser.ResolveRouteAsync(A<List<string>>._)).Returns(new List<string> { "tbl_intermediate" });

        A.CallTo(() => _fakeRelationRepository.GetRelationAsync("tbl_test", "tbl_intermediate"))
            .Returns(new TableRelation { SourceKey = "test_id", ForeignKey = "test_id" });

        A.CallTo(() => _fakeRelationRepository.GetRelationAsync("tbl_intermediate", "tbl_anchor"))
            .Returns(new TableRelation { SourceKey = "anchor_id", ForeignKey = "anchor_id" });

        // Act
        var result = await _resolver.ResolveSqlAsync(
            "test_facet",
            "tbl_test",
            "test_id",
            userInput,
            configuration,
            "tbl_anchor",
            "anchor_id"
        );

        // Assert
        Assert.Contains("SELECT", result);
        Assert.Contains("FROM tbl_test", result);
        Assert.Contains("JOIN tbl_intermediate", result);
        Assert.Contains("target_id", result);
        Assert.Contains("anchor_id", result);
    }

    [Fact]
    public async Task ResolveSqlAsync_WithNoPicks_ReturnsBaseSqlWithoutCriteria()
    {
        // Arrange
        var userInput = new DiscreteFacetUserInput { Picks = new List<object>() };
        var configuration = new AnchorTemplate { Route = new List<string>() };

        // Act
        var result = await _resolver.ResolveSqlAsync("test_facet", "tbl_test", "test_id", userInput, configuration, "tbl_test", "test_id");

        // Assert
        Assert.Contains("SELECT", result);
        Assert.Contains("FROM tbl_test", result);
        Assert.Contains("@criteria", result);
    }

    [Fact]
    public async Task ResolveSqlAsync_WithDistinctRequired_AddsDistinctClause()
    {
        // Arrange
        var userInput = new DiscreteFacetUserInput
        {
            Picks = new List<object> { 1, 2, 3 },
        };
        var configuration = new AnchorTemplate { Route = new List<string>(), RequiresDistinct = true };

        // Act
        var result = await _resolver.ResolveSqlAsync("test_facet", "tbl_test", "test_id", userInput, configuration, "tbl_test", "test_id");

        // Assert
        Assert.Contains("SELECT DISTINCT", result);
    }

    [Theory]
    [InlineData("tbl_sites", "s")]
    [InlineData("tbl_sample_groups", "sg")]
    [InlineData("tbl_physical_samples", "ps")]
    [InlineData("short_name", "sho")]
    public void GetTableAlias_WithVariousTableNames_ReturnsExpectedAlias(string tableName, string expectedAlias)
    {
        // Act
        var result = _resolver.GetTableAlias(tableName);

        // Assert
        Assert.Equal(expectedAlias, result);
    }
}
