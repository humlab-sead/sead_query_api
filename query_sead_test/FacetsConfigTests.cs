using Autofac;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using QuerySeadDomain;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace QuerySeadTests.FacetsConfig
{
    [TestClass]
    public class FacetsConfigTests {

        [TestMethod]
        public void CanCreateSimpleConfig()
        {
            FacetsConfig2 facetsConfig = GetTestFacetsConfig();
            string output = JsonConvert.SerializeObject(facetsConfig);
            FacetsConfig2 facetConfig2 = JsonConvert.DeserializeObject<FacetsConfig2>(output);
        }

        private static FacetsConfig2 GetTestFacetsConfig()
        {
            FacetsConfig2 facetsConfig = new FacetsConfig2() {
                RequestId = "1",
                Language = "en_GB",
                TargetCode = "sites",       // requested_facet
                RequestType = "populate",
                TriggerCode = "ecocode",    // f_action.f_code
                FacetConfigs = new List<FacetConfig2>() {
                    new FacetConfig2() {
                        FacetCode = "sites",
                        Position = 1,
                        StartRow = 0,
                        RowCount = 150,
                        TextFilter = "",
                        Picks = new List<FacetConfigPick>() {
                            new FacetConfigPick(EFacetPickType.discrete, 1470, "1470"),
                            new FacetConfigPick(EFacetPickType.discrete, 447, "447"),
                            new FacetConfigPick(EFacetPickType.discrete, 951, "951"),
                            new FacetConfigPick(EFacetPickType.discrete, 445, "445")
                        }
                    },
                    new FacetConfig2() {
                        FacetCode = "ecocode",
                        Position = 0,
                        StartRow = 0,
                        RowCount = 150,
                        TextFilter = "",
                        Picks = new List<FacetConfigPick>() {
                            new FacetConfigPick(EFacetPickType.discrete, 38, "38"),
                            new FacetConfigPick(EFacetPickType.discrete, 12, "12"),
                            new FacetConfigPick(EFacetPickType.discrete, 92, "92"),
                        }
                    }
                }
            };
            return facetsConfig;
        }

        [TestMethod]
        public void CanLoadSingleDiscreteConfigWithoutPicks()
        {
            FacetsConfig2 facetsConfig = new FacetsConfig2() {
                RequestId = "1",
                Language = "en_GB",
                TargetCode = "sites",       // requested_facet
                RequestType = "populate",
                TriggerCode = "sites",    // f_action.f_code
                FacetConfigs = new List<FacetConfig2>() {
                    new FacetConfig2() {
                        FacetCode = "sites",
                        Position = 0,
                        StartRow = 0,
                        RowCount = 150,
                        TextFilter = "",
                        Picks = new List<FacetConfigPick>() {
                        }
                    }
                }
            };
            IContainer container = new TestDependencyService().Register(null);

            using (var scope = container.BeginLifetimeScope()) {
                facetsConfig.Context = scope.Resolve<IUnitOfWork>();
                facetsConfig.FacetConfigs.ForEach(z => z.Context = facetsConfig.Context);
                var service = container.ResolveKeyed<IFacetContentService>(facetsConfig.TargetFacet.FacetTypeId);
                var facetContent = service.Load(facetsConfig);
                Assert.IsTrue(facetContent.Items.Count > 0);
                string output = JsonConvert.SerializeObject(facetContent);
            }
        }

        [TestMethod]
        public void CanLoadDualDiscreteConfigWithPicks()
        {
            FacetsConfig2 facetsConfig = GetTestFacetsConfig();

            IContainer container = new TestDependencyService().Register(null);

            using (var scope = container.BeginLifetimeScope()) {
                facetsConfig.SetContext(scope.Resolve<IUnitOfWork>());
                facetsConfig.FacetConfigs.ForEach(z => z.Context = facetsConfig.Context);
                var service = container.ResolveKeyed<IFacetContentService>(facetsConfig.TargetFacet.FacetTypeId);
                var facetContent = service.Load(facetsConfig);
                string output = JsonConvert.SerializeObject(facetContent);
                Assert.IsTrue(facetContent.Items.Count > 0);
           }
        }
    }
}
