using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SeadQueryCore;
using SQT.Infrastructure;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace IntegrationTests
{

    public class FacetsControllerHostWithContainer : TestHostBuilderFixture<TestStartup<TestDependencyService>>
    {

    }

    public class FacetsControllerTests : ControllerTest<FacetsControllerHostWithContainer>, IClassFixture<FacetsControllerHostWithContainer>
    {
        public FacetsControllerTests(FacetsControllerHostWithContainer fixture): base(fixture)
        {
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

        [Fact]
        public async Task API_GET_Health_IsGood()
        {
            using (var response = await Fixture.Client.GetAsync("api/values")) {
                response.EnsureSuccessStatusCode();
                var json = await response.Content.ReadAsStringAsync();
                Assert.Matches(@"\[""\d{4}"",""\d{2}""\]", json);
            }
        }

        [Fact]
        public async Task API_GET_Facets_Success()
        {
            using (var response = await Fixture.Client.GetAsync("api/facets")) {
                response.EnsureSuccessStatusCode();
                var json = await response.Content.ReadAsStringAsync();
                List<JObject> facets = JsonConvert.DeserializeObject<List<JObject>>(json.ToString());
                Assert.NotEmpty(facets);
            }
        }

        [Theory]
        [InlineData(1)]
        [InlineData(5)]
        [InlineData(10)]
        public async Task API_GET_Facets_ById_Success(int facetId)
        {
            using (var response = await Fixture.Client.GetAsync($"api/facets/{facetId}")) {
                response.EnsureSuccessStatusCode();
                var json = await response.Content.ReadAsStringAsync();
                Facet facet = JsonConvert.DeserializeObject<Facet>(json.ToString());
                Assert.Equal(facetId, facet.FacetId);
            }
        }

        [Fact]
        public async Task API_GET_Facets_Domain_Success()
        {
            using (var response = await Fixture.Client.GetAsync("api/facets/domain")) {
                response.EnsureSuccessStatusCode();
                var json = await response.Content.ReadAsStringAsync();
                List<Facet> facets = JsonConvert.DeserializeObject<List<Facet>>(json.ToString());
                Assert.NotEmpty(facets);
                var facetCodes = facets.Select(x => x.FacetCode);
                Assert.Contains("pollen", facetCodes);
            }
        }

        [Theory]
        [InlineData("palaeoentomology")]
        [InlineData("archaeobotany")]
        [InlineData("pollen")]
        [InlineData("geoarchaeology")]
        [InlineData("dendrochronology")]
        [InlineData("ceramic")]
        [InlineData("isotope")]
        public async Task API_GET_Facets_Domain_ById_Success(string facetCode)
        {
            using (var response = await Fixture.Client.GetAsync($"api/facets/domain/{facetCode}")) {
                response.EnsureSuccessStatusCode();
                var json = await response.Content.ReadAsStringAsync();
                List<Facet> facets = JsonConvert.DeserializeObject<List<Facet>>(json.ToString());
                Assert.Single(facets);
                var facet = facets[0];
                Assert.Equal(facetCode, facet.FacetCode);
                Assert.NotEmpty(facet.Clauses);
            }
        }

    }
}
