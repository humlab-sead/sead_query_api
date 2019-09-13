using Newtonsoft.Json;
using SeadQueryCore;
using SeadQueryTest.Infrastructure;
using SeadQueryTest.Infrastructure.Scaffolding;
using Xunit;

namespace SeadQueryTest2.FacetsConfig
{
    public class FacetsConfigTests
    {
        [Fact]
        public void CanCreateSimpleConfig()
        {
            using (var context = ScaffoldUtility.DefaultFacetContext())
            using (var container = new TestDependencyService(context).Register()) {

                var fixture = new SeadQueryTest.fixtures.FacetConfigGenerator(container, context);

                FacetsConfig2 facetsConfig = fixture.GenerateSingleFacetsConfigWithoutPicks("sites");

                Assert.Equal("sites", facetsConfig.TargetCode);
                Assert.Equal(facetsConfig.TargetCode, facetsConfig.TargetFacet.FacetCode);
            }
        }

        [Fact]
        public void CanCreateSimpleConfigByJSON()
        {
            using (var context = ScaffoldUtility.DefaultFacetContext())
            using (var container = new TestDependencyService(context).Register())
              {
                var fixture = new SeadQueryTest.fixtures.FacetConfigGenerator(container, context);
                FacetsConfig2 facetsConfig = fixture.GenerateSingleFacetsConfigWithoutPicks("sites");
                string json1 = JsonConvert.SerializeObject(facetsConfig);
                FacetsConfig2 facetsConfig2 = JsonConvert.DeserializeObject<FacetsConfig2>(json1);
                facetsConfig2.SetContext(fixture.RepositoryRegistry);
                string json2 = JsonConvert.SerializeObject(facetsConfig);
                Assert.Equal(json1, json2);
            }
        }

        [Fact]
        public void CanGenerateSingleFacetsConfigWithoutPicks()
        {
            using (var context = ScaffoldUtility.DefaultFacetContext())
            using (var container = new TestDependencyService(context).Register()) {
                var fixture = new SeadQueryTest.fixtures.FacetConfigGenerator(container, context);
                foreach (var facetCode in fixture.Data.DiscreteFacetComputeCount.Keys) {
                    FacetsConfig2 facetsConfig = fixture.GenerateSingleFacetsConfigWithoutPicks(facetCode);
                    Assert.Equal(facetCode, facetsConfig.TargetFacet.FacetCode);
                }
            }
        }
    }
}
