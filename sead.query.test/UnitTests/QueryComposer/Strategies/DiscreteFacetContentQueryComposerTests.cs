using System;
using FluentAssertions;
using SeadQueryCore;
using SeadQueryCore.QueryComposer;
using Xunit;

namespace SQT.UnitTests.QueryComposer.Strategies;

public class DiscreteFacetContentQueryComposerTests
{
    private readonly DiscreteFacetContentQueryComposer _composer = new();

    private static FacetsConfig2 CreateFacetsConfig(string facetCode = "country", string targetTableName = "tbl_sites")
    {
        var table = new Table
        {
            TableOrUdfName = targetTableName,
            PrimaryKeyName = "site_id",
        };

        var targetFacetTable = new FacetTable
        {
            SequenceId = 1,
            Table = table,
            Alias = "site_tbl",
        };

        var targetFacet = new Facet
        {
            FacetCode = facetCode,
            FacetTypeId = EFacetType.Discrete,
            CategoryIdExpr = "site_tbl.country_id",
            Tables = [targetFacetTable],
        };

        return new FacetsConfig2
        {
            TargetCode = facetCode,
            TargetFacet = targetFacet,
            FacetConfigs = [],
        };
    }

    private static ComposedFilterQuery CreateComposedFilterQuery(string anchorTable = "tbl_sites")
    {
        return new ComposedFilterQuery
        {
            AnchorTable = anchorTable,
            AnchorKeyColumn = QueryComposerAliases.AnchorKeyColumn,
            Sql = "select target_id from composed_predicates",
        };
    }

    [Fact]
    public void Compose_WithNullFacetsConfig_ThrowsArgumentNullException()
    {
        Action act = () => _composer.Compose(null!, CreateComposedFilterQuery());

        act.Should().Throw<ArgumentNullException>().WithParameterName("facetsConfig");
    }

    [Fact]
    public void Compose_WithMissingTargetFacet_ThrowsArgumentException()
    {
        var facetsConfig = new FacetsConfig2 { TargetCode = "country", FacetConfigs = [] };

        Action act = () => _composer.Compose(facetsConfig, CreateComposedFilterQuery());

        act.Should().Throw<ArgumentException>().WithMessage("*TargetFacet must be set*");
    }

    [Fact]
    public void Compose_WithNonDiscreteTargetFacet_ThrowsInvalidOperationException()
    {
        var facetsConfig = CreateFacetsConfig();
        facetsConfig.TargetFacet.FacetTypeId = EFacetType.Range;

        Action act = () => _composer.Compose(facetsConfig, CreateComposedFilterQuery());

        act.Should().Throw<InvalidOperationException>().WithMessage("*not a discrete facet*");
    }

    [Fact]
    public void Compose_WithDifferentAnchorTable_ThrowsInvalidOperationException()
    {
        var facetsConfig = CreateFacetsConfig(targetTableName: "tbl_sites");

        Action act = () => _composer.Compose(facetsConfig, CreateComposedFilterQuery(anchorTable: "tbl_samples"));

        act.Should().Throw<InvalidOperationException>().WithMessage("*expected anchor table*tbl_samples*");
    }

    [Fact]
    public void Compose_WithDiscreteTargetFacet_BuildsFacetContentQueryPlan()
    {
        var facetsConfig = CreateFacetsConfig();
        var composedFilterQuery = CreateComposedFilterQuery();

        var result = _composer.Compose(facetsConfig, composedFilterQuery);

        result.TargetFacetCode.Should().Be("country");
        result.AnchorTable.Should().Be("tbl_sites");
        result.AnchorKeyColumn.Should().Be(QueryComposerAliases.AnchorKeyColumn);
        result.ComposedFilterSql.Should().Be("select target_id from composed_predicates");
        result.Sql.Should().Contain("with composed_filter as");
        result.Sql.Should().Contain("select site_tbl.country_id as category, count(*)::int as count");
        result.Sql.Should().Contain("from tbl_sites AS site_tbl");
        result.Sql.Should().Contain("join composed_filter on composed_filter.target_id = site_tbl.site_id");
        result.Sql.Should().Contain("group by site_tbl.country_id");
        result.Sql.Should().Contain("order by site_tbl.country_id");
    }
}