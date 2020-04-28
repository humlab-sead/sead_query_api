using Moq;
using Newtonsoft.Json.Linq;
using SeadQueryAPI.Controllers;
using SeadQueryAPI.Serializers;
using SeadQueryAPI.Services;
using SQT;
using SQT.Infrastructure;
using System.Threading.Tasks;
using Xunit;

namespace IntegrationTests
{
    public class ResultTestStartupWithContainer : TestStartup<TestDependencyService>
    {
    }

    public class ResultTestHostWithContainer : TestHostBuilderFixture<ResultTestStartupWithContainer>
    {

    }

    [Collection("JsonSeededFacetContext")]
    public class ResultControllerTests : ControllerTest<ResultTestHostWithContainer>, IClassFixture<ResultTestHostWithContainer>
    {
        public JsonSeededFacetContextFixture FacetContextFixture { get; }
        public DisposableFacetContextContainer MockService { get; }

        public ResultControllerTests(ResultTestHostWithContainer hostBuilderFixture, JsonSeededFacetContextFixture facetContextFixture) : base(hostBuilderFixture)
        {
            FacetContextFixture = facetContextFixture;
            MockService = new DisposableFacetContextContainer(facetContextFixture);
        }

        [Fact]
        public async Task API_GET_Server_IsAwake()
        {
            using (var response = await Fixture.Client.GetAsync("api/values")) {
                response.EnsureSuccessStatusCode();
                Assert.NotEmpty(await response.Content.ReadAsStringAsync());
            }
        }

        // FIXME Add more tests!!!


        //public IWebHostBuilder CreateTestWebHostBuilder2<T>() where T : class
        //{
        //    return new WebHostBuilder()
        //        .UseKestrel()
        //        .ConfigureServices(services => services.AddAutofac())
        //        //.UseConfiguration(config)
        //        .UseContentRoot(Directory.GetCurrentDirectory())
        //        .UseStartup<T>();
        //}

        //[Fact(Skip = "Not working")]
        //public async Task LoadOfFinishSitesShouldEqualExpectedItems()
        //{
        //    var testConfigs = new Dictionary<(string, string, string), int>()
        //    {
        //        { ("tabular", "site_level", "sites:country@73/sites:"), 30 },
        //        { ("tabular", "aggregate_all", "sites:country@73/sites:"), 1 },
        //        { ("tabular", "sample_group_level", "sites:country@73/sites:"), 30 },
        //        { ("map", "map_result", "sites:country@73/sites:"), 32 }
        //    };

        //    var builder = CreateTestWebHostBuilder2<TestStartup<TestDependencyService>>();

        //    using (var server = new TestServer(builder)) {

        //        foreach (var ((viewTypeId, resultKey, uri), expectedCount) in testConfigs) {
        //            using (var client = server.CreateClient()) {
        //                await ExecuteLoadCommand(client, viewTypeId, resultKey, uri, expectedCount);
        //            }
        //        }
        //    }
        //}

        //private async Task ExecuteLoadCommand(HttpClient client, string viewTypeId, string resultKey, string uri, int expectedCount)
        //{
        //    // FIXME! Mock
        //    // Arrange
        //    FacetsConfig2 facetsConfig = facetConfigFixture.Create(uri);
        //    var resultConfig = ResultConfigFactory.Create(viewTypeId, resultKey);
        //    var jObject = new { facetsConfig = facetsConfig, resultConfig = resultConfig };
        //    var json = JsonConvert.SerializeObject(jObject);
        //    var requestContent = new StringContent(json, Encoding.UTF8, "application/json");

        //    // Act
        //    var response = await client.PostAsync(new Uri("/api/result/load"), requestContent);
        //    response.EnsureSuccessStatusCode();
        //    var responseJson = await response.Content.ReadAsStringAsync();
        //    var resultContent = JsonConvert.DeserializeObject<ResultContentSet>(responseJson);

        //    // Assert
        //    Assert.NotNull(resultContent?.Data?.DataCollection);
        //    var items = resultContent.Data.DataCollection.ToList();
        //    Assert.Equal(expectedCount, items.Count);
        //}
    }
}
