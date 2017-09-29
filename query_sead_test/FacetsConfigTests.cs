using Autofac;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using QuerySeadDomain;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Newtonsoft.Json.Linq;

namespace QuerySeadTests.FacetsConfig
{
    [TestClass]
    public class FacetsConfigTests {

        private fixtures.FacetConfigFixture fixture;
        private IContainer container;

        [ClassInitialize()]
        public void SetupDependencies()
        {
            container = new TestDependencyService().Register();
        }

        [TestInitialize()]
        public void Initialize() {
            fixture = new fixtures.FacetConfigFixture();
        }

        [TestMethod]
        public void CanCreateSimpleConfig()
        {
            FacetsConfig2 facetsConfig = fixture.GenerateSingleFacetsConfigWithoutPicks("sites");
            Assert.AreEqual(facetsConfig.TargetCode, facetsConfig.TargetFacet.FacetCode);
        }

        [TestMethod]
        public void CanCreateSimpleConfigByJSON()
        {
            FacetsConfig2 facetsConfig = fixture.GenerateSingleFacetsConfigWithoutPicks("sites");
            string output = JsonConvert.SerializeObject(facetsConfig);
            FacetsConfig2 facetsConfig2 = JsonConvert.DeserializeObject<FacetsConfig2>(output);
            Assert.AreEqual(facetsConfig, facetsConfig2);
        }
    }
}
