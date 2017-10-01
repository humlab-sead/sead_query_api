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

namespace QuerySeadTests.FacetsConfig
{
    [TestClass]
    public class FacetContentServiceTests
    {
        private fixtures.FacetConfigFixture fixture;
        private static IContainer container;
        private string logDir = @"\temp\json\";

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
        public void Initialize()
        {
            fixture = new fixtures.FacetConfigFixture();
        }

        [TestMethod]
        public void CanLoadSingleDiscreteConfigWithoutPicks()
        {
            foreach (var facetCode in fixture.Data.DiscreteFacetComputeCount.Keys)
            {
                IContainer container = new TestDependencyService().Register();
                FacetsConfig2 facetsConfig = fixture.GenerateSingleFacetsConfigWithoutPicks(facetCode);
                //Utility.SaveAsJson(facetsConfig, "facet_load_config", logDir);

                using (var scope = container.BeginLifetimeScope())
                {
                    facetsConfig.Context = scope.Resolve<IUnitOfWork>();
                    facetsConfig.FacetConfigs.ForEach(z => z.Context = facetsConfig.Context);
                    var service = container.ResolveKeyed<IFacetContentService>(facetsConfig.TargetFacet.FacetTypeId);

                    // Act
                    var facetContent = service.Load(facetsConfig);

                    // Assert
                    TestContext.WriteLine($"{facetCode}: {facetContent.Items.Count}");
                    Assert.AreEqual(fixture.Data.DiscreteFacetComputeCount[facetCode], facetContent.Items.Count, facetCode);
                    Assert.AreEqual<FacetsConfig2>(facetsConfig, facetContent.FacetsConfig, facetCode);
                    //Utility.SaveAsJson(facetContent, "facet_load_content", logDir);
                }
            }
        }

        [TestMethod, Ignore]
        public void CanLoadAbundanceClassificationDiscreteConfigWithoutPicks()
        {

            var facetKeys = new List<string>() { "abundance_classification" };

            foreach (var facetKey in facetKeys)
            {
                IContainer container = new TestDependencyService().Register();
                FacetsConfig2 facetsConfig = fixture.GenerateSingleFacetsConfigWithoutPicks(facetKey);
                //Utility.SaveAsJson(facetsConfig, "facet_load_config", logDir);

                using (var scope = container.BeginLifetimeScope())
                {
                    facetsConfig.Context = scope.Resolve<IUnitOfWork>();
                    facetsConfig.FacetConfigs.ForEach(z => z.Context = facetsConfig.Context);
                    var service = container.ResolveKeyed<IFacetContentService>(facetsConfig.TargetFacet.FacetTypeId);
                    var facetContent = service.Load(facetsConfig);

                    //Assert.IsTrue(facetContent.Items.Count == 1544);
                    Assert.AreEqual<FacetsConfig2>(facetsConfig, facetContent.FacetsConfig, facetKey);
                    //Utility.SaveAsJson(facetContent, "facet_load_content", logDir);
                }
            }
        }

        [TestMethod]
        public void CanLoadSingleDiscreteWithAliasConfigWithoutPicks()
        {

            var facetKeys = new List<string>() {  "country" };

            foreach (var facetKey in facetKeys)
            {
                IContainer container = new TestDependencyService().Register();
                FacetsConfig2 facetsConfig = fixture.GenerateSingleFacetsConfigWithoutPicks(facetKey);
                //Utility.SaveAsJson(facetsConfig, "facet_load_config", logDir);

                using (var scope = container.BeginLifetimeScope())
                {
                    facetsConfig.Context = scope.Resolve<IUnitOfWork>();
                    facetsConfig.FacetConfigs.ForEach(z => z.Context = facetsConfig.Context);
                    var service = container.ResolveKeyed<IFacetContentService>(facetsConfig.TargetFacet.FacetTypeId);
                    var facetContent = service.Load(facetsConfig);

                    //Assert.IsTrue(facetContent.Items.Count == 1544);
                    Assert.AreEqual<FacetsConfig2>(facetsConfig, facetContent.FacetsConfig, facetKey);
                    //Utility.SaveAsJson(facetContent, "facet_load_content", logDir);
                }
            }
        }
        [TestMethod]
        public void CanLoadSingleDiscreteConfigWithPicks()
        {
            FacetsConfig2 facetsConfig = fixture.GenerateFacetsConfig(
                "sites", "sites",
                new List<FacetConfig2>() {
                    fixture.GenerateFacetConfig(
                        "sites", 0, fixture.GenerateDiscreteFacetPicks(new List<int>() { 1470, 447, 951, 445 })
                    )
                }
            );

            IContainer container = new TestDependencyService().Register();

            using (var scope = container.BeginLifetimeScope())
            {
                facetsConfig.SetContext(scope.Resolve<IUnitOfWork>());
                facetsConfig.FacetConfigs.ForEach(z => z.Context = facetsConfig.Context);
                var service = container.ResolveKeyed<IFacetContentService>(facetsConfig.TargetFacet.FacetTypeId);
                var facetContent = service.Load(facetsConfig);
                string output = JsonConvert.SerializeObject(facetContent);
                Assert.IsTrue(facetContent.Items.Count > 0);
            }
        }

        [TestMethod]
        public void CanLoadDualDiscreteConfigWithPicks()
        {
            FacetsConfig2 facetsConfig = fixture.GenerateFacetsConfig(
                "sites",
                "sites",
                new List<FacetConfig2>() {
                    fixture.GenerateFacetConfig(
                        "sites", 0, fixture.GenerateDiscreteFacetPicks(new List<int>() { 1470, 447, 951, 445 })
                    ),
                    fixture.GenerateFacetConfig(
                        "ecocode", 1, fixture.GenerateDiscreteFacetPicks(new List<int>() { 38, 12, 92 })
                    )
                }
             );
            Utility.SaveAsJson(facetsConfig, "facet_load_config", logDir);

            IContainer container = new TestDependencyService().Register();

            using (var scope = container.BeginLifetimeScope())
            {
                facetsConfig.SetContext(scope.Resolve<IUnitOfWork>());
                facetsConfig.FacetConfigs.ForEach(z => z.Context = facetsConfig.Context);
                var service = container.ResolveKeyed<IFacetContentService>(facetsConfig.TargetFacet.FacetTypeId);
                var facetContent = service.Load(facetsConfig);
                string output = JsonConvert.SerializeObject(facetContent);
                Assert.IsTrue(facetContent.Items.Count > 0);

                Utility.SaveAsJson(facetContent, "facet_load_content", logDir);
            }
        }

        //[DataRow("abundances_all")]
        [DataRow("tbl_denormalized_measured_values_33_0")]
        [TestMethod]
        public void CanLoadSingleRangeConfigWithoutPicks(string facetCode)
        {
            IContainer container = new TestDependencyService().Register();
            FacetsConfig2 facetsConfig = fixture.GenerateByUri($"{facetCode}:{facetCode}");
            //Utility.SaveAsJson(facetsConfig, "facet_load_config_", logDir);

            using (var scope = container.BeginLifetimeScope())
            {
                facetsConfig.Context = scope.Resolve<IUnitOfWork>();
                facetsConfig.FacetConfigs.ForEach(z => z.Context = facetsConfig.Context);
                var service = container.ResolveKeyed<IFacetContentService>(facetsConfig.TargetFacet.FacetTypeId);
                var facetContent = service.Load(facetsConfig);

                Assert.AreEqual(121, facetContent.Items.Count);
                Assert.AreEqual(facetsConfig, facetContent.FacetsConfig);
                Assert.AreEqual(496, facetContent.Items.Where(z => z.Name == "312 to 336").FirstOrDefault().Count);
                Assert.AreEqual(8, facetContent.Items.Where(z => z.Name == "1032 to 1056").FirstOrDefault().Count);
                //Utility.SaveAsJson(facetContent, "facet_load_content", logDir);
            }
        }

        [TestMethod]
        public void LoadOfFinishSitesShouldEqualExpectedItems()
        {
            IContainer container = new TestDependencyService().Register();
            var config = fixture.Data.DiscreteTestConfigsWithPicks.Where(z => z.UriConfig == "sites@sites:country@73/sites:").First();
            FacetsConfig2 facetsConfig = fixture.GenerateByConfig(config);
            using (var scope = container.BeginLifetimeScope())
            {
                var context = scope.Resolve<IUnitOfWork>();
                facetsConfig.SetContext(context);

                var service = container.ResolveKeyed<IFacetContentService>(facetsConfig.TargetFacet.FacetTypeId);
                var facetContent = service.Load(facetsConfig);

                Assert.IsNotNull(facetContent);
                Assert.AreEqual(30, facetContent.Items.Count);
                Dictionary<string,int> items = facetContent.Items.ToDictionary(z => z.Category, z => z.Count ?? 0);
                Dictionary<string,int> expected = fixture.Data.FinishSiteCount;
                var isEqual = (expected == items) || (expected.Count == items.Count && !expected.Except(items).Any());
                Assert.IsTrue(isEqual);
            }
        }
    }
}
