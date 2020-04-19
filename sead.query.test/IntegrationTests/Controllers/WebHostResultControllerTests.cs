using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Newtonsoft.Json;
using SeadQueryCore;
using SeadQueryCore.Model;
using SQT;
using SQT.Infrastructure;
using SQT.Mocks;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace IntegrationTests
{
    [Collection("JsonSeededFacetContext")]
    public class HostResultControllerTests : DisposableFacetContextContainer
    {
        private MockFacetsConfigFactory facetConfigFixture;

        public HostResultControllerTests(JsonSeededFacetContextFixture fixture) : base(fixture)
        {
            facetConfigFixture = new MockFacetsConfigFactory(Registry.Facets);
        }

        public IWebHostBuilder CreateTestWebHostBuilder2<T>() where T: class
        {
            return new WebHostBuilder()
                .UseKestrel()
                .ConfigureServices(services => services.AddAutofac())
                //.UseConfiguration(config)
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseStartup<T>();
        }

        [Fact(Skip = "Not working")]
        public async Task LoadOfFinishSitesShouldEqualExpectedItems()
        {
            var testConfigs = new Dictionary<(string, string, string), int>()
            {
                { ("tabular", "site_level", "sites@sites:country@73/sites:"), 30 },
                { ("tabular", "aggregate_all", "sites@sites:country@73/sites:"), 1 },
                { ("tabular", "sample_group_level", "sites@sites:country@73/sites:"), 30 },
                { ("map", "map_result", "sites@sites:country@73/sites:"), 32 }
            };

            var builder = CreateTestWebHostBuilder2<TestStartup<TestDependencyService>>();

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
            FacetsConfig2 facetsConfig = facetConfigFixture.Create(uri);
            var resultConfig = ResultConfigFactory.Create(viewTypeId, resultKey);
            var jObject = new { facetsConfig = facetsConfig, resultConfig = resultConfig };
            var json = JsonConvert.SerializeObject(jObject);
            var requestContent = new StringContent(json, Encoding.UTF8, "application/json");

            // Act
            var response = await client.PostAsync(new Uri("/api/result/load"), requestContent);
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
