using Autofac;
using Moq;
using Newtonsoft.Json;
using SeadQueryCore;
using SeadQueryCore.QueryBuilder;
using SeadQueryInfra;
using SeadQueryTest.Fixtures;
using SeadQueryTest.Infrastructure;
using SeadQueryTest.Infrastructure.Scaffolding;
using System;
using System.Collections.Generic;
using Xunit;

namespace SeadQueryTest.Services.FacetContent
{
    public class FacetContentServiceTests
    {
        private RepositoryRegistry mockRegistry;
        private IFacetSetting mockSettings;

        public FacetContentServiceTests()
        {
            mockSettings = new MockOptionBuilder().Build().Value.Facet;
            mockRegistry = new RepositoryRegistry(ScaffoldUtility.JsonSeededFacetContext());
        }

        private Facet GetFacet(string facetCode)
        {
            return Infrastructure.Scaffolds.FacetInstances.Store[facetCode];
        }

        private FacetContentService CreateService(QuerySetup querySetup)
        {
            var mockQuerySetupBuilder = new Mock<IQuerySetupCompiler>();
            mockQuerySetupBuilder.Setup(
                x => x.Build(It.IsAny<FacetsConfig2>(), It.IsAny<Facet>(), It.IsAny<List<string>>())
            ).Returns(querySetup);
            mockQuerySetupBuilder.Setup(
                x => x.Build(It.IsAny<FacetsConfig2>(), It.IsAny<Facet>(), It.IsAny<List<string>>(), It.IsAny<List<string>>())
            ).Returns(querySetup);

            return new FacetContentService(
                mockSettings,
                mockRegistry,
                mockQuerySetupBuilder.Object
           );
        }

        [Fact]
        public void Load_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var querySetup = Infrastructure.Scaffolds.QuerySetupInstances.Store["measured_values_33(3,52)"];
            var service = this.CreateService(querySetup);
            FacetsConfig2 facetsConfig = null;

            // Act
            var result = service.Load(
                facetsConfig);

            // Assert
            Assert.True(false);
        }



        [Fact]
        public void Load_SingleDiscreteConfigWithoutPicks_IsLoaded()
        {
            var fixture = new SeadQueryTest.Fixtures.ScaffoldFacetsConfig(mockRegistry);
            var data = new Fixtures.FacetConfigFixtureData();

            foreach (var facetCode in data.DiscreteFacetComputeCount.Keys) {
                IContainer container = new TestDependencyService().Register();
                FacetsConfig2 facetsConfig = fixture.CreateSingleFacetsConfigWithoutPicks(facetCode);
                //Utility.SaveAsJson(facetsConfig, "facet_load_config", logDir);

                var service = container.ResolveKeyed<IFacetContentService>(facetsConfig.TargetFacet.FacetTypeId);

                // Act
                var facetContent = service.Load(facetsConfig);

                // Assert
                // TestContext.WriteLine($"{facetCode}: {facetContent.Items.Count}");
                Assert.Equal(data.DiscreteFacetComputeCount[facetCode], facetContent.Items.Count);
                Assert.Equal(facetsConfig, facetContent.FacetsConfig);
                //Utility.SaveAsJson(facetContent, "facet_load_content", logDir);
            }
        }

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

        [Fact]
        public void CanLoadSingleDiscreteConfigWithPicks()
        {
            var fixture = new SeadQueryTest.Fixtures.ScaffoldFacetsConfig(mockRegistry);
            FacetsConfig2 facetsConfig = fixture.CreateFacetsConfig(
                "sites", "sites",
                new List<FacetConfig2>() {
                    ScaffoldFacetConfig.Create(
                        GetFacet("sites"),
                        0,
                        FacetConfigPick.CreateDiscrete(new List<int>() { 1470, 447, 951, 445 })
                    )
                }
            );

            IContainer container = new TestDependencyService().Register();

            using (var scope = container.BeginLifetimeScope()) {
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

        [Fact]
        public void RangeFacetBugTest_PD20181107()
        {
            var fixture = new SeadQueryTest.Fixtures.ScaffoldFacetsConfig(mockRegistry);

            var uri = "tbl_denormalized_measured_values_33_0:tbl_denormalized_measured_values_33_0@(3,52)";
            IContainer container = new TestDependencyService().Register();

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
