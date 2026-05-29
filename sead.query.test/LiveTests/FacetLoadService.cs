using System.Collections.Generic;
using System.Linq;
using Autofac;
using FluentAssertions;
using SeadQueryAPI.Services;
using SeadQueryCore;
using SeadQueryCore.Model;
using SeadQueryCore.QueryBuilder;
using SeadQueryCore.QueryComposer;
using SQT.Mocks;
using Xunit;

namespace SQT.LiveServices
{
    [Collection("UsePostgresFixture")]
    public class FacetLoadServiceTests
    {
        public SeadQueryAPI.DependencyService DependencyService { get; private set; }
        public Autofac.IContainer Container { get; private set; }
        public IFacetContext FacetContext { get; private set; }
        public IRepositoryRegistry Registry { get; private set; }

        public FacetLoadServiceTests()
        {
            DependencyService = new SeadQueryAPI.DependencyService() { Options = SettingFactory.DefaultSettings };
            var builder = new Autofac.ContainerBuilder();
            builder.RegisterModule(DependencyService);
            Container = builder.Build();
            FacetContext = Container.Resolve<IFacetContext>();
            Registry = Container.Resolve<IRepositoryRegistry>();
        }

        public FacetsConfig2 UriToFacetsConfig(string uri) => new MockFacetsConfigFactory(Registry.Facets).Create(uri);

        public virtual ResultConfig FakeResultConfig(string facetCode, string specificationKey, string viewTypeId) =>
            ResultConfigFactory.Create(Registry.Facets.GetByCode(facetCode), Registry.Results.GetByKey(specificationKey), viewTypeId);

        public static IEnumerable<object[]> SupportedComposedVisibleAndDiscreteLiveUris =>
            [
                ["result_facet:sites@4/result_facet"],
                ["sites:sample_groups@1/sites"],
                ["sample_groups:sites@4/sample_groups"],
                ["country:sites@4/country"],
                ["constructions:sites@4/constructions"],
                ["ecocode:sites@4/ecocode"],
                ["sites:sites"],
                ["sites:country@1,2,5/sites"],
                ["ecocode:country@1,2,5/ecocode"],
                ["feature_type:country@1,2,5/feature_type"],
                ["ecocode_system:country@1,2,5/ecocode_system"],
                ["genus:genus"],
                ["genus:country@1,2,5/genus"],
                ["species:country@1,2,5/species"],
                ["species_author:country@1,2,5/species_author"],
                ["family:country@1,2,5/family"],
                ["dataset_methods:country@1,2,5/dataset_methods"],
                ["record_types:country@1,2,5/record_types"],
                ["dataset_provider:country@1,2,5/dataset_provider"],
                ["relative_age_name:country@1,2,5/relative_age_name"],
                ["abundance_classification:country@1,2,5/abundance_classification"],
                ["tbl_biblio_sample_groups:country@1,2,5/tbl_biblio_sample_groups"],
                ["tbl_biblio_sites:country@1,2,5/tbl_biblio_sites"],
                ["tbl_biblio_modern:country@1,2,5/tbl_biblio_modern"],
                ["region:country@1,2,5/region"],
                ["activeseason:country@1,2,5/activeseason"],
                ["sample_groups:sample_groups"],
                ["sample_groups:country@1,2,5/sample_groups"],
                ["data_types:data_types"],
                ["data_types:country@1,2,5/data_types"],
                ["rdb_systems:rdb_systems"],
                ["rdb_systems:country@1,2,5/rdb_systems"],
            ];

        public static IEnumerable<object[]> SupportedComposedRangeLiveUris =>
            [
                ["geochronology:country@1,2,5/geochronology"],
                ["tbl_denormalized_measured_values_33_0:country@1,2,5/tbl_denormalized_measured_values_33_0"],
                ["tbl_denormalized_measured_values_33_82:country@1,2,5/tbl_denormalized_measured_values_33_82"],
                ["tbl_denormalized_measured_values_32:country@1,2,5/tbl_denormalized_measured_values_32"],
                ["tbl_denormalized_measured_values_37:country@1,2,5/tbl_denormalized_measured_values_37"],
                ["abundances_all:country@1,2,5/abundances_all"],
            ];

        public static IEnumerable<object[]> SupportedComposedLiveUris =>
            [.. SupportedComposedVisibleAndDiscreteLiveUris, .. SupportedComposedRangeLiveUris];

        [Theory]
        [InlineData("genus:genus")]
        [InlineData("abundance_classification:abundance_classification")]
        [InlineData(
            "sites_polygon:sites_polygon@63.872484,20.093291,63.947006,20.501316,63.878949,20.673213,63.748021,20.252953,63.793983,20.095738"
        )]
        [InlineData("sites_polygon:sites_polygon")]
        [InlineData("sites:sites/tbl_denormalized_measured_values_33_0")]
        public void Load_VariousConfigs_Success(string uri)
        {
            var fakeFacetsConfig = UriToFacetsConfig(uri);
            var service = this.Container.Resolve<ILoadFacetService>();

            var data = service.Load(fakeFacetsConfig);

            Assert.NotNull(data);
        }

        [Theory]
        [InlineData("analysis_entity_ages:analysis_entity_ages")]
        public void Load_Intersect_Facets(string uri)
        {
            var facetsConfig = UriToFacetsConfig(uri);
            var service = this.Container.Resolve<ILoadFacetService>();

            var data = service.Load(facetsConfig);

            Assert.NotNull(data);
        }

        [Theory]
        [InlineData("analysis_entity_ages:analysis_entity_ages")]
        public void FacetContentService_UnsupportedIntersectSlice_FallsBackToLegacyFacetContent(string uri)
        {
            AssertFallsBackToLegacyFacetContent(uri);
        }

        [Theory]
        [InlineData(
            "sites_polygon:sites_polygon@63.872484,20.093291,63.947006,20.501316,63.878949,20.673213,63.748021,20.252953,63.793983,20.095738"
        )]
        public void FacetContentService_UnsupportedSitesPolygonSlice_FallsBackToLegacyFacetContent(string uri)
        {
            AssertFallsBackToLegacyFacetContent(uri);
        }

        [Theory]
        [MemberData(nameof(SupportedComposedLiveUris))]
        public void FacetContentService_ComposedSupportedLiveSlices_UseComposedFacetContentQuery(string uri)
        {
            AssertUsesComposedFacetContentQuery(uri);
        }

        [Theory]
        [MemberData(nameof(SupportedComposedLiveUris))]
        public void FacetContentService_ComposedSupportedLiveSlices_MatchLegacyFacetContent(string uri)
        {
            AssertMatchesLegacyFacetContent(uri);
        }

        [Theory]
        [MemberData(nameof(SupportedComposedRangeLiveUris))]
        public void FacetContentService_ComposedSupportedRangeLiveSlices_UseComposedFacetContentQuery(string uri)
        {
            AssertUsesComposedFacetContentQuery(uri);
        }

        [Theory]
        [MemberData(nameof(SupportedComposedRangeLiveUris))]
        public void FacetContentService_ComposedSupportedRangeLiveSlices_MatchLegacyFacetContent(string uri)
        {
            AssertMatchesLegacyFacetContent(uri);
        }

        [Theory]
        [MemberData(nameof(SupportedComposedVisibleAndDiscreteLiveUris))]
        public void FacetContentService_ComposedSupportedVisibleAndDiscreteLiveSlices_UseComposedFacetContentQuery(string uri)
        {
            AssertUsesComposedFacetContentQuery(uri);
        }

        [Theory]
        [MemberData(nameof(SupportedComposedVisibleAndDiscreteLiveUris))]
        public void FacetContentService_ComposedSupportedVisibleAndDiscreteLiveSlices_MatchLegacyFacetContent(string uri)
        {
            AssertMatchesLegacyFacetContent(uri);
        }

        [Theory]
        [InlineData("genus:genus")]
        public void FacetContentService_ComposedTargetOnlyGenusSlice_UsesComposedFacetContentQuery(string uri)
        {
            AssertUsesComposedFacetContentQuery(uri, "target_route as", "tbl_taxa_tree_genera");
        }

        [Theory]
        [InlineData("genus:genus")]
        public void FacetContentService_ComposedTargetOnlyGenusSlice_MatchesLegacyFacetContent(string uri)
        {
            AssertMatchesLegacyFacetContent(uri);
        }

        [Theory]
        [InlineData("sites:sites")]
        public void FacetContentService_ComposedTargetOnlySitesSlice_UsesComposedFacetContentQuery(string uri)
        {
            AssertUsesComposedFacetContentQuery(uri, "target_route as", "tbl_sites");
        }

        [Theory]
        [InlineData("sites:sites")]
        public void FacetContentService_ComposedTargetOnlySitesSlice_MatchesLegacyFacetContent(string uri)
        {
            AssertMatchesLegacyFacetContent(uri);
        }

        [Theory]
        [InlineData("sample_groups:sample_groups")]
        public void FacetContentService_ComposedTargetOnlySampleGroupsSlice_UsesComposedFacetContentQuery(string uri)
        {
            AssertUsesComposedFacetContentQuery(uri, "target_route as", "tbl_sample_groups");
        }

        [Theory]
        [InlineData("sample_groups:sample_groups")]
        public void FacetContentService_ComposedTargetOnlySampleGroupsSlice_MatchesLegacyFacetContent(string uri)
        {
            AssertMatchesLegacyFacetContent(uri);
        }

        [Theory]
        [InlineData("data_types:data_types")]
        public void FacetContentService_ComposedTargetOnlyDataTypesSlice_UsesComposedFacetContentQuery(string uri)
        {
            AssertUsesComposedFacetContentQuery(uri, "target_route as", "tbl_data_types");
        }

        [Theory]
        [InlineData("data_types:data_types")]
        public void FacetContentService_ComposedTargetOnlyDataTypesSlice_MatchesLegacyFacetContent(string uri)
        {
            AssertMatchesLegacyFacetContent(uri);
        }

        [Theory]
        [InlineData("rdb_systems:rdb_systems")]
        public void FacetContentService_ComposedTargetOnlyRdbSystemsSlice_UsesComposedFacetContentQuery(string uri)
        {
            AssertUsesComposedFacetContentQuery(uri, "target_route as", "tbl_rdb_systems");
        }

        [Theory]
        [InlineData("rdb_systems:rdb_systems")]
        public void FacetContentService_ComposedTargetOnlyRdbSystemsSlice_MatchesLegacyFacetContent(string uri)
        {
            AssertMatchesLegacyFacetContent(uri);
        }

        [Theory]
        [InlineData("sites:sample_groups@1/sites")]
        public void FacetContentService_ComposedVisibleSitesSlice_UsesComposedFacetContentQuery(string uri)
        {
            AssertUsesComposedFacetContentQuery(uri, "target_route as", "join target_route");
        }

        [Theory]
        [InlineData("sample_groups:sites@4/sample_groups")]
        public void FacetContentService_ComposedVisibleSampleGroupsSlice_UsesComposedFacetContentQuery(string uri)
        {
            AssertUsesComposedFacetContentQuery(uri, "target_route as", "join target_route");
        }

        [Theory]
        [InlineData("country:sites@4/country")]
        public void FacetContentService_ComposedVisibleCountrySlice_UsesComposedFacetContentQuery(string uri)
        {
            AssertUsesComposedFacetContentQuery(uri, "target_route as", "join target_route");
        }

        [Theory]
        [InlineData("constructions:sites@4/constructions")]
        public void FacetContentService_ComposedVisibleConstructionsSlice_UsesComposedFacetContentQuery(string uri)
        {
            AssertUsesComposedFacetContentQuery(uri, "target_route as", "tbl_sample_group_descriptions");
        }

        [Theory]
        [InlineData("sites:country@1,2,5/sites")]
        public void FacetContentService_ComposedCountryPredicateSitesSlice_UsesComposedFacetContentQuery(string uri)
        {
            AssertUsesComposedFacetContentQuery(uri, "target_route as", "X_0.location_type_id=1");
        }

        [Theory]
        [InlineData("ecocode:sites@4/ecocode")]
        public void FacetContentService_ComposedVisibleEcocodeSlice_UsesComposedFacetContentQuery(string uri)
        {
            AssertUsesComposedFacetContentQuery(uri, "target_route as", "tbl_ecocode_definitions");
        }

        [Theory]
        [InlineData("ecocode:country@1,2,5/ecocode")]
        public void FacetContentService_ComposedCountryPredicateEcocodeSlice_UsesComposedFacetContentQuery(string uri)
        {
            AssertUsesComposedFacetContentQuery(uri, "target_route as", "tbl_ecocode_definitions", "X_0.location_type_id=1");
        }

        [Theory]
        [InlineData("feature_type:country@1,2,5/feature_type")]
        public void FacetContentService_ComposedCountryPredicateFeatureTypeSlice_UsesComposedFacetContentQuery(string uri)
        {
            AssertUsesComposedFacetContentQuery(uri, "target_route as", "tbl_feature_types", "X_0.location_type_id=1");
        }

        [Theory]
        [InlineData("ecocode_system:country@1,2,5/ecocode_system")]
        public void FacetContentService_ComposedCountryPredicateEcocodeSystemSlice_UsesComposedFacetContentQuery(string uri)
        {
            AssertUsesComposedFacetContentQuery(uri, "target_route as", "tbl_ecocode_systems", "X_0.location_type_id=1");
        }

        [Theory]
        [InlineData("genus:country@1,2,5/genus")]
        public void FacetContentService_ComposedCountryPredicateGenusSlice_UsesComposedFacetContentQuery(string uri)
        {
            AssertUsesComposedFacetContentQuery(uri, "target_route as", "tbl_taxa_tree_genera", "X_0.location_type_id=1");
        }

        [Theory]
        [InlineData("species:country@1,2,5/species")]
        public void FacetContentService_ComposedCountryPredicateSpeciesSlice_UsesComposedFacetContentQuery(string uri)
        {
            AssertUsesComposedFacetContentQuery(uri, "target_route as", "abundance_taxon_shortcut", "X_0.location_type_id=1");
        }

        [Theory]
        [InlineData("species_author:country@1,2,5/species_author")]
        public void FacetContentService_ComposedCountryPredicateSpeciesAuthorSlice_UsesComposedFacetContentQuery(string uri)
        {
            AssertUsesComposedFacetContentQuery(uri, "target_route as", "tbl_taxa_tree_authors", "X_0.location_type_id=1");
        }

        [Theory]
        [InlineData("family:country@1,2,5/family")]
        public void FacetContentService_ComposedCountryPredicateFamilySlice_UsesComposedFacetContentQuery(string uri)
        {
            AssertUsesComposedFacetContentQuery(uri, "target_route as", "tbl_taxa_tree_families", "X_0.location_type_id=1");
        }

        [Theory]
        [InlineData("dataset_methods:country@1,2,5/dataset_methods")]
        public void FacetContentService_ComposedCountryPredicateDatasetMethodsSlice_UsesComposedFacetContentQuery(string uri)
        {
            AssertUsesComposedFacetContentQuery(uri, "target_route as", "tbl_methods", "X_0.location_type_id=1");
        }

        [Theory]
        [InlineData("record_types:country@1,2,5/record_types")]
        public void FacetContentService_ComposedCountryPredicateRecordTypesSlice_UsesComposedFacetContentQuery(string uri)
        {
            AssertUsesComposedFacetContentQuery(uri, "target_route as", "tbl_record_types", "X_0.location_type_id=1");
        }

        [Theory]
        [InlineData("dataset_provider:country@1,2,5/dataset_provider")]
        public void FacetContentService_ComposedCountryPredicateDatasetProviderSlice_UsesComposedFacetContentQuery(string uri)
        {
            AssertUsesComposedFacetContentQuery(uri, "target_route as", "tbl_dataset_masters", "X_0.location_type_id=1");
        }

        [Theory]
        [InlineData("relative_age_name:country@1,2,5/relative_age_name")]
        public void FacetContentService_ComposedCountryPredicateRelativeAgeNameSlice_UsesComposedFacetContentQuery(string uri)
        {
            AssertUsesComposedFacetContentQuery(uri, "target_route as", "tbl_relative_ages", "X_0.location_type_id=1");
        }

        [Theory]
        [InlineData("geochronology:country@1,2,5/geochronology")]
        public void FacetContentService_ComposedCountryPredicateGeochronologySlice_UsesComposedFacetContentQuery(string uri)
        {
            AssertUsesComposedFacetContentQuery(
                uri,
                "categories(category, lower, upper) as",
                "tbl_geochronology.age::integer",
                "X_0.location_type_id=1"
            );
        }

        [Theory]
        [InlineData("geochronology:country@1,2,5/geochronology")]
        public void FacetContentService_ComposedCountryPredicateGeochronologySlice_MatchesLegacyFacetContent(string uri)
        {
            AssertMatchesLegacyFacetContent(uri);
        }

        [Theory]
        [InlineData("tbl_denormalized_measured_values_33_0:country@1,2,5/tbl_denormalized_measured_values_33_0")]
        public void FacetContentService_ComposedCountryPredicateMeasuredValue33_0Slice_UsesComposedFacetContentQuery(string uri)
        {
            AssertUsesComposedFacetContentQuery(
                uri,
                "categories(category, lower, upper) as",
                "method_values_33.measured_value",
                "X_0.location_type_id=1"
            );
        }

        [Theory]
        [InlineData("tbl_denormalized_measured_values_33_0:country@1,2,5/tbl_denormalized_measured_values_33_0")]
        public void FacetContentService_ComposedCountryPredicateMeasuredValue33_0Slice_MatchesLegacyFacetContent(string uri)
        {
            AssertMatchesLegacyFacetContent(uri);
        }

        [Theory]
        [InlineData("tbl_denormalized_measured_values_32:country@1,2,5/tbl_denormalized_measured_values_32")]
        public void FacetContentService_ComposedCountryPredicateMeasuredValue32Slice_UsesComposedFacetContentQuery(string uri)
        {
            AssertUsesComposedFacetContentQuery(
                uri,
                "categories(category, lower, upper) as",
                "method_values_32.measured_value",
                "X_0.location_type_id=1"
            );
        }

        [Theory]
        [InlineData("tbl_denormalized_measured_values_32:country@1,2,5/tbl_denormalized_measured_values_32")]
        public void FacetContentService_ComposedCountryPredicateMeasuredValue32Slice_MatchesLegacyFacetContent(string uri)
        {
            AssertMatchesLegacyFacetContent(uri);
        }

        [Theory]
        [InlineData("tbl_denormalized_measured_values_37:country@1,2,5/tbl_denormalized_measured_values_37")]
        public void FacetContentService_ComposedCountryPredicateMeasuredValue37Slice_UsesComposedFacetContentQuery(string uri)
        {
            AssertUsesComposedFacetContentQuery(
                uri,
                "categories(category, lower, upper) as",
                "method_values_37.measured_value",
                "X_0.location_type_id=1"
            );
        }

        [Theory]
        [InlineData("tbl_denormalized_measured_values_37:country@1,2,5/tbl_denormalized_measured_values_37")]
        public void FacetContentService_ComposedCountryPredicateMeasuredValue37Slice_MatchesLegacyFacetContent(string uri)
        {
            AssertMatchesLegacyFacetContent(uri);
        }

        [Theory]
        [InlineData("tbl_denormalized_measured_values_33_82:country@1,2,5/tbl_denormalized_measured_values_33_82")]
        public void FacetContentService_ComposedCountryPredicateMeasuredValue33_82Slice_UsesComposedFacetContentQuery(string uri)
        {
            AssertUsesComposedFacetContentQuery(
                uri,
                "categories(category, lower, upper) as",
                "method_values_33_82.measured_value",
                "X_0.location_type_id=1"
            );
        }

        [Theory]
        [InlineData("tbl_denormalized_measured_values_33_82:country@1,2,5/tbl_denormalized_measured_values_33_82")]
        public void FacetContentService_ComposedCountryPredicateMeasuredValue33_82Slice_MatchesLegacyFacetContent(string uri)
        {
            AssertMatchesLegacyFacetContent(uri);
        }

        [Theory]
        [InlineData("abundance_classification:country@1,2,5/abundance_classification")]
        public void FacetContentService_ComposedCountryPredicateAbundanceClassificationSlice_UsesComposedFacetContentQuery(string uri)
        {
            AssertUsesComposedFacetContentQuery(uri, "facet.view_abundance.elements_part_mod", "X_0.location_type_id=1");
        }

        [Theory]
        [InlineData("abundance_classification:country@1,2,5/abundance_classification")]
        public void FacetContentService_ComposedCountryPredicateAbundanceClassificationSlice_MatchesLegacyFacetContent(string uri)
        {
            AssertMatchesLegacyFacetContent(uri);
        }

        [Theory]
        [InlineData("tbl_biblio_sample_groups:country@1,2,5/tbl_biblio_sample_groups")]
        public void FacetContentService_ComposedCountryPredicateBiblioSampleGroupsSlice_UsesComposedFacetContentQuery(string uri)
        {
            AssertUsesComposedFacetContentQuery(
                uri,
                "tbl_biblio.biblio_id",
                "facet.view_sample_group_references.biblio_id is not null",
                "X_0.location_type_id=1"
            );
        }

        [Theory]
        [InlineData("tbl_biblio_sample_groups:country@1,2,5/tbl_biblio_sample_groups")]
        public void FacetContentService_ComposedCountryPredicateBiblioSampleGroupsSlice_MatchesLegacyFacetContent(string uri)
        {
            AssertMatchesLegacyFacetContent(uri);
        }

        [Theory]
        [InlineData("tbl_biblio_sites:country@1,2,5/tbl_biblio_sites")]
        public void FacetContentService_ComposedCountryPredicateBiblioSitesSlice_UsesComposedFacetContentQuery(string uri)
        {
            AssertUsesComposedFacetContentQuery(
                uri,
                "tbl_biblio.biblio_id",
                "facet.view_site_references.biblio_id is not null",
                "X_0.location_type_id=1"
            );
        }

        [Theory]
        [InlineData("tbl_biblio_sites:country@1,2,5/tbl_biblio_sites")]
        public void FacetContentService_ComposedCountryPredicateBiblioSitesSlice_MatchesLegacyFacetContent(string uri)
        {
            AssertMatchesLegacyFacetContent(uri);
        }

        [Theory]
        [InlineData("tbl_biblio_modern:country@1,2,5/tbl_biblio_modern")]
        public void FacetContentService_ComposedCountryPredicateBiblioModernSlice_UsesComposedFacetContentQuery(string uri)
        {
            AssertUsesComposedFacetContentQuery(uri, "facet.view_taxa_biblio.biblio_id", "X_0.location_type_id=1");
        }

        [Theory]
        [InlineData("tbl_biblio_modern:country@1,2,5/tbl_biblio_modern")]
        public void FacetContentService_ComposedCountryPredicateBiblioModernSlice_MatchesLegacyFacetContent(string uri)
        {
            AssertMatchesLegacyFacetContent(uri);
        }

        [Theory]
        [InlineData("region:country@1,2,5/region")]
        public void FacetContentService_ComposedCountryPredicateRegionSlice_UsesComposedFacetContentQuery(string uri)
        {
            AssertUsesComposedFacetContentQuery(uri, "X_0.location_type_id=1");
        }

        [Theory]
        [InlineData("region:country@1,2,5/region")]
        public void FacetContentService_ComposedCountryPredicateRegionSlice_MatchesLegacyFacetContent(string uri)
        {
            AssertMatchesLegacyFacetContent(uri);
        }

        [Theory]
        [InlineData("activeseason:country@1,2,5/activeseason")]
        public void FacetContentService_ComposedCountryPredicateActiveSeasonSlice_UsesComposedFacetContentQuery(string uri)
        {
            AssertUsesComposedFacetContentQuery(uri, "X_0.location_type_id=1");
        }

        [Theory]
        [InlineData("activeseason:country@1,2,5/activeseason")]
        public void FacetContentService_ComposedCountryPredicateActiveSeasonSlice_MatchesLegacyFacetContent(string uri)
        {
            AssertMatchesLegacyFacetContent(uri);
        }

        [Theory]
        [InlineData("sample_groups:country@1,2,5/sample_groups")]
        public void FacetContentService_ComposedCountryPredicateSampleGroupsSlice_UsesComposedFacetContentQuery(string uri)
        {
            AssertUsesComposedFacetContentQuery(uri, "X_0.location_type_id=1");
        }

        [Theory]
        [InlineData("sample_groups:country@1,2,5/sample_groups")]
        public void FacetContentService_ComposedCountryPredicateSampleGroupsSlice_MatchesLegacyFacetContent(string uri)
        {
            AssertMatchesLegacyFacetContent(uri);
        }

        [Theory]
        [InlineData("data_types:country@1,2,5/data_types")]
        public void FacetContentService_ComposedCountryPredicateDataTypesSlice_UsesComposedFacetContentQuery(string uri)
        {
            AssertUsesComposedFacetContentQuery(uri, "target_route as", "tbl_data_types", "X_0.location_type_id=1");
        }

        [Theory]
        [InlineData("data_types:country@1,2,5/data_types")]
        public void FacetContentService_ComposedCountryPredicateDataTypesSlice_MatchesLegacyFacetContent(string uri)
        {
            AssertMatchesLegacyFacetContent(uri);
        }

        [Theory]
        [InlineData("rdb_systems:country@1,2,5/rdb_systems")]
        public void FacetContentService_ComposedCountryPredicateRdbSystemsSlice_UsesComposedFacetContentQuery(string uri)
        {
            AssertUsesComposedFacetContentQuery(uri, "target_route as", "tbl_rdb_systems", "X_0.location_type_id=1");
        }

        [Theory]
        [InlineData("rdb_systems:country@1,2,5/rdb_systems")]
        public void FacetContentService_ComposedCountryPredicateRdbSystemsSlice_MatchesLegacyFacetContent(string uri)
        {
            AssertMatchesLegacyFacetContent(uri);
        }

        [Theory]
        [InlineData("abundances_all:country@1,2,5/abundances_all")]
        public void FacetContentService_ComposedCountryPredicateAbundancesAllSlice_UsesComposedFacetContentQuery(string uri)
        {
            AssertUsesComposedFacetContentQuery(
                uri,
                "categories(category, lower, upper) as",
                "facet.view_abundance.abundance",
                "facet.view_abundance.abundance is not null",
                "X_0.location_type_id=1"
            );
        }

        [Theory]
        [InlineData("abundances_all:country@1,2,5/abundances_all")]
        public void FacetContentService_ComposedCountryPredicateAbundancesAllSlice_MatchesLegacyFacetContent(string uri)
        {
            AssertMatchesLegacyFacetContent(uri);
        }

        private IFacetContentService CreateLegacyFacetContentService()
        {
            var registry = Container.Resolve<IRepositoryRegistry>();
            var facetSettings = Container.Resolve<IFacetSetting>();
            var querySetupBuilder = Container.Resolve<IQuerySetupBuilder>();
            var queryProxy = Container.Resolve<ITypedQueryProxy>();
            var categoryCountService = Container.Resolve<ICategoryCountService>();

            return new FacetContentService(facetSettings, registry, querySetupBuilder, queryProxy, categoryCountService, null);
        }

        private void AssertUsesComposedFacetContentQuery(string uri, params string[] expectedSqlFragments)
        {
            var facetsConfig = UriToFacetsConfig(uri);
            var composedService = Container.Resolve<IComposedFacetContentService>();
            var service = Assert.IsType<FacetContentService>(Container.Resolve<IFacetContentService>());

            Assert.True(composedService.CanHandle(facetsConfig));
            Assert.NotNull(service.ComposedFacetContentService);

            var data = service.Load(facetsConfig);

            Assert.NotNull(data);
            Assert.Contains("with composed_filter as", data.SqlQuery, System.StringComparison.OrdinalIgnoreCase);
            Assert.Contains("join composed_filter", data.SqlQuery, System.StringComparison.OrdinalIgnoreCase);

            foreach (var expectedSqlFragment in expectedSqlFragments)
            {
                Assert.Contains(expectedSqlFragment, data.SqlQuery, System.StringComparison.OrdinalIgnoreCase);
            }
        }

        private void AssertMatchesLegacyFacetContent(string uri)
        {
            var composedService = Container.Resolve<IFacetContentService>();
            var legacyService = CreateLegacyFacetContentService();

            var composedData = composedService.Load(UriToFacetsConfig(uri));
            var legacyData = legacyService.Load(UriToFacetsConfig(uri));

            Assert.Equal(ToCategoryCounts(legacyData.Items), ToCategoryCounts(composedData.Items));
            Assert.Equal(ToCategoryCounts(legacyData.Distribution.Values), ToCategoryCounts(composedData.Distribution.Values));
            Assert.Equal(legacyData.Picks.Keys.OrderBy(key => key), composedData.Picks.Keys.OrderBy(key => key));
        }

        private void AssertFallsBackToLegacyFacetContent(string uri)
        {
            var facetsConfig = UriToFacetsConfig(uri);
            var composedService = Container.Resolve<IComposedFacetContentService>();
            var service = Assert.IsType<FacetContentService>(Container.Resolve<IFacetContentService>());
            var legacyService = CreateLegacyFacetContentService();

            Assert.False(composedService.CanHandle(facetsConfig));
            Assert.NotNull(service.ComposedFacetContentService);

            var data = service.Load(facetsConfig);
            var legacyData = legacyService.Load(UriToFacetsConfig(uri));

            Assert.NotNull(data);
            Assert.DoesNotContain("with composed_filter as", data.SqlQuery, System.StringComparison.OrdinalIgnoreCase);
            Assert.DoesNotContain("join composed_filter", data.SqlQuery, System.StringComparison.OrdinalIgnoreCase);
            Assert.Equal(legacyData.SqlQuery, data.SqlQuery);
            Assert.Equal(ToCategoryCounts(legacyData.Items), ToCategoryCounts(data.Items));
            Assert.Equal(ToCategoryCounts(legacyData.Distribution.Values), ToCategoryCounts(data.Distribution.Values));
            Assert.Equal(legacyData.Picks.Keys.OrderBy(key => key), data.Picks.Keys.OrderBy(key => key));
        }

        private static List<string> ToCategoryCounts(IEnumerable<CategoryItem> items)
        {
            return items.Select(item => $"{item.Category}:{item.Count}").OrderBy(value => value).ToList();
        }
    }
}
