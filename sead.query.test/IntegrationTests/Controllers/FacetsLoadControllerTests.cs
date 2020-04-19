using FluentAssertions;
using Moq;
using SeadQueryAPI.Controllers;
using SeadQueryAPI.Serializers;
using SeadQueryAPI.Services;
using SeadQueryCore;
using SQT;
using SQT.Infrastructure;
using Xunit;

namespace IntegrationTests
{
    [Collection("JsonSeededFacetContext")]
    public class FacetsLoadControllerTests : DisposableFacetContextContainer
    {
        public FacetsLoadControllerTests(JsonSeededFacetContextFixture fixture) : base(fixture)
        {
        }

        private FacetsController CreateFacetsController()
        {
            var mockLoadFacetService = new Mock<IFacetReconstituteService>();
            var mockReconstituteService = new Mock<IFacetConfigReconstituteService>();

            return new FacetsController(
                Registry,
                mockReconstituteService.Object,
                mockLoadFacetService.Object
            );

        }
        //[Fact(Skip = "Not working")]
        //public async Task CanLoadSimpleDiscreteFacetWithoutPicks()
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
        //                var response = await client.PostAsync("/api/facets/load", request_content);
        //                response.EnsureSuccessStatusCode();
        //                var response_content = await response.Content.ReadAsStringAsync();
        //                var facetContent = JsonConvert.DeserializeObject<FacetContent>(response_content);
        //                Assert.Equal(expectedCount, facetContent.Items.Count);
        //            }
        //        }
        //    }
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

        [Fact(Skip = "Not implemented")]
        public void Load_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var facetsController = this.CreateFacetsController();
            FacetsConfig2 facetsConfig = null;

            // Act
            var result = facetsController.Load(
                facetsConfig);

            // Assert
            result.Should().Be(true);

        }
    }
}
