using Autofac;
using Moq;
using Newtonsoft.Json;
using SeadQueryCore;
using SeadQueryCore.QueryBuilder;
using SeadQueryInfra;
using SeadQueryTest.Fixtures;
using SeadQueryTest.Infrastructure;
using SeadQueryTest.Mocks;
using System;
using System.Collections.Generic;
using Xunit;

namespace SeadQueryTest.Services.FacetContent
{
    [Collection("JsonSeededFacetContext")]
    public class FacetContentServiceTests : DisposableFacetContextContainer
    {
        public FacetContentServiceTests(JsonSeededFacetContextFixture fixture) : base(fixture)
        {
        }

        private Facet GetFacet(string facetCode)
        {
            return Registry.Facets.GetByCode(facetCode);
        }

        //private FacetContentService CreateService(QuerySetup querySetup)
        //{
        //    var mockSettings = new SettingFactory().Create().Value.Facet;
        //    var mockQuerySetupBuilder = new Mock<IQuerySetupCompiler>();
        //    mockQuerySetupBuilder.Setup(
        //        x => x.Build(It.IsAny<FacetsConfig2>(), It.IsAny<Facet>(), It.IsAny<List<string>>())
        //    ).Returns(querySetup);
        //    mockQuerySetupBuilder.Setup(
        //        x => x.Build(It.IsAny<FacetsConfig2>(), It.IsAny<Facet>(), It.IsAny<List<string>>(), It.IsAny<List<string>>())
        //    ).Returns(querySetup);

        //    return new FacetContentService(
        //        mockSettings,
        //        Registry,
        //        mockQuerySetupBuilder.Object
        //   );
        //}

        //[Fact(Skip = "Not implemented")]
        //public void Load_StateUnderTest_ExpectedBehavior()
        //{
        //    // Arrange
        //    var querySetup = QuerySetupFixtures.Store["measured_values_33(3,52)"];
        //    var service = this.CreateService(querySetup);
        //    FacetsConfig2 facetsConfig = null;

        //    // Act
        //    var result = service.Load(
        //        facetsConfig);

        //    // Assert
        //    Assert.True(false);
        //}

        // FIXME: Hardcoded counts can only be used for if fixed value data is used during testing
        public static Dictionary<string, int> __DiscreteFacetComputeCount = new Dictionary<string, int> {
             { "species", 4904 },
             { "tbl_biblio_modern", 3460 },
             { "relative_age_name", 388 },
             { "record_types", 19 },
             { "sample_groups", 2195 },
             { "sites", 1544 },
             { "country", 260 },
             { "ecocode", 158 },
             { "family", 529 },
             { "genus", 3951 },
             { "species_author", 3101 },
             { "feature_type", 41 },
             { "ecocode_system", 3 },
             //{ "abundance_classification", 0 },
             { "activeseason", 18 },
             { "tbl_biblio_sample_groups", 2344 }
            };

        public Dictionary<string, int> DiscreteFacetComputeCount { get { return __DiscreteFacetComputeCount; } }


        //[Theory]
        //[InlineData("species", 4904)]
        //[InlineData("tbl_biblio_modern", 3460)]
        //[InlineData("relative_age_name", 388)]
        //[InlineData("record_types", 19)]
        //[InlineData("sample_groups", 2195)]
        //[InlineData("sites", 1544)]
        //[InlineData("country", 260)]
        //[InlineData("ecocode", 158)]
        //[InlineData("family", 529)]
        //[InlineData("genus", 3951)]
        //[InlineData("species_author", 3101)]
        //[InlineData("feature_type", 41)]
        //[InlineData("ecocode_system", 3)]
        //[InlineData("activeseason", 18)]
        //[InlineData("tbl_biblio_sample_groups", 2344)]
        //[Fact(Skip = "Needs rework")]
        //public void Load_SingleDiscreteConfigWithoutPicks_IsLoaded(string facetCode, int count)
        //{
        //    var fixture = new MockFacetsConfigFactory(Registry.Facets);

        //    using (IContainer container = TestDependencyService.CreateContainer(FacetContext, null)) {

        //        FacetsConfig2 facetsConfig = fixture.CreateSingleFacetsConfigWithoutPicks(facetCode);
        //        //Utility.SaveAsJson(facetsConfig, "facet_load_config", logDir);

        //        var service = container.ResolveKeyed<IFacetContentService>(facetsConfig.TargetFacet.FacetTypeId);

        //        // Act
        //        var facetContent = service.Load(facetsConfig);

        //        // Assert
        //        // TestContext.WriteLine($"{facetCode}: {facetContent.Items.Count}");
        //        Assert.Equal(count, facetContent.Items.Count);
        //        Assert.Equal(facetsConfig, facetContent.FacetsConfig);
        //        //Utility.SaveAsJson(facetContent, "facet_load_content", logDir);

        //    }
        //}

        //[TestMethod, Ignore]
        //public void CanLoadAbundanceClassificationDiscreteConfigWithoutPicks()
        //{

        //    var facetKeys = new List<string>() { "abundance_classification" };

        //    foreach (var facetKey in facetKeys)
        //    {
        //        IContainer container = new TestDependencyService().Register();
        //        FacetsConfig2 facetsConfig = fixture.GenerateSingleFacetsConfigWithoutPicks(facetKey);
        //        //Utility.SaveAsJson(facetsConfig, "facet_load_config", logDir);

        //        using (var scope = container.BeginLifetimeScope())
        //        {
        //            facetsConfig.Context = scope.Resolve<IRepositoryRegistry>();
        //            facetsConfig.FacetConfigs.ForEach(z => z.Context = facetsConfig.Context);
        //            var service = container.ResolveKeyed<IFacetContentService>(facetsConfig.TargetFacet.FacetTypeId);
        //            var facetContent = service.Load(facetsConfig);

        //            //Assert.IsTrue(facetContent.Items.Count == 1544);
        //            Assert.AreEqual<FacetsConfig2>(facetsConfig, facetContent.FacetsConfig, facetKey);
        //            //Utility.SaveAsJson(facetContent, "facet_load_content", logDir);
        //        }
        //    }
        //}

        //[TestMethod]
        //public void CanLoadSingleDiscreteWithAliasConfigWithoutPicks()
        //{

        //    var facetKeys = new List<string>() {  "country" };

        //    foreach (var facetKey in facetKeys)
        //    {
        //        IContainer container = new TestDependencyService().Register();
        //        FacetsConfig2 facetsConfig = fixture.GenerateSingleFacetsConfigWithoutPicks(facetKey);
        //        //Utility.SaveAsJson(facetsConfig, "facet_load_config", logDir);

        //        using (var scope = container.BeginLifetimeScope())
        //        {
        //            facetsConfig.Context = scope.Resolve<IRepositoryRegistry>();
        //            facetsConfig.FacetConfigs.ForEach(z => z.Context = facetsConfig.Context);
        //            var service = container.ResolveKeyed<IFacetContentService>(facetsConfig.TargetFacet.FacetTypeId);
        //            var facetContent = service.Load(facetsConfig);

        //            //Assert.IsTrue(facetContent.Items.Count == 1544);
        //            Assert.AreEqual<FacetsConfig2>(facetsConfig, facetContent.FacetsConfig, facetKey);
        //            //Utility.SaveAsJson(facetContent, "facet_load_content", logDir);
        //        }
        //    }
        //}

        [Fact(Skip="Needs rework")]
        public void CanLoadSingleDiscreteConfigWithPicks()
        {

            //var graph = new FacetGraphFactory(Registry).Build();
            //var joinCompiler = new Mock<EdgeSqlCompiler>();
            //var pickCompiler = new Mock<IPickFilterCompiler>();
            //var querySetupCompiler = new QuerySetupCompiler(gra);
            //var service = new FacetContentService();

            using (IContainer container = TestDependencyService.CreateContainer(null, null))
            using (var scope = container.BeginLifetimeScope()) {
                var registry = container.Resolve<IRepositoryRegistry>();
                var fixture = new MockFacetsConfigFactory(Registry.Facets);
                FacetsConfig2 facetsConfig = fixture.Create(
                    "sites", "sites",
                    new List<FacetConfig2>() {
                        Mocks.MockFacetConfigFactory.Create(
                            GetFacet("sites"),
                            0,
                            FacetConfigPick.CreateDiscrete(new List<int>() {
                                1470,
                                447,
                                951,
                                445
                            })
                        )
                    }
                );
                var service = container.ResolveKeyed<IFacetContentService>(facetsConfig.TargetFacet.FacetTypeId);
                var facetContent = service.Load(facetsConfig);
                string output = JsonConvert.SerializeObject(facetContent);
                Assert.True(facetContent.Items.Count > 0);

            }
        }

        //[DataRow("species@species")]
        ////[DataRow("sites@sites:sites@1470,447,951,445/ecocode@38,12,92")]
        //[TestMethod]
        //public void CanLoadDiscreteFacets(string uri)
        //{
        //    IContainer container = new TestDependencyService().Register();
        //    using (var scope = container.BeginLifetimeScope())
        //    {
        //        // Arrange
        //        FacetsConfig2 facetsConfig = fixture.GenerateByUri(uri);
        //        facetsConfig.SetContext(scope.Resolve<IRepositoryRegistry>());
        //        var service = container.ResolveKeyed<IFacetContentService>(facetsConfig.TargetFacet.FacetTypeId);

        //        // Act
        //        var facetContent = service.Load(facetsConfig);

        //        // Assert
        //        // string output = JsonConvert.SerializeObject(facetContent);
        //        Assert.IsTrue(facetContent.Items.Count > 0);
        //    }
        //}
        //public JArray ParseJSON(string path)
        //{
        //    //JArray data = ParseJSON($"C:\\Users\\roma0050\\Documents\\Projects\\SEAD\\query_sead_api_core\\query_sead_test/fixtures/json/{facetCode}.json");
        //    return JArray.Parse(File.ReadAllText(path, Encoding.UTF8));
        //}

        ////[DataRow("abundances_all")]
        ////[DataRow("tbl_denormalized_measured_values_33_0")]
        //[DataRow("tbl_denormalized_measured_values_33_0:tbl_denormalized_measured_values_33_0@(0,10)")]
        //[TestMethod]
        //public void CanLoadRangeFacets(string uri)
        //{
        //    IContainer container = new TestDependencyService().Register();

        //    using (var scope = container.BeginLifetimeScope())
        //    {
        //        // Arrange
        //        FacetsConfig2 facetsConfig = fixture.GenerateByUri(uri);
        //        facetsConfig.Context = scope.Resolve<IRepositoryRegistry>();
        //        facetsConfig.FacetConfigs.ForEach(z => z.Context = facetsConfig.Context);
        //        var service = container.ResolveKeyed<IFacetContentService>(facetsConfig.TargetFacet.FacetTypeId);

        //        // Act
        //        var facetContent = service.Load(facetsConfig);

        //        // Assert
        //        var expectedData = new RangeFacetCategoryCountData().Data[facetsConfig.TargetCode];
        //        Assert.AreEqual(expectedData.Where(z => z.Value > 0).Count(), facetContent.Items.Where(z => z.Count > 0).Count(), "Number of categories differs");
        //        facetContent.Items.Where(z => z.Count > 0).ForEach(z => Assert.AreEqual(expectedData.GetValueOrDefault(z.Category), z.Count, z.Category));
        //    }
        //}

        //[TestMethod]
        //public void LoadOfFinishSitesShouldEqualExpectedItems()
        //{
        //    IContainer container = new TestDependencyService().Register();
        //    var config = fixture.Data.DiscreteTestConfigsWithPicks.Where(z => z.UriConfig == "sites@sites:country@73/sites:").First();
        //    FacetsConfig2 facetsConfig = fixture.GenerateByConfig(config);
        //    using (var scope = container.BeginLifetimeScope())
        //    {
        //        var context = scope.Resolve<IRepositoryRegistry>();
        //        facetsConfig.SetContext(context);

        //        var service = container.ResolveKeyed<IFacetContentService>(facetsConfig.TargetFacet.FacetTypeId);
        //        var facetContent = service.Load(facetsConfig);

        //        Assert.IsNotNull(facetContent);
        //        Assert.AreEqual(30, facetContent.Items.Count);
        //        Dictionary<string,int> items = facetContent.Items.ToDictionary(z => z.Category, z => z.Count ?? 0);
        //        Dictionary<string,int> expected = fixture.Data.FinishSiteCount;
        //        var isEqual = (expected == items) || (expected.Count == items.Count && !expected.Except(items).Any());
        //        Assert.IsTrue(isEqual);
        //    }
        //}

        [Fact(Skip = "Not implemented")]
        public void RangeFacetBugTest_PD20181107()
        {
            var fixture = new MockFacetsConfigFactory(Registry.Facets);

            var uri = "tbl_denormalized_measured_values_33_0:tbl_denormalized_measured_values_33_0@(3,52)";
            using (var container = TestDependencyService.CreateContainer(FacetContext, null))
            using (var scope = container.BeginLifetimeScope()) {
                // Arrange
                FacetsConfig2 facetsConfig = fixture.Create(uri);
                // var json = JsonConvert.SerializeObject(facetsConfig).ToString();
                var service = container.ResolveKeyed<IFacetContentService>(facetsConfig.TargetFacet.FacetTypeId);

                // Act
                var facetContent = service.Load(facetsConfig);

                // Assert
                //var expectedData = "";  // new RangeFacetCategoryCountData().Data[facetsConfig.TargetCode];
                //Assert.AreEqual(expectedData.Where(z => z.Value > 0).Count(), facetContent.Items.Where(z => z.Count > 0).Count(), "Number of categories differs");
                //facetContent.Items.Where(z => z.Count > 0).ForEach(z => Assert.AreEqual(expectedData.GetValueOrDefault(z.Category), z.Count, z.Category));
            }
        }
    }
}
