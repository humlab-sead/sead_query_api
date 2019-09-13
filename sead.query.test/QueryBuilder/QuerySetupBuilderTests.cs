using Autofac;
using Autofac.Features.Indexed;
using Moq;
using SeadQueryCore;
using SeadQueryCore.QueryBuilder;
using SeadQueryTest.Infrastructure;
using SeadQueryTest.Infrastructure.Scaffolding;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace SeadQueryTest2.QueryCompilerTests
{
    public class QueryCompilerTests
    {

        [Fact]
        public void CanResolveQueryBuilder()
        {
            using (var context = ScaffoldUtility.DefaultFacetContext())
            using (var container = new TestDependencyService(context).Register())
            using (var scope = container.BeginLifetimeScope()) {
                IQuerySetupBuilder builder = scope.Resolve<IQuerySetupBuilder>();
                Assert.NotNull(builder);
            }
        }

        [Theory]
        [InlineData("sites")]
        [InlineData("country")]
        [InlineData("ecocode")]
        public void CanBuildCategoryQuerySetupForSingleDiscreteFacetWithoutPicks(string facetCode)
        {
            using (var context = ScaffoldUtility.DefaultFacetContext())
            using (var container = new TestDependencyService(context).Register())
            using (var scope = container.BeginLifetimeScope()) {

                var fixture = new SeadQueryTest.fixtures.FacetConfigGenerator(container, context);

                FacetsConfig2 facetsConfig = fixture.GenerateSingleFacetsConfigWithoutPicks(facetCode);
                // Arrange
                IQuerySetupBuilder builder = scope.Resolve<IQuerySetupBuilder>();
                facetsConfig.SetContext(fixture.RepositoryRegistry);

                // Act

                QuerySetup querySetup = builder.Build(facetsConfig, facetCode, new List<string>(), new List<string>() { facetCode });

                // Assert

                var facet = fixture.RepositoryRegistry.Facets.GetByCode(facetCode);

                Assert.NotNull(querySetup);
                Assert.Equal(facet.HasAliasName ? 1 : 0, querySetup.Routes.Count);
                Assert.Equal(facet.HasAliasName ? 1 : 0, querySetup.ReducedRoutes.Count);

            }
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
        internal class MockIndex<T, T1> : Dictionary<T, T1>, IIndex<T, T1>
        {
        }

        [Theory]
        [MemberData(nameof(RouteTestData))]
        public void CanBuildCategoryCountQuerySetupForSingleDiscreteFacetWithoutPicks(string facetCode, List<List<string>> expectedRoutes)
        {
            using (var context = ScaffoldUtility.DefaultFacetContext())
            using (var container = new TestDependencyService(context).Register())
            using (var scope = container.BeginLifetimeScope()) {

                var fixture = new SeadQueryTest.fixtures.FacetConfigGenerator(container, context);

                FacetsConfig2 facetsConfig = fixture.GenerateSingleFacetsConfigWithoutPicks(facetCode);

                // Arrange
                IRepositoryRegistry registry = fixture.RepositoryRegistry;
                IFacetsGraph graph = new FacetGraphFactory(registry).Build();
                IEdgeSqlCompiler edgeCompiler = new EdgeSqlCompiler();

                var mockPickCompiler = new Mock<IPickFilterCompiler>();
                mockPickCompiler.Setup(
                    foo => foo.Compile(It.IsAny<Facet>(), It.IsAny<Facet>(), It.IsAny<FacetConfig2>())
                ).Returns("");

                IIndex<int, IPickFilterCompiler> pickCompilers = new MockIndex<int, IPickFilterCompiler>
                {
                    { 1, mockPickCompiler.Object /* new DiscreteFacetPickFilterCompiler() */},
                    { 2, mockPickCompiler.Object /* new RangeFacetPickFilterCompiler() */ }
                };

                IQuerySetupBuilder builder = new QuerySetupBuilder(
                    registry,
                    graph,
                    pickCompilers,
                    edgeCompiler
                );
                facetsConfig.SetContext(registry);

                Facet facet = registry.Facets.GetByCode(facetCode);
                Facet countFacet = registry.Facets.Get(facet.AggregateFacetId); // default to ID 1 = "result_facet"

                string targetCode = Utility.Coalesce(facetsConfig?.TargetCode, countFacet.FacetCode);

                Facet targetFacet = registry.Facets.GetByCode(targetCode);
                List<string> tables = GetDiscreteTables(facetsConfig, countFacet, targetFacet);

                List<string> facetCodes = facetsConfig.GetFacetCodes();
                facetCodes.MyInsertBeforeItem(targetFacet.FacetCode, countFacet.FacetCode);

                // Act
                QuerySetup querySetup = builder.Build(facetsConfig, countFacet.FacetCode, tables, facetCodes);

                // Assert
                var expected = expectedRoutes;
                Assert.NotNull(querySetup);
                Assert.Equal(countFacet.FacetCode, querySetup.Facet.FacetCode);
                for (var i = 0; i < expected.Count; i++)
                {
                    Assert.Equal(expected[i].Count, querySetup.ReducedRoutes[i].Items.Count);
                    for (var j = 0; j < expected[i].Count; j++)
                    {
                        Assert.Equal(expected[i][j], querySetup.ReducedRoutes[i].Items[j].ToStringPair());
                    }
                }
                // for (var route in querySetup.reduced_route) TestContext.WriteLine(querySetup.reduced_routes.IndexOf(route) + ": " + route.ToString());
            }
        }

        class TestRoute
        {
            public List<string> Trail { get; set; }
            public List<string> Pairs { get { return ToPairs(Trail); } }
            public TestRoute(List<string> trail) {
                Trail = trail;
            }
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
 
            using (var context = ScaffoldUtility.DefaultFacetContext())
            using (var container = new TestDependencyService(context).Register())
            using (var scope = container.BeginLifetimeScope()) {

                var fixture = new SeadQueryTest.fixtures.FacetConfigGenerator(container, context);
                FacetsConfig2 facetsConfig = fixture.GenerateFacetsConfig(targetCode, targetCode, testCodes.Select(z => fixture.GenerateFacetConfig(z, testCodes.IndexOf(z))).ToList());

                // Arrange
                IFacetsGraph graph = new FacetGraphFactory(fixture.RepositoryRegistry).Build();
                IEdgeSqlCompiler edgeCompiler = new EdgeSqlCompiler();

                var mockPickCompiler = new Mock<IPickFilterCompiler>();
                mockPickCompiler.Setup(foo => foo.Compile(It.IsAny<Facet>(), It.IsAny<Facet>(), It.IsAny<FacetConfig2>())).Returns("");

                IIndex<int, IPickFilterCompiler> pickCompilers = new MockIndex<int, IPickFilterCompiler>
                {
                    { 1, mockPickCompiler.Object },
                    { 2, mockPickCompiler.Object }
                };

                IQuerySetupBuilder builder = new QuerySetupBuilder(
                    fixture.RepositoryRegistry,
                    graph,
                    pickCompilers,
                    edgeCompiler
                );
                facetsConfig.SetContext(fixture.RepositoryRegistry);

                Facet targetFacet = fixture.RepositoryRegistry.Facets.GetByCode(targetCode);
                Facet computeFacet = targetFacet;
                List<string> facetCodes = facetsConfig.GetFacetCodes();
                List<string> tables = targetFacet.ExtraTables.Select(x => x.ObjectName).ToList();

                if (true)
                {
                    computeFacet = fixture.RepositoryRegistry.Facets.Get(targetFacet.AggregateFacetId);
                    facetCodes.MyInsertBeforeItem(targetFacet.FacetCode, computeFacet.FacetCode);
                }

                if (facetsConfig.TargetCode != null)
                    tables.Add(targetFacet.ResolvedName);

                if (computeFacet.FacetCode != targetFacet.FacetCode)
                    tables.Add(computeFacet.TargetTable.ObjectName);

                tables = tables.Distinct().ToList();
                // Act
                QuerySetup querySetup = builder.Build(facetsConfig, computeFacet.FacetCode, tables, facetCodes);

                // Assert
                Assert.NotNull(querySetup);
                Assert.Equal(computeFacet.FacetCode, querySetup.Facet.FacetCode);
                Assert.Equal(expectedRoutes.Count, querySetup.ReducedRoutes.Count);
                for (var i = 0; i < expectedRoutes.Count; i++)
                {
                    Assert.Equal(expectedRoutes[i].Count, querySetup.ReducedRoutes[i].Items.Count);
                    for (var j = 0; j < expectedRoutes[i].Count; j++)
                    {
                        Assert.Equal(expectedRoutes[i][j], querySetup.ReducedRoutes[i].Items[j].ToStringPair());
                    }
                }
                // for (var route in querySetup.reduced_route) TestContext.WriteLine(querySetup.reduced_routes.IndexOf(route) + ": " + route.ToString());

            }
        }
        private static List<string> GetDiscreteTables(FacetsConfig2 facetsConfig, Facet countFacet, Facet targetFacet)
        {
            List<string> tables = targetFacet.ExtraTables.Select(x => x.ObjectName).ToList();

            if (facetsConfig.TargetCode != null)
                tables.Add(targetFacet.ResolvedName);

            if (countFacet.FacetCode != targetFacet.FacetCode)
                tables.Add(countFacet.TargetTable.ObjectName);

            return tables.Distinct().ToList();
        }


    }
}
