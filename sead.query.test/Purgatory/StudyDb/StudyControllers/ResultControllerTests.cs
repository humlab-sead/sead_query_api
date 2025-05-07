// using IntegrationTests;
// using Newtonsoft.Json;
// using SeadQueryCore;
// using SeadQueryCore.Model;
// using SQT;
// using SQT.Infrastructure;

// using System.Net.Http;
// using System.Text;
// using System.Threading.Tasks;
// using Xunit;

// namespace Deprecated.StudyDb
// {
//     public class ResultTestHostWithContainer : TestHostFixture<StudyDependencyService>
//     {
//     }

//     [Collection("StudyDbSqliteFacetContext")]
//     public class ResultControllerTests : ControllerTest<TestHostWithContainer>, IClassFixture<TestHostWithContainer>
//     {
//         public MockerWithFacetContext MockService { get; }

//         public ResultControllerTests(TestHostWithContainer hostBuilderFixture, StudyDbSqliteFacetContext facetContextFixture) : base(hostBuilderFixture)
//         {
//             MockService = new MockerWithFacetContext(facetContextFixture);
//         }

//         [Fact(Skip = "Pending delete or refactor of StudyDb")]
//         public async Task API_GET_Server_IsAwake()
//         {
//             using (var response = await Fixture.Client.GetAsync("api/values"))
//             {
//                 response.EnsureSuccessStatusCode();
//                 Assert.NotEmpty(await response.Content.ReadAsStringAsync());
//             }
//         }

//         public class LoadPayload
//         {
// #pragma warning disable IDE1006 // Naming Styles
//             public FacetsConfig2 facetsConfig { get; set; }
//             public ResultConfig resultConfig { get; set; }
// #pragma warning restore IDE1006 // Naming Styles
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

//     }
// }
