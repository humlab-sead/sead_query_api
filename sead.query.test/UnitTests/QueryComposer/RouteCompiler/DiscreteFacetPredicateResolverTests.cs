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
        _routeSqlCompiler.Verify(x => x.Compile(It.IsAny<List<string>>()), Times.Never);
    }

    [Fact]
    public void ResolveSql_WithRouteAndPicks_CompilesFullChainAndAppendsWhereClause()
    {
        var input = new DiscreteFacetUserInput { Picks = [101, 102] };
        var anchorTemplate = new AnchorTemplate { Route = ["tbl_sample_groups"], RequiresDistinct = true };
        const string compiledSql = "select distinct X_0.site_id as source_id, X_1.sample_id as target_id\nfrom tbl_sites as X_0";

        _routeSqlCompiler
            .Setup(x => x.Compile(It.Is<List<string>>(tables =>
                tables.Count == 3
                && tables[0] == "tbl_sites"
                && tables[1] == "tbl_sample_groups"
                && tables[2] == "tbl_samples")))
            .Returns(compiledSql);

        var sql = _resolver.ResolveSql("tbl_sites", "site_id", input, anchorTemplate, "tbl_samples", "sample_id");

        sql.Should().Contain(compiledSql);
        sql.Should().Contain("where source_id in (101, 102)");
    }

    [Fact]
    public void ResolveSql_WithoutRoute_BuildsIdentitySql()
    {
        var input = new DiscreteFacetUserInput();
        var anchorTemplate = new AnchorTemplate { RequiresDistinct = true };

        var sql = _resolver.ResolveSql("tbl_sites", "site_id", input, anchorTemplate, "tbl_sites", "site_id");

        sql.Should().Contain("select distinct site_id as source_id, site_id as target_id");
        sql.Should().Contain("from tbl_sites");
        _routeSqlCompiler.Verify(x => x.Compile(It.IsAny<List<string>>()), Times.Never);
    }

    [Fact]
    public void ResolveSql_WithSingleValueOperator_UsesScalarComparison()
    {
        var input = new DiscreteFacetUserInput { Picks = ["SEAD"], Operator = "=" };
        var anchorTemplate = new AnchorTemplate();

        var sql = _resolver.ResolveSql("tbl_sites", "site_id", input, anchorTemplate, "tbl_sites", "site_id");

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
}