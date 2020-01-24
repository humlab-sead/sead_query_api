using Autofac;
using Autofac.Features.Indexed;
using SeadQueryInfra.DataAccessProvider;
using Moq;
using SeadQueryCore;
using SeadQueryCore.QueryBuilder;
using SeadQueryInfra;
using SeadQueryTest;
using SeadQueryTest.Infrastructure;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using SeadQueryTest.Fixtures;
using System;
using SeadQueryTest.Mocks;

namespace SeadQueryTest.QueryBuilder
{

    public class QuerySetupCompilerTests : IDisposable
    {
        private RepositoryRegistry fakeRegistry;
        private IFacetContext fakeContext;
        public object ReconstituteFacetConfigService { get; private set; }

        public QuerySetupCompilerTests()
        {
            fakeContext = JsonSeededFacetContextFactory.Create();
            fakeRegistry = new RepositoryRegistry(fakeContext);
        }

        public void Dispose()
        {
            fakeRegistry.Dispose();
            fakeContext.Dispose();
        }

        private FacetsConfig2 CreateSingleFacetsConfig(string facetCode)
        {
            var facetsConfig = new FacetsConfig2 {
                RequestId = "1",
                Language = "",
                RequestType = "populate",
                TargetCode = facetCode,
                TriggerCode = facetCode,
                FacetConfigs = new List<FacetConfig2>()
                {
                    new FacetConfig2
                    {
                        FacetCode = facetCode,
                        Position = 1,
                        TextFilter = "",
                        Picks = new List<FacetConfigPick> { },
                        Facet = GetFacet(facetCode)
                    }
                },
                InactiveConfigs = null,
                TargetFacet = GetFacet(facetCode),
                TriggerFacet = GetFacet(facetCode)
            };
            return facetsConfig;
        }

        private Facet GetFacet(string facetCode)
        {
            return FacetFixtures.Store[facetCode];
        }

        private IIndex<int, IPickFilterCompiler> ConcretePickCompilers()
        {
            return new MockIndex<int, IPickFilterCompiler>
            {
                    { 1, new DiscreteFacetPickFilterCompiler() },
                    { 2, new RangeFacetPickFilterCompiler() }
            };
        }

        private IIndex<int, IPickFilterCompiler> MockPickCompilers(string returnValue = "")
        {
            var mockPickCompiler = new Mock<IPickFilterCompiler>();
            mockPickCompiler.Setup(foo => foo.Compile(It.IsAny<Facet>(), It.IsAny<Facet>(), It.IsAny<FacetConfig2>())).Returns(returnValue);

            return new MockIndex<int, IPickFilterCompiler>
            {
                    { 1, mockPickCompiler.Object },
                    { 2, mockPickCompiler.Object }
            };
        }

        private QuerySetupCompiler CreateQuerySetupCompiler()
        {
            var mockFacetGraph = ScaffoldUtility.DefaultFacetsGraph(fakeContext);
            var pickCompilers = ConcretePickCompilers();
            var edgeCompiler = new EdgeSqlCompiler();

            var builder = new QuerySetupCompiler(
                mockFacetGraph,
                pickCompilers,
                edgeCompiler
            );
            return builder;
        }

        [Fact]
        public void Build_WhenFacetsConfigIsSingleDiscreteWithoutPicks_ReturnsValidQuerySetup()
        {
            // Arrange
            var facetCode = "sites";
            var facet = FacetFixtures.Store[facetCode];

            var facetsConfig = new FacetsConfig2 {
                RequestId = "1",
                Language = "",
                RequestType = "populate",
                TargetCode = facetCode,
                TriggerCode = facetCode,
                FacetConfigs = new List<FacetConfig2>() {
                    new FacetConfig2
                    {
                        FacetCode = facetCode,
                        Position = 1,
                        TextFilter = "",
                        Picks = new List<FacetConfigPick> { },
                        Facet = facet
                    }
                },
                InactiveConfigs = null,
                TargetFacet = facet,
                TriggerFacet = facet
            };

            QuerySetupCompiler builder = CreateQuerySetupCompiler();

            // Act
            QuerySetup querySetup = builder.Build(
                facetsConfig, facet, new List<string>(), new List<string>() { facetCode });

            // Assert
            ScaffoldUtility.Dump(querySetup, "");

            Assert.NotNull(querySetup);
            Assert.Equal(facet.TargetTable.HasAlias ? 1 : 0, querySetup.Routes.Count);
        }

        [Theory]
        [InlineData("sites")]
        [InlineData("country")]
        [InlineData("ecocode")]
        public void CanBuildCategoryQuerySetupForSingleDiscreteFacetWithoutPicks(string facetCode)
        {
            // Arrange
            var fixture = new FacetsConfigFactory(fakeRegistry);
            var facet = fixture.Registry.Facets.GetByCode(facetCode);
            var facetsConfig = fixture.CreateSingleFacetsConfigWithoutPicks(facetCode);

            QuerySetupCompiler builder = CreateQuerySetupCompiler();

            // Act
            QuerySetup querySetup = builder.Build(facetsConfig, facet, new List<string>(), new List<string>() { facetCode });

            // Assert
            Assert.NotNull(querySetup);
            Assert.Equal(facet.TargetTable.HasAlias ? 1 : 0, querySetup.Routes.Count);
        }

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

        [Theory]
        [MemberData(nameof(RouteTestData))]
        public void CanBuildCategoryCountQuerySetupForSingleDiscreteFacetWithoutPicks(string facetCode, List<List<string>> expectedRoutes)
        {

            var fixture = new FacetsConfigFactory(fakeRegistry);

            FacetsConfig2 facetsConfig = fixture.CreateSingleFacetsConfigWithoutPicks(facetCode);

            // Arrange
            var registry = fixture.Registry;
            var factory = new FacetGraphFactory(registry);

            QuerySetupCompiler builder = CreateQuerySetupCompiler();

            Facet facet = registry.Facets.GetByCode(facetCode);
            Facet countFacet = registry.Facets.Get(facet.AggregateFacetId); // default to ID 1 = "result_facet"

            string targetCode = Utility.Coalesce(facetsConfig?.TargetCode, countFacet.FacetCode);

            Facet targetFacet = registry.Facets.GetByCode(targetCode);
            List<string> tables = GetDiscreteTables(facetsConfig, countFacet, targetFacet);

            List<string> facetCodes = facetsConfig.GetFacetCodes();
            facetCodes.MyInsertBeforeItem(targetFacet.FacetCode, countFacet.FacetCode);

            // Act
            QuerySetup querySetup = builder.Build(facetsConfig, countFacet, tables, facetCodes);

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

        private static class TestRoute
        {
            //public List<string> Trail { get; set; }
            //public List<string> Pairs { get { return ToPairs(Trail); } }

            //public TestRoute(List<string> trail) {
            //    Trail = trail;
            //}

            public static List<string>  ToPairs(List<string> trail) {
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


        [Theory]
        [MemberData(nameof(DataCategoryCountQuerySetupForDiscreteFacetWithoutPicks))]
        public void CanBuildCategoryCountQuerySetupForDiscreteFacetWithoutPicks(string uri, List<List<string>> expectedRoutes)
        {
            var parts = uri.Split(':').ToList();
            var testCodes = parts[1].Split('/').ToList();
            var targetCode = parts[0];

            // Arrange
            var facetsConfigScaffolder = new FacetsConfigFactory(fakeRegistry);

            FacetsConfig2 facetsConfig = facetsConfigScaffolder.CreateFacetsConfig(
                targetCode,
                targetCode,
                testCodes.Select(
                    z => FacetConfigFactory.Create(fakeRegistry.Facets.GetByCode(z), testCodes.IndexOf(z)
                )
            ).ToList());

            var graph = ScaffoldUtility.DefaultFacetsGraph(fakeContext);

            IEdgeSqlCompiler edgeCompiler = new EdgeSqlCompiler();

            IIndex<int, IPickFilterCompiler> pickCompilers = MockPickCompilers("");

            IQuerySetupCompiler builder = new QuerySetupCompiler(
                graph,
                pickCompilers,
                edgeCompiler
            );

            Facet targetFacet = fakeRegistry.Facets.GetByCode(targetCode);
            Facet computeFacet = fakeRegistry.Facets.Get(targetFacet.AggregateFacetId);

            List<string> facetCodes = facetsConfig.GetFacetCodes();

            facetCodes.MyInsertBeforeItem(targetFacet.FacetCode, computeFacet.FacetCode);

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

        private static List<string> GetTargetTables(FacetsConfig2 facetsConfig, Facet computeFacet)
        {
            List<string> tables = facetsConfig.TargetFacet.Tables.Select(x => x.ResolvedAliasOrTableOrUdfName).ToList();

            if (computeFacet.FacetCode != facetsConfig.TargetFacet.FacetCode)
                tables.AddRange(computeFacet.Tables.Select(x => x.ResolvedAliasOrTableOrUdfName).ToList());

            tables = tables.Distinct().ToList();
            return tables;
        }

        private static List<string> GetDiscreteTables(FacetsConfig2 facetsConfig, Facet countFacet, Facet targetFacet)
        {
            List<string> tables = targetFacet
                .Tables
                .Select(x => x.ResolvedAliasOrTableOrUdfName)
                .ToList();

            if (countFacet.FacetCode != targetFacet.FacetCode)
                tables.Add(countFacet.TargetTable.ResolvedAliasOrTableOrUdfName);

            return tables.Distinct().ToList();
        }
    }
}
