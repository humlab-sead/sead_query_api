using Moq;
using SeadQueryCore;
using SeadQueryCore.QueryBuilder;
using SQT.Infrastructure;
using SQT.Mocks;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using Xunit;

namespace SQT.QueryBuilder
{

    [Collection("JsonSeededFacetContext")]
    public class QuerySetupBuilderTests : DisposableFacetContextContainer
    {
        public QuerySetupBuilderTests(JsonSeededFacetContextFixture fixture) : base(fixture)
        {
        }

        public Dictionary<string, List<GraphRoute>> FakeFacetQueryResultRoutes()
        {
            return new Dictionary<string, List<GraphRoute>> {

                {
                    "result_facet:sites/result_facet",
                    new List<GraphRoute> {
                        FakeRoute2("tbl_analysis_entities", "tbl_physical_samples", "tbl_sample_groups", "tbl_sites"),
                        FakeRoute2("tbl_analysis_entities", "tbl_datasets")
                    }
                },
                {
                    "result_facet:ecocode/result_facet",
                    new List<GraphRoute> {
                        FakeRoute2("tbl_analysis_entities", "tbl_abundances", "tbl_taxa_tree_master", "tbl_ecocodes", "tbl_ecocode_definitions"),
                        FakeRoute2("tbl_analysis_entities", "tbl_physical_samples"),
                        FakeRoute2("tbl_analysis_entities", "tbl_datasets")
                    }
                },
                {
                    "result_facet:tbl_denormalized_measured_values_33_0/result_facet",
                    new List<GraphRoute> {
                        FakeRoute2( "tbl_analysis_entities", "facet.method_measured_values(33,0)")
                    }
                },
            };
        }

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
        [InlineData("result_facet:sites@1/result_facet", "result_facet:sites/result_facet")]
        [InlineData("result_facet:tbl_denormalized_measured_values_33_0/result_facet", null)]
        [InlineData("result_facet:ecocode/result_facet", null)]
        public void Build_WithVariousConfigs_Success(string uri, string routeKey)
        {
            // Arrange
            var facetsConfig = new MockFacetsConfigFactory(Registry.Facets).Create(uri);
            var mockPickCompilers = MockPickCompilerLocator("SELECT * FROM fot.bar");
            var mockJoinCompiler = MockJoinSqlCompiler("A JOIN B ON A.X = B.Y");
            var mockRoutes = FakeFacetQueryResultRoutes()[routeKey ?? uri];
            var mockFacetsGraph = MockFacetsGraph(mockRoutes);
            var facetCodes = new List<string>() { facetsConfig.TargetCode };
            var extraTables = new List<string>();

            // Act
            var compiler = new QuerySetupBuilder(mockFacetsGraph.Object, mockPickCompilers.Object, mockJoinCompiler.Object);

            QuerySetup querySetup = compiler.Build(facetsConfig, facetsConfig.TargetFacet, extraTables, facetCodes);

            //DumpUriObject(uri, querySetup);

            // Assert

            Assert.Same(facetsConfig.TargetConfig, querySetup.TargetConfig);
            Assert.Same(facetsConfig.TargetFacet, querySetup.Facet);
            Assert.Equal(facetsConfig.FacetConfigs.Where(z => z.HasConstraints()).Count(), querySetup.Criterias.Count);
            Assert.True(AreEqualByProperty(mockRoutes, querySetup.Routes));
            Assert.Equal(mockRoutes.Aggregate(0, (i,z) => i + z.Items.Count), querySetup.Joins.Count);
        }


    }
}
