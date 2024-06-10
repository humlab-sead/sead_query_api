using Moq;
using SeadQueryCore;
using SeadQueryCore.QueryBuilder;
using SQT.Infrastructure;
using SQT.Mocks;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace SQT.QueryBuilder
{
    [Collection("SeadJsonFacetContextFixture")]
    public class QuerySetupBuilderTests : DisposableFacetContextContainer
    {
        public QuerySetupBuilderTests(SeadJsonFacetContextFixture fixture, ITestOutputHelper output) : base(fixture)
        {
            Output = output;
        }

        private readonly ITestOutputHelper Output;

        [Fact]
        public void SkitTest()
        {
            string[] trail = { "tbl_analysis_entities", "tbl_abundances", "tbl_taxa_tree_master", "tbl_ecocodes", "tbl_ecocode_definitions" };
            var pairs = RouteHelper.ToPairs(trail);
            var splits = pairs.Select(x => x.Split("/")).Select(z => new { Source = z[0], Target = z[1] }).ToList();
            var relations = splits.Select(z => Registry.TableRelations.FindByName(z.Source, z.Target) ??
                                 (TableRelation)Registry.TableRelations.FindByName(z.Target, z.Source).Reverse()).ToList();
            Assert.True(true);
        }

        [Theory]
        [InlineData("sites:sites")]
        [InlineData("sites:country@1/sites")]
        [InlineData("sites:country@1,2,3/sites")]
        [InlineData("data_types:country@1,2,3/data_types")]
        [InlineData("tbl_denormalized_measured_values_33_82:country@1,2,5/tbl_denormalized_measured_values_33_82")]
        [InlineData("geochronology:country@1,2,5/geochronology")]
        [InlineData("relative_age_name:country@1,2,5/relative_age_name")]
        [InlineData("record_types:country@1,2,5/record_types")]
        [InlineData("sites:country@1,2,5/sites")]
        [InlineData("ecocode:country@1,2,5/ecocode")]
        // [InlineData("family:country@1,2,5/family")]
        [InlineData("genus:country@1,2,5/genus")]
        [InlineData("species_author:country@1,2,5/species_author")]
        [InlineData("feature_type:country@1,2,5/feature_type")]
        [InlineData("ecocode_system:country@1,2,5/ecocode_system")]
        [InlineData("abundance_classification:country@1,2,5/abundance_classification")]
        [InlineData("abundances_all:country@1,2,5/abundances_all")]
        [InlineData("activeseason:country@1,2,5/activeseason")]
        [InlineData("tbl_biblio_modern:country@1,2,5/tbl_biblio_modern")]
        [InlineData("tbl_biblio_sample_groups:country@1,2,5/tbl_biblio_sample_groups")]
        [InlineData("tbl_biblio_sites:country@1,2,5/tbl_biblio_sites")]
        [InlineData("dataset_provider:country@1,2,5/dataset_provider")]
        [InlineData("dataset_methods:country@1,2,5/dataset_methods")]
        [InlineData("region:country@1,2,5/region")]
        [InlineData("tbl_denormalized_measured_values_33_0:country@1,2,5/tbl_denormalized_measured_values_33_0")]
        [InlineData("tbl_denormalized_measured_values_32:country@1,2,5/tbl_denormalized_measured_values_32")]
        [InlineData("tbl_denormalized_measured_values_37:country@1,2,5/tbl_denormalized_measured_values_37")]
        [InlineData("species:country@1,2,5/species")]
        //[InlineData("country:country@1,2,5/country")]
        [InlineData("sample_groups:country@1,2,5/sample_groups")]
        [InlineData("data_types:country@1,2,5/data_types")]
        [InlineData("rdb_systems:country@1,2,5/rdb_systems")]
        [InlineData("rdb_codes:country@1,2,5/rdb_codes")]
        [InlineData("modification_types:country@1,2,5/modification_types")]
        [InlineData("abundance_elements:country@1,2,5/abundance_elements")]
        [InlineData("sample_group_sampling_contexts:country@1,2,5/sample_group_sampling_contexts")]
        public void Build_WithConcretePickCompilerAndVariousConfigs_GivesExpecteCriteriadCount(string uri)
        {
            // Arrange
            var facetsConfig = FakeFacetsConfig(uri);
            var pickCriterias = new List<string> { "Q1 = Q2", "Q3 = Q4" };
            var mockPicksCompiler = MockPicksFilterCompiler(pickCriterias ?? new List<string>());
            var fakeJoins = FakeJoinsClause(5);
            var mockJoinCompiler = MockJoinsClauseCompiler(fakeJoins);
            var mockRoutes = new List<GraphRoute> {
                FakeRoute2( "A", "B", "C", "D"),
                FakeRoute2( "E", "K")
            };
            var mockFacetsGraph = MockFacetsGraph(mockRoutes);
            var extraTables = new List<string>();

            // Act
            var builder = new QuerySetupBuilder(mockFacetsGraph.Object, mockPicksCompiler.Object, mockJoinCompiler.Object);

            QuerySetup querySetup = builder.Build(facetsConfig, facetsConfig.TargetFacet, extraTables, null);

            Output.WriteLine($"URI: {uri}");
            foreach (var criteria in querySetup.Criterias)
                Output.WriteLine($" criteria: {criteria}");

            //DumpUriObject(uri, querySetup);

            // Assert

            Assert.Same(facetsConfig.TargetConfig, querySetup.TargetConfig);
            Assert.Same(facetsConfig.TargetFacet, querySetup.Facet);
            // Assert.Equal(mockRoutes.Aggregate(0, (i,z) => i + z.Items.Count), querySetup.Joins.Count);
        }

        //[InlineData("result_facet:sites@1/result_facet")]
        //[InlineData("result_facet:tbl_denormalized_measured_values_33_0/result_facet")]
        //[InlineData("result_facet:ecocode/result_facet")]

        [Theory]
        [InlineData("palaeoentomology://sites:country@5/sites", 2)]
        public void Build_WithVariousDomainConfigs_Success(string uri, int expectedConfigCount)
        {
            // Arrange
            var facetsConfig = FakeFacetsConfig(uri);

            Assert.NotEmpty(facetsConfig.DomainCode);

            var pickCriterias = new List<string> { "Q1 = Q2", "Q3 = Q4" };
            var mockPicksCompiler = MockPicksFilterCompiler(pickCriterias ?? new List<string>());
            var fakeJoins = FakeJoinsClause(5);
            var mockJoinCompiler = MockJoinsClauseCompiler(fakeJoins);
            var mockRoutes = new List<GraphRoute> {
                FakeRoute2( "A", "B", "C", "D"),
                FakeRoute2( "E", "K")
            };
            var mockFacetsGraph = MockFacetsGraph(mockRoutes);
            var extraTables = new List<string>();

            // Act
            var builder = new QuerySetupBuilder(mockFacetsGraph.Object, mockPicksCompiler.Object, mockJoinCompiler.Object);

            QuerySetup querySetup = builder.Build(facetsConfig, facetsConfig.TargetFacet, extraTables, null);

            //DumpUriObject(uri, querySetup);

            // Assert
            Assert.Same(facetsConfig.TargetFacet, querySetup.Facet);
            Assert.Equal(expectedConfigCount, facetsConfig.FacetConfigs.Count);
            //Assert.Equal(mockRoutes.Aggregate(0, (i, z) => i + z.Items.Count), querySetup.Joins.Count);
        }

        //private int ExpectedCriteriaCount(FacetsConfig2 facetsConfig)
        //{
        //    var expectedCount = 0;
        //    var involvedConfigs = facetsConfig.GetConfigsThatAffectsTarget(facetsConfig.TargetCode, facetsConfig.GetFacetCodes());

        //    expectedCount += involvedConfigs.Where(z => z.HasPicks()).Count();

        //    /* Add all pick clauses */
        //    expectedCount += involvedConfigs
        //        .Where(z => z.HasPicks())
        //        .SelectMany(z => z.Facet.Clauses).Count();

        //    /* Add facet clauses where constraint is enforced */
        //    expectedCount += involvedConfigs
        //        .Where(z => !z.HasPicks())
        //        .SelectMany(z => z.Facet.Clauses.Where(x => x.EnforceConstraint)).Count();

        //    if (facetsConfig.DomainCode.IsNotEmpty())
        //        expectedCount += facetsConfig.DomainFacet.Clauses.Count;

        //    return expectedCount;

        //}
    }
}
