using System;
using System.Collections.Generic;
using System.Data;
using FluentAssertions;
using Moq;
using SeadQueryComposer.QueryComposer.Services;
using SeadQueryComposer.RouteCompiler;
using SeadQueryCore;
using SeadQueryCore.Plugin.Discrete;
using SeadQueryCore.Plugin.Range;
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
    public void Load_WithUnsupportedRequest_ThrowsInvalidOperationException()
    {
        var queryProxy = new Mock<ITypedQueryProxy>(MockBehavior.Strict);
        var service = CreateService(queryProxy.Object);
        var facetsConfig = CreateFacetsConfig(sampleGroupCategoryExpression: "sample_group_tbl.group_name");

        var act = () => service.Load(facetsConfig);

        act.Should().Throw<InvalidOperationException>().WithMessage("*Call CanHandle(...)*legacy runtime*");
        queryProxy.Verify(proxy => proxy.QueryRows(It.IsAny<string>(), It.IsAny<Func<IDataReader, CategoryItem>>()), Times.Never);
    }

    [Fact]
    public void Load_WithUnsupportedJoinedTableFacetClause_ThrowsInvalidOperationException()
    {
        var queryProxy = new Mock<ITypedQueryProxy>(MockBehavior.Strict);
        var service = CreateService(queryProxy.Object);
        var facetsConfig = CreateCountryToSitesFacetsConfig(countryClause: "tbl_sites.deleted = true");

        var act = () => service.Load(facetsConfig);

        act.Should().Throw<InvalidOperationException>().WithMessage("*Call CanHandle(...)*legacy runtime*");
        queryProxy.Verify(proxy => proxy.QueryRows(It.IsAny<string>(), It.IsAny<Func<IDataReader, CategoryItem>>()), Times.Never);
    }

    [Fact]
    public void Load_WithUnsupportedTargetJoinDerivation_ThrowsInvalidOperationException()
    {
        var queryProxy = new Mock<ITypedQueryProxy>(MockBehavior.Strict);
        var service = CreateService(queryProxy.Object);
        var facetsConfig = CreateCountryToSpeciesFacetsConfig(targetCategoryExpression: "coalesce(facet.abundance_taxon_shortcut.taxon_id, 0)");

        var act = () => service.Load(facetsConfig);

        act.Should().Throw<InvalidOperationException>().WithMessage("*Call CanHandle(...)*legacy runtime*");
        queryProxy.Verify(proxy => proxy.QueryRows(It.IsAny<string>(), It.IsAny<Func<IDataReader, CategoryItem>>()), Times.Never);
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
    public void CanHandle_WithTargetOnlyDiscreteRequest_ReturnsTrue()
    {
        var service = CreateService();
        var facetsConfig = CreateTargetOnlyDiscreteFacetsConfig();

        service.CanHandle(facetsConfig).Should().BeTrue();
    }

    [Fact]
    public void CanHandle_WithTargetOnlyDiscreteRequestUsingDifferentAnchorTable_ReturnsTrue()
    {
        var service = CreateService();
        var facetsConfig = CreateTargetOnlyRoutedDiscreteFacetsConfig();

        service.CanHandle(facetsConfig).Should().BeTrue();
    }

    [Fact]
    public void Load_WithTargetOnlyDiscreteRequest_UsesUnfilteredAnchorQuery()
    {
        var fakeItems = new List<CategoryItem>
        {
            new()
            {
                Category = "Pinus",
                Count = 4,
                Name = "Pinus",
                Extent = [4],
            },
        };
        var queryProxy = new Mock<ITypedQueryProxy>();
        string capturedSql = null;
        queryProxy
            .Setup(proxy => proxy.QueryRows(It.IsAny<string>(), It.IsAny<Func<IDataReader, CategoryItem>>()))
            .Callback<string, Func<IDataReader, CategoryItem>>((sql, _) => capturedSql = sql)
            .Returns(fakeItems);

        var service = CreateService(queryProxy.Object);
        var facetsConfig = CreateTargetOnlyDiscreteFacetsConfig();

        var result = service.Load(facetsConfig);

        result.Items.Should().BeEquivalentTo(fakeItems);
        result.SqlQuery.Should().Be(capturedSql);
        result.SqlQuery.Should().Contain("select distinct genus_id as target_id");
        result.SqlQuery.Should().Contain("from tbl_taxa_tree_genera");
        result.SqlQuery.Should().Contain("join composed_filter on composed_filter.target_id = tbl_taxa_tree_genera.genus_id");
        result.SqlQuery.Should().NotContain("predicate_0 as");
    }

    [Fact]
    public void Load_WithTargetOnlyRoutedDiscreteRequest_UsesOuterCategoryInfoForReturnedItems()
    {
        var countedItems = new List<CategoryItem>
        {
            new()
            {
                Category = "12",
                Count = 2,
                Name = "12",
                Extent = [2],
            },
        };
        var outerItems = new List<CategoryItem>
        {
            new()
            {
                Category = "12",
                Count = 0,
                Name = "Feature 12",
                Extent = [0],
            },
            new()
            {
                Category = "99",
                Count = 0,
                Name = "Feature 99",
                Extent = [0],
            },
        };
        var queryProxy = new Mock<ITypedQueryProxy>();
        const string outerCategorySql = "select '12' as category, 'Feature 12' as name union all select '99', 'Feature 99'";
        string capturedSql = null;
        queryProxy
            .Setup(proxy => proxy.QueryRows(It.Is<string>(sql => sql == outerCategorySql), It.IsAny<Func<IDataReader, CategoryItem>>()))
            .Returns(outerItems);
        queryProxy
            .Setup(proxy => proxy.QueryRows(It.Is<string>(sql => sql != outerCategorySql), It.IsAny<Func<IDataReader, CategoryItem>>()))
            .Callback<string, Func<IDataReader, CategoryItem>>((sql, _) => capturedSql = sql)
            .Returns(countedItems);

        var discreteCategoryInfoService = new Mock<IDiscreteCategoryInfoService>();
        discreteCategoryInfoService.SetupGet(service => service.SqlCompiler).Returns(Mock.Of<IDiscreteCategoryInfoSqlCompiler>());
        discreteCategoryInfoService
            .Setup(service => service.GetCategoryInfo(It.IsAny<FacetsConfig2>(), "feature_type", null))
            .Returns(new FacetContent.CategoryInfo { Count = 2, Query = outerCategorySql });

        var service = CreateService(queryProxy.Object, discreteCategoryInfoService: discreteCategoryInfoService.Object);
        var facetsConfig = CreateTargetOnlyRoutedDiscreteFacetsConfig();

        var result = service.Load(facetsConfig);

        result.SqlQuery.Should().Be(capturedSql);
        result.IntervalInfo.Query.Should().Be(outerCategorySql);
        result.Items.Should().BeEquivalentTo(
            new[]
            {
                new CategoryItem { Category = "12", Count = 2, Name = "Feature 12", Extent = [0] },
                new CategoryItem { Category = "99", Count = 0, Name = "Feature 99", Extent = [0] },
            }
        );
        result.Distribution.Should().ContainKey("12");
        result.Distribution.Should().NotContainKey("99");
        result.SqlQuery.Should().Contain("target_route as");
        result.SqlQuery.Should().Contain("join composed_filter");
        result.SqlQuery.Should().NotContain("predicate_0 as");
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
    public void CanHandle_WithJoinedTableFacetClause_ReturnsFalse()
    {
        var service = CreateService();
        var facetsConfig = CreateCountryToSitesFacetsConfig(countryClause: "tbl_sites.deleted = true");

        service.CanHandle(facetsConfig).Should().BeFalse();
    }

    [Fact]
    public void CanHandle_WithRangeTargetAndDiscretePredicate_ReturnsTrue()
    {
        var service = CreateService();
        var facetsConfig = CreateCountryToGeochronologyFacetsConfig();

        service.CanHandle(facetsConfig).Should().BeTrue();
    }

    [Fact]
    public void CanHandle_WithRangeTargetAndPlaceholderPrimaryKey_ReturnsTrue()
    {
        var service = CreateService();
        var facetsConfig = CreateCountryToAbundancesAllFacetsConfig();

        service.CanHandle(facetsConfig).Should().BeTrue();
    }

    [Fact]
    public void Load_WithRangeTargetAndDiscretePredicate_ReturnsIntervalBackedFacetContent()
    {
        var intervalSql = "select '0 to 10', 0, 10 union all select '10 to 20', 10, 20";
        var categoryInfo = new FacetContent.CategoryInfo { Count = 2, Query = intervalSql };
        var outerItems = new List<CategoryItem>
        {
            new()
            {
                Category = "0 to 10",
                Count = null,
                Name = "0 to 10",
                Extent = [0, 10],
            },
            new()
            {
                Category = "10 to 20",
                Count = null,
                Name = "10 to 20",
                Extent = [10, 20],
            },
        };
        var countedItems = new List<CategoryItem>
        {
            new()
            {
                Category = "0 to 10",
                Count = 2,
                Name = "0 to 10",
                Extent = [0, 10],
            },
            new()
            {
                Category = "10 to 20",
                Count = 0,
                Name = "10 to 20",
                Extent = [10, 20],
            },
        };

        var queryProxy = new Mock<ITypedQueryProxy>();
        string capturedSql = null;
        queryProxy
            .Setup(proxy => proxy.QueryRows(It.Is<string>(sql => sql == intervalSql), It.IsAny<Func<IDataReader, CategoryItem>>()))
            .Returns(outerItems);
        queryProxy
            .Setup(proxy => proxy.QueryRows(It.Is<string>(sql => sql != intervalSql), It.IsAny<Func<IDataReader, CategoryItem>>()))
            .Callback<string, Func<IDataReader, CategoryItem>>((sql, _) => capturedSql = sql)
            .Returns(countedItems);

        var rangeInfoSqlCompiler = new Mock<IRangeCategoryInfoSqlCompiler>();
        var rangeInfoService = new Mock<IRangeCategoryInfoService>();
        rangeInfoService.SetupGet(service => service.SqlCompiler).Returns(rangeInfoSqlCompiler.Object);
        rangeInfoService.Setup(service => service.GetCategoryInfo(It.IsAny<FacetsConfig2>(), "geochronology", null)).Returns(categoryInfo);

        var service = CreateService(queryProxy.Object, rangeCategoryInfoService: rangeInfoService.Object);
        var facetsConfig = CreateCountryToGeochronologyFacetsConfig();

        var result = service.Load(facetsConfig);

        result.IntervalInfo.Should().BeSameAs(categoryInfo);
        result.SqlQuery.Should().Be(capturedSql);
        result.SqlQuery.Should().Contain("categories(category, lower, upper) as");
        result.SqlQuery.Should().Contain("tbl_geochronology.age::integer");
        result.SqlQuery.Should().Contain("count(distinct composed_filter.target_id)");
        result.SqlQuery.Should().Contain("target_route.target_id = tbl_geochronology.geochron_id");
        result.Items.Should().BeEquivalentTo(countedItems);
        result.Distribution.Should().HaveCount(2);
    }

    [Fact]
    public void Load_WithRangeTargetAndEnforcedTargetClause_IncludesClauseInSql()
    {
        var intervalSql = "select '0 to 10', 0, 10 union all select '10 to 20', 10, 20";
        var categoryInfo = new FacetContent.CategoryInfo { Count = 2, Query = intervalSql };
        var outerItems = new List<CategoryItem>
        {
            new()
            {
                Category = "0 to 10",
                Count = null,
                Name = "0 to 10",
                Extent = [0, 10],
            },
        };
        var countedItems = new List<CategoryItem>
        {
            new()
            {
                Category = "0 to 10",
                Count = 2,
                Name = "0 to 10",
                Extent = [0, 10],
            },
        };

        var queryProxy = new Mock<ITypedQueryProxy>();
        string capturedSql = null;
        queryProxy
            .Setup(proxy => proxy.QueryRows(It.Is<string>(sql => sql == intervalSql), It.IsAny<Func<IDataReader, CategoryItem>>()))
            .Returns(outerItems);
        queryProxy
            .Setup(proxy => proxy.QueryRows(It.Is<string>(sql => sql != intervalSql), It.IsAny<Func<IDataReader, CategoryItem>>()))
            .Callback<string, Func<IDataReader, CategoryItem>>((sql, _) => capturedSql = sql)
            .Returns(countedItems);

        var rangeInfoSqlCompiler = new Mock<IRangeCategoryInfoSqlCompiler>();
        var rangeInfoService = new Mock<IRangeCategoryInfoService>();
        rangeInfoService.SetupGet(service => service.SqlCompiler).Returns(rangeInfoSqlCompiler.Object);
        rangeInfoService.Setup(service => service.GetCategoryInfo(It.IsAny<FacetsConfig2>(), "abundances_all", null)).Returns(categoryInfo);

        var service = CreateService(queryProxy.Object, rangeCategoryInfoService: rangeInfoService.Object);
        var facetsConfig = CreateCountryToAbundancesAllFacetsConfig();

        var result = service.Load(facetsConfig);

        result.SqlQuery.Should().Be(capturedSql);
        result.SqlQuery.Should().Contain("facet.view_abundance.abundance is not null");
        result.SqlQuery.Should().Contain("target_route.target_id = facet.view_abundance.analysis_entity_id");
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

    [Fact]
    public void CanHandle_WithPlaceholderTargetPrimaryKeyAndNonSimpleCategoryExpression_ReturnsFalse()
    {
        var service = CreateService();
        var facetsConfig = CreateCountryToSpeciesFacetsConfig(targetCategoryExpression: "coalesce(facet.abundance_taxon_shortcut.taxon_id, 0)");

        service.CanHandle(facetsConfig).Should().BeFalse();
    }

    private static ComposedFacetContentService CreateService(
        ITypedQueryProxy queryProxy = null,
        IDiscreteCategoryInfoService discreteCategoryInfoService = null,
        IRangeCategoryInfoService rangeCategoryInfoService = null
    )
    {
        var sites = CreateTable(1, "tbl_sites", "site_id");
        var samples = CreateTable(2, "tbl_physical_samples", "physical_sample_id");
        var sampleGroups = CreateTable(3, "tbl_sample_groups", "sample_group_id");
        var analysisEntities = CreateTable(4, "tbl_analysis_entities", "analysis_entity_id");
        var countryShortcut = CreateTable(5, "facet.site_location_shortcut", "xxxx");
        var featureTypes = CreateTable(6, "tbl_feature_types", "feature_type_id");
        var physicalSampleFeatures = CreateTable(7, "tbl_physical_sample_features", "physical_sample_feature_id");
        var abundanceTaxonShortcut = CreateTable(8, "facet.abundance_taxon_shortcut", "xxx");
        var geochronology = CreateTable(9, "tbl_geochronology", "geochron_id");
        var viewAbundance = CreateTable(10, "facet.view_abundance", "xxxx");
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
            CreateRelation(geochronology, analysisEntities, "analysis_entity_id", "analysis_entity_id"),
            CreateRelation(viewAbundance, analysisEntities, "analysis_entity_id", "analysis_entity_id"),
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
        var defaultDiscreteInfoService = discreteCategoryInfoService ?? Mock.Of<IDiscreteCategoryInfoService>();
        var defaultRangeInfoService = rangeCategoryInfoService ?? Mock.Of<IRangeCategoryInfoService>();
        return new ComposedFacetContentService(
            registry.Object,
            queryProxy ?? Mock.Of<ITypedQueryProxy>(),
            new PathFinder([.. graph, .. graph.ReversedEdges()]),
            routeSqlCompiler,
            new DiscreteFacetPredicateResolver(routeSqlCompiler),
            new IntersectComposedFilterQueryComposer(),
            new DiscreteFacetContentQueryComposer(new PathFinder([.. graph, .. graph.ReversedEdges()]), joinsClauseCompiler.Object),
            defaultDiscreteInfoService,
            defaultRangeInfoService
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

    private static FacetsConfig2 CreateTargetOnlyDiscreteFacetsConfig()
    {
        var genera = CreateTable(12, "tbl_taxa_tree_genera", "genus_id");

        var targetFacet = new Facet
        {
            FacetCode = "genus",
            FacetId = 30,
            FacetTypeId = EFacetType.Discrete,
            CategoryIdExpr = "tbl_taxa_tree_genera.genus_id",
            Tables = [new FacetTable { SequenceId = 1, Table = genera }],
        };

        return new FacetsConfig2
        {
            TargetCode = "genus",
            TargetFacet = targetFacet,
            FacetConfigs = [new FacetConfig2(targetFacet, 1, string.Empty, [])],
        };
    }

    private static FacetsConfig2 CreateTargetOnlyRoutedDiscreteFacetsConfig()
    {
        var featureTypes = CreateTable(6, "tbl_feature_types", "feature_type_id");
        var physicalSampleFeatures = CreateTable(7, "tbl_physical_sample_features", "physical_sample_feature_id");

        var targetFacet = new Facet
        {
            FacetCode = "feature_type",
            FacetId = 31,
            FacetTypeId = EFacetType.Discrete,
            AggregateFacetId = 10,
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
            FacetConfigs = [new FacetConfig2(targetFacet, 1, string.Empty, [])],
        };
    }

    private static FacetsConfig2 CreateCountryToSpeciesFacetsConfig(
        string targetCategoryExpression = "facet.abundance_taxon_shortcut.taxon_id"
    )
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
            CategoryIdExpr = targetCategoryExpression,
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

    private static FacetsConfig2 CreateCountryToSitesFacetsConfig(string countryClause = "countries.location_type_id=1")
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
            Clauses = [new FacetClause { Clause = countryClause, EnforceConstraint = true }],
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

    private static FacetsConfig2 CreateCountryToGeochronologyFacetsConfig()
    {
        var countryShortcut = CreateTable(5, "facet.site_location_shortcut", "xxxx");
        var geochronology = CreateTable(9, "tbl_geochronology", "geochron_id");

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
            FacetCode = "geochronology",
            FacetTypeId = EFacetType.Range,
            AggregateFacetId = 11,
            CategoryIdExpr = "tbl_geochronology.age",
            CategoryIdType = "integer",
            Tables = [new FacetTable { SequenceId = 1, Table = geochronology }],
        };

        return new FacetsConfig2
        {
            TargetCode = "geochronology",
            TargetFacet = targetFacet,
            FacetConfigs =
            [
                new FacetConfig2(countryFacet, 1, string.Empty, [new FacetConfigPick(1), new FacetConfigPick(2), new FacetConfigPick(5)]),
                new FacetConfig2(targetFacet, 2, string.Empty, []),
            ],
        };
    }

    private static FacetsConfig2 CreateCountryToAbundancesAllFacetsConfig()
    {
        var countryShortcut = CreateTable(5, "facet.site_location_shortcut", "xxxx");
        var viewAbundance = CreateTable(10, "facet.view_abundance", "xxxx");

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
            FacetCode = "abundances_all",
            FacetTypeId = EFacetType.Range,
            FacetType = new FacetType { FacetTypeId = EFacetType.Range, ReloadAsTarget = true },
            AggregateFacetId = 10,
            CategoryIdExpr = "facet.view_abundance.abundance",
            CategoryIdType = "integer",
            Tables = [new FacetTable { SequenceId = 1, Table = viewAbundance }],
            Clauses = [new FacetClause { Clause = "facet.view_abundance.abundance is not null", EnforceConstraint = true }],
        };

        return new FacetsConfig2
        {
            TargetCode = "abundances_all",
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
