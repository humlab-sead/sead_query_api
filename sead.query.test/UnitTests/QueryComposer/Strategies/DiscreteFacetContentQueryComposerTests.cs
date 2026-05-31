using System;
using FluentAssertions;
using Moq;
using SeadQueryCore;
using SeadQueryCore.QueryBuilder;
using SeadQueryCore.QueryComposer;
using Xunit;

namespace SQT.UnitTests.QueryComposer.Strategies;

public class DiscreteFacetContentQueryComposerTests
{
    private readonly Mock<IPathFinder> _pathFinder = new();
    private readonly Mock<IJoinsClauseCompiler> _joinsClauseCompiler = new();
    private readonly DiscreteFacetContentQueryComposer _composer;

    public DiscreteFacetContentQueryComposerTests()
    {
        _joinsClauseCompiler
            .Setup(x =>
                x.Compile(
                    It.IsAny<System.Collections.Generic.List<System.Collections.Generic.List<TableRelation>>>(),
                    It.IsAny<FacetsConfig2>()
                )
            )
            .Returns([]);
        _composer = new DiscreteFacetContentQueryComposer(_pathFinder.Object, _joinsClauseCompiler.Object);
    }

    private static FacetsConfig2 CreateFacetsConfig(string facetCode = "country", string targetTableName = "tbl_sites")
    {
        var table = new Table { TableOrUdfName = targetTableName, PrimaryKeyName = "site_id" };

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

    private static FacetsConfig2 CreateConstructionFacetsConfig()
    {
        var sampleGroups = new Table { TableOrUdfName = "tbl_sample_groups", PrimaryKeyName = "sample_group_id" };
        var descriptions = new Table { TableOrUdfName = "tbl_sample_group_descriptions", PrimaryKeyName = "sample_group_description_id" };

        var targetFacet = new Facet
        {
            FacetCode = "constructions",
            FacetTypeId = EFacetType.Discrete,
            CategoryIdExpr = "tbl_sample_group_descriptions.sample_group_description_id",
            Tables = [new FacetTable { SequenceId = 1, Table = sampleGroups }, new FacetTable { SequenceId = 2, Table = descriptions }],
        };

        return new FacetsConfig2
        {
            TargetCode = "constructions",
            TargetFacet = targetFacet,
            FacetConfigs = [],
        };
    }

    private static FacetsConfig2 CreateGeoPolygonFacetsConfig()
    {
        var table = new Table { TableOrUdfName = "tbl_sites", PrimaryKeyName = "site_id" };

        var targetFacet = new Facet
        {
            FacetCode = "sites_polygon",
            FacetTypeId = EFacetType.GeoPolygon,
            CategoryIdExpr = "tbl_sites.site_id",
            Tables = [new FacetTable { SequenceId = 1, Table = table }],
        };

        return new FacetsConfig2
        {
            TargetCode = "sites_polygon",
            TargetFacet = targetFacet,
            FacetConfigs = [],
        };
    }

    private static FacetsConfig2 CreateBiblioSampleGroupsFacetsConfig()
    {
        var biblio = new Table { TableOrUdfName = "tbl_biblio", PrimaryKeyName = "biblio_id" };
        var sampleGroupReferences = new Table { TableOrUdfName = "facet.view_sample_group_references", PrimaryKeyName = "xxxx" };

        var targetFacet = new Facet
        {
            FacetCode = "tbl_biblio_sample_groups",
            FacetTypeId = EFacetType.Discrete,
            CategoryIdExpr = "tbl_biblio.biblio_id",
            Tables =
            [
                new FacetTable { SequenceId = 1, Table = biblio },
                new FacetTable { SequenceId = 2, Table = sampleGroupReferences },
            ],
            Clauses = [new FacetClause { Clause = "facet.view_sample_group_references.biblio_id is not null", EnforceConstraint = true }],
        };

        return new FacetsConfig2
        {
            TargetCode = "tbl_biblio_sample_groups",
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
        Action act = () => _composer.Compose(null!, CreateComposedFilterQuery(), "site_id", string.Empty);

        act.Should().Throw<ArgumentNullException>().WithParameterName("facetsConfig");
    }

    [Fact]
    public void Compose_WithMissingTargetFacet_ThrowsArgumentException()
    {
        var facetsConfig = new FacetsConfig2 { TargetCode = "country", FacetConfigs = [] };

        Action act = () => _composer.Compose(facetsConfig, CreateComposedFilterQuery(), "site_id", string.Empty);

        act.Should().Throw<ArgumentException>().WithMessage("*TargetFacet must be set*");
    }

    [Fact]
    public void Compose_WithNonDiscreteTargetFacet_ThrowsInvalidOperationException()
    {
        var facetsConfig = CreateFacetsConfig();
        facetsConfig.TargetFacet.FacetTypeId = EFacetType.Unknown;

        Action act = () => _composer.Compose(facetsConfig, CreateComposedFilterQuery(), "site_id", string.Empty);

        act.Should().Throw<InvalidOperationException>().WithMessage("*is not supported by the composed content composer*");
    }

    [Fact]
    public void Compose_WithDifferentAnchorTableAndNoRoute_ThrowsInvalidOperationException()
    {
        var facetsConfig = CreateFacetsConfig(targetTableName: "tbl_sites");

        Action act = () => _composer.Compose(facetsConfig, CreateComposedFilterQuery(anchorTable: "tbl_samples"), "site_id", string.Empty);

        act.Should().Throw<InvalidOperationException>().WithMessage("*requires an anchor-to-target route*");
    }

    [Fact]
    public void Compose_WithDiscreteTargetFacet_BuildsFacetContentQueryPlan()
    {
        var facetsConfig = CreateFacetsConfig();
        var composedFilterQuery = CreateComposedFilterQuery();

        var result = _composer.Compose(facetsConfig, composedFilterQuery, "site_id", string.Empty);

        result.TargetFacetCode.Should().Be("country");
        result.AnchorTable.Should().Be("tbl_sites");
        result.AnchorKeyColumn.Should().Be(QueryComposerAliases.AnchorKeyColumn);
        result.AnchorJoinColumn.Should().Be("site_id");
        result.ComposedFilterSql.Should().Be("select target_id from composed_predicates");
        result.Sql.Should().Contain("with composed_filter as");
        result.Sql.Should().Contain("select site_tbl.country_id as category, count(distinct composed_filter.target_id)::int as count");
        result.Sql.Should().Contain("from tbl_sites AS site_tbl");
        result.Sql.Should().Contain("join composed_filter on composed_filter.target_id = site_tbl.site_id");
        result.Sql.Should().Contain("group by site_tbl.country_id");
        result.Sql.Should().Contain("order by site_tbl.country_id");
    }

    [Fact]
    public void Compose_WithGeoPolygonTargetFacet_UsesDistinctCategoryRows()
    {
        var facetsConfig = CreateGeoPolygonFacetsConfig();

        var result = _composer.Compose(
            facetsConfig,
            CreateComposedFilterQuery(),
            "site_id",
            string.Empty,
            "select 1 as category, 2 as count_column, 3 as longitude_dd, 4 as latitude_dd"
        );

        result.Sql.Should().Contain("select distinct c.category, c.count_column, c.longitude_dd, c.latitude_dd");
    }

    [Fact]
    public void Compose_WithAnchorToTargetRoute_BuildsRoutedFacetContentQueryPlan()
    {
        var facetsConfig = CreateFacetsConfig(targetTableName: "tbl_sites");
        var composedFilterQuery = CreateComposedFilterQuery(anchorTable: "tbl_analysis_entities");
        const string anchorToTargetSql = "select distinct source_id, target_id from anchor_route";

        var result = _composer.Compose(facetsConfig, composedFilterQuery, "site_id", anchorToTargetSql);

        result.AnchorTable.Should().Be("tbl_analysis_entities");
        result.AnchorJoinColumn.Should().Be("site_id");
        result.Sql.Should().Contain(")");
        result.Sql.Should().Contain("target_route as (");
        result.Sql.Should().Contain("select distinct source_id, target_id from anchor_route");
        result.Sql.Should().Contain("join target_route on target_route.target_id = site_tbl.site_id");
        result.Sql.Should().Contain("join composed_filter on composed_filter.target_id = target_route.source_id");
    }

    [Fact]
    public void Compose_WithJoinedTargetFacetTables_IncludesTargetFacetJoins()
    {
        var facetsConfig = CreateConstructionFacetsConfig();
        var composedFilterQuery = CreateComposedFilterQuery(anchorTable: "tbl_analysis_entities");
        const string anchorToTargetSql = "select distinct source_id, target_id from anchor_route";

        _pathFinder
            .Setup(x => x.Find("tbl_sample_groups", It.IsAny<System.Collections.Generic.List<string>>(), true))
            .Returns(
                [
                    [],
                ]
            );
        _joinsClauseCompiler
            .Setup(x =>
                x.Compile(It.IsAny<System.Collections.Generic.List<System.Collections.Generic.List<TableRelation>>>(), facetsConfig)
            )
            .Returns(
                ["join tbl_sample_group_descriptions on tbl_sample_group_descriptions.sample_group_id = tbl_sample_groups.sample_group_id"]
            );

        var result = _composer.Compose(facetsConfig, composedFilterQuery, "sample_group_id", anchorToTargetSql);

        result.Sql.Should().Contain("from tbl_sample_groups");
        result
            .Sql.Should()
            .Contain(
                "join tbl_sample_group_descriptions on tbl_sample_group_descriptions.sample_group_id = tbl_sample_groups.sample_group_id"
            );
        result.Sql.Should().Contain("select tbl_sample_group_descriptions.sample_group_description_id as category");
        result.Sql.Should().Contain("join target_route on target_route.target_id = tbl_sample_groups.sample_group_id");
    }

    [Fact]
    public void Compose_WithDiscreteTargetClause_IncludesClauseInSql()
    {
        var facetsConfig = CreateBiblioSampleGroupsFacetsConfig();
        var composedFilterQuery = CreateComposedFilterQuery(anchorTable: "tbl_analysis_entities");
        const string anchorToTargetSql = "select distinct source_id, target_id from anchor_route";

        _pathFinder
            .Setup(x => x.Find("tbl_biblio", It.IsAny<System.Collections.Generic.List<string>>(), true))
            .Returns(
                [
                    [],
                ]
            );
        _joinsClauseCompiler
            .Setup(x => x.Compile(It.IsAny<System.Collections.Generic.List<System.Collections.Generic.List<TableRelation>>>(), facetsConfig))
            .Returns(["join facet.view_sample_group_references on facet.view_sample_group_references.biblio_id = tbl_biblio.biblio_id"]);

        var result = _composer.Compose(facetsConfig, composedFilterQuery, "biblio_id", anchorToTargetSql);

        result.Sql.Should().Contain("join facet.view_sample_group_references on facet.view_sample_group_references.biblio_id = tbl_biblio.biblio_id");
        result.Sql.Should().Contain("where facet.view_sample_group_references.biblio_id is not null");
        result.Sql.Should().Contain("group by tbl_biblio.biblio_id");
    }
}
