using System;
using System.Linq;
using Autofac;
using FluentAssertions;
using SeadQueryCore;
using SeadQueryCore.Model;
using SeadQueryCore.Services.Result;
using Xunit;

namespace SQT.UnitTests.QueryComposer.Services;

[Collection("UsePostgresFixture")]
public class ResultProjectionHandoffBuilderTests : IntegrationTestBase
{
    [Fact]
    public void Build_WithCountryFilteredTabularResult_UsesDirectComposedFilterJoin()
    {
        var builder = Container.Resolve<IResultProjectionHandoffBuilder>();
        var facetsConfig = FakeFacetsConfig("country:country@57");
        var resultConfig = FakeResultConfig("result_facet", "site_level", "tabular");

        var result = builder.Build(facetsConfig, resultConfig);

        result.QuerySetup.LeadingSql.Should().Contain("with composed_filter as");
        result.QuerySetup.LeadingSql.Should().NotContain("target_route as (");
        result
            .QuerySetup.Joins.Should()
            .Contain(join => join.Contains("join composed_filter on composed_filter.target_id = tbl_analysis_entities.analysis_entity_id"));
    }

    [Fact]
    public void Build_WithCountryFilteredMapResult_UsesTargetRouteJoin()
    {
        var builder = Container.Resolve<IResultProjectionHandoffBuilder>();
        var facetsConfig = FakeFacetsConfig("country:country@57");
        var resultConfig = FakeResultConfig("map_result", "map_result", "map");

        var result = builder.Build(facetsConfig, resultConfig);

        result.QuerySetup.LeadingSql.Should().Contain("with composed_filter as");
        result.QuerySetup.LeadingSql.Should().Contain("target_route as (");
        result.QuerySetup.Joins.Should().Contain(join => join.Contains("join target_route on target_route.target_id = tbl_sites.site_id"));
        result
            .QuerySetup.Joins.Should()
            .Contain(join => join.Contains("join composed_filter on composed_filter.target_id = target_route.source_id"));
    }

    [Fact]
    public void Build_WithCountryFilteredMapResultAfterBogusPickUpdate_StillUsesTargetRouteJoin()
    {
        var builder = Container.Resolve<IResultProjectionHandoffBuilder>();
        var bogusPickService = Container.Resolve<IBogusPickService>();
        var facetsConfig = FakeFacetsConfig("country:country@57");
        var resultConfig = FakeResultConfig("map_result", "map_result", "map");

        bogusPickService.Update(facetsConfig);
        var result = builder.Build(facetsConfig, resultConfig);

        result.QuerySetup.LeadingSql.Should().Contain("with composed_filter as");
        result.QuerySetup.LeadingSql.Should().Contain("target_route as (");
        result.QuerySetup.Joins.Should().Contain(join => join.Contains("join target_route on target_route.target_id = tbl_sites.site_id"));
        result
            .QuerySetup.Joins.Should()
            .Contain(join => join.Contains("join composed_filter on composed_filter.target_id = target_route.source_id"));
    }

    [Fact]
    public void Build_WithRangeFilteredTabularResult_UsesComposedFilterJoin()
    {
        var builder = Container.Resolve<IResultProjectionHandoffBuilder>();
        var facetsConfig = FakeFacetsConfig("sites:sites@1,2/geochronology@(0,100)");
        var resultConfig = FakeResultConfig("result_facet", "site_level", "tabular");

        var result = builder.Build(facetsConfig, resultConfig);

        result.QuerySetup.LeadingSql.Should().Contain("with composed_filter as");
        result.QuerySetup.LeadingSql.Should().Contain(" >= 0");
        result.QuerySetup.LeadingSql.Should().Contain(" <= 100");
        result
            .QuerySetup.Joins.Should()
            .Contain(join => join.Contains("join composed_filter on composed_filter.target_id = tbl_analysis_entities.analysis_entity_id"));
    }

    [Fact]
    public void Build_WithIntersectFilteredTabularResult_UsesComposedFilterJoin()
    {
        var builder = Container.Resolve<IResultProjectionHandoffBuilder>();
        var facetsConfig = FakeFacetsConfig("analysis_entity_ages:analysis_entity_ages@850000,2350000");
        var resultConfig = FakeResultConfig("result_facet", "site_level", "tabular");

        var result = builder.Build(facetsConfig, resultConfig);

        result.QuerySetup.LeadingSql.Should().Contain("with composed_filter as");
        result.QuerySetup.LeadingSql.Should().Contain("int4range(850000, 2350000, '[]')");
        result
            .QuerySetup.Joins.Should()
            .Contain(join => join.Contains("join composed_filter on composed_filter.target_id = tbl_analysis_entities.analysis_entity_id"));
    }

    [Fact]
    public void Build_WithRangeFilteredMapResult_UsesTargetRouteJoin()
    {
        var builder = Container.Resolve<IResultProjectionHandoffBuilder>();
        var facetsConfig = FakeFacetsConfig("sites:geochronology@(0,100)/sites");
        var resultConfig = FakeResultConfig("map_result", "map_result", "map");

        var result = builder.Build(facetsConfig, resultConfig);

        result.QuerySetup.LeadingSql.Should().Contain("with composed_filter as");
        result.QuerySetup.LeadingSql.Should().Contain("target_route as (");
        result.QuerySetup.LeadingSql.Should().Contain(" >= 0");
        result.QuerySetup.LeadingSql.Should().Contain(" <= 100");
        result.QuerySetup.Joins.Should().Contain(join => join.Contains("join target_route on target_route.target_id = tbl_sites.site_id"));
        result
            .QuerySetup.Joins.Should()
            .Contain(join => join.Contains("join composed_filter on composed_filter.target_id = target_route.source_id"));
    }

    [Fact]
    public void Build_WithGeoPolygonFilteredTabularResult_UsesComposedFilterJoin()
    {
        var builder = Container.Resolve<IResultProjectionHandoffBuilder>();
        var facetsConfig = FakeFacetsConfig(
            "sites_polygon:sites_polygon@63.872484,20.093291,63.947006,20.501316,63.878949,20.673213,63.748021,20.252953,63.793983,20.095738"
        );
        var resultConfig = FakeResultConfig("result_facet", "site_level", "tabular");

        var result = builder.Build(facetsConfig, resultConfig);

        result.QuerySetup.LeadingSql.Should().Contain("with composed_filter as");
        result.QuerySetup.LeadingSql.Should().Contain("ST_Within(");
        result
            .QuerySetup.Joins.Should()
            .Contain(join => join.Contains("join composed_filter on composed_filter.target_id = tbl_analysis_entities.analysis_entity_id"));
    }

    [Fact]
    public void Build_WithGeoPolygonFilteredMapResult_UsesTargetRouteJoin()
    {
        var builder = Container.Resolve<IResultProjectionHandoffBuilder>();
        var facetsConfig = FakeFacetsConfig(
            "sites_polygon:sites_polygon@63.872484,20.093291,63.947006,20.501316,63.878949,20.673213,63.748021,20.252953,63.793983,20.095738"
        );
        var resultConfig = FakeResultConfig("map_result", "map_result", "map");

        var result = builder.Build(facetsConfig, resultConfig);

        result.QuerySetup.LeadingSql.Should().Contain("with composed_filter as");
        result.QuerySetup.LeadingSql.Should().Contain("target_route as (");
        result.QuerySetup.LeadingSql.Should().Contain("ST_Within(");
        result.QuerySetup.Joins.Should().Contain(join => join.Contains("join target_route on target_route.target_id = tbl_sites.site_id"));
        result
            .QuerySetup.Joins.Should()
            .Contain(join => join.Contains("join composed_filter on composed_filter.target_id = target_route.source_id"));
    }

    [Fact]
    public void Build_WithIntersectFilteredMapResult_UsesTargetRouteJoin()
    {
        var builder = Container.Resolve<IResultProjectionHandoffBuilder>();
        var facetsConfig = FakeFacetsConfig("analysis_entity_ages:analysis_entity_ages@850000,2350000/sites");
        var resultConfig = FakeResultConfig("map_result", "map_result", "map");

        var result = builder.Build(facetsConfig, resultConfig);

        result.QuerySetup.LeadingSql.Should().Contain("with composed_filter as");
        result.QuerySetup.LeadingSql.Should().Contain("target_route as (");
        result.QuerySetup.LeadingSql.Should().Contain("int4range(850000, 2350000, '[]')");
        result.QuerySetup.Joins.Should().Contain(join => join.Contains("join target_route on target_route.target_id = tbl_sites.site_id"));
        result
            .QuerySetup.Joins.Should()
            .Contain(join => join.Contains("join composed_filter on composed_filter.target_id = target_route.source_id"));
    }

    [Fact]
    public void Build_WithTargetOnlyGenusTabularResult_UsesUnfilteredComposedFilterJoin()
    {
        var builder = Container.Resolve<IResultProjectionHandoffBuilder>();
        var facetsConfig = FakeFacetsConfig("genus:genus");
        var resultConfig = FakeResultConfig("result_facet", "site_level", "tabular");

        var result = builder.Build(facetsConfig, resultConfig);

        result.QuerySetup.LeadingSql.Should().Contain("with composed_filter as");
        result.QuerySetup.LeadingSql.Should().Contain("from tbl_analysis_entities");
        result
            .QuerySetup.Joins.Should()
            .Contain(join => join.Contains("join composed_filter on composed_filter.target_id = tbl_analysis_entities.analysis_entity_id"));
    }

    [Fact]
    public void Build_WithTargetOnlyArchaeobotanyGenusTabularResult_UsesUnfilteredComposedFilterJoin()
    {
        var builder = Container.Resolve<IResultProjectionHandoffBuilder>();
        var facetsConfig = FakeFacetsConfig("archaeobotany://genus:genus");
        var resultConfig = FakeResultConfig("result_facet", "site_level", "tabular");

        var result = builder.Build(facetsConfig, resultConfig);

        result.QuerySetup.LeadingSql.Should().Contain("with composed_filter as");
        result.QuerySetup.LeadingSql.Should().Contain("from tbl_analysis_entities");
        result
            .QuerySetup.Joins.Should()
            .Contain(join => join.Contains("join composed_filter on composed_filter.target_id = tbl_analysis_entities.analysis_entity_id"));
    }

    [Fact]
    public void Build_WithTargetOnlyPollenGenusTabularResult_UsesUnfilteredComposedFilterJoin()
    {
        var builder = Container.Resolve<IResultProjectionHandoffBuilder>();
        var facetsConfig = FakeFacetsConfig("pollen://genus:genus");
        var resultConfig = FakeResultConfig("result_facet", "site_level", "tabular");

        var result = builder.Build(facetsConfig, resultConfig);

        result.QuerySetup.LeadingSql.Should().Contain("with composed_filter as");
        result.QuerySetup.LeadingSql.Should().Contain("from tbl_analysis_entities");
        result
            .QuerySetup.Joins.Should()
            .Contain(join => join.Contains("join composed_filter on composed_filter.target_id = tbl_analysis_entities.analysis_entity_id"));
    }

    [Fact]
    public void Build_WithTargetOnlyDendrochronologyGenusTabularResult_UsesUnfilteredComposedFilterJoin()
    {
        var builder = Container.Resolve<IResultProjectionHandoffBuilder>();
        var facetsConfig = FakeFacetsConfig("dendrochronology://genus:genus");
        var resultConfig = FakeResultConfig("result_facet", "site_level", "tabular");

        var result = builder.Build(facetsConfig, resultConfig);

        result.QuerySetup.LeadingSql.Should().Contain("with composed_filter as");
        result.QuerySetup.LeadingSql.Should().Contain("from tbl_analysis_entities");
        result
            .QuerySetup.Joins.Should()
            .Contain(join => join.Contains("join composed_filter on composed_filter.target_id = tbl_analysis_entities.analysis_entity_id"));
    }

    [Fact]
    public void Build_WithTargetOnlyPalaeoentomologyGenusTabularResult_UsesUnfilteredComposedFilterJoin()
    {
        var builder = Container.Resolve<IResultProjectionHandoffBuilder>();
        var facetsConfig = FakeFacetsConfig("palaeoentomology://genus:genus");
        var resultConfig = FakeResultConfig("result_facet", "site_level", "tabular");

        var result = builder.Build(facetsConfig, resultConfig);

        result.QuerySetup.LeadingSql.Should().Contain("with composed_filter as");
        result.QuerySetup.LeadingSql.Should().Contain("from tbl_analysis_entities");
        result
            .QuerySetup.Joins.Should()
            .Contain(join => join.Contains("join composed_filter on composed_filter.target_id = tbl_analysis_entities.analysis_entity_id"));
    }

    [Theory]
    [InlineData("palaeoentomology://feature_type:feature_type")]
    [InlineData("archaeobotany://feature_type:feature_type")]
    [InlineData("pollen://feature_type:feature_type")]
    [InlineData("geoarchaeology://feature_type:feature_type")]
    [InlineData("dendrochronology://feature_type:feature_type")]
    [InlineData("ceramic://feature_type:feature_type")]
    public void Build_WithTargetOnlyPrefixedFeatureTypeTabularResult_UsesUnfilteredComposedFilterJoin(string uri)
    {
        var builder = Container.Resolve<IResultProjectionHandoffBuilder>();
        var facetsConfig = FakeFacetsConfig(uri);
        var resultConfig = FakeResultConfig("result_facet", "site_level", "tabular");

        var result = builder.Build(facetsConfig, resultConfig);

        result.QuerySetup.LeadingSql.Should().Contain("with composed_filter as");
        result.QuerySetup.LeadingSql.Should().Contain("from tbl_analysis_entities");
        result
            .QuerySetup.Joins.Should()
            .Contain(join => join.Contains("join composed_filter on composed_filter.target_id = tbl_analysis_entities.analysis_entity_id"));
    }

    [Theory]
    [InlineData("palaeoentomology://sample_group_sampling_contexts:sample_group_sampling_contexts")]
    [InlineData("archaeobotany://sample_group_sampling_contexts:sample_group_sampling_contexts")]
    [InlineData("pollen://sample_group_sampling_contexts:sample_group_sampling_contexts")]
    [InlineData("geoarchaeology://sample_group_sampling_contexts:sample_group_sampling_contexts")]
    [InlineData("dendrochronology://sample_group_sampling_contexts:sample_group_sampling_contexts")]
    [InlineData("ceramic://sample_group_sampling_contexts:sample_group_sampling_contexts")]
    public void Build_WithTargetOnlyPrefixedSampleGroupSamplingContextsTabularResult_UsesUnfilteredComposedFilterJoin(string uri)
    {
        var builder = Container.Resolve<IResultProjectionHandoffBuilder>();
        var facetsConfig = FakeFacetsConfig(uri);
        var resultConfig = FakeResultConfig("result_facet", "site_level", "tabular");

        var result = builder.Build(facetsConfig, resultConfig);

        result.QuerySetup.LeadingSql.Should().Contain("with composed_filter as");
        result.QuerySetup.LeadingSql.Should().Contain("from tbl_analysis_entities");
        result
            .QuerySetup.Joins.Should()
            .Contain(join => join.Contains("join composed_filter on composed_filter.target_id = tbl_analysis_entities.analysis_entity_id"));
    }

    [Theory]
    [InlineData("palaeoentomology://tbl_biblio_modern:tbl_biblio_modern")]
    [InlineData("archaeobotany://tbl_biblio_modern:tbl_biblio_modern")]
    [InlineData("pollen://tbl_biblio_modern:tbl_biblio_modern")]
    [InlineData("geoarchaeology://tbl_biblio_modern:tbl_biblio_modern")]
    [InlineData("dendrochronology://tbl_biblio_modern:tbl_biblio_modern")]
    [InlineData("ceramic://tbl_biblio_modern:tbl_biblio_modern")]
    public void Build_WithTargetOnlyPrefixedBiblioModernTabularResult_UsesUnfilteredComposedFilterJoin(string uri)
    {
        var builder = Container.Resolve<IResultProjectionHandoffBuilder>();
        var facetsConfig = FakeFacetsConfig(uri);
        var resultConfig = FakeResultConfig("result_facet", "site_level", "tabular");

        var result = builder.Build(facetsConfig, resultConfig);

        result.QuerySetup.LeadingSql.Should().Contain("with composed_filter as");
        result.QuerySetup.LeadingSql.Should().Contain("from tbl_analysis_entities");
        result
            .QuerySetup.Joins.Should()
            .Contain(join => join.Contains("join composed_filter on composed_filter.target_id = tbl_analysis_entities.analysis_entity_id"));
    }

    [Theory]
    [InlineData("palaeoentomology://species:species")]
    [InlineData("archaeobotany://species:species")]
    [InlineData("pollen://species:species")]
    [InlineData("dendrochronology://species:species")]
    public void Build_WithUnsupportedOutOfDraftSpeciesTabularResult_ThrowsExplicitFailure(string uri)
    {
        var builder = Container.Resolve<IResultProjectionHandoffBuilder>();
        var facetsConfig = FakeFacetsConfig(uri);
        var resultConfig = FakeResultConfig("result_facet", "site_level", "tabular");

        var action = () => builder.Build(facetsConfig, resultConfig);

        action
            .Should()
            .Throw<InvalidOperationException>()
            .WithMessage("*cannot handle this request*simple source key column*no longer fall back to the legacy runtime*");
    }

    [Fact]
    public void Build_WithTargetOnlyIsotopeSitesTabularResult_UsesUnfilteredComposedFilterJoin()
    {
        var builder = Container.Resolve<IResultProjectionHandoffBuilder>();
        var facetsConfig = FakeFacetsConfig("isotope://sites:sites");
        var resultConfig = FakeResultConfig("result_facet", "site_level", "tabular");

        var result = builder.Build(facetsConfig, resultConfig);

        result.QuerySetup.LeadingSql.Should().Contain("with composed_filter as");
        result.QuerySetup.LeadingSql.Should().Contain("from tbl_analysis_entities");
        result
            .QuerySetup.Joins.Should()
            .Contain(join => join.Contains("join composed_filter on composed_filter.target_id = tbl_analysis_entities.analysis_entity_id"));
    }

    [Fact]
    public void Build_WithTargetOnlyIsotopeSitesMapResult_UsesTargetRouteJoin()
    {
        var builder = Container.Resolve<IResultProjectionHandoffBuilder>();
        var facetsConfig = FakeFacetsConfig("isotope://sites:sites");
        var resultConfig = FakeResultConfig("map_result", "map_result", "map");

        var result = builder.Build(facetsConfig, resultConfig);

        result.QuerySetup.LeadingSql.Should().Contain("with composed_filter as");
        result.QuerySetup.LeadingSql.Should().Contain("target_route as (");
        result.QuerySetup.LeadingSql.Should().Contain("from tbl_analysis_entities");
        result.QuerySetup.Joins.Should().Contain(join => join.Contains("join target_route on target_route.target_id = tbl_sites.site_id"));
        result
            .QuerySetup.Joins.Should()
            .Contain(join => join.Contains("join composed_filter on composed_filter.target_id = target_route.source_id"));
    }

    [Fact]
    public void Build_WithTargetOnlySitesMapResult_UsesTargetRouteJoin()
    {
        var builder = Container.Resolve<IResultProjectionHandoffBuilder>();
        var facetsConfig = FakeFacetsConfig("sites:sites");
        var resultConfig = FakeResultConfig("map_result", "map_result", "map");

        var result = builder.Build(facetsConfig, resultConfig);

        result.QuerySetup.LeadingSql.Should().Contain("with composed_filter as");
        result.QuerySetup.LeadingSql.Should().Contain("target_route as (");
        result.QuerySetup.LeadingSql.Should().Contain("from tbl_analysis_entities");
        result.QuerySetup.Joins.Should().Contain(join => join.Contains("join target_route on target_route.target_id = tbl_sites.site_id"));
        result
            .QuerySetup.Joins.Should()
            .Contain(join => join.Contains("join composed_filter on composed_filter.target_id = target_route.source_id"));
    }

    [Fact]
    public void Build_WithTargetOnlyPollenSitesMapResult_UsesTargetRouteJoin()
    {
        var builder = Container.Resolve<IResultProjectionHandoffBuilder>();
        var facetsConfig = FakeFacetsConfig("pollen://sites:sites");
        var resultConfig = FakeResultConfig("map_result", "map_result", "map");

        var result = builder.Build(facetsConfig, resultConfig);

        result.QuerySetup.LeadingSql.Should().Contain("with composed_filter as");
        result.QuerySetup.LeadingSql.Should().Contain("target_route as (");
        result.QuerySetup.LeadingSql.Should().Contain("from tbl_analysis_entities");
        result.QuerySetup.Joins.Should().Contain(join => join.Contains("join target_route on target_route.target_id = tbl_sites.site_id"));
        result
            .QuerySetup.Joins.Should()
            .Contain(join => join.Contains("join composed_filter on composed_filter.target_id = target_route.source_id"));
    }

    [Fact]
    public void Build_WithTargetOnlyCeramicSitesMapResult_UsesTargetRouteJoin()
    {
        var builder = Container.Resolve<IResultProjectionHandoffBuilder>();
        var facetsConfig = FakeFacetsConfig("ceramic://sites:sites");
        var resultConfig = FakeResultConfig("map_result", "map_result", "map");

        var result = builder.Build(facetsConfig, resultConfig);

        result.QuerySetup.LeadingSql.Should().Contain("with composed_filter as");
        result.QuerySetup.LeadingSql.Should().Contain("target_route as (");
        result.QuerySetup.LeadingSql.Should().Contain("from tbl_analysis_entities");
        result.QuerySetup.Joins.Should().Contain(join => join.Contains("join target_route on target_route.target_id = tbl_sites.site_id"));
        result
            .QuerySetup.Joins.Should()
            .Contain(join => join.Contains("join composed_filter on composed_filter.target_id = target_route.source_id"));
    }

    [Fact]
    public void Build_WithTargetOnlyGeoarchaeologySitesMapResult_UsesTargetRouteJoin()
    {
        var builder = Container.Resolve<IResultProjectionHandoffBuilder>();
        var facetsConfig = FakeFacetsConfig("geoarchaeology://sites:sites");
        var resultConfig = FakeResultConfig("map_result", "map_result", "map");

        var result = builder.Build(facetsConfig, resultConfig);

        result.QuerySetup.LeadingSql.Should().Contain("with composed_filter as");
        result.QuerySetup.LeadingSql.Should().Contain("target_route as (");
        result.QuerySetup.LeadingSql.Should().Contain("from tbl_analysis_entities");
        result.QuerySetup.Joins.Should().Contain(join => join.Contains("join target_route on target_route.target_id = tbl_sites.site_id"));
        result
            .QuerySetup.Joins.Should()
            .Contain(join => join.Contains("join composed_filter on composed_filter.target_id = target_route.source_id"));
    }

    [Fact]
    public void Build_WithTargetOnlyDendrochronologySitesMapResult_UsesTargetRouteJoin()
    {
        var builder = Container.Resolve<IResultProjectionHandoffBuilder>();
        var facetsConfig = FakeFacetsConfig("dendrochronology://sites:sites");
        var resultConfig = FakeResultConfig("map_result", "map_result", "map");

        var result = builder.Build(facetsConfig, resultConfig);

        result.QuerySetup.LeadingSql.Should().Contain("with composed_filter as");
        result.QuerySetup.LeadingSql.Should().Contain("target_route as (");
        result.QuerySetup.LeadingSql.Should().Contain("from tbl_analysis_entities");
        result.QuerySetup.Joins.Should().Contain(join => join.Contains("join target_route on target_route.target_id = tbl_sites.site_id"));
        result
            .QuerySetup.Joins.Should()
            .Contain(join => join.Contains("join composed_filter on composed_filter.target_id = target_route.source_id"));
    }

    [Fact]
    public void Build_WithTargetOnlyArchaeobotanySitesMapResult_UsesTargetRouteJoin()
    {
        var builder = Container.Resolve<IResultProjectionHandoffBuilder>();
        var facetsConfig = FakeFacetsConfig("archaeobotany://sites:sites");
        var resultConfig = FakeResultConfig("map_result", "map_result", "map");

        var result = builder.Build(facetsConfig, resultConfig);

        result.QuerySetup.LeadingSql.Should().Contain("with composed_filter as");
        result.QuerySetup.LeadingSql.Should().Contain("target_route as (");
        result.QuerySetup.LeadingSql.Should().Contain("from tbl_analysis_entities");
        result.QuerySetup.Joins.Should().Contain(join => join.Contains("join target_route on target_route.target_id = tbl_sites.site_id"));
        result
            .QuerySetup.Joins.Should()
            .Contain(join => join.Contains("join composed_filter on composed_filter.target_id = target_route.source_id"));
    }

    [Fact]
    public void Build_WithTargetOnlyPalaeoentomologySitesMapResult_UsesTargetRouteJoin()
    {
        var builder = Container.Resolve<IResultProjectionHandoffBuilder>();
        var facetsConfig = FakeFacetsConfig("palaeoentomology://sites:sites");
        var resultConfig = FakeResultConfig("map_result", "map_result", "map");

        var result = builder.Build(facetsConfig, resultConfig);

        result.QuerySetup.LeadingSql.Should().Contain("with composed_filter as");
        result.QuerySetup.LeadingSql.Should().Contain("target_route as (");
        result.QuerySetup.LeadingSql.Should().Contain("from tbl_analysis_entities");
        result.QuerySetup.Joins.Should().Contain(join => join.Contains("join target_route on target_route.target_id = tbl_sites.site_id"));
        result
            .QuerySetup.Joins.Should()
            .Contain(join => join.Contains("join composed_filter on composed_filter.target_id = target_route.source_id"));
    }

    [Fact]
    public void Build_WithTargetOnlyPalaeoentomologyDataTypesTabularResult_UsesUnfilteredComposedFilterJoin()
    {
        var builder = Container.Resolve<IResultProjectionHandoffBuilder>();
        var facetsConfig = FakeFacetsConfig("palaeoentomology://data_types:data_types");
        var resultConfig = FakeResultConfig("result_facet", "site_level", "tabular");

        var result = builder.Build(facetsConfig, resultConfig);

        result.QuerySetup.LeadingSql.Should().Contain("with composed_filter as");
        result.QuerySetup.LeadingSql.Should().Contain("from tbl_analysis_entities");
        result
            .QuerySetup.Joins.Should()
            .Contain(join => join.Contains("join composed_filter on composed_filter.target_id = tbl_analysis_entities.analysis_entity_id"));
    }

    [Fact]
    public void Build_WithTargetOnlyPalaeoentomologySampleGroupsTabularResult_UsesUnfilteredComposedFilterJoin()
    {
        var builder = Container.Resolve<IResultProjectionHandoffBuilder>();
        var facetsConfig = FakeFacetsConfig("palaeoentomology://sample_groups:sample_groups");
        var resultConfig = FakeResultConfig("result_facet", "site_level", "tabular");

        var result = builder.Build(facetsConfig, resultConfig);

        result.QuerySetup.LeadingSql.Should().Contain("with composed_filter as");
        result.QuerySetup.LeadingSql.Should().Contain("from tbl_analysis_entities");
        result
            .QuerySetup.Joins.Should()
            .Contain(join => join.Contains("join composed_filter on composed_filter.target_id = tbl_analysis_entities.analysis_entity_id"));
    }

    [Fact]
    public void Build_WithTargetOnlyPalaeoentomologySampleGroupsMapResult_UsesTargetRouteJoin()
    {
        var builder = Container.Resolve<IResultProjectionHandoffBuilder>();
        var facetsConfig = FakeFacetsConfig("palaeoentomology://sample_groups:sample_groups");
        var resultConfig = FakeResultConfig("map_result", "map_result", "map");

        var result = builder.Build(facetsConfig, resultConfig);

        result.QuerySetup.LeadingSql.Should().Contain("with composed_filter as");
        result.QuerySetup.LeadingSql.Should().Contain("target_route as (");
        result.QuerySetup.LeadingSql.Should().Contain("from tbl_analysis_entities");
        result.QuerySetup.Joins.Should().Contain(join => join.Contains("join target_route on target_route.target_id = tbl_sites.site_id"));
        result
            .QuerySetup.Joins.Should()
            .Contain(join => join.Contains("join composed_filter on composed_filter.target_id = target_route.source_id"));
    }

    [Fact]
    public void Build_WithTargetOnlyArchaeobotanyDataTypesTabularResult_UsesUnfilteredComposedFilterJoin()
    {
        var builder = Container.Resolve<IResultProjectionHandoffBuilder>();
        var facetsConfig = FakeFacetsConfig("archaeobotany://data_types:data_types");
        var resultConfig = FakeResultConfig("result_facet", "site_level", "tabular");

        var result = builder.Build(facetsConfig, resultConfig);

        result.QuerySetup.LeadingSql.Should().Contain("with composed_filter as");
        result.QuerySetup.LeadingSql.Should().Contain("from tbl_analysis_entities");
        result
            .QuerySetup.Joins.Should()
            .Contain(join => join.Contains("join composed_filter on composed_filter.target_id = tbl_analysis_entities.analysis_entity_id"));
    }

    [Fact]
    public void Build_WithTargetOnlyPalaeoentomologyRdbCodesTabularResult_UsesUnfilteredComposedFilterJoin()
    {
        var builder = Container.Resolve<IResultProjectionHandoffBuilder>();
        var facetsConfig = FakeFacetsConfig("palaeoentomology://rdb_codes:rdb_codes");
        var resultConfig = FakeResultConfig("result_facet", "site_level", "tabular");

        var result = builder.Build(facetsConfig, resultConfig);

        result.QuerySetup.LeadingSql.Should().Contain("with composed_filter as");
        result.QuerySetup.LeadingSql.Should().Contain("from tbl_analysis_entities");
        result
            .QuerySetup.Joins.Should()
            .Contain(join => join.Contains("join composed_filter on composed_filter.target_id = tbl_analysis_entities.analysis_entity_id"));
    }

    [Fact]
    public void Build_WithTargetOnlyPalaeoentomologyRdbCodesMapResult_UsesTargetRouteJoin()
    {
        var builder = Container.Resolve<IResultProjectionHandoffBuilder>();
        var facetsConfig = FakeFacetsConfig("palaeoentomology://rdb_codes:rdb_codes");
        var resultConfig = FakeResultConfig("map_result", "map_result", "map");

        var result = builder.Build(facetsConfig, resultConfig);

        result.QuerySetup.LeadingSql.Should().Contain("with composed_filter as");
        result.QuerySetup.LeadingSql.Should().Contain("target_route as (");
        result.QuerySetup.LeadingSql.Should().Contain("from tbl_analysis_entities");
        result.QuerySetup.Joins.Should().Contain(join => join.Contains("join target_route on target_route.target_id = tbl_sites.site_id"));
        result
            .QuerySetup.Joins.Should()
            .Contain(join => join.Contains("join composed_filter on composed_filter.target_id = target_route.source_id"));
    }

    [Fact]
    public void Build_WithTargetOnlyPollenDataTypesTabularResult_UsesUnfilteredComposedFilterJoin()
    {
        var builder = Container.Resolve<IResultProjectionHandoffBuilder>();
        var facetsConfig = FakeFacetsConfig("pollen://data_types:data_types");
        var resultConfig = FakeResultConfig("result_facet", "site_level", "tabular");

        var result = builder.Build(facetsConfig, resultConfig);

        result.QuerySetup.LeadingSql.Should().Contain("with composed_filter as");
        result.QuerySetup.LeadingSql.Should().Contain("from tbl_analysis_entities");
        result
            .QuerySetup.Joins.Should()
            .Contain(join => join.Contains("join composed_filter on composed_filter.target_id = tbl_analysis_entities.analysis_entity_id"));
    }

    [Fact]
    public void Build_WithTargetOnlyAbundanceClassificationTabularResult_UsesUnfilteredComposedFilterJoin()
    {
        var builder = Container.Resolve<IResultProjectionHandoffBuilder>();
        var facetsConfig = FakeFacetsConfig("abundance_classification:abundance_classification");
        var resultConfig = FakeResultConfig("result_facet", "site_level", "tabular");

        var result = builder.Build(facetsConfig, resultConfig);

        result.QuerySetup.LeadingSql.Should().Contain("with composed_filter as");
        result.QuerySetup.LeadingSql.Should().Contain("from tbl_analysis_entities");
        result
            .QuerySetup.Joins.Should()
            .Contain(join => join.Contains("join composed_filter on composed_filter.target_id = tbl_analysis_entities.analysis_entity_id"));
    }

    [Fact]
    public void Build_WithTargetOnlyGeoarchaeologyDataTypesTabularResult_UsesUnfilteredComposedFilterJoin()
    {
        var builder = Container.Resolve<IResultProjectionHandoffBuilder>();
        var facetsConfig = FakeFacetsConfig("geoarchaeology://data_types:data_types");
        var resultConfig = FakeResultConfig("result_facet", "site_level", "tabular");

        var result = builder.Build(facetsConfig, resultConfig);

        result.QuerySetup.LeadingSql.Should().Contain("with composed_filter as");
        result.QuerySetup.LeadingSql.Should().Contain("from tbl_analysis_entities");
        result
            .QuerySetup.Joins.Should()
            .Contain(join => join.Contains("join composed_filter on composed_filter.target_id = tbl_analysis_entities.analysis_entity_id"));
    }

    [Fact]
    public void Build_WithTargetOnlyDendrochronologyDataTypesTabularResult_UsesUnfilteredComposedFilterJoin()
    {
        var builder = Container.Resolve<IResultProjectionHandoffBuilder>();
        var facetsConfig = FakeFacetsConfig("dendrochronology://data_types:data_types");
        var resultConfig = FakeResultConfig("result_facet", "site_level", "tabular");

        var result = builder.Build(facetsConfig, resultConfig);

        result.QuerySetup.LeadingSql.Should().Contain("with composed_filter as");
        result.QuerySetup.LeadingSql.Should().Contain("from tbl_analysis_entities");
        result
            .QuerySetup.Joins.Should()
            .Contain(join => join.Contains("join composed_filter on composed_filter.target_id = tbl_analysis_entities.analysis_entity_id"));
    }

    [Fact]
    public void Build_WithTargetOnlyCeramicDataTypesTabularResult_UsesUnfilteredComposedFilterJoin()
    {
        var builder = Container.Resolve<IResultProjectionHandoffBuilder>();
        var facetsConfig = FakeFacetsConfig("ceramic://data_types:data_types");
        var resultConfig = FakeResultConfig("result_facet", "site_level", "tabular");

        var result = builder.Build(facetsConfig, resultConfig);

        result.QuerySetup.LeadingSql.Should().Contain("with composed_filter as");
        result.QuerySetup.LeadingSql.Should().Contain("from tbl_analysis_entities");
        result
            .QuerySetup.Joins.Should()
            .Contain(join => join.Contains("join composed_filter on composed_filter.target_id = tbl_analysis_entities.analysis_entity_id"));
    }

    [Fact]
    public void Build_WithTargetOnlyPalaeoentomologyRdbSystemsTabularResult_UsesUnfilteredComposedFilterJoin()
    {
        var builder = Container.Resolve<IResultProjectionHandoffBuilder>();
        var facetsConfig = FakeFacetsConfig("palaeoentomology://rdb_systems:rdb_systems");
        var resultConfig = FakeResultConfig("result_facet", "site_level", "tabular");

        var result = builder.Build(facetsConfig, resultConfig);

        result.QuerySetup.LeadingSql.Should().Contain("with composed_filter as");
        result.QuerySetup.LeadingSql.Should().Contain("from tbl_analysis_entities");
        result
            .QuerySetup.Joins.Should()
            .Contain(join => join.Contains("join composed_filter on composed_filter.target_id = tbl_analysis_entities.analysis_entity_id"));
    }

    [Fact]
    public void Build_WithTargetOnlyPalaeoentomologyRdbSystemsMapResult_UsesTargetRouteJoin()
    {
        var builder = Container.Resolve<IResultProjectionHandoffBuilder>();
        var facetsConfig = FakeFacetsConfig("palaeoentomology://rdb_systems:rdb_systems");
        var resultConfig = FakeResultConfig("map_result", "map_result", "map");

        var result = builder.Build(facetsConfig, resultConfig);

        result.QuerySetup.LeadingSql.Should().Contain("with composed_filter as");
        result.QuerySetup.LeadingSql.Should().Contain("target_route as (");
        result.QuerySetup.LeadingSql.Should().Contain("from tbl_analysis_entities");
        result.QuerySetup.Joins.Should().Contain(join => join.Contains("join target_route on target_route.target_id = tbl_sites.site_id"));
        result
            .QuerySetup.Joins.Should()
            .Contain(join => join.Contains("join composed_filter on composed_filter.target_id = target_route.source_id"));
    }

    [Fact]
    public void Build_WithTargetOnlyPalaeoentomologyCountryTabularResult_UsesComposedFilterJoin()
    {
        var builder = Container.Resolve<IResultProjectionHandoffBuilder>();
        var facetsConfig = FakeFacetsConfig("palaeoentomology://country:country");
        var resultConfig = FakeResultConfig("result_facet", "site_level", "tabular");

        var result = builder.Build(facetsConfig, resultConfig);

        result.QuerySetup.LeadingSql.Should().Contain("with composed_filter as");
        result.QuerySetup.LeadingSql.Should().Contain("facet.site_location_shortcut");
        result
            .QuerySetup.Joins.Should()
            .Contain(join => join.Contains("join composed_filter on composed_filter.target_id = tbl_analysis_entities.analysis_entity_id"));
    }

    [Fact]
    public void Build_WithTargetOnlyPalaeoentomologyCountryMapResult_UsesTargetRouteJoin()
    {
        var builder = Container.Resolve<IResultProjectionHandoffBuilder>();
        var facetsConfig = FakeFacetsConfig("palaeoentomology://country:country");
        var resultConfig = FakeResultConfig("map_result", "map_result", "map");

        var result = builder.Build(facetsConfig, resultConfig);

        result.QuerySetup.LeadingSql.Should().Contain("with composed_filter as");
        result.QuerySetup.LeadingSql.Should().Contain("target_route as (");
        result.QuerySetup.LeadingSql.Should().Contain("facet.site_location_shortcut");
        result.QuerySetup.Joins.Should().Contain(join => join.Contains("join target_route on target_route.target_id = tbl_sites.site_id"));
        result
            .QuerySetup.Joins.Should()
            .Contain(join => join.Contains("join composed_filter on composed_filter.target_id = target_route.source_id"));
    }

    [Fact]
    public void Build_WithTargetOnlyDendrochronologyCountryTabularResult_UsesComposedFilterJoin()
    {
        var builder = Container.Resolve<IResultProjectionHandoffBuilder>();
        var facetsConfig = FakeFacetsConfig("dendrochronology://country:country");
        var resultConfig = FakeResultConfig("result_facet", "site_level", "tabular");

        var result = builder.Build(facetsConfig, resultConfig);

        result.QuerySetup.LeadingSql.Should().Contain("with composed_filter as");
        result.QuerySetup.LeadingSql.Should().Contain("facet.site_location_shortcut");
        result
            .QuerySetup.Joins.Should()
            .Contain(join => join.Contains("join composed_filter on composed_filter.target_id = tbl_analysis_entities.analysis_entity_id"));
    }

    [Fact]
    public void Build_WithTargetOnlyDendrochronologyCountryMapResult_UsesTargetRouteJoin()
    {
        var builder = Container.Resolve<IResultProjectionHandoffBuilder>();
        var facetsConfig = FakeFacetsConfig("dendrochronology://country:country");
        var resultConfig = FakeResultConfig("map_result", "map_result", "map");

        var result = builder.Build(facetsConfig, resultConfig);

        result.QuerySetup.LeadingSql.Should().Contain("with composed_filter as");
        result.QuerySetup.LeadingSql.Should().Contain("target_route as (");
        result.QuerySetup.LeadingSql.Should().Contain("facet.site_location_shortcut");
        result.QuerySetup.Joins.Should().Contain(join => join.Contains("join target_route on target_route.target_id = tbl_sites.site_id"));
        result
            .QuerySetup.Joins.Should()
            .Contain(join => join.Contains("join composed_filter on composed_filter.target_id = target_route.source_id"));
    }

    [Fact]
    public void Build_WithTargetOnlyCeramicCountryTabularResult_UsesComposedFilterJoin()
    {
        var builder = Container.Resolve<IResultProjectionHandoffBuilder>();
        var facetsConfig = FakeFacetsConfig("ceramic://country:country");
        var resultConfig = FakeResultConfig("result_facet", "site_level", "tabular");

        var result = builder.Build(facetsConfig, resultConfig);

        result.QuerySetup.LeadingSql.Should().Contain("with composed_filter as");
        result.QuerySetup.LeadingSql.Should().Contain("facet.site_location_shortcut");
        result
            .QuerySetup.Joins.Should()
            .Contain(join => join.Contains("join composed_filter on composed_filter.target_id = tbl_analysis_entities.analysis_entity_id"));
    }

    [Fact]
    public void Build_WithTargetOnlyCeramicCountryMapResult_UsesTargetRouteJoin()
    {
        var builder = Container.Resolve<IResultProjectionHandoffBuilder>();
        var facetsConfig = FakeFacetsConfig("ceramic://country:country");
        var resultConfig = FakeResultConfig("map_result", "map_result", "map");

        var result = builder.Build(facetsConfig, resultConfig);

        result.QuerySetup.LeadingSql.Should().Contain("with composed_filter as");
        result.QuerySetup.LeadingSql.Should().Contain("target_route as (");
        result.QuerySetup.LeadingSql.Should().Contain("facet.site_location_shortcut");
        result.QuerySetup.Joins.Should().Contain(join => join.Contains("join target_route on target_route.target_id = tbl_sites.site_id"));
        result
            .QuerySetup.Joins.Should()
            .Contain(join => join.Contains("join composed_filter on composed_filter.target_id = target_route.source_id"));
    }

    [Fact]
    public void Build_WithTargetOnlyArchaeobotanyCountryTabularResult_UsesComposedFilterJoin()
    {
        var builder = Container.Resolve<IResultProjectionHandoffBuilder>();
        var facetsConfig = FakeFacetsConfig("archaeobotany://country:country");
        var resultConfig = FakeResultConfig("result_facet", "site_level", "tabular");

        var result = builder.Build(facetsConfig, resultConfig);

        result.QuerySetup.LeadingSql.Should().Contain("with composed_filter as");
        result.QuerySetup.LeadingSql.Should().Contain("facet.site_location_shortcut");
        result
            .QuerySetup.Joins.Should()
            .Contain(join => join.Contains("join composed_filter on composed_filter.target_id = tbl_analysis_entities.analysis_entity_id"));
    }

    [Fact]
    public void Build_WithTargetOnlyArchaeobotanyCountryMapResult_UsesTargetRouteJoin()
    {
        var builder = Container.Resolve<IResultProjectionHandoffBuilder>();
        var facetsConfig = FakeFacetsConfig("archaeobotany://country:country");
        var resultConfig = FakeResultConfig("map_result", "map_result", "map");

        var result = builder.Build(facetsConfig, resultConfig);

        result.QuerySetup.LeadingSql.Should().Contain("with composed_filter as");
        result.QuerySetup.LeadingSql.Should().Contain("target_route as (");
        result.QuerySetup.LeadingSql.Should().Contain("facet.site_location_shortcut");
        result.QuerySetup.Joins.Should().Contain(join => join.Contains("join target_route on target_route.target_id = tbl_sites.site_id"));
        result
            .QuerySetup.Joins.Should()
            .Contain(join => join.Contains("join composed_filter on composed_filter.target_id = target_route.source_id"));
    }

    [Fact]
    public void Build_WithTargetOnlyPollenCountryTabularResult_UsesComposedFilterJoin()
    {
        var builder = Container.Resolve<IResultProjectionHandoffBuilder>();
        var facetsConfig = FakeFacetsConfig("pollen://country:country");
        var resultConfig = FakeResultConfig("result_facet", "site_level", "tabular");

        var result = builder.Build(facetsConfig, resultConfig);

        result.QuerySetup.LeadingSql.Should().Contain("with composed_filter as");
        result.QuerySetup.LeadingSql.Should().Contain("facet.site_location_shortcut");
        result
            .QuerySetup.Joins.Should()
            .Contain(join => join.Contains("join composed_filter on composed_filter.target_id = tbl_analysis_entities.analysis_entity_id"));
    }

    [Fact]
    public void Build_WithTargetOnlyPollenCountryMapResult_UsesTargetRouteJoin()
    {
        var builder = Container.Resolve<IResultProjectionHandoffBuilder>();
        var facetsConfig = FakeFacetsConfig("pollen://country:country");
        var resultConfig = FakeResultConfig("map_result", "map_result", "map");

        var result = builder.Build(facetsConfig, resultConfig);

        result.QuerySetup.LeadingSql.Should().Contain("with composed_filter as");
        result.QuerySetup.LeadingSql.Should().Contain("target_route as (");
        result.QuerySetup.LeadingSql.Should().Contain("facet.site_location_shortcut");
        result.QuerySetup.Joins.Should().Contain(join => join.Contains("join target_route on target_route.target_id = tbl_sites.site_id"));
        result
            .QuerySetup.Joins.Should()
            .Contain(join => join.Contains("join composed_filter on composed_filter.target_id = target_route.source_id"));
    }

    [Fact]
    public void Build_WithTargetOnlyGeoarchaeologyCountryTabularResult_UsesComposedFilterJoin()
    {
        var builder = Container.Resolve<IResultProjectionHandoffBuilder>();
        var facetsConfig = FakeFacetsConfig("geoarchaeology://country:country");
        var resultConfig = FakeResultConfig("result_facet", "site_level", "tabular");

        var result = builder.Build(facetsConfig, resultConfig);

        result.QuerySetup.LeadingSql.Should().Contain("with composed_filter as");
        result.QuerySetup.LeadingSql.Should().Contain("facet.site_location_shortcut");
        result
            .QuerySetup.Joins.Should()
            .Contain(join => join.Contains("join composed_filter on composed_filter.target_id = tbl_analysis_entities.analysis_entity_id"));
    }

    [Fact]
    public void Build_WithTargetOnlyGeoarchaeologyCountryMapResult_UsesTargetRouteJoin()
    {
        var builder = Container.Resolve<IResultProjectionHandoffBuilder>();
        var facetsConfig = FakeFacetsConfig("geoarchaeology://country:country");
        var resultConfig = FakeResultConfig("map_result", "map_result", "map");

        var result = builder.Build(facetsConfig, resultConfig);

        result.QuerySetup.LeadingSql.Should().Contain("with composed_filter as");
        result.QuerySetup.LeadingSql.Should().Contain("target_route as (");
        result.QuerySetup.LeadingSql.Should().Contain("facet.site_location_shortcut");
        result.QuerySetup.Joins.Should().Contain(join => join.Contains("join target_route on target_route.target_id = tbl_sites.site_id"));
        result
            .QuerySetup.Joins.Should()
            .Contain(join => join.Contains("join composed_filter on composed_filter.target_id = target_route.source_id"));
    }

    [Fact]
    public void Build_WithTargetOnlyArchaeobotanySampleGroupsTabularResult_UsesUnfilteredComposedFilterJoin()
    {
        var builder = Container.Resolve<IResultProjectionHandoffBuilder>();
        var facetsConfig = FakeFacetsConfig("archaeobotany://sample_groups:sample_groups");
        var resultConfig = FakeResultConfig("result_facet", "site_level", "tabular");

        var result = builder.Build(facetsConfig, resultConfig);

        result.QuerySetup.LeadingSql.Should().Contain("with composed_filter as");
        result.QuerySetup.LeadingSql.Should().Contain("from tbl_analysis_entities");
        result
            .QuerySetup.Joins.Should()
            .Contain(join => join.Contains("join composed_filter on composed_filter.target_id = tbl_analysis_entities.analysis_entity_id"));
    }

    [Fact]
    public void Build_WithTargetOnlyArchaeobotanySampleGroupsMapResult_UsesTargetRouteJoin()
    {
        var builder = Container.Resolve<IResultProjectionHandoffBuilder>();
        var facetsConfig = FakeFacetsConfig("archaeobotany://sample_groups:sample_groups");
        var resultConfig = FakeResultConfig("map_result", "map_result", "map");

        var result = builder.Build(facetsConfig, resultConfig);

        result.QuerySetup.LeadingSql.Should().Contain("with composed_filter as");
        result.QuerySetup.LeadingSql.Should().Contain("from tbl_analysis_entities");
        result.QuerySetup.LeadingSql.Should().Contain("target_route as (");
        result.QuerySetup.Joins.Should().Contain(join => join.Contains("join target_route on target_route.target_id = tbl_sites.site_id"));
        result
            .QuerySetup.Joins.Should()
            .Contain(join => join.Contains("join composed_filter on composed_filter.target_id = target_route.source_id"));
    }

    [Fact]
    public void Build_WithTargetOnlyPollenSampleGroupsTabularResult_UsesUnfilteredComposedFilterJoin()
    {
        var builder = Container.Resolve<IResultProjectionHandoffBuilder>();
        var facetsConfig = FakeFacetsConfig("pollen://sample_groups:sample_groups");
        var resultConfig = FakeResultConfig("result_facet", "site_level", "tabular");

        var result = builder.Build(facetsConfig, resultConfig);

        result.QuerySetup.LeadingSql.Should().Contain("with composed_filter as");
        result.QuerySetup.LeadingSql.Should().Contain("from tbl_analysis_entities");
        result
            .QuerySetup.Joins.Should()
            .Contain(join => join.Contains("join composed_filter on composed_filter.target_id = tbl_analysis_entities.analysis_entity_id"));
    }

    [Fact]
    public void Build_WithTargetOnlyPollenSampleGroupsMapResult_UsesTargetRouteJoin()
    {
        var builder = Container.Resolve<IResultProjectionHandoffBuilder>();
        var facetsConfig = FakeFacetsConfig("pollen://sample_groups:sample_groups");
        var resultConfig = FakeResultConfig("map_result", "map_result", "map");

        var result = builder.Build(facetsConfig, resultConfig);

        result.QuerySetup.LeadingSql.Should().Contain("with composed_filter as");
        result.QuerySetup.LeadingSql.Should().Contain("from tbl_analysis_entities");
        result.QuerySetup.LeadingSql.Should().Contain("target_route as (");
        result.QuerySetup.Joins.Should().Contain(join => join.Contains("join target_route on target_route.target_id = tbl_sites.site_id"));
        result
            .QuerySetup.Joins.Should()
            .Contain(join => join.Contains("join composed_filter on composed_filter.target_id = target_route.source_id"));
    }

    [Fact]
    public void Build_WithTargetOnlyGeoarchaeologySampleGroupsTabularResult_UsesUnfilteredComposedFilterJoin()
    {
        var builder = Container.Resolve<IResultProjectionHandoffBuilder>();
        var facetsConfig = FakeFacetsConfig("geoarchaeology://sample_groups:sample_groups");
        var resultConfig = FakeResultConfig("result_facet", "site_level", "tabular");

        var result = builder.Build(facetsConfig, resultConfig);

        result.QuerySetup.LeadingSql.Should().Contain("with composed_filter as");
        result.QuerySetup.LeadingSql.Should().Contain("from tbl_analysis_entities");
        result
            .QuerySetup.Joins.Should()
            .Contain(join => join.Contains("join composed_filter on composed_filter.target_id = tbl_analysis_entities.analysis_entity_id"));
    }

    [Fact]
    public void Build_WithTargetOnlyGeoarchaeologySampleGroupsMapResult_UsesTargetRouteJoin()
    {
        var builder = Container.Resolve<IResultProjectionHandoffBuilder>();
        var facetsConfig = FakeFacetsConfig("geoarchaeology://sample_groups:sample_groups");
        var resultConfig = FakeResultConfig("map_result", "map_result", "map");

        var result = builder.Build(facetsConfig, resultConfig);

        result.QuerySetup.LeadingSql.Should().Contain("with composed_filter as");
        result.QuerySetup.LeadingSql.Should().Contain("from tbl_analysis_entities");
        result.QuerySetup.LeadingSql.Should().Contain("target_route as (");
        result.QuerySetup.Joins.Should().Contain(join => join.Contains("join target_route on target_route.target_id = tbl_sites.site_id"));
        result
            .QuerySetup.Joins.Should()
            .Contain(join => join.Contains("join composed_filter on composed_filter.target_id = target_route.source_id"));
    }

    [Fact]
    public void Build_WithTargetOnlyDendrochronologySampleGroupsTabularResult_UsesUnfilteredComposedFilterJoin()
    {
        var builder = Container.Resolve<IResultProjectionHandoffBuilder>();
        var facetsConfig = FakeFacetsConfig("dendrochronology://sample_groups:sample_groups");
        var resultConfig = FakeResultConfig("result_facet", "site_level", "tabular");

        var result = builder.Build(facetsConfig, resultConfig);

        result.QuerySetup.LeadingSql.Should().Contain("with composed_filter as");
        result.QuerySetup.LeadingSql.Should().Contain("from tbl_analysis_entities");
        result
            .QuerySetup.Joins.Should()
            .Contain(join => join.Contains("join composed_filter on composed_filter.target_id = tbl_analysis_entities.analysis_entity_id"));
    }

    [Fact]
    public void Build_WithTargetOnlyDendrochronologySampleGroupsMapResult_UsesTargetRouteJoin()
    {
        var builder = Container.Resolve<IResultProjectionHandoffBuilder>();
        var facetsConfig = FakeFacetsConfig("dendrochronology://sample_groups:sample_groups");
        var resultConfig = FakeResultConfig("map_result", "map_result", "map");

        var result = builder.Build(facetsConfig, resultConfig);

        result.QuerySetup.LeadingSql.Should().Contain("with composed_filter as");
        result.QuerySetup.LeadingSql.Should().Contain("from tbl_analysis_entities");
        result.QuerySetup.LeadingSql.Should().Contain("target_route as (");
        result.QuerySetup.Joins.Should().Contain(join => join.Contains("join target_route on target_route.target_id = tbl_sites.site_id"));
        result
            .QuerySetup.Joins.Should()
            .Contain(join => join.Contains("join composed_filter on composed_filter.target_id = target_route.source_id"));
    }

    [Fact]
    public void Build_WithTargetOnlyCeramicSampleGroupsTabularResult_UsesUnfilteredComposedFilterJoin()
    {
        var builder = Container.Resolve<IResultProjectionHandoffBuilder>();
        var facetsConfig = FakeFacetsConfig("ceramic://sample_groups:sample_groups");
        var resultConfig = FakeResultConfig("result_facet", "site_level", "tabular");

        var result = builder.Build(facetsConfig, resultConfig);

        result.QuerySetup.LeadingSql.Should().Contain("with composed_filter as");
        result.QuerySetup.LeadingSql.Should().Contain("from tbl_analysis_entities");
        result
            .QuerySetup.Joins.Should()
            .Contain(join => join.Contains("join composed_filter on composed_filter.target_id = tbl_analysis_entities.analysis_entity_id"));
    }

    [Fact]
    public void Build_WithTargetOnlyCeramicSampleGroupsMapResult_UsesTargetRouteJoin()
    {
        var builder = Container.Resolve<IResultProjectionHandoffBuilder>();
        var facetsConfig = FakeFacetsConfig("ceramic://sample_groups:sample_groups");
        var resultConfig = FakeResultConfig("map_result", "map_result", "map");

        var result = builder.Build(facetsConfig, resultConfig);

        result.QuerySetup.LeadingSql.Should().Contain("with composed_filter as");
        result.QuerySetup.LeadingSql.Should().Contain("from tbl_analysis_entities");
        result.QuerySetup.LeadingSql.Should().Contain("target_route as (");
        result.QuerySetup.Joins.Should().Contain(join => join.Contains("join target_route on target_route.target_id = tbl_sites.site_id"));
        result
            .QuerySetup.Joins.Should()
            .Contain(join => join.Contains("join composed_filter on composed_filter.target_id = target_route.source_id"));
    }
}
