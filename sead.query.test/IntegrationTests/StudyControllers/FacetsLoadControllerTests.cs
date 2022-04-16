using KellermanSoftware.CompareNetObjects;
using Newtonsoft.Json;
using SeadQueryCore;
using SQT;
using SQT.Infrastructure;
using SQT.SQL.Matcher;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xunit;

namespace IntegrationTests.StudyDb
{
    public class TestHostWithContainer : TestHostFixture<StartupWithContainer>
    {

    }


    [Collection("StudyJsonSeededFacetContext")]
    public class FacetsLoadControllerTests : ControllerTest<TestHostWithContainer>, IClassFixture<TestHostWithContainer>
    {
        public JsonFacetContextFixture FacetContextFixture { get; }
        public DisposableFacetContextContainer MockService { get; }

        public FacetsLoadControllerTests(TestHostWithContainer hostBuilderFixture, StudyJsonFacetContextFixture facetContextFixture) : base(hostBuilderFixture)
        {
            FacetContextFixture = facetContextFixture;
            MockService = new DisposableFacetContextContainer(FacetContextFixture);
        }

        [Fact]
        public async Task API_GET_Server_IsAwake()
        {
            using var response = await Fixture.Client.GetAsync("api/facets");
            response.EnsureSuccessStatusCode();
            Assert.NotEmpty(await response.Content.ReadAsStringAsync());
        }

        /// <summary>
        /// Integeration test of SQL generated by a call to "api/facets/load"
        /// </summary>
        /// <param name="uri">Facet configuration</param>
        /// <param name="expectedJoinCount">Basically the number of tables involved in the join i.e. unique routes returned from Graoh.Find</param>
        /// <returns></returns>
        [Theory(Skip = "Not implemented")]
        [InlineData("study:study", false)]
        public async Task Load_VariousFacetConfigs_HasExpectedSqlQuery(string uri, bool checkNotEmpty, params string[] expectedJoins)
        {
            // Arrange
            var facetsConfig = MockService.FakeFacetsConfig(uri);
            var json = JsonConvert.SerializeObject(facetsConfig);
            var payload = new StringContent(json, Encoding.UTF8, "application/json");

            // Act
            /* using */
            var response = await Fixture.Client.PostAsync("api/facets/load", payload);

            // Assert
            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();

            var facetContent = JsonConvert.DeserializeObject<FacetContent>(responseContent);

            Assert.NotNull(facetContent);

            if (checkNotEmpty)
                Assert.NotEmpty(facetContent.Items);

            // CompareLogic compare = new CompareLogic();
            // compare.Config.MembersToIgnore.AddRange(new string[] { "DomainFacet", "TargetFacet", "Facet", "Text" });
            // var areEqual = compare.Compare(facetsConfig, facetContent.FacetsConfig).AreEqual; // Will fail if bogus picks are removed
            // Assert.True(areEqual);

            var sqlQuery = facetContent.SqlQuery.Squeeze();

            var matcher = CategoryCountSqlCompilerMatcher.Create(facetsConfig.TargetFacet.FacetTypeId);
            var match = matcher.Match(sqlQuery);

            Assert.True(match.Success);
            Assert.Equal("count", match.AggregateType);
            Assert.True(match.InnerSelect.Success);
            Assert.NotEmpty(match.InnerSelect.Tables);
            Assert.True(expectedJoins.All(x => match.InnerSelect.Tables.Contains(x)));
        }


    }
}