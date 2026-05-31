using System;
using System.Collections.Generic;
using FluentAssertions;
using Moq;
using SeadQueryComposer.QueryComposer.Inputs;
using SeadQueryComposer.RouteCompiler;
using Xunit;

namespace SQT.UnitTests.QueryComposer.RouteCompiler;

public class DiscreteFacetPredicateResolverTests
{
    private readonly Mock<IRouteSqlCompiler> _routeSqlCompiler;
    private readonly DiscreteFacetPredicateResolver _resolver;

    public DiscreteFacetPredicateResolverTests()
    {
        _routeSqlCompiler = new Mock<IRouteSqlCompiler>();
        _resolver = new DiscreteFacetPredicateResolver(_routeSqlCompiler.Object);
    }

    [Fact]
    public void Constructor_WithNullRouteSqlCompiler_ThrowsArgumentNullException()
    {
        Action act = () => new DiscreteFacetPredicateResolver(null!);

        act.Should().Throw<ArgumentNullException>().WithParameterName("routeSqlCompiler");
    }

    [Fact]
    public void ResolveSql_WithExplicitSql_ReturnsExplicitSqlWithoutCompilingRoute()
    {
        var input = new DiscreteFacetUserInput { Picks = [1, 2] };
        var anchorTemplate = new AnchorTemplate { ExplicitSql = "select source_id, target_id from custom_sql" };

        var sql = _resolver.ResolveSql("tbl_sites", "site_id", input, anchorTemplate, "tbl_samples", "sample_id");

        sql.Should().Be("select source_id, target_id from custom_sql");
        _routeSqlCompiler.Verify(x => x.Compile(It.IsAny<IReadOnlyList<string>>()), Times.Never);
        _routeSqlCompiler.Verify(x => x.Compile(It.IsAny<IReadOnlyList<string>>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public void ResolveSql_WithRouteAndPicks_CompilesFullChainAndAppendsWhereClause()
    {
        var input = new DiscreteFacetUserInput { Picks = [101, 102] };
        var anchorTemplate = new AnchorTemplate { Route = ["tbl_sample_groups"], RequiresDistinct = true };
        const string compiledSql = "select distinct X_0.site_id as source_id, X_1.sample_id as target_id\nfrom tbl_sites as X_0";

        _routeSqlCompiler
            .Setup(x =>
                x.Compile(
                    It.Is<IReadOnlyList<string>>(tables =>
                        tables.Count == 3 && tables[0] == "tbl_sites" && tables[1] == "tbl_sample_groups" && tables[2] == "tbl_samples"
                    ),
                    "site_id",
                    "sample_id"
                )
            )
            .Returns(compiledSql);

        var sql = _resolver.ResolveSql("tbl_sites", "site_id", input, anchorTemplate, "tbl_samples", "sample_id");

        sql.Should().Contain("select *");
        sql.Should().Contain(compiledSql);
        sql.Should().Contain("as predicate_query");
        sql.Should().Contain("where source_id in (101, 102)");
    }

    [Fact]
    public void ResolveSql_WithRoute_UsesSourceAndAnchorKeyOverrides()
    {
        var input = new DiscreteFacetUserInput { Picks = [1, 2, 5] };
        var anchorTemplate = new AnchorTemplate { Route = ["tbl_sites"], RequiresDistinct = true };
        const string compiledSql =
            "select distinct X_0.location_id as source_id, X_2.site_id as target_id\nfrom facet.site_location_shortcut as X_0";

        _routeSqlCompiler
            .Setup(x =>
                x.Compile(
                    It.Is<IReadOnlyList<string>>(tables =>
                        tables.Count == 3
                        && tables[0] == "facet.site_location_shortcut"
                        && tables[1] == "tbl_sites"
                        && tables[2] == "tbl_analysis_entities"
                    ),
                    "location_id",
                    "site_id"
                )
            )
            .Returns(compiledSql);

        var sql = _resolver.ResolveSql(
            "facet.site_location_shortcut",
            "location_id",
            input,
            anchorTemplate,
            "tbl_analysis_entities",
            "site_id"
        );

        sql.Should().Contain(compiledSql);
        sql.Should().Contain("where source_id in (1, 2, 5)");
    }

    [Fact]
    public void ResolveSql_WithRouteAndSourceCriteria_AppendsCriteriaInsideBaseQuery()
    {
        var input = new DiscreteFacetUserInput { Picks = [1, 2, 5] };
        var anchorTemplate = new AnchorTemplate { Route = ["tbl_sites"], RequiresDistinct = true };
        const string compiledSql =
            "select distinct X_0.location_id as source_id, X_2.site_id as target_id\nfrom facet.site_location_shortcut as X_0\nwhere X_0.location_type_id=1";

        _routeSqlCompiler
            .Setup(x =>
                x.Compile(
                    It.Is<IReadOnlyList<string>>(tables =>
                        tables.Count == 3
                        && tables[0] == "facet.site_location_shortcut"
                        && tables[1] == "tbl_sites"
                        && tables[2] == "tbl_analysis_entities"
                    ),
                    "location_id",
                    "site_id",
                    It.Is<IReadOnlyList<string>>(criteria => criteria.Count == 1 && criteria[0] == "X_0.location_type_id=1")
                )
            )
            .Returns(compiledSql);

        var sql = _resolver.ResolveSql(
            "facet.site_location_shortcut",
            "location_id",
            input,
            anchorTemplate,
            "tbl_analysis_entities",
            "site_id",
            ["X_0.location_type_id=1"]
        );

        sql.Should().Contain("where X_0.location_type_id=1");
        sql.Should().Contain("where source_id in (1, 2, 5)");
    }

    [Fact]
    public void ResolveSql_WithoutRouteAndWithSourceCriteria_AliasesSourceTableAndAppliesCriteria()
    {
        var input = new DiscreteFacetUserInput { Picks = [1] };
        var anchorTemplate = new AnchorTemplate { RequiresDistinct = true, IsIdentityRoute = true };

        var sql = _resolver.ResolveSql(
            "facet.site_location_shortcut",
            "location_id",
            input,
            anchorTemplate,
            "facet.site_location_shortcut",
            "location_id",
            ["X_0.location_type_id=1"]
        );

        sql.Should().Contain("select distinct X_0.location_id as source_id, X_0.location_id as target_id");
        sql.Should().Contain("from facet.site_location_shortcut as X_0");
        sql.Should().Contain("where X_0.location_type_id=1");
        sql.Should().Contain("where source_id in (1)");
    }

    [Fact]
    public void ResolveSql_WithoutRoute_BuildsIdentitySql()
    {
        var input = new DiscreteFacetUserInput();
        var anchorTemplate = new AnchorTemplate { RequiresDistinct = true, IsIdentityRoute = true };

        var sql = _resolver.ResolveSql("tbl_sites", "site_id", input, anchorTemplate, "tbl_sites", "site_id");

        sql.Should().Contain("select distinct site_id as source_id, site_id as target_id");
        sql.Should().Contain("from tbl_sites");
        _routeSqlCompiler.Verify(x => x.Compile(It.IsAny<IReadOnlyList<string>>()), Times.Never);
        _routeSqlCompiler.Verify(x => x.Compile(It.IsAny<IReadOnlyList<string>>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public void ResolveSql_WithSingleValueOperator_UsesScalarComparison()
    {
        var input = new DiscreteFacetUserInput { Picks = ["SEAD"], Operator = "=" };
        var anchorTemplate = new AnchorTemplate();

        var sql = _resolver.ResolveSql("tbl_sites", "site_id", input, anchorTemplate, "tbl_sites", "site_id");

        sql.Should().Contain("as predicate_query");
        sql.Should().Contain("where source_id = 'SEAD'");
    }

    [Fact]
    public void ResolveSql_WithScalarOperatorAndMultipleValues_ThrowsArgumentException()
    {
        var input = new DiscreteFacetUserInput { Picks = [1, 2], Operator = "=" };
        var anchorTemplate = new AnchorTemplate();

        Action act = () => _resolver.ResolveSql("tbl_sites", "site_id", input, anchorTemplate, "tbl_sites", "site_id");

        act.Should().Throw<ArgumentException>().WithMessage("*exactly one selected value*");
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void ResolveSql_WithMissingOperator_ThrowsArgumentException(string @operator)
    {
        var input = new DiscreteFacetUserInput { Picks = [1], Operator = @operator! };
        var anchorTemplate = new AnchorTemplate();

        Action act = () => _resolver.ResolveSql("tbl_sites", "site_id", input, anchorTemplate, "tbl_sites", "site_id");

        act.Should().Throw<ArgumentException>().WithParameterName("userInput").WithMessage("*operator*");
    }
}
