using System;
using FluentAssertions;
using SeadQueryCore.QueryComposer;
using Xunit;

namespace SQT.UnitTests.QueryComposer.Strategies;

public class IntersectComposedFilterQueryComposerTests
{
    private readonly IntersectComposedFilterQueryComposer _composer = new();

    private static PredicateQueryPlan CreatePredicateQueryPlan(
        string facetCode,
        string sql,
        string anchorTable = "tbl_samples",
        string anchorKeyColumn = QueryComposerAliases.AnchorKeyColumn)
    {
        return new PredicateQueryPlan
        {
            FacetCode = facetCode,
            AnchorTable = anchorTable,
            AnchorKeyColumn = anchorKeyColumn,
            Sql = sql,
        };
    }

    [Fact]
    public void Compose_WithNullPredicateQueries_ThrowsArgumentNullException()
    {
        Action act = () => _composer.Compose(null!, "tbl_samples", QueryComposerAliases.AnchorKeyColumn);

        act.Should().Throw<ArgumentNullException>().WithParameterName("predicateQueries");
    }

    [Fact]
    public void Compose_WithEmptyAnchorTable_ThrowsArgumentException()
    {
        Action act = () => _composer.Compose([], string.Empty, QueryComposerAliases.AnchorKeyColumn);

        act.Should().Throw<ArgumentException>().WithParameterName("anchorTable");
    }

    [Fact]
    public void Compose_WithNoPredicateQueries_ReturnsEmptyAnchorSetQuery()
    {
        var result = _composer.Compose([], "tbl_samples", QueryComposerAliases.AnchorKeyColumn);

        result.AnchorTable.Should().Be("tbl_samples");
        result.AnchorKeyColumn.Should().Be(QueryComposerAliases.AnchorKeyColumn);
        result.PredicateQueries.Should().BeEmpty();
        result.Sql.Should().Contain("where 1 = 0");
        result.Sql.Should().Contain(QueryComposerAliases.AnchorKeyColumn);
    }

    [Fact]
    public void Compose_WithSinglePredicateQuery_WrapsQueryInSingleCte()
    {
        var predicateSql = CreatePredicateQueryPlan("facet_a", "select source_id, target_id from predicate_source;");

        var result = _composer.Compose([predicateSql], "tbl_samples", QueryComposerAliases.AnchorKeyColumn);

        result.PredicateQueries.Should().ContainSingle().Which.Sql.Should().Be("select source_id, target_id from predicate_source");
        result.Sql.Should().Contain("with");
        result.Sql.Should().Contain("predicate_0 as");
        result.Sql.Should().Contain("select distinct target_id");
        result.Sql.Should().NotContain("intersect");
    }

    [Fact]
    public void Compose_WithMultiplePredicateQueries_UsesIntersectAcrossAllQueries()
    {
        var first = CreatePredicateQueryPlan("facet_a", "select source_id, target_id from predicate_one");
        var second = CreatePredicateQueryPlan("facet_b", "select source_id, target_id from predicate_two");
        var third = CreatePredicateQueryPlan("facet_c", "select source_id, target_id from predicate_three");

        var result = _composer.Compose([first, second, third], "tbl_samples", QueryComposerAliases.AnchorKeyColumn);

        result.PredicateQueries.Should().HaveCount(3);
        result.Sql.Should().Contain("predicate_0 as");
        result.Sql.Should().Contain("predicate_1 as");
        result.Sql.Should().Contain("predicate_2 as");
        result.Sql.Should().Contain("from predicate_0");
        result.Sql.Should().Contain("from predicate_1");
        result.Sql.Should().Contain("from predicate_2");
        result.Sql.Should().Contain("intersect");
    }

    [Fact]
    public void Compose_WithWhitespaceOnlyQueries_SkipsEmptyQueries()
    {
        var result = _composer.Compose(
            [
                CreatePredicateQueryPlan("facet_a", " "),
                CreatePredicateQueryPlan("facet_b", "select source_id, target_id from predicate_one")
            ],
            "tbl_samples",
            QueryComposerAliases.AnchorKeyColumn);

        result.PredicateQueries.Should().ContainSingle();
        result.Sql.Should().Contain("predicate_0 as");
        result.Sql.Should().NotContain("predicate_1 as");
    }

    [Fact]
    public void Compose_WithDifferentAnchorTables_ThrowsInvalidOperationException()
    {
        var first = CreatePredicateQueryPlan("facet_a", "select source_id, target_id from predicate_one", anchorTable: "tbl_sites");
        var second = CreatePredicateQueryPlan("facet_b", "select source_id, target_id from predicate_two", anchorTable: "tbl_samples");

        Action act = () => _composer.Compose([first, second], "tbl_samples", QueryComposerAliases.AnchorKeyColumn);

        act.Should().Throw<InvalidOperationException>().WithMessage("*anchor table*");
    }

    [Fact]
    public void Compose_WithDifferentAnchorKeyAliases_ThrowsInvalidOperationException()
    {
        var first = CreatePredicateQueryPlan("facet_a", "select source_id, target_id from predicate_one");
        var second = CreatePredicateQueryPlan("facet_b", "select source_id, anchor_id from predicate_two", anchorKeyColumn: "anchor_id");

        Action act = () => _composer.Compose([first, second], "tbl_samples", QueryComposerAliases.AnchorKeyColumn);

        act.Should().Throw<InvalidOperationException>().WithMessage("*anchor key*");
    }
}