using KellermanSoftware.CompareNetObjects;
using Newtonsoft.Json;
using SeadQueryCore;
using SQT;
using SQT.Infrastructure;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace IntegrationTests
{
    public class TestStartupWithContainer : TestStartup<TestDependencyService>
    {

    }

    public class TestHostWithContainer : TestHostBuilderFixture<TestStartupWithContainer>
    {

    }


    [Collection("JsonSeededFacetContext")]
    public class FacetsLoadControllerTests : ControllerTest<TestHostWithContainer>, IClassFixture<TestHostWithContainer>
    {
        public JsonSeededFacetContextFixture FacetContextFixture { get; }
        public DisposableFacetContextContainer MockService { get; }

        public FacetsLoadControllerTests(TestHostWithContainer hostBuilderFixture, JsonSeededFacetContextFixture facetContextFixture) : base(hostBuilderFixture)
        {
            FacetContextFixture = facetContextFixture;
            MockService = new DisposableFacetContextContainer(facetContextFixture);
        }

        [Fact]
        public async Task API_GET_Server_IsAwake()
        {
            // var builder = new SeadTestHostBuilder().Create<TestStartup<TestDependencyService>>();
            // using (var server = await Fixture.Builder.StartAsync())
            // using (var client = await Fixture.Server.GetTestClient())
            using (var response = await Fixture.Client.GetAsync("api/facets")) {
                response.EnsureSuccessStatusCode();
                Assert.NotEmpty(await response.Content.ReadAsStringAsync());
            }
        }

        [Theory]
        [InlineData("relative_age_name:relative_age_name")]
        [InlineData("master_dataset:master_dataset@1")]
        [InlineData("sites:country@5/sites")]
        public async Task Load_VariousFacetConfigs_IsLoaded(string uri)
        {
            var facetsConfig = MockService.FakeFacetsConfig(uri);
            var json = JsonConvert.SerializeObject(facetsConfig);
            var payload = new StringContent(json, Encoding.UTF8, "application/json");

            using (var response = await Fixture.Client.PostAsync("api/facets/load", payload)) {

                response.EnsureSuccessStatusCode();

                var responseContent = await response.Content.ReadAsStringAsync();

                var facetContent = JsonConvert.DeserializeObject<FacetContent>(responseContent);

                Assert.NotNull(facetContent);
                Assert.NotEmpty(facetContent.Items);

                CompareLogic compare = new CompareLogic();
                compare.Config.MembersToIgnore.AddRange(new string[] { "DomainFacet", "TargetFacet", "Facet" });

                var areEqual = compare.Compare(facetsConfig, facetContent.FacetsConfig).AreEqual;
                Assert.True(areEqual);

            }
        }

    }
}
