using Autofac;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using QuerySeadDomain;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Newtonsoft.Json.Linq;
using System;
using QuerySeadDomain.QueryBuilder;
using System.Diagnostics;

namespace QuerySeadTests.FacetsConfig
{
    [TestClass]
    public class QueryCompilerTests
    {
        private fixtures.FacetConfigFixture fixture;
        private static IContainer container;
        private TestContext testContextInstance;

        /// <summary>
        ///  Gets or sets the test context which provides
        ///  information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get { return testContextInstance; }
            set { testContextInstance = value; }
        }

        [ClassInitialize()]
        public static void InitializeClass(TestContext context)
        {
            container = new TestDependencyService().Register();
        }

        [TestInitialize()]
        public void Initialize() {
            fixture = new fixtures.FacetConfigFixture();
        }

        [TestMethod]
        public void CanResolveQueryBuilder()
        {
            IContainer container = new TestDependencyService().Register();
            using (var scope = container.BeginLifetimeScope()) {
                IQuerySetupBuilder builder = scope.Resolve<IQuerySetupBuilder>();
                Assert.IsNotNull(builder);
            }
        }

        [TestMethod]
        public void CanBuildCategoryQuerySetupForSingleDiscreteFacetWithoutPicks()
        {
            List<string> testCodes = new List<string>(){ "sites", "country", "ecocode" };
            foreach (var facetCode in testCodes)
            {
                FacetsConfig2 facetsConfig = fixture.GenerateSingleFacetsConfigWithoutPicks(facetCode);
                IContainer container = new TestDependencyService().Register();
                using (var scope = container.BeginLifetimeScope())
                {
                    // Arrange
                    IQuerySetupBuilder builder = scope.Resolve<IQuerySetupBuilder>();
                    IUnitOfWork context = scope.Resolve<IUnitOfWork>();
                    facetsConfig.SetContext(context);

                    // Act

                    QuerySetup querySetup = builder.Build(facetsConfig, facetCode, new List<string>(), new List<string>() { facetCode });

                    // Assert

                    var facet = context.Facets.GetByCode(facetCode);

                    Assert.IsNotNull(querySetup);
                    Assert.AreEqual(facet.HasAliasName ? 1 : 0, querySetup.Routes.Count);
                    Assert.AreEqual(facet.HasAliasName ? 1 : 0, querySetup.ReducedRoutes.Count);

                }
            }
        }

        Dictionary<string, List<List<string>>> singleExpectedRoutes = new Dictionary<string, List<List<string>>> {
            { "sites", new List<List<string>> {
                    new List<string> { "tbl_analysis_entities/tbl_physical_samples", "tbl_physical_samples/tbl_sample_groups", "tbl_sample_groups/tbl_sites" },
                    new List<string> { "tbl_analysis_entities/tbl_datasets" }
                }
            },

            { "country", new List<List<string>> {
                    new List<string> { "tbl_analysis_entities/tbl_physical_samples", "tbl_physical_samples/tbl_sample_groups", "tbl_sample_groups/tbl_sites", "tbl_sites/tbl_site_locations" },
                    new List<string> { "tbl_site_locations/countries" },
                    new List<string> { "tbl_analysis_entities/tbl_datasets" }
                }
            },

            { "tbl_denormalized_measured_values_33_0", new List<List<string>> {
                    new List<string> { "metainformation.tbl_denormalized_measured_values", "tbl_physical_samples", "tbl_analysis_entities" }
                }
            }
        };

        [TestMethod]
        public void CanBuildCategoryCountQuerySetupForSingleDiscreteFacetWithoutPicks()
        {
            var testCodes = singleExpectedRoutes.Keys;
            foreach (var facetCode in singleExpectedRoutes.Keys)
            {
                FacetsConfig2 facetsConfig = fixture.GenerateSingleFacetsConfigWithoutPicks(facetCode);
                IContainer container = new TestDependencyService().Register();
                using (var scope = container.BeginLifetimeScope())
                {
                    // Arrange
                    IQuerySetupBuilder builder = scope.Resolve<IQuerySetupBuilder>();
                    IUnitOfWork context = scope.Resolve<IUnitOfWork>();
                    facetsConfig.SetContext(context);

                    FacetDefinition facet = context.Facets.GetByCode(facetCode);
                    FacetDefinition countFacet = context.Facets.Get(facet.AggregateFacetId); // default to ID 1 = "result_facet"

                    string targetCode = Utility.Coalesce(facetsConfig?.TargetCode, countFacet.FacetCode);

                    FacetDefinition targetFacet = context.Facets.GetByCode(targetCode);
                    List<string> tables = GetDiscreteTables(facetsConfig, countFacet, targetFacet);

                    List<string> facetCodes = facetsConfig.GetFacetCodes();
                    facetCodes.MyInsertBeforeItem(targetFacet.FacetCode, countFacet.FacetCode);

                    // Act
                    QuerySetup querySetup = builder.Build(facetsConfig, countFacet.FacetCode, tables, facetCodes);

                    // Assert
                    var expected = singleExpectedRoutes[facetCode];
                    Assert.IsNotNull(querySetup);
                    Assert.AreEqual(countFacet.FacetCode, querySetup.Facet.FacetCode);
                    for (var i = 0; i < expected.Count; i++)
                    {
                        Assert.AreEqual(expected[i].Count, querySetup.ReducedRoutes[i].Items.Count);
                        for (var j = 0; j < expected[i].Count; j++)
                        {
                            Assert.AreEqual(expected[i][j], querySetup.ReducedRoutes[i].Items[j].ToStringPair());
                        }
                    }
                    // for (var route in querySetup.reduced_route) TestContext.WriteLine(querySetup.reduced_routes.IndexOf(route) + ": " + route.ToString());

                }
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

        [TestMethod]
        public void CanBuildCategoryCountQuerySetupForDiscreteFacetWithoutPicks()
        {
            Dictionary<string, List<List<string>>> expectedRoutes = new Dictionary<string, List<List<string>>> {
                { "sites:sites", new List<List<string>> {
                        TestRoute.ToPairs("tbl_analysis_entities", "tbl_physical_samples", "tbl_sample_groups", "tbl_sites"),
                        TestRoute.ToPairs("tbl_analysis_entities", "tbl_datasets")
                    }
                },
                { "country:country", new List<List<string>> {
                        TestRoute.ToPairs("tbl_analysis_entities", "tbl_physical_samples", "tbl_sample_groups", "tbl_sites", "tbl_site_locations"),
                        TestRoute.ToPairs("tbl_site_locations", "countries"),
                        TestRoute.ToPairs("tbl_analysis_entities", "tbl_datasets")
                    }
                },
                { "ecocode:sites/ecocode", new List<List<string>> {
                        TestRoute.ToPairs("tbl_analysis_entities", "tbl_abundances", "tbl_taxa_tree_master", "tbl_ecocodes", "tbl_ecocode_definitions"),
                        TestRoute.ToPairs("tbl_analysis_entities", "tbl_physical_samples"),
                        TestRoute.ToPairs("tbl_analysis_entities", "tbl_datasets")
                    }
                },

                { "country:sites/country", new List<List<string>> {
                        TestRoute.ToPairs("tbl_analysis_entities", "tbl_physical_samples", "tbl_sample_groups", "tbl_sites", "tbl_site_locations"),
                        TestRoute.ToPairs("tbl_site_locations", "countries"),
                        TestRoute.ToPairs("tbl_analysis_entities", "tbl_datasets")
                    }
                },

                { "ecocode:country/sites/ecocode", new List<List<string>> {
                        TestRoute.ToPairs("tbl_analysis_entities", "tbl_abundances", "tbl_taxa_tree_master", "tbl_ecocodes", "tbl_ecocode_definitions"),
                        TestRoute.ToPairs("tbl_analysis_entities", "tbl_physical_samples"),
                        TestRoute.ToPairs("tbl_analysis_entities", "tbl_datasets")
                    }
                },
                { "sites:country/sites/ecocode", new List<List<string>> {
                        TestRoute.ToPairs("tbl_analysis_entities", "tbl_physical_samples", "tbl_sample_groups", "tbl_sites"),
                        TestRoute.ToPairs("tbl_analysis_entities", "tbl_datasets")
                    }
                }
            };
            foreach (var key in expectedRoutes.Keys)
            {
                var parts = key.Split(':').ToList();
                var testCodes = parts[1].Split('/').ToList();
                var targetCode = parts[0];
                FacetsConfig2 facetsConfig = fixture.GenerateFacetsConfig(targetCode, targetCode, testCodes.Select(z => fixture.GenerateFacetConfig(z, testCodes.IndexOf(z))).ToList());
                IContainer container = new TestDependencyService().Register();
                using (var scope = container.BeginLifetimeScope())
                {
                    // Arrange
                    IQuerySetupBuilder builder = scope.Resolve<IQuerySetupBuilder>();
                    IUnitOfWork context = scope.Resolve<IUnitOfWork>();
                    facetsConfig.SetContext(context);

                    FacetDefinition targetFacet = context.Facets.GetByCode(targetCode);
                    FacetDefinition computeFacet = targetFacet;
                    List<string> facetCodes = facetsConfig.GetFacetCodes();
                    List<string> tables = targetFacet.ExtraTables.Select(x => x.TableName).ToList();

                    if (true)
                    {
                        computeFacet = context.Facets.Get(targetFacet.AggregateFacetId);
                        facetCodes.MyInsertBeforeItem(targetFacet.FacetCode, computeFacet.FacetCode);
                    }

                    if (facetsConfig.TargetCode != null)
                        tables.Add(targetFacet.ResolvedName);

                    if (computeFacet.FacetCode != targetFacet.FacetCode)
                        tables.Add(computeFacet.TargetTableName);

                    tables = tables.Distinct().ToList();
                    // Act
                    QuerySetup querySetup = builder.Build(facetsConfig, computeFacet.FacetCode, tables, facetCodes);

                    // Assert
                    Assert.IsNotNull(querySetup, key);
                    var expected = expectedRoutes[key];
                    Assert.AreEqual(computeFacet.FacetCode, querySetup.Facet.FacetCode, key);
                    Assert.AreEqual(expected.Count, querySetup.ReducedRoutes.Count, key);
                    for (var i = 0; i < expected.Count; i++)
                    {
                        Assert.AreEqual(expected[i].Count, querySetup.ReducedRoutes[i].Items.Count, key);
                        for (var j = 0; j < expected[i].Count; j++)
                        {
                            Assert.AreEqual(expected[i][j], querySetup.ReducedRoutes[i].Items[j].ToStringPair(), key);
                        }
                    }
                    // for (var route in querySetup.reduced_route) TestContext.WriteLine(querySetup.reduced_routes.IndexOf(route) + ": " + route.ToString());

                }
            }
        }
        private static List<string> GetDiscreteTables(FacetsConfig2 facetsConfig, FacetDefinition countFacet, FacetDefinition targetFacet)
        {
            List<string> tables = targetFacet.ExtraTables.Select(x => x.TableName).ToList();

            if (facetsConfig.TargetCode != null)
                tables.Add(targetFacet.ResolvedName);

            if (countFacet.FacetCode != targetFacet.FacetCode)
                tables.Add(countFacet.TargetTableName);

            return tables.Distinct().ToList();
        }

        
    }
}
