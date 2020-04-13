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
    public class QuerySetupCompilerTests : DisposableFacetContextContainer
    {
        public object ReconstituteFacetConfigService { get; private set; }
        public MockFacetsConfigFactory FacetsConfigFactory { get; }

        public static List<object[]> RouteTestData = new List<object[]>() {
            new object[] { "sites", new List<List<string>> {
                    new List<string> { "tbl_analysis_entities/tbl_physical_samples", "tbl_physical_samples/tbl_sample_groups", "tbl_sample_groups/tbl_sites" },
                    new List<string> { "tbl_analysis_entities/tbl_datasets" }
                }
            },

            new object[] { "country", new List<List<string>> {
                    new List<string> { "tbl_analysis_entities/tbl_physical_samples", "tbl_physical_samples/tbl_sample_groups", "tbl_sample_groups/tbl_sites", "tbl_sites/tbl_site_locations" },
                    new List<string> { "tbl_site_locations/countries" },
                    new List<string> { "tbl_analysis_entities/tbl_datasets" }
                }
            },

            new object[] { "tbl_denormalized_measured_values_33_0", new List<List<string>> {
                    new List<string> { "tbl_analysis_entities/tbl_physical_samples", "tbl_physical_samples/metainformation.tbl_denormalized_measured_values" }
                }
            }
        };

        private static class TestRoute
        {
            //public List<string> Trail { get; set; }
            //public List<string> Pairs { get { return ToPairs(Trail); } }

            //public TestRoute(List<string> trail) {
            //    Trail = trail;
            //}

            public static List<string> ToPairs(List<string> trail)
            {
                return trail.Take(trail.Count - 1).Select((e, i) => e + "/" + trail[i + 1]).ToList();
            }

            public static List<string> ToPairs(params string[] trail)
            {
                return ToPairs(trail.ToList());
            }
        }

        public static List<object[]> DataCategoryCountQuerySetupForDiscreteFacetWithoutPicks = new List<object[]>() {
                new object[] {
                    "sites:sites", new List<List<string>> {
                        TestRoute.ToPairs("tbl_analysis_entities", "tbl_physical_samples", "tbl_sample_groups", "tbl_sites"),
                        TestRoute.ToPairs("tbl_analysis_entities", "tbl_datasets")
                    }
                },
                new object[] { "country:country", new List<List<string>> {
                        TestRoute.ToPairs("tbl_analysis_entities", "tbl_physical_samples", "tbl_sample_groups", "tbl_sites", "tbl_site_locations"),
                        TestRoute.ToPairs("tbl_site_locations", "countries"),
                        TestRoute.ToPairs("tbl_analysis_entities", "tbl_datasets")
                    }
                },
                new object[] { "ecocode:sites/ecocode", new List<List<string>> {
                        TestRoute.ToPairs("tbl_analysis_entities", "tbl_abundances", "tbl_taxa_tree_master", "tbl_ecocodes", "tbl_ecocode_definitions"),
                        TestRoute.ToPairs("tbl_analysis_entities", "tbl_physical_samples"),
                        TestRoute.ToPairs("tbl_analysis_entities", "tbl_datasets")
                    }
                },
                new object[] { "country:sites/country", new List<List<string>> {
                        TestRoute.ToPairs("tbl_analysis_entities", "tbl_physical_samples", "tbl_sample_groups", "tbl_sites", "tbl_site_locations"),
                        TestRoute.ToPairs("tbl_site_locations", "countries"),
                        TestRoute.ToPairs("tbl_analysis_entities", "tbl_datasets")
                    }
                },
                new object[] { "ecocode:country/sites/ecocode", new List<List<string>> {
                        TestRoute.ToPairs("tbl_analysis_entities", "tbl_abundances", "tbl_taxa_tree_master", "tbl_ecocodes", "tbl_ecocode_definitions"),
                        TestRoute.ToPairs("tbl_analysis_entities", "tbl_physical_samples"),
                        TestRoute.ToPairs("tbl_analysis_entities", "tbl_datasets")
                    }
                },
                new object[] { "sites:country/sites/ecocode", new List<List<string>> {
                        TestRoute.ToPairs("tbl_analysis_entities", "tbl_physical_samples", "tbl_sample_groups", "tbl_sites"),
                        TestRoute.ToPairs("tbl_analysis_entities", "tbl_datasets")
                    }
                }
            };

        public QuerySetupCompilerTests(JsonSeededFacetContextFixture fixture) : base(fixture)
        {
            FacetsConfigFactory = new MockFacetsConfigFactory(Registry.Facets);
        }

        private Facet GetFacet(string facetCode)
        {
            return Registry.Facets.GetByCode(facetCode);
        }

        private IPickFilterCompilerLocator ConcretePickCompilers()
        {
            return new PickFilterCompilerLocator(
                new MockIndex<int, IPickFilterCompiler> {
                    { 1, new DiscreteFacetPickFilterCompiler() },
                    { 2, new RangeFacetPickFilterCompiler() }
                }
            );
        }

        private Mock<IPickFilterCompilerLocator> MockPickCompilers(string returnValue = "")
        {
            var mockPickCompiler = new Mock<IPickFilterCompiler>();

            mockPickCompiler
                .Setup(foo => foo.Compile(It.IsAny<Facet>(), It.IsAny<Facet>(), It.IsAny<FacetConfig2>()))
                .Returns(returnValue);

            var mockLocator = new Mock<IPickFilterCompilerLocator>();

            mockLocator
                .Setup(x => x.Locate(It.IsAny<EFacetType>()))
                .Returns(mockPickCompiler.Object);

            return mockLocator;
        }

        private Mock<IEdgeSqlCompiler> MockEdgeCompiler(string returnValue)
        {
            var mockEdgeCompiler = new Mock<IEdgeSqlCompiler>();
            mockEdgeCompiler
                .Setup(x => x.Compile(
                    It.IsAny<TableRelation>(),
                    It.IsAny<FacetTable>(),
                    It.IsAny<bool>()
                ))
                .Returns(returnValue);
            return mockEdgeCompiler;
        }

        private Mock<IFacetsGraph> MockFacetsGraph()
        {
            throw new NotImplementedException("FacetGraph.Find must be configured!");

            var mockFacetsGraph = new Mock<IFacetsGraph>();

            //mockFacetsGraph
            //    .Setup(x => x.Find());
            return mockFacetsGraph;
        }

        private List<string> GetTargetTables(FacetsConfig2 facetsConfig, Facet computeFacet)
        {
            List<string> tables = facetsConfig.TargetFacet.Tables.Select(x => x.ResolvedAliasOrTableOrUdfName).ToList();

            if (computeFacet.FacetCode != facetsConfig.TargetFacet.FacetCode)
                tables.AddRange(computeFacet.Tables.Select(x => x.ResolvedAliasOrTableOrUdfName).ToList());

            tables = tables.Distinct().ToList();
            return tables;
        }

        private List<string> GetDiscreteTables(FacetsConfig2 facetsConfig, Facet countFacet, Facet targetFacet)
        {
            List<string> tables = targetFacet
                .Tables
                .Select(x => x.ResolvedAliasOrTableOrUdfName)
                .ToList();

            if (countFacet.FacetCode != targetFacet.FacetCode)
                tables.Add(countFacet.TargetTable.ResolvedAliasOrTableOrUdfName);

            return tables.Distinct().ToList();
        }

        //private QuerySetupCompiler CreateQuerySetupCompiler()
        //{
        //    var mockFacetGraph = ScaffoldUtility.DefaultFacetsGraph(Context);
        //    var mockPickCompilers = ConcretePickCompilers();
        //    var edgeCompile = new EdgeSqlCompiler();

        //    var builder = new QuerySetupCompiler(
        //        mockFacetGraph,
        //        mockPickCompilers.Obj,
        //        edgeCompiler
        //    );
        //    return builder;
        //}

        [Theory]
        [InlineData("sites:sites", "sites")]
        public void Build_WhenFacetsConfigIsSingleDiscreteWithoutPicks_ReturnsValidQuerySetup(string uri, string facetCode)
        {
            // Arrange
            var facet = Registry.Facets.GetByCode(facetCode);
            var facetsConfig = FacetsConfigFactory.Create(uri);
            var mockPickCompilers = MockPickCompilers("");
            var mockEdgeCompiler = MockEdgeCompiler("A JOIN B ON A.X = B.Y");
            var mockFacetsGraph = MockFacetsGraph();
            var facetCodes = new List<string>() { facetCode };
            var extraTables = new List<string>();

            // Act
            var compiler = new QuerySetupCompiler(
                mockFacetsGraph.Object,
                mockPickCompilers.Object,
                mockEdgeCompiler.Object
            );

            QuerySetup querySetup = compiler.Build(facetsConfig, facet, extraTables, facetCodes);

            // Assert
            ScaffoldUtility.Dump(querySetup, "");

            Assert.NotNull(querySetup);
            Assert.Equal(facet.TargetTable.HasAlias ? 1 : 0, querySetup.Routes.Count);
        }

        [Theory]
        [InlineData("sites:sites", "sites")]
        [InlineData("country:country", "country")]
        [InlineData("ecocode:ecocode", "ecocode")]
        public void CanBuildCategoryQuerySetupForSingleDiscreteFacetWithoutPicks(string uri, string facetCode)
        {
            // Arrange
            var facet = Registry.Facets.GetByCode(facetCode);
            var facetsConfig = FacetsConfigFactory.Create(uri);
            var mockPickCompilers = MockPickCompilers("");
            var mockEdgeCompiler = MockEdgeCompiler("A JOIN B ON A.X = B.Y");
            var mockFacetsGraph = MockFacetsGraph();
            var facetCodes = new List<string>() { facetCode };
            var extraTables = new List<string>();

            var compiler = new QuerySetupCompiler(
                mockFacetsGraph.Object,
                mockPickCompilers.Object,
                mockEdgeCompiler.Object
            );

            // Act
            QuerySetup querySetup = compiler.Build(facetsConfig, facet, extraTables, facetCodes);

            // Assert
            Assert.NotNull(querySetup);
            Assert.Equal(facet.TargetTable.HasAlias ? 1 : 0, querySetup.Routes.Count);
        }

        [Theory]
        [MemberData(nameof(RouteTestData))]
        public void CanBuildCategoryCountQuerySetupForSingleDiscreteFacetWithoutPicks(string facetCode, List<List<string>> expectedRoutes)
        {

            var fixture = new MockFacetsConfigFactory(Registry.Facets);

            FacetsConfig2 facetsConfig = fixture.CreateSingleFacetsConfigWithoutPicks(facetCode);

            // Arrange
            var facetRepository = fixture.Repository;
            var mockPickCompilers = MockPickCompilers("");
            var mockEdgeCompiler = MockEdgeCompiler("A JOIN B ON A.X = B.Y");
            var mockFacetsGraph = MockFacetsGraph();

            var compiler = new QuerySetupCompiler(
                mockFacetsGraph.Object,
                mockPickCompilers.Object,
                mockEdgeCompiler.Object
            );

            Facet facet = facetRepository.GetByCode(facetCode);
            Facet countFacet = facetRepository.Get(facet.AggregateFacetId); // default to ID 1 = "result_facet"

            string targetCode = Utility.Coalesce(facetsConfig?.TargetCode, countFacet.FacetCode);

            Facet targetFacet = facetRepository.GetByCode(targetCode);
            List<string> tables = GetDiscreteTables(facetsConfig, countFacet, targetFacet);

            List<string> facetCodes = facetsConfig.GetFacetCodes();
            facetCodes.InsertAt(targetFacet.FacetCode, countFacet.FacetCode);

            // Act
            QuerySetup querySetup = compiler.Build(facetsConfig, countFacet, tables, facetCodes);

            // Assert
            var expected = expectedRoutes;
            Assert.NotNull(querySetup);
            Assert.Equal(countFacet.FacetCode, querySetup.Facet.FacetCode);
            for (var i = 0; i < expected.Count; i++)
            {
                Assert.Equal(expected[i].Count, querySetup.Routes[i].Items.Count);
                for (var j = 0; j < expected[i].Count; j++)
                {
                    Assert.Equal(expected[i][j], querySetup.Routes[i].Items[j].ToStringPair());
                }
            }
            // for (var route in querySetup.reduced_route) TestContext.WriteLine(querySetup.reduced_routes.IndexOf(route) + ": " + route.ToString());
        }

        [Theory]
        [MemberData(nameof(DataCategoryCountQuerySetupForDiscreteFacetWithoutPicks))]
        public void CanBuildCategoryCountQuerySetupForDiscreteFacetWithoutPicks(string uri, List<List<string>> expectedRoutes)
        {
            var parts = uri.Split(':').ToList();
            var testCodes = parts[1].Split('/').ToList();
            var targetCode = parts[0];

            // Arrange
            var facetsConfigScaffolder = new MockFacetsConfigFactory(Registry.Facets);

            FacetsConfig2 facetsConfig = facetsConfigScaffolder.Create(
                targetCode,
                targetCode,
                testCodes.Select(
                    z => Mocks.MockFacetConfigFactory.Create(Registry.Facets.GetByCode(z), testCodes.IndexOf(z)
                )
            ).ToList());

            var graph = ScaffoldUtility.DefaultFacetsGraph(FacetContext);

            IEdgeSqlCompiler edgeCompiler = new EdgeSqlCompiler();

            var pickCompilers = MockPickCompilers("");

            IQuerySetupCompiler builder = new QuerySetupCompiler(
                graph,
                pickCompilers.Object,
                edgeCompiler
            );

            Facet targetFacet = Registry.Facets.GetByCode(targetCode);
            Facet computeFacet = Registry.Facets.Get(targetFacet.AggregateFacetId);

            List<string> facetCodes = facetsConfig.GetFacetCodes();

            facetCodes.InsertAt(targetFacet.FacetCode, computeFacet.FacetCode);

            List<string> tables = GetTargetTables(facetsConfig, computeFacet);

            // Act
            QuerySetup querySetup = builder.Build(facetsConfig, computeFacet, tables, facetCodes);

            // Assert

            Assert.NotNull(querySetup);
            Assert.Equal(computeFacet.FacetCode, querySetup.Facet.FacetCode);
            Assert.Equal(expectedRoutes.Count, querySetup.Routes.Count);

            for (var i = 0; i < expectedRoutes.Count; i++) {
                Assert.Equal(expectedRoutes[i].Count, querySetup.Routes[i].Items.Count);
                for (var j = 0; j < expectedRoutes[i].Count; j++) {
                    Assert.Equal(expectedRoutes[i][j], querySetup.Routes[i].Items[j].ToStringPair());
                }
            }
        }
    }
}
