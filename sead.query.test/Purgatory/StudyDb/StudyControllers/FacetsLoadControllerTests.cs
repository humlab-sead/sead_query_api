// using IntegrationTests;
// using SQT;
// using SQT.Infrastructure;
// using System.Threading.Tasks;
// using Xunit;

// namespace Deprecated.StudyDb
// {
//     public class TestHostWithContainer : TestHostFixture<StartupWithContainer>
//     {
//     }

//     [Collection("StudyDbSqliteFacetContext")]
//     public class FacetsLoadControllerTests : ControllerTest<TestHostWithContainer>, IClassFixture<TestHostWithContainer>
//     {
//         public MockerWithFacetContext MockService { get; }

//         public FacetsLoadControllerTests(TestHostWithContainer hostBuilderFixture, StudyDbSqliteFacetContext facetContextFixture) : base(hostBuilderFixture)
//         {
//             MockService = new MockerWithFacetContext(facetContextFixture);
//         }

//         [Fact(Skip = "Pending delete or refactor of StudyDb")]
//         public async Task API_GET_Server_IsAwake()
//         {
//             using var response = await Fixture.Client.GetAsync("api/facets");
//             response.EnsureSuccessStatusCode();
//             Assert.NotEmpty(await response.Content.ReadAsStringAsync());
//         }

//     }
// }
