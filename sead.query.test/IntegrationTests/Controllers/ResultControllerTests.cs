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

    }
}
