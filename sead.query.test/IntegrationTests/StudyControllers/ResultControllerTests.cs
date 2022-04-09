// using Moq;
// using Newtonsoft.Json;
// using Newtonsoft.Json.Linq;
// using SeadQueryCore;
// using SeadQueryCore.Model;
// using SQT;
// using SQT.ClassData;
// using SQT.Infrastructure;
// using SQT.SQL.Matcher;
// using System.Linq;
// using System.Net.Http;
// using System.Text;
// using System.Threading.Tasks;
// using Xunit;

// namespace IntegrationTests.StudyDb
// {

//     public class ResultTestHostWithContainer : TestHostFixture<StudyDependencyService>
//     {

//     }

//     [Collection("StudyJsonSeededFacetContext")]
//     public class ResultControllerTests : ControllerTest<ResultTestHostWithContainer>, IClassFixture<ResultTestHostWithContainer>
//     {
//         public JsonFacetContextFixture FacetContextFixture { get; }
//         public DisposableFacetContextContainer MockService { get; }

//         public ResultControllerTests(ResultTestHostWithContainer hostBuilderFixture, StudyJsonFacetContextFixture facetContextFixture) : base(hostBuilderFixture)
//         {
//             FacetContextFixture = facetContextFixture;
//             MockService = new DisposableFacetContextContainer(facetContextFixture);
//         }

//         [Fact]
//         public async Task API_GET_Server_IsAwake()
//         {
//             using (var response = await Fixture.Client.GetAsync("api/values")) {
//                 response.EnsureSuccessStatusCode();
//                 Assert.NotEmpty(await response.Content.ReadAsStringAsync());
//             }
//         }

//         public class LoadPayload {
//             #pragma warning disable IDE1006 // Naming Styles
//             public FacetsConfig2 facetsConfig { get; set; }
//             public ResultConfig resultConfig { get; set; }
//             #pragma warning restore IDE1006 // Naming Styles
//         }

//         protected StringContent FakeLoadResultPayload(string uri, string resultFacetCode, string specificationKey, string viewType)
//         {
//             var facetsConfig = MockService.FakeFacetsConfig(uri);
//             var resultConfig = MockService.FakeResultConfig(resultFacetCode, specificationKey, viewType);
//             var bothConfigs = new LoadPayload { facetsConfig = facetsConfig, resultConfig = resultConfig };
//             var json = JsonConvert.SerializeObject(bothConfigs);
//             var payload = new StringContent(json, Encoding.UTF8, "application/json");
//             return payload;
//         }

//         /// <summary>
//         /// Integeration test of SQL generated by a call to "api/facets/load"
//         /// </summary>
//         /// <param name="uri">Facet configuration</param>
//         /// <param name="expectedJoinCount">Basically the number of tables involved in the join i.e. unique routes returned from Graoh.Find</param>
//         /// <returns></returns>
//         [Theory]
//         [InlineData("palaeoentomology://data_types:data_types", "tbl_analysis_entities", "tbl_datasets")]
//         public async Task Load_VariousFacetConfigs_HasExpectedSqlQuery(string uri, params string[] expectedJoins)
//         {
//             // Arrange
//             var facetsConfig = MockService.FakeFacetsConfig(uri);
//             var resultConfig = MockService.FakeResultConfig("result_facet", "site_level", "tabular");
//             var bothConfigs = new LoadPayload { facetsConfig = facetsConfig, resultConfig = resultConfig };
//             var json = JsonConvert.SerializeObject(bothConfigs);
//             var payload = new StringContent(json, Encoding.UTF8, "application/json");

//             // Act
//             using var response = await Fixture.Client.PostAsync("api/result/load", payload);

//             // Assert
//             response.EnsureSuccessStatusCode();

//             var responseContent = await response.Content.ReadAsStringAsync();

//             var result = JsonConvert.DeserializeObject<ResultContentSet>(responseContent);

//             Assert.NotNull(result);
//             Assert.NotNull(result.Data);
//             Assert.NotNull(result.Data.DataCollection);
//             Assert.NotNull(result.Meta);
//             Assert.NotNull(result.Meta.Columns);
//             Assert.NotEmpty(result.Query);

//             var sqlQuery = result.Query.Squeeze();

//             var matcher = new TabularResultSqlCompilerMatcher();
//             var match = matcher.Match(sqlQuery);

//             Assert.True(match.Success);
//             Assert.True(match.InnerSelect.Success);
//             Assert.NotEmpty(match.InnerSelect.Tables);

//             Assert.True(expectedJoins.All(x => match.InnerSelect.Tables.Contains(x)));
//         }

//     }
// }
