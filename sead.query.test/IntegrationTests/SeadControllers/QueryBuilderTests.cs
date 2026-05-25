using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Autofac;
using SeadQueryCore;
using Xunit;

namespace IntegrationTests.Sead
{
    public class PathFinderFixture : IntegrationTestBase, IDisposable
    {
        public IPathFinder PathFinder { get; }

        public PathFinderFixture()
        {
            PathFinder = Container.Resolve<IPathFinder>();
        }

        public void Dispose()
        {
            // cleanup if needed
        }
    }

    [Collection("UsePostgresFixture")]
    public class QueryBuilderTests : IntegrationTestBase, IClassFixture<PathFinderFixture>
    {
        protected static string _anchorRoutesFilePath = "anchor_routes.txt";
        protected object _lock = new();

        private readonly IPathFinder _pathFinder;

        public QueryBuilderTests(PathFinderFixture fixture)
            : base()
        {
            _pathFinder = fixture.PathFinder;
        }

        public static IEnumerable<object[]> Edges =>
            [
                // [("facet.method_measured_values", "tbl_analysis_entities")],
                // [("facet.method_measured_values", "tbl_datasets")],
                // [("facet.method_measured_values", "tbl_physical_samples")],
                // [("facet.method_measured_values", "tbl_sites")],
                // [("facet.method_measured_values", "tbl_taxa_tree_master")],
                [("facet.sample_group_construction_purposes", "tbl_analysis_entities")],
                [("facet.sample_group_construction_purposes", "tbl_datasets")],
                [("facet.sample_group_construction_purposes", "tbl_physical_samples")],
                [("facet.sample_group_construction_purposes", "tbl_sites")],
                [("facet.sample_group_construction_purposes", "tbl_taxa_tree_master")],
                [("tbl_abundance_elements", "tbl_analysis_entities")],
                [("tbl_abundance_elements", "tbl_datasets")],
                [("tbl_abundance_elements", "tbl_physical_samples")],
                [("tbl_abundance_elements", "tbl_sites")],
                [("tbl_abundance_elements", "tbl_taxa_tree_master")],
                [("tbl_analysis_entities", "tbl_datasets")],
                [("tbl_analysis_entities", "tbl_physical_samples")],
                [("tbl_analysis_entities", "tbl_sites")],
                [("tbl_analysis_entities", "tbl_taxa_tree_master")],
                [("tbl_analysis_entity_ages", "tbl_analysis_entities")],
                [("tbl_analysis_entity_ages", "tbl_datasets")],
                [("tbl_analysis_entity_ages", "tbl_physical_samples")],
                [("tbl_analysis_entity_ages", "tbl_sites")],
                [("tbl_analysis_entity_ages", "tbl_taxa_tree_master")],
                [("tbl_biblio", "tbl_analysis_entities")],
                [("tbl_biblio", "tbl_datasets")],
                [("tbl_biblio", "tbl_physical_samples")],
                [("tbl_biblio", "tbl_sites")],
                [("tbl_biblio", "tbl_taxa_tree_master")],
                [("tbl_data_types", "tbl_analysis_entities")],
                [("tbl_data_types", "tbl_datasets")],
                [("tbl_data_types", "tbl_physical_samples")],
                [("tbl_data_types", "tbl_sites")],
                [("tbl_data_types", "tbl_taxa_tree_master")],
                [("tbl_dataset_masters", "tbl_analysis_entities")],
                [("tbl_dataset_masters", "tbl_datasets")],
                [("tbl_dataset_masters", "tbl_physical_samples")],
                [("tbl_dataset_masters", "tbl_sites")],
                [("tbl_dataset_masters", "tbl_taxa_tree_master")],
                [("tbl_dataset_methods", "tbl_analysis_entities")],
                [("tbl_dataset_methods", "tbl_datasets")],
                [("tbl_dataset_methods", "tbl_physical_samples")],
                [("tbl_dataset_methods", "tbl_sites")],
                [("tbl_dataset_methods", "tbl_taxa_tree_master")],
                [("tbl_datasets", "tbl_analysis_entities")],
                [("tbl_datasets", "tbl_physical_samples")],
                [("tbl_datasets", "tbl_sites")],
                [("tbl_datasets", "tbl_taxa_tree_master")],
                [("tbl_dendro_dates", "tbl_analysis_entities")],
                [("tbl_dendro_dates", "tbl_datasets")],
                [("tbl_dendro_dates", "tbl_physical_samples")],
                [("tbl_dendro_dates", "tbl_sites")],
                [("tbl_dendro_dates", "tbl_taxa_tree_master")],
                [("tbl_ecocode_definitions", "tbl_analysis_entities")],
                [("tbl_ecocode_definitions", "tbl_datasets")],
                [("tbl_ecocode_definitions", "tbl_physical_samples")],
                [("tbl_ecocode_definitions", "tbl_sites")],
                [("tbl_ecocode_definitions", "tbl_taxa_tree_master")],
                [("tbl_ecocode_systems", "tbl_analysis_entities")],
                [("tbl_ecocode_systems", "tbl_datasets")],
                [("tbl_ecocode_systems", "tbl_physical_samples")],
                [("tbl_ecocode_systems", "tbl_sites")],
                [("tbl_ecocode_systems", "tbl_taxa_tree_master")],
                [("tbl_feature_types", "tbl_analysis_entities")],
                [("tbl_feature_types", "tbl_datasets")],
                [("tbl_feature_types", "tbl_physical_samples")],
                [("tbl_feature_types", "tbl_sites")],
                [("tbl_feature_types", "tbl_taxa_tree_master")],
                [("tbl_geochronology", "tbl_analysis_entities")],
                [("tbl_geochronology", "tbl_datasets")],
                [("tbl_geochronology", "tbl_physical_samples")],
                [("tbl_geochronology", "tbl_sites")],
                [("tbl_geochronology", "tbl_taxa_tree_master")],
                [("tbl_location_types", "tbl_analysis_entities")],
                [("tbl_location_types", "tbl_datasets")],
                [("tbl_location_types", "tbl_physical_samples")],
                [("tbl_location_types", "tbl_sites")],
                [("tbl_location_types", "tbl_taxa_tree_master")],
                [("tbl_modification_types", "tbl_analysis_entities")],
                [("tbl_modification_types", "tbl_datasets")],
                [("tbl_modification_types", "tbl_physical_samples")],
                [("tbl_modification_types", "tbl_sites")],
                [("tbl_modification_types", "tbl_taxa_tree_master")],
                [("tbl_rdb_codes", "tbl_analysis_entities")],
                [("tbl_rdb_codes", "tbl_datasets")],
                [("tbl_rdb_codes", "tbl_physical_samples")],
                [("tbl_rdb_codes", "tbl_sites")],
                [("tbl_rdb_codes", "tbl_taxa_tree_master")],
                [("tbl_rdb_systems", "tbl_analysis_entities")],
                [("tbl_rdb_systems", "tbl_datasets")],
                [("tbl_rdb_systems", "tbl_physical_samples")],
                [("tbl_rdb_systems", "tbl_sites")],
                [("tbl_rdb_systems", "tbl_taxa_tree_master")],
                [("tbl_record_types", "tbl_analysis_entities")],
                [("tbl_record_types", "tbl_datasets")],
                [("tbl_record_types", "tbl_physical_samples")],
                [("tbl_record_types", "tbl_sites")],
                [("tbl_record_types", "tbl_taxa_tree_master")],
                [("tbl_relative_ages", "tbl_analysis_entities")],
                [("tbl_relative_ages", "tbl_datasets")],
                [("tbl_relative_ages", "tbl_physical_samples")],
                [("tbl_relative_ages", "tbl_sites")],
                [("tbl_relative_ages", "tbl_taxa_tree_master")],
                [("tbl_sample_group_sampling_contexts", "tbl_analysis_entities")],
                [("tbl_sample_group_sampling_contexts", "tbl_datasets")],
                [("tbl_sample_group_sampling_contexts", "tbl_physical_samples")],
                [("tbl_sample_group_sampling_contexts", "tbl_sites")],
                [("tbl_sample_group_sampling_contexts", "tbl_taxa_tree_master")],
                [("tbl_sample_groups", "tbl_analysis_entities")],
                [("tbl_sample_groups", "tbl_datasets")],
                [("tbl_sample_groups", "tbl_physical_samples")],
                [("tbl_sample_groups", "tbl_sites")],
                [("tbl_sample_groups", "tbl_taxa_tree_master")],
                [("tbl_seasons", "tbl_analysis_entities")],
                [("tbl_seasons", "tbl_datasets")],
                [("tbl_seasons", "tbl_physical_samples")],
                [("tbl_seasons", "tbl_sites")],
                [("tbl_seasons", "tbl_taxa_tree_master")],
                [("tbl_sites", "tbl_analysis_entities")],
                [("tbl_sites", "tbl_datasets")],
                [("tbl_sites", "tbl_physical_samples")],
                [("tbl_sites", "tbl_taxa_tree_master")],
                [("tbl_taxa_tree_authors", "tbl_analysis_entities")],
                [("tbl_taxa_tree_authors", "tbl_datasets")],
                [("tbl_taxa_tree_authors", "tbl_physical_samples")],
                [("tbl_taxa_tree_authors", "tbl_sites")],
                [("tbl_taxa_tree_authors", "tbl_taxa_tree_master")],
                [("tbl_taxa_tree_families", "tbl_analysis_entities")],
                [("tbl_taxa_tree_families", "tbl_datasets")],
                [("tbl_taxa_tree_families", "tbl_physical_samples")],
                [("tbl_taxa_tree_families", "tbl_sites")],
                [("tbl_taxa_tree_families", "tbl_taxa_tree_master")],
                [("tbl_taxa_tree_genera", "tbl_analysis_entities")],
                [("tbl_taxa_tree_genera", "tbl_datasets")],
                [("tbl_taxa_tree_genera", "tbl_physical_samples")],
                [("tbl_taxa_tree_genera", "tbl_sites")],
                [("tbl_taxa_tree_genera", "tbl_taxa_tree_master")],
            ];

        [Theory]
        [MemberData(nameof(Edges))]
        public void CreatePathFinder((string source, string target) edge)
        {
            var route = _pathFinder.Find(edge.source, edge.target);

            var routeLine = string.Join(" -> ", route.Select(item => item.SourceName));
            if (route.Count == 0)
                routeLine += " -> ???";
            else
                routeLine += $" -> {route.Last().TargetName}";

            routeLine = $"{edge.source};{edge.target};{routeLine}";
            // Append routeLine to text file "anchor_routes.txt" in projects root folder
            lock (_lock)
            {
                File.AppendAllText(_anchorRoutesFilePath, $"{routeLine}\n");
            }
        }

        [Theory]
        [InlineData("species:species")]
        public void Load_VariousFacetConfigs_HasExpectedSqlQuery(string uri)
        {
            var facetsConfig = FakeFacetsConfig(uri);
            var resultConfig = FakeResultConfig("result_facet", "site_level", "tabular");

            var queryFields = resultConfig.GetSortedFields();

            var querySetup = QuerySetupBuilder.Build(facetsConfig, resultConfig.Facet, queryFields);

            var sqlQuery = SqlCompilerLocator.Locate(resultConfig.ViewTypeId).Compile(querySetup, resultConfig.Facet, queryFields);

            Assert.NotNull(sqlQuery);
        }
    }
}
