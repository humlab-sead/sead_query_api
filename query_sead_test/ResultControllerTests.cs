using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.IO;
using Npgsql.Logging;
using Autofac;
using QuerySeadDomain;
using QuerySeadAPI;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Text;
using QuerySeadTests.fixtures;
using QuerySeadDomain.Model;

namespace QuerySeadTests
{

    [TestClass]
    public class ResultControllerTests
    {
        private FacetConfigFixture facetConfigFixture;
        private ResultConfigFixture resultConfigFixture;

        private TestContext testContextInstance;
        public TestContext TestContext
        {
            get { return testContextInstance; }
            set { testContextInstance = value; }
        }

        [TestInitialize()]
        public void Initialize()
        {
            facetConfigFixture = new fixtures.FacetConfigFixture();
            resultConfigFixture = new fixtures.ResultConfigFixture();
        }

        [TestMethod]
        public IWebHostBuilder CreateTestWebHostBuilder2<T>() where T: class
        {
            return new WebHostBuilder()
                .UseKestrel()
                //.UseConfiguration(config)
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseStartup<T>();
        }

        [TestMethod]
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
            Assert.IsNotNull(resultContent?.Data?.DataCollection, viewTypeId);
            var items = resultContent.Data.DataCollection.ToList();
            Assert.AreEqual(expectedCount, items.Count, viewTypeId);
        }
    }
}
