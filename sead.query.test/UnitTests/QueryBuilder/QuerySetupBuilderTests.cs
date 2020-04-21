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

    [Collection("JsonSeededFacetContext")]
    public class QuerySetupBuilderTests : DisposableFacetContextContainer
    {
        public QuerySetupBuilderTests(JsonSeededFacetContextFixture fixture, ITestOutputHelper output) : base(fixture)
        {
            Output = output;
        }

        private readonly ITestOutputHelper Output;

        [Fact]
        public void SkitTest ()
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
        [InlineData("sites@sites:country@1,2,3/sites")]
        [InlineData("sites@country:country@1,2,3/sites")]
        public void Build_WithConcretePickCompilerAndVariousConfigs_GivesExpecteCriteriadCount(string uri)
        {
            // Arrange
            var facetsConfig = FakeFacetsConfig(uri);
            var mockPickCompilers = MockConcretePickCompilerLocator();
            var mockJoinCompiler = MockJoinSqlCompiler("==JOIN<==");
            var mockRoutes = new List<GraphRoute> {
                FakeRoute2( "A", "B", "C", "D"),
                FakeRoute2( "E", "K")
            };
            var mockFacetsGraph = MockFacetsGraph(mockRoutes);
            var extraTables = new List<string>();

            // Act
            var builder = new QuerySetupBuilder(mockFacetsGraph.Object, mockPickCompilers.Object, mockJoinCompiler.Object);

            QuerySetup querySetup = builder.Build(facetsConfig, facetsConfig.TargetFacet, extraTables, null);

            Output.WriteLine($"URI: {uri}");
            foreach (var criteria in querySetup.Criterias)
                Output.WriteLine($" criteria: {criteria}");

            //DumpUriObject(uri, querySetup);

            // Assert
            var expectedCriteriaCount = ExpectedCriteriaCount(facetsConfig);
            var resultPickCriteriaCount = querySetup.Criterias.Count;

            Assert.Same(facetsConfig.TargetConfig, querySetup.TargetConfig);
            Assert.Same(facetsConfig.TargetFacet, querySetup.Facet);
            Assert.Equal(expectedCriteriaCount, resultPickCriteriaCount);
            Assert.True(AreEqualByProperty(mockRoutes, querySetup.Routes));
            Assert.Equal(mockRoutes.Aggregate(0, (i,z) => i + z.Items.Count), querySetup.Joins.Count);
        }

        //[InlineData("result_facet:sites@1/result_facet")]
        //[InlineData("result_facet:tbl_denormalized_measured_values_33_0/result_facet")]
        //[InlineData("result_facet:ecocode/result_facet")]

        [Theory]
        [InlineData("palaeoentomology://sites:country@5/sites", 2)]
        public void Build_WithVariousDomainConfigs_Success(string uri, int expectedConfigCount)
        {
            //"palaeoentomology"
            //"archaeobotany"
            //"pollen"
            //"geoarchaeology"
            //"dendrochronology"
            //"ceramic"
            //"isotope"
            // Arrange
            var facetsConfig = FakeFacetsConfig(uri);

            Assert.NotEmpty(facetsConfig.DomainCode);
            var mockPickCompilers = MockConcretePickCompilerLocator();
            var mockJoinCompiler = MockJoinSqlCompiler("==>JOIN<== ");
            var mockRoutes = new List<GraphRoute> {
                FakeRoute2( "A", "B", "C", "D"),
                FakeRoute2( "E", "K")
            };
            var mockFacetsGraph = MockFacetsGraph(mockRoutes);
            var extraTables = new List<string>();

            // Act
            var builder = new QuerySetupBuilder(mockFacetsGraph.Object, mockPickCompilers.Object, mockJoinCompiler.Object);

            QuerySetup querySetup = builder.Build(facetsConfig, facetsConfig.TargetFacet, extraTables, null);

            //DumpUriObject(uri, querySetup);

            // Assert

            var expectedCriteriaCount = ExpectedCriteriaCount(facetsConfig);

            Assert.Same(facetsConfig.TargetConfig, querySetup.TargetConfig);
            Assert.Same(facetsConfig.TargetFacet, querySetup.Facet);
            Assert.Equal(expectedConfigCount, facetsConfig.FacetConfigs.Count);
            Assert.Equal(expectedCriteriaCount, querySetup.Criterias.Count);
            Assert.True(AreEqualByProperty(mockRoutes, querySetup.Routes));
            Assert.Equal(mockRoutes.Aggregate(0, (i, z) => i + z.Items.Count), querySetup.Joins.Count);
        }

        private int ExpectedCriteriaCount(FacetsConfig2 facetsConfig)
        {
 
            var expectedCount = 0;
            var involvedConfigs = facetsConfig.GetConfigsThatAffectsTarget(facetsConfig.TargetCode, facetsConfig.GetFacetCodes());

            expectedCount += involvedConfigs.Where(z => z.HasPicks()).Count();

            /* Add all pick clauses */
            expectedCount += involvedConfigs
                .Where(z => z.HasPicks())
                .SelectMany(z => z.Facet.Clauses).Count();

            /* Add facet clauses where constraint is enforced */
            expectedCount += involvedConfigs
                .Where(z => !z.HasPicks())
                .SelectMany(z => z.Facet.Clauses.Where(x => x.EnforceConstraint)).Count();

            if (facetsConfig.DomainCode.IsNotEmpty())
                expectedCount += facetsConfig.DomainFacet.Clauses.Count;

            return expectedCount;

        }
    }
}
