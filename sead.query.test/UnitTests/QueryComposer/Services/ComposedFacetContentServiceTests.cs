using System;
using System.Collections.Generic;
using System.Data;
using FluentAssertions;
using Moq;
using SeadQueryComposer.QueryComposer.Services;
using SeadQueryComposer.RouteCompiler;
using SeadQueryCore;
using SeadQueryCore.QueryBuilder;
using SeadQueryCore.QueryComposer;
using Xunit;

namespace SQT.UnitTests.QueryComposer.Services;

public class ComposedFacetContentServiceTests
{
    [Fact]
    public void CanHandle_WithNonPrimaryKeyFilterExpression_ReturnsFalse()
    {
        var service = CreateService();
        var facetsConfig = CreateFacetsConfig(sampleGroupCategoryExpression: "sample_group_tbl.group_name");

        service.CanHandle(facetsConfig).Should().BeFalse();
    }

    [Fact]
    public void Load_WithSupportedDiscreteRequest_ReturnsFacetContentFromComposedQuery()
    {
        var fakeItems = new List<CategoryItem>
        {
            new()
            {
                Category = "SE",
                Count = 3,
                Name = "SE",
                Extent = [3],
            },
            new()
            {
                Category = "NO",
                Count = 1,
                Name = "NO",
                Extent = [1],
            },
        };
        var queryProxy = new Mock<ITypedQueryProxy>();
        string capturedSql = null;
        queryProxy
            .Setup(proxy => proxy.QueryRows(It.IsAny<string>(), It.IsAny<Func<IDataReader, CategoryItem>>()))
            .Callback<string, Func<IDataReader, CategoryItem>>((sql, _) => capturedSql = sql)
            .Returns(fakeItems);

        var service = CreateService(queryProxy.Object);
        var facetsConfig = CreateFacetsConfig();

        var result = service.Load(facetsConfig);

        result.Items.Should().BeEquivalentTo(fakeItems);
        result.Distribution.Should().HaveCount(2);
        result.SqlQuery.Should().Be(capturedSql);
        result.SqlQuery.Should().Contain("with composed_filter as");
        result.SqlQuery.Should().Contain("target_route as");
        result.SqlQuery.Should().Contain("join target_route on target_route.target_id = site_tbl.site_id");
        result.SqlQuery.Should().Contain("join composed_filter on composed_filter.target_id = target_route.source_id");
        queryProxy.Verify(proxy => proxy.QueryRows(It.IsAny<string>(), It.IsAny<Func<IDataReader, CategoryItem>>()), Times.Once);
    }

    [Fact]
    public void CanHandle_WithRoutedPredicateUsingSameTableCategoryKey_ReturnsTrue()
    {
        var service = CreateService();
        var facetsConfig = CreateCountryToFeatureTypeFacetsConfig();

        service.CanHandle(facetsConfig).Should().BeTrue();
    }

    [Fact]
    public void Load_WithRoutedPredicateUsingSameTableCategoryKey_UsesSourceKeyOverride()
    {
        var fakeItems = new List<CategoryItem>
        {
            new()
            {
                Category = "12",
                Count = 2,
                Name = "12",
                Extent = [2],
            },
        };
        var queryProxy = new Mock<ITypedQueryProxy>();
        string capturedSql = null;
        queryProxy
            .Setup(proxy => proxy.QueryRows(It.IsAny<string>(), It.IsAny<Func<IDataReader, CategoryItem>>()))
            .Callback<string, Func<IDataReader, CategoryItem>>((sql, _) => capturedSql = sql)
            .Returns(fakeItems);

        var service = CreateService(queryProxy.Object);
        var facetsConfig = CreateCountryToFeatureTypeFacetsConfig();

        var result = service.Load(facetsConfig);

        result.Items.Should().BeEquivalentTo(fakeItems);
        result.SqlQuery.Should().Be(capturedSql);
        result.SqlQuery.Should().Contain("location_id as source_id");
        result.SqlQuery.Should().Contain("site_id as target_id");
    }

    [Fact]
    public void CanHandle_WithSupportedSameTableFacetClause_ReturnsTrue()
    {
        var service = CreateService();
        var facetsConfig = CreateCountryToSitesFacetsConfig();

        service.CanHandle(facetsConfig).Should().BeTrue();
    }

    [Fact]
    public void Load_WithSupportedSameTableFacetClause_IncludesClauseInPredicateSql()
    {
        var fakeItems = new List<CategoryItem>
        {
            new()
            {
                Category = "12",
                Count = 2,
                Name = "12",
                Extent = [2],
            },
        };
        var queryProxy = new Mock<ITypedQueryProxy>();
        string capturedSql = null;
        queryProxy
            .Setup(proxy => proxy.QueryRows(It.IsAny<string>(), It.IsAny<Func<IDataReader, CategoryItem>>()))
            .Callback<string, Func<IDataReader, CategoryItem>>((sql, _) => capturedSql = sql)
            .Returns(fakeItems);

        var service = CreateService(queryProxy.Object);
        var facetsConfig = CreateCountryToSitesFacetsConfig();

        var result = service.Load(facetsConfig);

        result.Items.Should().BeEquivalentTo(fakeItems);
        result.SqlQuery.Should().Be(capturedSql);
        result.SqlQuery.Should().Contain("X_0.location_type_id=1");
        result.SqlQuery.Should().Contain("location_id as source_id");
    }

    [Fact]
    public void Load_WithSchemaQualifiedTargetCategoryExpression_UsesResolvedTargetJoinColumn()
    {
        var fakeItems = new List<CategoryItem>
        {
            new()
            {
                Category = "12",
                Count = 2,
                Name = "12",
                Extent = [2],
            },
        };
        var queryProxy = new Mock<ITypedQueryProxy>();
        string capturedSql = null;
        queryProxy
            .Setup(proxy => proxy.QueryRows(It.IsAny<string>(), It.IsAny<Func<IDataReader, CategoryItem>>()))
            .Callback<string, Func<IDataReader, CategoryItem>>((sql, _) => capturedSql = sql)
            .Returns(fakeItems);

        var service = CreateService(queryProxy.Object);
        var facetsConfig = CreateCountryToSpeciesFacetsConfig();

        var result = service.Load(facetsConfig);

        result.Items.Should().BeEquivalentTo(fakeItems);
        result.SqlQuery.Should().Be(capturedSql);
        result.SqlQuery.Should().Contain("target_route.target_id = facet.abundance_taxon_shortcut.taxon_id");
        result.SqlQuery.Should().NotContain(".xxx", "schema-qualified target expressions should resolve to their real join column");
    }

    private static ComposedFacetContentService CreateService(ITypedQueryProxy queryProxy = null)
    {
        var sites = CreateTable(1, "tbl_sites", "site_id");
        var samples = CreateTable(2, "tbl_physical_samples", "physical_sample_id");
        var sampleGroups = CreateTable(3, "tbl_sample_groups", "sample_group_id");
        var analysisEntities = CreateTable(4, "tbl_analysis_entities", "analysis_entity_id");
        var countryShortcut = CreateTable(5, "facet.site_location_shortcut", "xxxx");
        var featureTypes = CreateTable(6, "tbl_feature_types", "feature_type_id");
        var physicalSampleFeatures = CreateTable(7, "tbl_physical_sample_features", "physical_sample_feature_id");
        var abundanceTaxonShortcut = CreateTable(8, "facet.abundance_taxon_shortcut", "xxx");
        var aggregateFacet = new Facet
        {
            FacetId = 10,
            FacetCode = "result_facet",
            FacetTypeId = EFacetType.Discrete,
            CategoryIdExpr = "analysis_entity_id",
            Tables = [new FacetTable { SequenceId = 1, Table = analysisEntities }],
        };
        var sitesAggregateFacet = new Facet
        {
            FacetId = 11,
            FacetCode = "sites",
            FacetTypeId = EFacetType.Discrete,
            CategoryIdExpr = "site_id",
            Tables = [new FacetTable { SequenceId = 1, Table = sites }],
        };
        var graph = new List<TableRelation>
        {
            CreateRelation(sampleGroups, sites, "site_id", "site_id"),
            CreateRelation(sampleGroups, samples, "sample_group_id", "sample_group_id"),
            CreateRelation(samples, analysisEntities, "physical_sample_id", "physical_sample_id"),
            CreateRelation(countryShortcut, sites, "site_id", "site_id"),
            CreateRelation(featureTypes, physicalSampleFeatures, "feature_type_id", "feature_type_id"),
            CreateRelation(physicalSampleFeatures, samples, "physical_sample_id", "physical_sample_id"),
            CreateRelation(abundanceTaxonShortcut, analysisEntities, "analysis_entity_id", "analysis_entity_id"),
        };

        var facetRepository = new Mock<IFacetRepository>();
        facetRepository.Setup(repository => repository.Get(aggregateFacet.FacetId)).Returns(aggregateFacet);
        facetRepository.Setup(repository => repository.Get(sitesAggregateFacet.FacetId)).Returns(sitesAggregateFacet);

        var registry = new Mock<IRepositoryRegistry>();
        registry.SetupGet(current => current.Facets).Returns(facetRepository.Object);

        var routeResolver = new RouteResolver(graph);
        var routeSqlCompiler = new RouteSqlCompiler(Mock.Of<IRouteRepository>(), routeResolver);
        var joinsClauseCompiler = new Mock<IJoinsClauseCompiler>();
        joinsClauseCompiler
            .Setup(compiler => compiler.Compile(It.IsAny<List<List<TableRelation>>>(), It.IsAny<FacetsConfig2>()))
            .Returns([]);
        return new ComposedFacetContentService(
            registry.Object,
            queryProxy ?? Mock.Of<ITypedQueryProxy>(),
            new PathFinder([.. graph, .. graph.ReversedEdges()]),
            routeSqlCompiler,
            new DiscreteFacetPredicateResolver(routeSqlCompiler),
            new IntersectComposedFilterQueryComposer(),
            new DiscreteFacetContentQueryComposer(new PathFinder([.. graph, .. graph.ReversedEdges()]), joinsClauseCompiler.Object)
        );
    }

    private static FacetsConfig2 CreateFacetsConfig(string sampleGroupCategoryExpression = "sample_group_id")
    {
        var sites = CreateTable(1, "tbl_sites", "site_id");
        var sampleGroups = CreateTable(3, "tbl_sample_groups", "sample_group_id");

        var siteFacet = new Facet
        {
            FacetCode = "site",
            FacetId = 10,
            FacetTypeId = EFacetType.Discrete,
            CategoryIdExpr = "analysis_entity_id",
            Tables = [new FacetTable { SequenceId = 1, Table = CreateTable(4, "tbl_analysis_entities", "analysis_entity_id") }],
        };

        var sampleGroupFacet = new Facet
        {
            FacetCode = "sample_group",
            FacetTypeId = EFacetType.Discrete,
            CategoryIdExpr = sampleGroupCategoryExpression,
            Tables =
            [
                new FacetTable
                {
                    SequenceId = 1,
                    Table = sampleGroups,
                    Alias = "sample_group_tbl",
                },
            ],
        };

        var targetFacet = new Facet
        {
            FacetCode = "sites",
            FacetTypeId = EFacetType.Discrete,
            AggregateFacetId = 10,
            CategoryIdExpr = "site_tbl.site_id",
            Tables =
            [
                new FacetTable
                {
                    SequenceId = 1,
                    Table = sites,
                    Alias = "site_tbl",
                },
            ],
        };

        return new FacetsConfig2
        {
            TargetCode = "sites",
            TargetFacet = targetFacet,
            FacetConfigs =
            [
                new FacetConfig2(sampleGroupFacet, 1, string.Empty, [new FacetConfigPick(201)]),
                new FacetConfig2(targetFacet, 2, string.Empty, []),
            ],
        };
    }

    private static FacetsConfig2 CreateCountryToFeatureTypeFacetsConfig()
    {
        var countryShortcut = CreateTable(5, "facet.site_location_shortcut", "xxxx");
        var featureTypes = CreateTable(6, "tbl_feature_types", "feature_type_id");
        var physicalSampleFeatures = CreateTable(7, "tbl_physical_sample_features", "physical_sample_feature_id");

        var countryFacet = new Facet
        {
            FacetCode = "country",
            FacetId = 21,
            FacetTypeId = EFacetType.Discrete,
            CategoryIdExpr = "countries.location_id",
            Tables =
            [
                new FacetTable
                {
                    SequenceId = 1,
                    Table = countryShortcut,
                    Alias = "countries",
                },
            ],
        };

        var targetFacet = new Facet
        {
            FacetCode = "feature_type",
            FacetTypeId = EFacetType.Discrete,
            AggregateFacetId = 11,
            CategoryIdExpr = "tbl_feature_types.feature_type_id",
            Tables =
            [
                new FacetTable { SequenceId = 1, Table = featureTypes },
                new FacetTable { SequenceId = 2, Table = physicalSampleFeatures },
            ],
        };

        return new FacetsConfig2
        {
            TargetCode = "feature_type",
            TargetFacet = targetFacet,
            FacetConfigs =
            [
                new FacetConfig2(countryFacet, 1, string.Empty, [new FacetConfigPick(1), new FacetConfigPick(2), new FacetConfigPick(5)]),
                new FacetConfig2(targetFacet, 2, string.Empty, []),
            ],
        };
    }

    private static FacetsConfig2 CreateCountryToSpeciesFacetsConfig()
    {
        var countryShortcut = CreateTable(5, "facet.site_location_shortcut", "xxxx");
        var abundanceTaxonShortcut = CreateTable(8, "facet.abundance_taxon_shortcut", "xxx");

        var countryFacet = new Facet
        {
            FacetCode = "country",
            FacetId = 21,
            FacetTypeId = EFacetType.Discrete,
            CategoryIdExpr = "countries.location_id",
            Tables =
            [
                new FacetTable
                {
                    SequenceId = 1,
                    Table = countryShortcut,
                    Alias = "countries",
                },
            ],
        };

        var targetFacet = new Facet
        {
            FacetCode = "species",
            FacetTypeId = EFacetType.Discrete,
            AggregateFacetId = 11,
            CategoryIdExpr = "facet.abundance_taxon_shortcut.taxon_id",
            Tables = [new FacetTable { SequenceId = 1, Table = abundanceTaxonShortcut }],
        };

        return new FacetsConfig2
        {
            TargetCode = "species",
            TargetFacet = targetFacet,
            FacetConfigs =
            [
                new FacetConfig2(countryFacet, 1, string.Empty, [new FacetConfigPick(1), new FacetConfigPick(2), new FacetConfigPick(5)]),
                new FacetConfig2(targetFacet, 2, string.Empty, []),
            ],
        };
    }

    private static FacetsConfig2 CreateCountryToSitesFacetsConfig()
    {
        var sites = CreateTable(1, "tbl_sites", "site_id");
        var countryShortcut = CreateTable(5, "facet.site_location_shortcut", "xxxx");

        var countryFacet = new Facet
        {
            FacetCode = "country",
            FacetId = 21,
            FacetTypeId = EFacetType.Discrete,
            CategoryIdExpr = "countries.location_id",
            Tables =
            [
                new FacetTable
                {
                    SequenceId = 1,
                    Table = countryShortcut,
                    Alias = "countries",
                },
            ],
            Clauses = [new FacetClause { Clause = "countries.location_type_id=1", EnforceConstraint = true }],
        };

        var targetFacet = new Facet
        {
            FacetCode = "sites",
            FacetTypeId = EFacetType.Discrete,
            AggregateFacetId = 10,
            CategoryIdExpr = "site_tbl.site_id",
            Tables =
            [
                new FacetTable
                {
                    SequenceId = 1,
                    Table = sites,
                    Alias = "site_tbl",
                },
            ],
        };

        return new FacetsConfig2
        {
            TargetCode = "sites",
            TargetFacet = targetFacet,
            FacetConfigs =
            [
                new FacetConfig2(countryFacet, 1, string.Empty, [new FacetConfigPick(1), new FacetConfigPick(2), new FacetConfigPick(5)]),
                new FacetConfig2(targetFacet, 2, string.Empty, []),
            ],
        };
    }

    private static Table CreateTable(int id, string name, string primaryKey)
    {
        return new Table
        {
            TableId = id,
            TableOrUdfName = name,
            PrimaryKeyName = primaryKey,
        };
    }

    private static TableRelation CreateRelation(Table source, Table target, string sourceColumn, string targetColumn)
    {
        return new TableRelation
        {
            SourceTableId = source.TableId,
            TargetTableId = target.TableId,
            SourceTable = source,
            TargetTable = target,
            SourceColumnName = sourceColumn,
            TargetColumnName = targetColumn,
            Weight = 1,
        };
    }
}
