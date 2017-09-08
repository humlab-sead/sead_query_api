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
    public class LoadFacetTests
    {
        private fixtures.SetupFacetsConfig fixture;
        private static IContainer container;
        private string logDir = @"\temp\json\";

        [ClassInitialize()]
        public static void InitializeClass(TestContext context)
        {
            container = new TestDependencyService().Register();
        }

        [TestInitialize()]
        public void Initialize() {
            fixture = new fixtures.SetupFacetsConfig();
        }

        [TestMethod]
        public void CanLoadSingleDiscreteConfigWithoutPicks()
        {
 
            IContainer container = new TestDependencyService().Register();
            FacetsConfig2 facetsConfig = fixture.GenerateFacetsConfig(
                "sites", "sites",
                new List<FacetConfig2>() {
                    fixture.GenerateFacetConfig("sites", 0, new List<FacetConfigPick>())
                }
            );
            Utility.SaveAsJson(facetsConfig, "facet_load_config", logDir);

            using (var scope = container.BeginLifetimeScope()) {
                facetsConfig.Context = scope.Resolve<IUnitOfWork>();
                facetsConfig.FacetConfigs.ForEach(z => z.Context = facetsConfig.Context);
                var service = container.ResolveKeyed<IFacetContentService>(facetsConfig.TargetFacet.FacetTypeId);
                var facetContent = service.Load(facetsConfig);

                Assert.IsTrue(facetContent.Items.Count == 1544);
                Assert.AreEqual<FacetsConfig2>(facetsConfig, facetContent.FacetsConfig);
                Utility.SaveAsJson(facetContent, "facet_load_content", logDir);
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

            using (var scope = container.BeginLifetimeScope()) {
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
    }
}
