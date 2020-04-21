using FluentAssertions;
using KellermanSoftware.CompareNetObjects;
using Moq;
using Newtonsoft.Json;
using SeadQueryAPI.Controllers;
using SeadQueryAPI.Serializers;
using SeadQueryAPI.Services;
using SeadQueryCore;
using SQT;
using SQT.Infrastructure;
using SQT.Mocks;
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
        //[InlineData("sites:sites")]
        [InlineData("sites:country@5/sites")]
        public async Task Load_DiscreteFacetConfigs_IsLoaded(string uri)
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
                compare.Config.MembersToIgnore.AddRange(new string[] { "DomainFacet", "TriggerFacet", "TargetFacet", "Facet" });
                //.IgnoreProperty<FacetsConfig2>(x => x.TriggerFacet);

                var areEqual = compare.Compare(facetsConfig, facetContent.FacetsConfig).AreEqual;
                Assert.True(areEqual);

            }
        }

        //private FacetsController CreateFacetsController()
        //{
        //    var mockLoadFacetService = new Mock<IFacetReconstituteService>();
        //    var mockReconstituteService = new Mock<IFacetConfigReconstituteService>();

        //    return new FacetsController(
        //        Registry,
        //        mockReconstituteService.Object,
        //        mockLoadFacetService.Object
        //    );

        //}

        //[Fact(Skip = "Not working")]
        //public async Task CanLoadDiscreteFacetConfigsWithPicks()
        //{
        //    var data = new FacetConfigsByUriFixtures();
        //    var builder = new SeadTestHostBuilder().Create<TestStartup<TestDependencyService>>();
        //    using (var host = await builder.StartAsync()) {
        //        using (var client = host.GetTestClient()) {
        //            foreach (var item in data.DiscreteTestConfigsWithPicks) {
        //                var uri = item[0].ToString();
        //                var expectedCount = item[2];
        //                FacetsConfig2 facetsConfig = facetsConfigMockFactory.Create(uri);

        //                var json = JsonConvert.SerializeObject(facetsConfig);
        //                var request_content = new StringContent(json, Encoding.UTF8, "application/json");
        //                var response = await client.PostAsync(new Uri("/api/facets/load"), request_content);
        //                response.EnsureSuccessStatusCode();
        //                var response_content = await response.Content.ReadAsStringAsync();
        //                var facetContent = JsonConvert.DeserializeObject<FacetContent>(response_content);
        //                Assert.Equal(expectedCount, facetContent.Items.Count);
        //            }
        //        }
        //    }
        //}

    }
}
