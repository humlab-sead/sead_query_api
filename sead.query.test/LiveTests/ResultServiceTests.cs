using System.Linq;
using Autofac;
using SeadQueryAPI.Services;
using SeadQueryCore;
using SeadQueryCore.Model;
using SeadQueryCore.Services.Result;
using SQT.Mocks;
using Xunit;

namespace SQT.LiveServices
{
    [Collection("UsePostgresFixture")]
    public class ResultLoadServiceTests
    {
        public SeadQueryAPI.DependencyService DependencyService { get; private set; }
        public Autofac.IContainer Container { get; private set; }
        public IFacetContext FacetContext { get; private set; }
        public IRepositoryRegistry Registry { get; private set; }

        public ResultLoadServiceTests()
        {
            DependencyService = new SeadQueryAPI.DependencyService() { Options = SettingFactory.DefaultSettings };
            var builder = new Autofac.ContainerBuilder();
            builder.RegisterModule(DependencyService);
            Container = builder.Build();
            FacetContext = Container.Resolve<IFacetContext>();
            Registry = Container.Resolve<IRepositoryRegistry>();
        }

        public FacetsConfig2 FakeFacetsConfig(string uri) => new MockFacetsConfigFactory(Registry.Facets).Create(uri);

        public virtual ResultConfig FakeResultConfig(string facetCode, string specificationKey, string viewTypeId) =>
            ResultConfigFactory.Create(Registry.Facets.GetByCode(facetCode), Registry.Results.GetByKey(specificationKey), viewTypeId);

        [Theory]
        // [InlineData("abundance_classification:abundance_classification", "result_facet", "site_level", "tabular")]
        // [InlineData("isotope://sites:sites", "result_facet", "site_level", "tabular")]
        [InlineData("isotope://sites:sites", "result_facet", "map_result", "map")]
        // [InlineData("genus:genus", "result_facet", "site_level", "tabular")]
        // [InlineData("sites:country@5/sites@4,5", "result_facet", "site_level", "map", 10)]
        // [InlineData("sites:data_types@5/rdb_codes@13,21/sites", "result_facet", "site_level", "map", 10)]
        // [InlineData("sites:data_types@5/rdb_codes@13,21/sites", "result_facet", "site_level", "tabular", 10)]
        public void Load_VariousConfigs_Success(string uri, string resultCode, string aggregateCode, string viewType)
        {
            var fakeFacetsConfig = FakeFacetsConfig(uri);
            var fakeResultConfig = FakeResultConfig(resultCode, aggregateCode, viewType);
            var service = this.Container.Resolve<ILoadResultService>();
            var data = service.Load(fakeFacetsConfig, fakeResultConfig);
            Assert.NotNull(data);
        }

        [Fact]
        public void Load_RangeFilteredTabularResult_UsesComposedFilterSql()
        {
            var fakeFacetsConfig = FakeFacetsConfig("sites:sites@1,2/geochronology@(0,100)");
            var fakeResultConfig = FakeResultConfig("result_facet", "site_level", "tabular");
            var service = Container.Resolve<ILoadResultService>();

            var data = service.Load(fakeFacetsConfig, fakeResultConfig);

            Assert.NotNull(data);
            Assert.NotNull(data.Query);
            Assert.Contains("with composed_filter as", data.Query);
            Assert.Contains("join composed_filter on composed_filter.target_id = tbl_analysis_entities.analysis_entity_id", data.Query);
            Assert.Contains(" >= 0", data.Query);
            Assert.Contains(" <= 100", data.Query);
        }

        [Fact]
        public void Load_CountryFilteredTabularResult_UsesComposedFilterSql()
        {
            var fakeFacetsConfig = FakeFacetsConfig("country:country@57");
            var fakeResultConfig = FakeResultConfig("result_facet", "site_level", "tabular");
            var service = Container.Resolve<ILoadResultService>();

            var data = service.Load(fakeFacetsConfig, fakeResultConfig);

            Assert.NotNull(data);
            Assert.NotNull(data.Query);
            Assert.Contains("with composed_filter as", data.Query);
            Assert.Contains("join composed_filter on composed_filter.target_id = tbl_analysis_entities.analysis_entity_id", data.Query);
        }

        [Fact]
        public void Load_RangeFilteredMapResult_UsesComposedFilterSql()
        {
            var fakeFacetsConfig = FakeFacetsConfig("sites:geochronology@(0,100)/sites");
            var fakeResultConfig = FakeResultConfig("map_result", "map_result", "map");
            var service = Container.Resolve<ILoadResultService>();

            var data = service.Load(fakeFacetsConfig, fakeResultConfig);

            Assert.NotNull(data);
            Assert.NotNull(data.Query);
            Assert.Contains("with composed_filter as", data.Query);
            Assert.Contains("target_route as", data.Query);
            Assert.Contains("join target_route on target_route.target_id = tbl_sites.site_id", data.Query);
            Assert.Contains("join composed_filter on composed_filter.target_id = target_route.source_id", data.Query);
            Assert.Contains(" >= 0", data.Query);
            Assert.Contains(" <= 100", data.Query);
        }

        [Fact]
        public void Load_CountryFilteredMapResult_UsesComposedFilterSql()
        {
            var fakeFacetsConfig = FakeFacetsConfig("country:country@57");
            var fakeResultConfig = FakeResultConfig("map_result", "map_result", "map");
            var service = Container.Resolve<ILoadResultService>();

            var data = service.Load(fakeFacetsConfig, fakeResultConfig);

            Assert.NotNull(data);
            Assert.NotNull(data.Query);
            Assert.Contains("with composed_filter as", data.Query);
            Assert.Contains("target_route as", data.Query);
            Assert.Contains("join target_route on target_route.target_id = tbl_sites.site_id", data.Query);
            Assert.Contains("join composed_filter on composed_filter.target_id = target_route.source_id", data.Query);
        }

        [Fact]
        public void Load_RangeFilteredTabularResult_MatchesLegacyOutput()
        {
            AssertMatchesLegacyResult(
                uri: "sites:sites@1,2/geochronology@(0,100)",
                resultCode: "result_facet",
                specificationKey: "site_level",
                viewType: "tabular"
            );
        }

        [Fact]
        public void Load_CountryFilteredMapResult_MatchesLegacyOutput()
        {
            AssertMatchesLegacyResult(uri: "country:country@57", resultCode: "map_result", specificationKey: "map_result", viewType: "map");
        }

        [Fact]
        public void Load_IntersectFilteredTabularResult_UsesComposedFilterSql()
        {
            var fakeFacetsConfig = FakeFacetsConfig("analysis_entity_ages:analysis_entity_ages@850000,2350000");
            var fakeResultConfig = FakeResultConfig("result_facet", "site_level", "tabular");
            var service = Container.Resolve<ILoadResultService>();

            var data = service.Load(fakeFacetsConfig, fakeResultConfig);

            Assert.NotNull(data);
            Assert.NotNull(data.Query);
            Assert.Contains("with composed_filter as", data.Query);
            Assert.Contains("join composed_filter on composed_filter.target_id = tbl_analysis_entities.analysis_entity_id", data.Query);
            Assert.Contains("int4range(850000, 2350000, '[]')", data.Query);
        }

        [Fact]
        public void Load_GeoPolygonFilteredTabularResult_UsesComposedFilterSql()
        {
            var fakeFacetsConfig = FakeFacetsConfig(
                "sites_polygon:sites_polygon@63.872484,20.093291,63.947006,20.501316,63.878949,20.673213,63.748021,20.252953,63.793983,20.095738"
            );
            var fakeResultConfig = FakeResultConfig("result_facet", "site_level", "tabular");
            var service = Container.Resolve<ILoadResultService>();

            var data = service.Load(fakeFacetsConfig, fakeResultConfig);

            Assert.NotNull(data);
            Assert.NotNull(data.Query);
            Assert.Contains("with composed_filter as", data.Query);
            Assert.Contains("join composed_filter on composed_filter.target_id = tbl_analysis_entities.analysis_entity_id", data.Query);
            Assert.Contains("ST_Within(", data.Query);
        }

        [Fact]
        public void Load_GeoPolygonFilteredMapResult_UsesComposedFilterSql()
        {
            var fakeFacetsConfig = FakeFacetsConfig(
                "sites_polygon:sites_polygon@63.872484,20.093291,63.947006,20.501316,63.878949,20.673213,63.748021,20.252953,63.793983,20.095738"
            );
            var fakeResultConfig = FakeResultConfig("map_result", "map_result", "map");
            var service = Container.Resolve<ILoadResultService>();

            var data = service.Load(fakeFacetsConfig, fakeResultConfig);

            Assert.NotNull(data);
            Assert.NotNull(data.Query);
            Assert.Contains("with composed_filter as", data.Query);
            Assert.Contains("target_route as", data.Query);
            Assert.Contains("join target_route on target_route.target_id = tbl_sites.site_id", data.Query);
            Assert.Contains("join composed_filter on composed_filter.target_id = target_route.source_id", data.Query);
            Assert.Contains("ST_Within(", data.Query);
        }

        [Fact]
        public void Load_IntersectFilteredMapResult_UsesComposedFilterSql()
        {
            var fakeFacetsConfig = FakeFacetsConfig("analysis_entity_ages:analysis_entity_ages@850000,2350000/sites");
            var fakeResultConfig = FakeResultConfig("map_result", "map_result", "map");
            var service = Container.Resolve<ILoadResultService>();

            var data = service.Load(fakeFacetsConfig, fakeResultConfig);

            Assert.NotNull(data);
            Assert.NotNull(data.Query);
            Assert.Contains("with composed_filter as", data.Query);
            Assert.Contains("target_route as", data.Query);
            Assert.Contains("join target_route on target_route.target_id = tbl_sites.site_id", data.Query);
            Assert.Contains("join composed_filter on composed_filter.target_id = target_route.source_id", data.Query);
            Assert.Contains("int4range(850000, 2350000, '[]')", data.Query);
        }

        [Fact]
        public void Load_TargetOnlyGenusTabularResult_UsesComposedFilterSql()
        {
            var fakeFacetsConfig = FakeFacetsConfig("genus:genus");
            var fakeResultConfig = FakeResultConfig("result_facet", "site_level", "tabular");
            var service = Container.Resolve<ILoadResultService>();

            var data = service.Load(fakeFacetsConfig, fakeResultConfig);

            Assert.NotNull(data);
            Assert.NotNull(data.Query);
            Assert.Contains("with composed_filter as", data.Query);
            Assert.Contains("from tbl_analysis_entities", data.Query);
            Assert.Contains("join composed_filter on composed_filter.target_id = tbl_analysis_entities.analysis_entity_id", data.Query);
        }

        [Fact]
        public void Load_TargetOnlyArchaeobotanyGenusTabularResult_UsesComposedFilterSql()
        {
            var fakeFacetsConfig = FakeFacetsConfig("archaeobotany://genus:genus");
            var fakeResultConfig = FakeResultConfig("result_facet", "site_level", "tabular");
            var service = Container.Resolve<ILoadResultService>();

            var data = service.Load(fakeFacetsConfig, fakeResultConfig);

            Assert.NotNull(data);
            Assert.NotNull(data.Query);
            Assert.Contains("with composed_filter as", data.Query);
            Assert.Contains("from tbl_analysis_entities", data.Query);
            Assert.Contains("join composed_filter on composed_filter.target_id = tbl_analysis_entities.analysis_entity_id", data.Query);
        }

        [Fact]
        public void Load_TargetOnlyPollenGenusTabularResult_UsesComposedFilterSql()
        {
            var fakeFacetsConfig = FakeFacetsConfig("pollen://genus:genus");
            var fakeResultConfig = FakeResultConfig("result_facet", "site_level", "tabular");
            var service = Container.Resolve<ILoadResultService>();

            var data = service.Load(fakeFacetsConfig, fakeResultConfig);

            Assert.NotNull(data);
            Assert.NotNull(data.Query);
            Assert.Contains("with composed_filter as", data.Query);
            Assert.Contains("from tbl_analysis_entities", data.Query);
            Assert.Contains("join composed_filter on composed_filter.target_id = tbl_analysis_entities.analysis_entity_id", data.Query);
        }

        [Fact]
        public void Load_TargetOnlyDendrochronologyGenusTabularResult_UsesComposedFilterSql()
        {
            var fakeFacetsConfig = FakeFacetsConfig("dendrochronology://genus:genus");
            var fakeResultConfig = FakeResultConfig("result_facet", "site_level", "tabular");
            var service = Container.Resolve<ILoadResultService>();

            var data = service.Load(fakeFacetsConfig, fakeResultConfig);

            Assert.NotNull(data);
            Assert.NotNull(data.Query);
            Assert.Contains("with composed_filter as", data.Query);
            Assert.Contains("from tbl_analysis_entities", data.Query);
            Assert.Contains("join composed_filter on composed_filter.target_id = tbl_analysis_entities.analysis_entity_id", data.Query);
        }

        [Fact]
        public void Load_TargetOnlyPalaeoentomologyGenusTabularResult_UsesComposedFilterSql()
        {
            var fakeFacetsConfig = FakeFacetsConfig("palaeoentomology://genus:genus");
            var fakeResultConfig = FakeResultConfig("result_facet", "site_level", "tabular");
            var service = Container.Resolve<ILoadResultService>();

            var data = service.Load(fakeFacetsConfig, fakeResultConfig);

            Assert.NotNull(data);
            Assert.NotNull(data.Query);
            Assert.Contains("with composed_filter as", data.Query);
            Assert.Contains("from tbl_analysis_entities", data.Query);
            Assert.Contains("join composed_filter on composed_filter.target_id = tbl_analysis_entities.analysis_entity_id", data.Query);
        }

        [Theory]
        [InlineData("palaeoentomology://feature_type:feature_type")]
        [InlineData("archaeobotany://feature_type:feature_type")]
        [InlineData("pollen://feature_type:feature_type")]
        [InlineData("geoarchaeology://feature_type:feature_type")]
        [InlineData("dendrochronology://feature_type:feature_type")]
        [InlineData("ceramic://feature_type:feature_type")]
        public void Load_TargetOnlyPrefixedFeatureTypeTabularResult_UsesComposedFilterSql(string uri)
        {
            var fakeFacetsConfig = FakeFacetsConfig(uri);
            var fakeResultConfig = FakeResultConfig("result_facet", "site_level", "tabular");
            var service = Container.Resolve<ILoadResultService>();

            var data = service.Load(fakeFacetsConfig, fakeResultConfig);

            Assert.NotNull(data);
            Assert.NotNull(data.Query);
            Assert.Contains("with composed_filter as", data.Query);
            Assert.Contains("from tbl_analysis_entities", data.Query);
            Assert.Contains("join composed_filter on composed_filter.target_id = tbl_analysis_entities.analysis_entity_id", data.Query);
        }

        [Theory]
        [InlineData("palaeoentomology://feature_type:feature_type")]
        [InlineData("archaeobotany://feature_type:feature_type")]
        [InlineData("pollen://feature_type:feature_type")]
        [InlineData("geoarchaeology://feature_type:feature_type")]
        [InlineData("dendrochronology://feature_type:feature_type")]
        [InlineData("ceramic://feature_type:feature_type")]
        public void Load_TargetOnlyPrefixedFeatureTypeTabularResult_MatchesLegacyOutput(string uri)
        {
            AssertMatchesLegacyResult(uri: uri, resultCode: "result_facet", specificationKey: "site_level", viewType: "tabular");
        }

        [Fact]
        public void Load_TargetOnlyPalaeoentomologySampleGroupSamplingContextsTabularResult_UsesComposedFilterSql()
        {
            var fakeFacetsConfig = FakeFacetsConfig("palaeoentomology://sample_group_sampling_contexts:sample_group_sampling_contexts");
            var fakeResultConfig = FakeResultConfig("result_facet", "site_level", "tabular");
            var service = Container.Resolve<ILoadResultService>();

            var data = service.Load(fakeFacetsConfig, fakeResultConfig);

            Assert.NotNull(data);
            Assert.NotNull(data.Query);
            Assert.Contains("with composed_filter as", data.Query);
            Assert.Contains("from tbl_analysis_entities", data.Query);
            Assert.Contains("join composed_filter on composed_filter.target_id = tbl_analysis_entities.analysis_entity_id", data.Query);
        }

        [Fact]
        public void Load_TargetOnlyPalaeoentomologySampleGroupSamplingContextsTabularResult_MatchesLegacyOutput()
        {
            AssertMatchesLegacyResult(
                uri: "palaeoentomology://sample_group_sampling_contexts:sample_group_sampling_contexts",
                resultCode: "result_facet",
                specificationKey: "site_level",
                viewType: "tabular"
            );
        }

        [Fact]
        public void Load_TargetOnlyPalaeoentomologyBiblioModernTabularResult_UsesComposedFilterSql()
        {
            var fakeFacetsConfig = FakeFacetsConfig("palaeoentomology://tbl_biblio_modern:tbl_biblio_modern");
            var fakeResultConfig = FakeResultConfig("result_facet", "site_level", "tabular");
            var service = Container.Resolve<ILoadResultService>();

            var data = service.Load(fakeFacetsConfig, fakeResultConfig);

            Assert.NotNull(data);
            Assert.NotNull(data.Query);
            Assert.Contains("with composed_filter as", data.Query);
            Assert.Contains("from tbl_analysis_entities", data.Query);
            Assert.Contains("join composed_filter on composed_filter.target_id = tbl_analysis_entities.analysis_entity_id", data.Query);
        }

        [Fact]
        public void Load_TargetOnlyPalaeoentomologyBiblioModernTabularResult_MatchesLegacyOutput()
        {
            AssertMatchesLegacyResult(
                uri: "palaeoentomology://tbl_biblio_modern:tbl_biblio_modern",
                resultCode: "result_facet",
                specificationKey: "site_level",
                viewType: "tabular"
            );
        }

        [Fact]
        public void Load_TargetOnlyIsotopeSitesTabularResult_UsesComposedFilterSql()
        {
            var fakeFacetsConfig = FakeFacetsConfig("isotope://sites:sites");
            var fakeResultConfig = FakeResultConfig("result_facet", "site_level", "tabular");
            var service = Container.Resolve<ILoadResultService>();

            var data = service.Load(fakeFacetsConfig, fakeResultConfig);

            Assert.NotNull(data);
            Assert.NotNull(data.Query);
            Assert.Contains("with composed_filter as", data.Query);
            Assert.Contains("from tbl_analysis_entities", data.Query);
            Assert.Contains("join composed_filter on composed_filter.target_id = tbl_analysis_entities.analysis_entity_id", data.Query);
        }

        [Fact]
        public void Load_TargetOnlyIsotopeSitesMapResult_UsesComposedFilterSql()
        {
            var fakeFacetsConfig = FakeFacetsConfig("isotope://sites:sites");
            var fakeResultConfig = FakeResultConfig("map_result", "map_result", "map");
            var service = Container.Resolve<ILoadResultService>();

            var data = service.Load(fakeFacetsConfig, fakeResultConfig);

            Assert.NotNull(data);
            Assert.NotNull(data.Query);
            Assert.Contains("with composed_filter as", data.Query);
            Assert.Contains("target_route as", data.Query);
            Assert.Contains("join target_route on target_route.target_id = tbl_sites.site_id", data.Query);
            Assert.Contains("join composed_filter on composed_filter.target_id = target_route.source_id", data.Query);
        }

        [Fact]
        public void Load_TargetOnlySitesMapResult_UsesComposedFilterSql()
        {
            var fakeFacetsConfig = FakeFacetsConfig("sites:sites");
            var fakeResultConfig = FakeResultConfig("map_result", "map_result", "map");
            var service = Container.Resolve<ILoadResultService>();

            var data = service.Load(fakeFacetsConfig, fakeResultConfig);

            Assert.NotNull(data);
            Assert.NotNull(data.Query);
            Assert.Contains("with composed_filter as", data.Query);
            Assert.Contains("target_route as", data.Query);
            Assert.Contains("join target_route on target_route.target_id = tbl_sites.site_id", data.Query);
            Assert.Contains("join composed_filter on composed_filter.target_id = target_route.source_id", data.Query);
        }

        [Fact]
        public void Load_TargetOnlyPollenSitesMapResult_UsesComposedFilterSql()
        {
            var fakeFacetsConfig = FakeFacetsConfig("pollen://sites:sites");
            var fakeResultConfig = FakeResultConfig("map_result", "map_result", "map");
            var service = Container.Resolve<ILoadResultService>();

            var data = service.Load(fakeFacetsConfig, fakeResultConfig);

            Assert.NotNull(data);
            Assert.NotNull(data.Query);
            Assert.Contains("with composed_filter as", data.Query);
            Assert.Contains("target_route as", data.Query);
            Assert.Contains("join target_route on target_route.target_id = tbl_sites.site_id", data.Query);
            Assert.Contains("join composed_filter on composed_filter.target_id = target_route.source_id", data.Query);
        }

        [Fact]
        public void Load_TargetOnlyCeramicSitesMapResult_UsesComposedFilterSql()
        {
            var fakeFacetsConfig = FakeFacetsConfig("ceramic://sites:sites");
            var fakeResultConfig = FakeResultConfig("map_result", "map_result", "map");
            var service = Container.Resolve<ILoadResultService>();

            var data = service.Load(fakeFacetsConfig, fakeResultConfig);

            Assert.NotNull(data);
            Assert.NotNull(data.Query);
            Assert.Contains("with composed_filter as", data.Query);
            Assert.Contains("target_route as", data.Query);
            Assert.Contains("join target_route on target_route.target_id = tbl_sites.site_id", data.Query);
            Assert.Contains("join composed_filter on composed_filter.target_id = target_route.source_id", data.Query);
        }

        [Fact]
        public void Load_TargetOnlyGeoarchaeologySitesMapResult_UsesComposedFilterSql()
        {
            var fakeFacetsConfig = FakeFacetsConfig("geoarchaeology://sites:sites");
            var fakeResultConfig = FakeResultConfig("map_result", "map_result", "map");
            var service = Container.Resolve<ILoadResultService>();

            var data = service.Load(fakeFacetsConfig, fakeResultConfig);

            Assert.NotNull(data);
            Assert.NotNull(data.Query);
            Assert.Contains("with composed_filter as", data.Query);
            Assert.Contains("target_route as", data.Query);
            Assert.Contains("join target_route on target_route.target_id = tbl_sites.site_id", data.Query);
            Assert.Contains("join composed_filter on composed_filter.target_id = target_route.source_id", data.Query);
        }

        [Fact]
        public void Load_TargetOnlyDendrochronologySitesMapResult_UsesComposedFilterSql()
        {
            var fakeFacetsConfig = FakeFacetsConfig("dendrochronology://sites:sites");
            var fakeResultConfig = FakeResultConfig("map_result", "map_result", "map");
            var service = Container.Resolve<ILoadResultService>();

            var data = service.Load(fakeFacetsConfig, fakeResultConfig);

            Assert.NotNull(data);
            Assert.NotNull(data.Query);
            Assert.Contains("with composed_filter as", data.Query);
            Assert.Contains("target_route as", data.Query);
            Assert.Contains("join target_route on target_route.target_id = tbl_sites.site_id", data.Query);
            Assert.Contains("join composed_filter on composed_filter.target_id = target_route.source_id", data.Query);
        }

        [Fact]
        public void Load_TargetOnlyArchaeobotanySitesMapResult_UsesComposedFilterSql()
        {
            var fakeFacetsConfig = FakeFacetsConfig("archaeobotany://sites:sites");
            var fakeResultConfig = FakeResultConfig("map_result", "map_result", "map");
            var service = Container.Resolve<ILoadResultService>();

            var data = service.Load(fakeFacetsConfig, fakeResultConfig);

            Assert.NotNull(data);
            Assert.NotNull(data.Query);
            Assert.Contains("with composed_filter as", data.Query);
            Assert.Contains("target_route as", data.Query);
            Assert.Contains("join target_route on target_route.target_id = tbl_sites.site_id", data.Query);
            Assert.Contains("join composed_filter on composed_filter.target_id = target_route.source_id", data.Query);
        }

        [Fact]
        public void Load_TargetOnlyPalaeoentomologySitesMapResult_UsesComposedFilterSql()
        {
            var fakeFacetsConfig = FakeFacetsConfig("palaeoentomology://sites:sites");
            var fakeResultConfig = FakeResultConfig("map_result", "map_result", "map");
            var service = Container.Resolve<ILoadResultService>();

            var data = service.Load(fakeFacetsConfig, fakeResultConfig);

            Assert.NotNull(data);
            Assert.NotNull(data.Query);
            Assert.Contains("with composed_filter as", data.Query);
            Assert.Contains("target_route as", data.Query);
            Assert.Contains("join target_route on target_route.target_id = tbl_sites.site_id", data.Query);
            Assert.Contains("join composed_filter on composed_filter.target_id = target_route.source_id", data.Query);
        }

        [Fact]
        public void Load_TargetOnlyPalaeoentomologyDataTypesTabularResult_UsesComposedFilterSql()
        {
            var fakeFacetsConfig = FakeFacetsConfig("palaeoentomology://data_types:data_types");
            var fakeResultConfig = FakeResultConfig("result_facet", "site_level", "tabular");
            var service = Container.Resolve<ILoadResultService>();

            var data = service.Load(fakeFacetsConfig, fakeResultConfig);

            Assert.NotNull(data);
            Assert.NotNull(data.Query);
            Assert.Contains("with composed_filter as", data.Query);
            Assert.Contains("from tbl_analysis_entities", data.Query);
            Assert.Contains("join composed_filter on composed_filter.target_id = tbl_analysis_entities.analysis_entity_id", data.Query);
        }

        [Fact]
        public void Load_TargetOnlyPalaeoentomologySampleGroupsTabularResult_UsesComposedFilterSql()
        {
            var fakeFacetsConfig = FakeFacetsConfig("palaeoentomology://sample_groups:sample_groups");
            var fakeResultConfig = FakeResultConfig("result_facet", "site_level", "tabular");
            var service = Container.Resolve<ILoadResultService>();

            var data = service.Load(fakeFacetsConfig, fakeResultConfig);

            Assert.NotNull(data);
            Assert.NotNull(data.Query);
            Assert.Contains("with composed_filter as", data.Query);
            Assert.Contains("from tbl_analysis_entities", data.Query);
            Assert.Contains("join composed_filter on composed_filter.target_id = tbl_analysis_entities.analysis_entity_id", data.Query);
        }

        [Fact]
        public void Load_TargetOnlyPalaeoentomologySampleGroupsMapResult_UsesComposedFilterSql()
        {
            var fakeFacetsConfig = FakeFacetsConfig("palaeoentomology://sample_groups:sample_groups");
            var fakeResultConfig = FakeResultConfig("map_result", "map_result", "map");
            var service = Container.Resolve<ILoadResultService>();

            var data = service.Load(fakeFacetsConfig, fakeResultConfig);

            Assert.NotNull(data);
            Assert.NotNull(data.Query);
            Assert.Contains("with composed_filter as", data.Query);
            Assert.Contains("target_route as", data.Query);
            Assert.Contains("join target_route on target_route.target_id = tbl_sites.site_id", data.Query);
            Assert.Contains("join composed_filter on composed_filter.target_id = target_route.source_id", data.Query);
        }

        [Fact]
        public void Load_TargetOnlyArchaeobotanyDataTypesTabularResult_UsesComposedFilterSql()
        {
            var fakeFacetsConfig = FakeFacetsConfig("archaeobotany://data_types:data_types");
            var fakeResultConfig = FakeResultConfig("result_facet", "site_level", "tabular");
            var service = Container.Resolve<ILoadResultService>();

            var data = service.Load(fakeFacetsConfig, fakeResultConfig);

            Assert.NotNull(data);
            Assert.NotNull(data.Query);
            Assert.Contains("with composed_filter as", data.Query);
            Assert.Contains("from tbl_analysis_entities", data.Query);
            Assert.Contains("join composed_filter on composed_filter.target_id = tbl_analysis_entities.analysis_entity_id", data.Query);
        }

        [Fact]
        public void Load_TargetOnlyPalaeoentomologyRdbCodesTabularResult_UsesComposedFilterSql()
        {
            var fakeFacetsConfig = FakeFacetsConfig("palaeoentomology://rdb_codes:rdb_codes");
            var fakeResultConfig = FakeResultConfig("result_facet", "site_level", "tabular");
            var service = Container.Resolve<ILoadResultService>();

            var data = service.Load(fakeFacetsConfig, fakeResultConfig);

            Assert.NotNull(data);
            Assert.NotNull(data.Query);
            Assert.Contains("with composed_filter as", data.Query);
            Assert.Contains("from tbl_analysis_entities", data.Query);
            Assert.Contains("join composed_filter on composed_filter.target_id = tbl_analysis_entities.analysis_entity_id", data.Query);
        }

        [Fact]
        public void Load_TargetOnlyPalaeoentomologyRdbCodesMapResult_UsesComposedFilterSql()
        {
            var fakeFacetsConfig = FakeFacetsConfig("palaeoentomology://rdb_codes:rdb_codes");
            var fakeResultConfig = FakeResultConfig("map_result", "map_result", "map");
            var service = Container.Resolve<ILoadResultService>();

            var data = service.Load(fakeFacetsConfig, fakeResultConfig);

            Assert.NotNull(data);
            Assert.NotNull(data.Query);
            Assert.Contains("with composed_filter as", data.Query);
            Assert.Contains("target_route as", data.Query);
            Assert.Contains("join target_route on target_route.target_id = tbl_sites.site_id", data.Query);
            Assert.Contains("join composed_filter on composed_filter.target_id = target_route.source_id", data.Query);
        }

        [Fact]
        public void Load_TargetOnlyPollenDataTypesTabularResult_UsesComposedFilterSql()
        {
            var fakeFacetsConfig = FakeFacetsConfig("pollen://data_types:data_types");
            var fakeResultConfig = FakeResultConfig("result_facet", "site_level", "tabular");
            var service = Container.Resolve<ILoadResultService>();

            var data = service.Load(fakeFacetsConfig, fakeResultConfig);

            Assert.NotNull(data);
            Assert.NotNull(data.Query);
            Assert.Contains("with composed_filter as", data.Query);
            Assert.Contains("from tbl_analysis_entities", data.Query);
            Assert.Contains("join composed_filter on composed_filter.target_id = tbl_analysis_entities.analysis_entity_id", data.Query);
        }

        [Fact]
        public void Load_TargetOnlyAbundanceClassificationTabularResult_UsesComposedFilterSql()
        {
            var fakeFacetsConfig = FakeFacetsConfig("abundance_classification:abundance_classification");
            var fakeResultConfig = FakeResultConfig("result_facet", "site_level", "tabular");
            var service = Container.Resolve<ILoadResultService>();

            var data = service.Load(fakeFacetsConfig, fakeResultConfig);

            Assert.NotNull(data);
            Assert.NotNull(data.Query);
            Assert.Contains("with composed_filter as", data.Query);
            Assert.Contains("from tbl_analysis_entities", data.Query);
            Assert.Contains("join composed_filter on composed_filter.target_id = tbl_analysis_entities.analysis_entity_id", data.Query);
        }

        [Fact]
        public void Load_TargetOnlyGeoarchaeologyDataTypesTabularResult_UsesComposedFilterSql()
        {
            var fakeFacetsConfig = FakeFacetsConfig("geoarchaeology://data_types:data_types");
            var fakeResultConfig = FakeResultConfig("result_facet", "site_level", "tabular");
            var service = Container.Resolve<ILoadResultService>();

            var data = service.Load(fakeFacetsConfig, fakeResultConfig);

            Assert.NotNull(data);
            Assert.NotNull(data.Query);
            Assert.Contains("with composed_filter as", data.Query);
            Assert.Contains("from tbl_analysis_entities", data.Query);
            Assert.Contains("join composed_filter on composed_filter.target_id = tbl_analysis_entities.analysis_entity_id", data.Query);
        }

        [Fact]
        public void Load_TargetOnlyDendrochronologyDataTypesTabularResult_UsesComposedFilterSql()
        {
            var fakeFacetsConfig = FakeFacetsConfig("dendrochronology://data_types:data_types");
            var fakeResultConfig = FakeResultConfig("result_facet", "site_level", "tabular");
            var service = Container.Resolve<ILoadResultService>();

            var data = service.Load(fakeFacetsConfig, fakeResultConfig);

            Assert.NotNull(data);
            Assert.NotNull(data.Query);
            Assert.Contains("with composed_filter as", data.Query);
            Assert.Contains("from tbl_analysis_entities", data.Query);
            Assert.Contains("join composed_filter on composed_filter.target_id = tbl_analysis_entities.analysis_entity_id", data.Query);
        }

        [Fact]
        public void Load_TargetOnlyCeramicDataTypesTabularResult_UsesComposedFilterSql()
        {
            var fakeFacetsConfig = FakeFacetsConfig("ceramic://data_types:data_types");
            var fakeResultConfig = FakeResultConfig("result_facet", "site_level", "tabular");
            var service = Container.Resolve<ILoadResultService>();

            var data = service.Load(fakeFacetsConfig, fakeResultConfig);

            Assert.NotNull(data);
            Assert.NotNull(data.Query);
            Assert.Contains("with composed_filter as", data.Query);
            Assert.Contains("from tbl_analysis_entities", data.Query);
            Assert.Contains("join composed_filter on composed_filter.target_id = tbl_analysis_entities.analysis_entity_id", data.Query);
        }

        [Fact]
        public void Load_TargetOnlyPalaeoentomologyRdbSystemsTabularResult_UsesComposedFilterSql()
        {
            var fakeFacetsConfig = FakeFacetsConfig("palaeoentomology://rdb_systems:rdb_systems");
            var fakeResultConfig = FakeResultConfig("result_facet", "site_level", "tabular");
            var service = Container.Resolve<ILoadResultService>();

            var data = service.Load(fakeFacetsConfig, fakeResultConfig);

            Assert.NotNull(data);
            Assert.NotNull(data.Query);
            Assert.Contains("with composed_filter as", data.Query);
            Assert.Contains("from tbl_analysis_entities", data.Query);
            Assert.Contains("join composed_filter on composed_filter.target_id = tbl_analysis_entities.analysis_entity_id", data.Query);
        }

        [Fact]
        public void Load_TargetOnlyPalaeoentomologyRdbSystemsMapResult_UsesComposedFilterSql()
        {
            var fakeFacetsConfig = FakeFacetsConfig("palaeoentomology://rdb_systems:rdb_systems");
            var fakeResultConfig = FakeResultConfig("map_result", "map_result", "map");
            var service = Container.Resolve<ILoadResultService>();

            var data = service.Load(fakeFacetsConfig, fakeResultConfig);

            Assert.NotNull(data);
            Assert.NotNull(data.Query);
            Assert.Contains("with composed_filter as", data.Query);
            Assert.Contains("target_route as", data.Query);
            Assert.Contains("join target_route on target_route.target_id = tbl_sites.site_id", data.Query);
            Assert.Contains("join composed_filter on composed_filter.target_id = target_route.source_id", data.Query);
        }

        [Fact]
        public void Load_TargetOnlyPalaeoentomologyRdbSystemsMapResult_MatchesLegacyOutput()
        {
            AssertMatchesLegacyResult(
                uri: "palaeoentomology://rdb_systems:rdb_systems",
                resultCode: "map_result",
                specificationKey: "map_result",
                viewType: "map",
                ignoreRowOrder: true
            );
        }

        [Fact]
        public void Load_TargetOnlyPalaeoentomologyCountryTabularResult_UsesComposedFilterSql()
        {
            var fakeFacetsConfig = FakeFacetsConfig("palaeoentomology://country:country");
            var fakeResultConfig = FakeResultConfig("result_facet", "site_level", "tabular");
            var service = Container.Resolve<ILoadResultService>();

            var data = service.Load(fakeFacetsConfig, fakeResultConfig);

            Assert.NotNull(data);
            Assert.NotNull(data.Query);
            Assert.Contains("with composed_filter as", data.Query);
            Assert.Contains("facet.site_location_shortcut", data.Query);
            Assert.Contains("join composed_filter on composed_filter.target_id = tbl_analysis_entities.analysis_entity_id", data.Query);
        }

        [Fact]
        public void Load_TargetOnlyPalaeoentomologyCountryMapResult_UsesComposedFilterSql()
        {
            var fakeFacetsConfig = FakeFacetsConfig("palaeoentomology://country:country");
            var fakeResultConfig = FakeResultConfig("map_result", "map_result", "map");
            var service = Container.Resolve<ILoadResultService>();

            var data = service.Load(fakeFacetsConfig, fakeResultConfig);

            Assert.NotNull(data);
            Assert.NotNull(data.Query);
            Assert.Contains("with composed_filter as", data.Query);
            Assert.Contains("target_route as", data.Query);
            Assert.Contains("facet.site_location_shortcut", data.Query);
            Assert.Contains("join target_route on target_route.target_id = tbl_sites.site_id", data.Query);
            Assert.Contains("join composed_filter on composed_filter.target_id = target_route.source_id", data.Query);
        }

        [Fact]
        public void Load_TargetOnlyDendrochronologyCountryTabularResult_UsesComposedFilterSql()
        {
            var fakeFacetsConfig = FakeFacetsConfig("dendrochronology://country:country");
            var fakeResultConfig = FakeResultConfig("result_facet", "site_level", "tabular");
            var service = Container.Resolve<ILoadResultService>();

            var data = service.Load(fakeFacetsConfig, fakeResultConfig);

            Assert.NotNull(data);
            Assert.NotNull(data.Query);
            Assert.Contains("with composed_filter as", data.Query);
            Assert.Contains("facet.site_location_shortcut", data.Query);
            Assert.Contains("join composed_filter on composed_filter.target_id = tbl_analysis_entities.analysis_entity_id", data.Query);
        }

        [Fact]
        public void Load_TargetOnlyDendrochronologyCountryMapResult_UsesComposedFilterSql()
        {
            var fakeFacetsConfig = FakeFacetsConfig("dendrochronology://country:country");
            var fakeResultConfig = FakeResultConfig("map_result", "map_result", "map");
            var service = Container.Resolve<ILoadResultService>();

            var data = service.Load(fakeFacetsConfig, fakeResultConfig);

            Assert.NotNull(data);
            Assert.NotNull(data.Query);
            Assert.Contains("with composed_filter as", data.Query);
            Assert.Contains("target_route as", data.Query);
            Assert.Contains("facet.site_location_shortcut", data.Query);
            Assert.Contains("join target_route on target_route.target_id = tbl_sites.site_id", data.Query);
            Assert.Contains("join composed_filter on composed_filter.target_id = target_route.source_id", data.Query);
        }

        [Fact]
        public void Load_TargetOnlyCeramicCountryTabularResult_UsesComposedFilterSql()
        {
            var fakeFacetsConfig = FakeFacetsConfig("ceramic://country:country");
            var fakeResultConfig = FakeResultConfig("result_facet", "site_level", "tabular");
            var service = Container.Resolve<ILoadResultService>();

            var data = service.Load(fakeFacetsConfig, fakeResultConfig);

            Assert.NotNull(data);
            Assert.NotNull(data.Query);
            Assert.Contains("with composed_filter as", data.Query);
            Assert.Contains("facet.site_location_shortcut", data.Query);
            Assert.Contains("join composed_filter on composed_filter.target_id = tbl_analysis_entities.analysis_entity_id", data.Query);
        }

        [Fact]
        public void Load_TargetOnlyCeramicCountryMapResult_UsesComposedFilterSql()
        {
            var fakeFacetsConfig = FakeFacetsConfig("ceramic://country:country");
            var fakeResultConfig = FakeResultConfig("map_result", "map_result", "map");
            var service = Container.Resolve<ILoadResultService>();

            var data = service.Load(fakeFacetsConfig, fakeResultConfig);

            Assert.NotNull(data);
            Assert.NotNull(data.Query);
            Assert.Contains("with composed_filter as", data.Query);
            Assert.Contains("target_route as", data.Query);
            Assert.Contains("facet.site_location_shortcut", data.Query);
            Assert.Contains("join target_route on target_route.target_id = tbl_sites.site_id", data.Query);
            Assert.Contains("join composed_filter on composed_filter.target_id = target_route.source_id", data.Query);
        }

        [Fact]
        public void Load_TargetOnlyArchaeobotanyCountryTabularResult_UsesComposedFilterSql()
        {
            var fakeFacetsConfig = FakeFacetsConfig("archaeobotany://country:country");
            var fakeResultConfig = FakeResultConfig("result_facet", "site_level", "tabular");
            var service = Container.Resolve<ILoadResultService>();

            var data = service.Load(fakeFacetsConfig, fakeResultConfig);

            Assert.NotNull(data);
            Assert.NotNull(data.Query);
            Assert.Contains("with composed_filter as", data.Query);
            Assert.Contains("facet.site_location_shortcut", data.Query);
            Assert.Contains("join composed_filter on composed_filter.target_id = tbl_analysis_entities.analysis_entity_id", data.Query);
        }

        [Fact]
        public void Load_TargetOnlyArchaeobotanyCountryMapResult_UsesComposedFilterSql()
        {
            var fakeFacetsConfig = FakeFacetsConfig("archaeobotany://country:country");
            var fakeResultConfig = FakeResultConfig("map_result", "map_result", "map");
            var service = Container.Resolve<ILoadResultService>();

            var data = service.Load(fakeFacetsConfig, fakeResultConfig);

            Assert.NotNull(data);
            Assert.NotNull(data.Query);
            Assert.Contains("with composed_filter as", data.Query);
            Assert.Contains("target_route as", data.Query);
            Assert.Contains("facet.site_location_shortcut", data.Query);
            Assert.Contains("join target_route on target_route.target_id = tbl_sites.site_id", data.Query);
            Assert.Contains("join composed_filter on composed_filter.target_id = target_route.source_id", data.Query);
        }

        [Fact]
        public void Load_TargetOnlyPollenCountryTabularResult_UsesComposedFilterSql()
        {
            var fakeFacetsConfig = FakeFacetsConfig("pollen://country:country");
            var fakeResultConfig = FakeResultConfig("result_facet", "site_level", "tabular");
            var service = Container.Resolve<ILoadResultService>();

            var data = service.Load(fakeFacetsConfig, fakeResultConfig);

            Assert.NotNull(data);
            Assert.NotNull(data.Query);
            Assert.Contains("with composed_filter as", data.Query);
            Assert.Contains("facet.site_location_shortcut", data.Query);
            Assert.Contains("join composed_filter on composed_filter.target_id = tbl_analysis_entities.analysis_entity_id", data.Query);
        }

        [Fact]
        public void Load_TargetOnlyPollenCountryMapResult_UsesComposedFilterSql()
        {
            var fakeFacetsConfig = FakeFacetsConfig("pollen://country:country");
            var fakeResultConfig = FakeResultConfig("map_result", "map_result", "map");
            var service = Container.Resolve<ILoadResultService>();

            var data = service.Load(fakeFacetsConfig, fakeResultConfig);

            Assert.NotNull(data);
            Assert.NotNull(data.Query);
            Assert.Contains("with composed_filter as", data.Query);
            Assert.Contains("target_route as", data.Query);
            Assert.Contains("facet.site_location_shortcut", data.Query);
            Assert.Contains("join target_route on target_route.target_id = tbl_sites.site_id", data.Query);
            Assert.Contains("join composed_filter on composed_filter.target_id = target_route.source_id", data.Query);
        }

        [Fact]
        public void Load_TargetOnlyGeoarchaeologyCountryTabularResult_UsesComposedFilterSql()
        {
            var fakeFacetsConfig = FakeFacetsConfig("geoarchaeology://country:country");
            var fakeResultConfig = FakeResultConfig("result_facet", "site_level", "tabular");
            var service = Container.Resolve<ILoadResultService>();

            var data = service.Load(fakeFacetsConfig, fakeResultConfig);

            Assert.NotNull(data);
            Assert.NotNull(data.Query);
            Assert.Contains("with composed_filter as", data.Query);
            Assert.Contains("facet.site_location_shortcut", data.Query);
            Assert.Contains("join composed_filter on composed_filter.target_id = tbl_analysis_entities.analysis_entity_id", data.Query);
        }

        [Fact]
        public void Load_TargetOnlyGeoarchaeologyCountryMapResult_UsesComposedFilterSql()
        {
            var fakeFacetsConfig = FakeFacetsConfig("geoarchaeology://country:country");
            var fakeResultConfig = FakeResultConfig("map_result", "map_result", "map");
            var service = Container.Resolve<ILoadResultService>();

            var data = service.Load(fakeFacetsConfig, fakeResultConfig);

            Assert.NotNull(data);
            Assert.NotNull(data.Query);
            Assert.Contains("with composed_filter as", data.Query);
            Assert.Contains("target_route as", data.Query);
            Assert.Contains("facet.site_location_shortcut", data.Query);
            Assert.Contains("join target_route on target_route.target_id = tbl_sites.site_id", data.Query);
            Assert.Contains("join composed_filter on composed_filter.target_id = target_route.source_id", data.Query);
        }

        [Fact]
        public void Load_TargetOnlyArchaeobotanySampleGroupsTabularResult_UsesComposedFilterSql()
        {
            var fakeFacetsConfig = FakeFacetsConfig("archaeobotany://sample_groups:sample_groups");
            var fakeResultConfig = FakeResultConfig("result_facet", "site_level", "tabular");
            var service = Container.Resolve<ILoadResultService>();

            var data = service.Load(fakeFacetsConfig, fakeResultConfig);

            Assert.NotNull(data);
            Assert.NotNull(data.Query);
            Assert.Contains("with composed_filter as", data.Query);
            Assert.Contains("from tbl_analysis_entities", data.Query);
            Assert.Contains("join composed_filter on composed_filter.target_id = tbl_analysis_entities.analysis_entity_id", data.Query);
        }

        [Fact]
        public void Load_TargetOnlyArchaeobotanySampleGroupsMapResult_UsesComposedFilterSql()
        {
            var fakeFacetsConfig = FakeFacetsConfig("archaeobotany://sample_groups:sample_groups");
            var fakeResultConfig = FakeResultConfig("map_result", "map_result", "map");
            var service = Container.Resolve<ILoadResultService>();

            var data = service.Load(fakeFacetsConfig, fakeResultConfig);

            Assert.NotNull(data);
            Assert.NotNull(data.Query);
            Assert.Contains("with composed_filter as", data.Query);
            Assert.Contains("from tbl_analysis_entities", data.Query);
            Assert.Contains("target_route as", data.Query);
            Assert.Contains("join target_route on target_route.target_id = tbl_sites.site_id", data.Query);
            Assert.Contains("join composed_filter on composed_filter.target_id = target_route.source_id", data.Query);
        }

        [Fact]
        public void Load_TargetOnlyPollenSampleGroupsTabularResult_UsesComposedFilterSql()
        {
            var fakeFacetsConfig = FakeFacetsConfig("pollen://sample_groups:sample_groups");
            var fakeResultConfig = FakeResultConfig("result_facet", "site_level", "tabular");
            var service = Container.Resolve<ILoadResultService>();

            var data = service.Load(fakeFacetsConfig, fakeResultConfig);

            Assert.NotNull(data);
            Assert.NotNull(data.Query);
            Assert.Contains("with composed_filter as", data.Query);
            Assert.Contains("from tbl_analysis_entities", data.Query);
            Assert.Contains("join composed_filter on composed_filter.target_id = tbl_analysis_entities.analysis_entity_id", data.Query);
        }

        [Fact]
        public void Load_TargetOnlyPollenSampleGroupsMapResult_UsesComposedFilterSql()
        {
            var fakeFacetsConfig = FakeFacetsConfig("pollen://sample_groups:sample_groups");
            var fakeResultConfig = FakeResultConfig("map_result", "map_result", "map");
            var service = Container.Resolve<ILoadResultService>();

            var data = service.Load(fakeFacetsConfig, fakeResultConfig);

            Assert.NotNull(data);
            Assert.NotNull(data.Query);
            Assert.Contains("with composed_filter as", data.Query);
            Assert.Contains("from tbl_analysis_entities", data.Query);
            Assert.Contains("target_route as", data.Query);
            Assert.Contains("join target_route on target_route.target_id = tbl_sites.site_id", data.Query);
            Assert.Contains("join composed_filter on composed_filter.target_id = target_route.source_id", data.Query);
        }

        [Fact]
        public void Load_TargetOnlyGeoarchaeologySampleGroupsTabularResult_UsesComposedFilterSql()
        {
            var fakeFacetsConfig = FakeFacetsConfig("geoarchaeology://sample_groups:sample_groups");
            var fakeResultConfig = FakeResultConfig("result_facet", "site_level", "tabular");
            var service = Container.Resolve<ILoadResultService>();

            var data = service.Load(fakeFacetsConfig, fakeResultConfig);

            Assert.NotNull(data);
            Assert.NotNull(data.Query);
            Assert.Contains("with composed_filter as", data.Query);
            Assert.Contains("from tbl_analysis_entities", data.Query);
            Assert.Contains("join composed_filter on composed_filter.target_id = tbl_analysis_entities.analysis_entity_id", data.Query);
        }

        [Fact]
        public void Load_TargetOnlyGeoarchaeologySampleGroupsMapResult_UsesComposedFilterSql()
        {
            var fakeFacetsConfig = FakeFacetsConfig("geoarchaeology://sample_groups:sample_groups");
            var fakeResultConfig = FakeResultConfig("map_result", "map_result", "map");
            var service = Container.Resolve<ILoadResultService>();

            var data = service.Load(fakeFacetsConfig, fakeResultConfig);

            Assert.NotNull(data);
            Assert.NotNull(data.Query);
            Assert.Contains("with composed_filter as", data.Query);
            Assert.Contains("from tbl_analysis_entities", data.Query);
            Assert.Contains("target_route as", data.Query);
            Assert.Contains("join target_route on target_route.target_id = tbl_sites.site_id", data.Query);
            Assert.Contains("join composed_filter on composed_filter.target_id = target_route.source_id", data.Query);
        }

        [Fact]
        public void Load_TargetOnlyDendrochronologySampleGroupsTabularResult_UsesComposedFilterSql()
        {
            var fakeFacetsConfig = FakeFacetsConfig("dendrochronology://sample_groups:sample_groups");
            var fakeResultConfig = FakeResultConfig("result_facet", "site_level", "tabular");
            var service = Container.Resolve<ILoadResultService>();

            var data = service.Load(fakeFacetsConfig, fakeResultConfig);

            Assert.NotNull(data);
            Assert.NotNull(data.Query);
            Assert.Contains("with composed_filter as", data.Query);
            Assert.Contains("from tbl_analysis_entities", data.Query);
            Assert.Contains("join composed_filter on composed_filter.target_id = tbl_analysis_entities.analysis_entity_id", data.Query);
        }

        [Fact]
        public void Load_TargetOnlyDendrochronologySampleGroupsMapResult_UsesComposedFilterSql()
        {
            var fakeFacetsConfig = FakeFacetsConfig("dendrochronology://sample_groups:sample_groups");
            var fakeResultConfig = FakeResultConfig("map_result", "map_result", "map");
            var service = Container.Resolve<ILoadResultService>();

            var data = service.Load(fakeFacetsConfig, fakeResultConfig);

            Assert.NotNull(data);
            Assert.NotNull(data.Query);
            Assert.Contains("with composed_filter as", data.Query);
            Assert.Contains("from tbl_analysis_entities", data.Query);
            Assert.Contains("target_route as", data.Query);
            Assert.Contains("join target_route on target_route.target_id = tbl_sites.site_id", data.Query);
            Assert.Contains("join composed_filter on composed_filter.target_id = target_route.source_id", data.Query);
        }

        [Fact]
        public void Load_TargetOnlyCeramicSampleGroupsTabularResult_UsesComposedFilterSql()
        {
            var fakeFacetsConfig = FakeFacetsConfig("ceramic://sample_groups:sample_groups");
            var fakeResultConfig = FakeResultConfig("result_facet", "site_level", "tabular");
            var service = Container.Resolve<ILoadResultService>();

            var data = service.Load(fakeFacetsConfig, fakeResultConfig);

            Assert.NotNull(data);
            Assert.NotNull(data.Query);
            Assert.Contains("with composed_filter as", data.Query);
            Assert.Contains("from tbl_analysis_entities", data.Query);
            Assert.Contains("join composed_filter on composed_filter.target_id = tbl_analysis_entities.analysis_entity_id", data.Query);
        }

        [Fact]
        public void Load_TargetOnlyCeramicSampleGroupsMapResult_UsesComposedFilterSql()
        {
            var fakeFacetsConfig = FakeFacetsConfig("ceramic://sample_groups:sample_groups");
            var fakeResultConfig = FakeResultConfig("map_result", "map_result", "map");
            var service = Container.Resolve<ILoadResultService>();

            var data = service.Load(fakeFacetsConfig, fakeResultConfig);

            Assert.NotNull(data);
            Assert.NotNull(data.Query);
            Assert.Contains("with composed_filter as", data.Query);
            Assert.Contains("from tbl_analysis_entities", data.Query);
            Assert.Contains("target_route as", data.Query);
            Assert.Contains("join target_route on target_route.target_id = tbl_sites.site_id", data.Query);
            Assert.Contains("join composed_filter on composed_filter.target_id = target_route.source_id", data.Query);
        }

        private IContainer CreateLegacyResultContainer()
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule(new SeadQueryAPI.DependencyService { Options = SettingFactory.DefaultSettings });
            builder.RegisterType<LegacyResultProjectionHandoffBuilder>().As<IResultProjectionHandoffBuilder>();
            return builder.Build();
        }

        private void AssertMatchesLegacyResult(
            string uri,
            string resultCode,
            string specificationKey,
            string viewType,
            bool ignoreRowOrder = false
        )
        {
            var facetsConfig = FakeFacetsConfig(uri);
            var resultConfig = FakeResultConfig(resultCode, specificationKey, viewType);
            var composedService = Container.Resolve<ILoadResultService>();

            using var legacyContainer = CreateLegacyResultContainer();
            var legacyService = legacyContainer.Resolve<ILoadResultService>();

            var composedData = composedService.Load(facetsConfig, resultConfig);
            var legacyData = legacyService.Load(facetsConfig, resultConfig);

            Assert.Equal(ToResultColumns(legacyData), ToResultColumns(composedData));

            var legacyRows = ToDataRows(legacyData);
            var composedRows = ToDataRows(composedData);

            if (ignoreRowOrder)
            {
                legacyRows = legacyRows.OrderBy(row => row).ToList();
                composedRows = composedRows.OrderBy(row => row).ToList();
            }

            Assert.Equal(legacyRows, composedRows);
            Assert.Equal(legacyData.Payload is null, composedData.Payload is null);
        }

        private static System.Collections.Generic.List<string> ToResultColumns(ResultContentSet result)
        {
            return result.Meta.Columns.Select(column => $"{column.FieldKey}:{column.DisplayText}:{column.Type}").ToList();
        }

        private static System.Collections.Generic.List<string> ToDataRows(ResultContentSet result)
        {
            return result.Data.DataCollection.Select(row => string.Join("|", row.Select(value => value?.ToString() ?? "<null>"))).ToList();
        }
    }
}
