using Moq;
using SeadQueryCore;
using SeadQueryCore.QueryBuilder;
using SeadQueryTest.Infrastructure;
using SeadQueryTest.Mocks;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace SeadQueryTest.QueryBuilder
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
                    "tbl_denormalized_measured_values_33_0:tbl_denormalized_measured_values_33_0",
                    new List<GraphRoute> {
                        MockGraphRoute2("tbl_analysis_entities", "tbl_physical_samples", "metainformation.tbl_denormalized_measured_values")
                    }
                },

                {
                    "sites:sites",
                    new List<GraphRoute> {
                        MockGraphRoute2("tbl_analysis_entities", "tbl_physical_samples", "tbl_sample_groups", "tbl_sites"),
                        MockGraphRoute2("tbl_analysis_entities", "tbl_datasets")
                    }
                },
                {
                    "country:country",
                    new List<GraphRoute> {
                        MockGraphRoute2("tbl_analysis_entities", "tbl_physical_samples", "tbl_sample_groups", "tbl_sites", "tbl_site_locations"),
                        MockGraphRoute2("tbl_site_locations", "countries"),
                        MockGraphRoute2("tbl_analysis_entities", "tbl_datasets")
                    }
                },
                {
                    "ecocode:sites/ecocode",
                    new List<GraphRoute> {
                        MockGraphRoute2("tbl_analysis_entities", "tbl_abundances", "tbl_taxa_tree_master", "tbl_ecocodes", "tbl_ecocode_definitions"),
                        MockGraphRoute2("tbl_analysis_entities", "tbl_physical_samples"),
                        MockGraphRoute2("tbl_analysis_entities", "tbl_datasets")
                    }
                },
                {
                    "country:sites/country",
                    new List<GraphRoute> {
                        MockGraphRoute2("tbl_analysis_entities", "tbl_physical_samples", "tbl_sample_groups", "tbl_sites", "tbl_site_locations"),
                        MockGraphRoute2("tbl_site_locations", "countries"),
                        MockGraphRoute2("tbl_analysis_entities", "tbl_datasets")
                    }
                },
                {
                    "ecocode:country/sites/ecocode",
                    new List<GraphRoute> {
                        MockGraphRoute2("tbl_analysis_entities", "tbl_abundances", "tbl_taxa_tree_master", "tbl_ecocodes", "tbl_ecocode_definitions"),
                        MockGraphRoute2("tbl_analysis_entities", "tbl_physical_samples"),
                        MockGraphRoute2("tbl_analysis_entities", "tbl_datasets")
                    }
                },
                {
                    "sites:country/sites/ecocode",
                    new List<GraphRoute> {
                        MockGraphRoute2("tbl_analysis_entities", "tbl_physical_samples", "tbl_sample_groups", "tbl_sites"),
                        MockGraphRoute2("tbl_analysis_entities", "tbl_datasets")
                    }
                }
            };
        }

        //private List<string> GetTargetTables(FacetsConfig2 facetsConfig, Facet computeFacet)
        //{
        //    List<string> tables = facetsConfig.TargetFacet.Tables.Select(x => x.ResolvedAliasOrTableOrUdfName).ToList();

        //    if (computeFacet.FacetCode != facetsConfig.TargetFacet.FacetCode)
        //        tables.AddRange(computeFacet.Tables.Select(x => x.ResolvedAliasOrTableOrUdfName).ToList());

        //    tables = tables.Distinct().ToList();
        //    return tables;
        //}

        //private List<string> GetDiscreteTables(FacetsConfig2 facetsConfig, Facet countFacet, Facet targetFacet)
        //{
        //    List<string> tables = targetFacet
        //        .Tables
        //        .Select(x => x.ResolvedAliasOrTableOrUdfName)
        //        .ToList();

        //    if (countFacet.FacetCode != targetFacet.FacetCode)
        //        tables.Add(countFacet.TargetTable.ResolvedAliasOrTableOrUdfName);

        //    return tables.Distinct().ToList();
        //}

        [Theory]
        [InlineData("sites:sites")]
        [InlineData("country:country")]
        [InlineData("ecocode:ecocode")]
        public void Build_WhenFacetsConfigIsSingleDiscreteWithoutPicks_ReturnsValidQuerySetup(string uri)
        {
            // Arrange
            var facetsConfig = new MockFacetsConfigFactory(Registry.Facets).Create(uri);
            var mockPickCompilers = MockPickCompilerLocator("SELECT * FROM fot.bar");
            var mockJoinCompiler = MockJoinSqlCompiler("A JOIN B ON A.X = B.Y");
            var mockRoutes = FakeFacetQueryResultRoutes()[uri];
            var mockFacetsGraph = MockFacetsGraph(mockRoutes);
            var facetCodes = new List<string>() { facetsConfig.TargetCode };
            var extraTables = new List<string>();

            // FIXME: We must add a result facet!!!

            //Facet facet = facetRepository.GetByCode(facetCode);
            //Facet countFacet = facetRepository.Get(facet.AggregateFacetId); // default to ID 1 = "result_facet"

            //string targetCode = Utility.Coalesce(facetsConfig?.TargetCode, countFacet.FacetCode);

            //Facet targetFacet = facetRepository.GetByCode(targetCode);
            //List<string> tables = GetDiscreteTables(facetsConfig, countFacet, targetFacet);

            //List<string> facetCodes = facetsConfig.GetFacetCodes();
            //facetCodes.InsertAt(targetFacet.FacetCode, countFacet.FacetCode);

            // Act
            var compiler = new QuerySetupBuilder(
                mockFacetsGraph.Object,
                mockPickCompilers.Object,
                mockJoinCompiler.Object
            );

            QuerySetup querySetup = compiler.Build(facetsConfig, facetsConfig.TargetFacet, extraTables, facetCodes);
            // ScaffoldUtility.Dump(querySetup, "");

            // Assert

            Assert.NotNull(querySetup);

            // FIXME Some relevant assertions!
            
            //var expected = expectedRoutes;
            //Assert.NotNull(querySetup);
            //Assert.Equal(countFacet.FacetCode, querySetup.Facet.FacetCode);
            //for (var i = 0; i < expected.Count; i++) {
            //    Assert.Equal(expected[i].Count, querySetup.Routes[i].Items.Count);
            //    for (var j = 0; j < expected[i].Count; j++) {
            //        Assert.Equal(expected[i][j], querySetup.Routes[i].Items[j].ToStringPair());
            //    }
            //}
            // for (var route in querySetup.reduced_route) TestContext.WriteLine(querySetup.reduced_routes.IndexOf(route) + ": " + route.ToString());
        }
    }
}
