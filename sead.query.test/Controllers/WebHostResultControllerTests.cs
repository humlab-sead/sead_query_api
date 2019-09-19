using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Newtonsoft.Json;
using SeadQueryCore;
using SeadQueryCore.Model;
using SeadQueryInfra;
using SeadQueryTest.fixtures;
using SeadQueryTest.Infrastructure;
using SeadQueryTest.Infrastructure.Scaffolding;
using Xunit;

namespace SeadQueryTest
{
    public class ResultControllerTests
    {
        private FacetConfigGenerator facetConfigFixture;
        private ResultConfigGenerator resultConfigFixture;

        public ResultControllerTests()
        {
            var registry = new RepositoryRegistry(ScaffoldUtility.DefaultFacetContext());
            facetConfigFixture = new fixtures.FacetConfigGenerator(registry);
            resultConfigFixture = new fixtures.ResultConfigGenerator();
        }

        public IWebHostBuilder CreateTestWebHostBuilder2<T>() where T: class
        {
            return new WebHostBuilder()
                .UseKestrel()
                //.UseConfiguration(config)
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseStartup<T>();
        }

        [Fact]
        public async Task LoadOfFinishSitesShouldEqualExpectedItems()
        {
            var testConfigs = new Dictionary<(string, string, string), int>()
            {
                { ("tabular", "site_level", "sites@sites:country@73/sites:"), 30 },
                { ("tabular", "aggregate_all", "sites@sites:country@73/sites:"), 1 },
                { ("tabular", "sample_group_level", "sites@sites:country@73/sites:"), 30 },
                { ("map", "map_result", "sites@sites:country@73/sites:"), 32 }
            };

            var builder = CreateTestWebHostBuilder2<ControllerTestStartup<TestDependencyService>>();

            using (var server = new TestServer(builder)) {

                foreach (var ((viewTypeId, resultKey, uri), expectedCount) in testConfigs) {
                    using (var client = server.CreateClient()) {
                        await ExecuteLoadCommand(client, viewTypeId, resultKey, uri, expectedCount);
                    }
                }
            }
        }

        private async Task ExecuteLoadCommand(HttpClient client, string viewTypeId, string resultKey, string uri, int expectedCount)
        {
            // FIXME! Mock
            // Arrange
            FacetsConfig2 facetsConfig = facetConfigFixture.GenerateByUri(uri);
            var resultConfig = resultConfigFixture.GenerateConfig(viewTypeId, resultKey);
            var jObject = new { facetsConfig = facetsConfig, resultConfig = resultConfig };
            var json = JsonConvert.SerializeObject(jObject);
            var requestContent = new StringContent(json, Encoding.UTF8, "application/json");

            // Act
            var response = await client.PostAsync("/api/result/load", requestContent);
            response.EnsureSuccessStatusCode();
            var responseJson = await response.Content.ReadAsStringAsync();
            var resultContent = JsonConvert.DeserializeObject<ResultContentSet>(responseJson);

            // Assert
            Assert.NotNull(resultContent?.Data?.DataCollection);
            var items = resultContent.Data.DataCollection.ToList();
            Assert.Equal(expectedCount, items.Count);
        }
    }
}
