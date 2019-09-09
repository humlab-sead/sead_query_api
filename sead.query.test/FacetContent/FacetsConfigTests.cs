using Autofac;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using SeadQueryCore;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Newtonsoft.Json.Linq;

namespace SeadQueryTest.FacetsConfig
{
    [TestClass]
    public class FacetsConfigTests {

        private fixtures.FacetConfigGenerator fixture;

        [TestInitialize()]
        public void Initialize() {
            fixture = new fixtures.FacetConfigGenerator();
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
            string json1 = JsonConvert.SerializeObject(facetsConfig);
            FacetsConfig2 facetsConfig2 = JsonConvert.DeserializeObject<FacetsConfig2>(json1);
            facetsConfig2.SetContext(fixture.Context);
            string json2 = JsonConvert.SerializeObject(facetsConfig);
            Assert.AreEqual(json1, json2);
        }
    }
}
