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

namespace QuerySeadTests
{

    public class ControllerTestStartup<T> where T: DependencyService, new()
    {
        public IConfigurationRoot Configuration { get; }
        public IContainer Container { get; private set; }
        public IQueryBuilderSetting Options { get; private set; }

        public ControllerTestStartup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile("appsettings.test.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
            Options = Configuration.GetSection("QueryBuilderSetting").Get<QueryBuilderSetting>();
        }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddOptions().AddMvc();
            Container = new T().Register(services, Options);
            return Container.Resolve<IServiceProvider>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IApplicationLifetime appLifetime)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging")).AddDebug();
            NpgsqlLogManager.Provider = new ConsoleLoggingProvider(NpgsqlLogLevel.Trace, true, true);
            app.UseMvcWithDefaultRoute();
        }

    }
    [TestClass]
    public class ControllerTests
    {
        private fixtures.FacetConfigGenerator fixture;

        [TestInitialize()]
        public void Initialize()
        {
            fixture = new fixtures.FacetConfigGenerator();
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
        public async Task CanTestWebServer2()
        {
            var builder = CreateTestWebHostBuilder2<ControllerTestStartup<TestDependencyService>>();
            using (var server = new TestServer(builder))
            {
                var response = await server.CreateRequest("/api/values")
                    .SendAsync("GET");
                response.EnsureSuccessStatusCode();
                Assert.AreEqual("[\"value1\",\"value2\"]", await response.Content.ReadAsStringAsync());
            }
        }

        [TestMethod]
        public async Task CanGetFacets()
        {
            var builder = CreateTestWebHostBuilder2<ControllerTestStartup<TestDependencyService>>();
            using (var server = new TestServer(builder))
            {
                var response = await server.CreateRequest("/api/facets").SendAsync("GET");
                response.EnsureSuccessStatusCode();
                var json = await response.Content.ReadAsStringAsync();
                List<JObject> facets = JsonConvert.DeserializeObject<List<JObject>>(json.ToString());
                Assert.AreEqual(22, facets.Count());
            }
        }

        [TestMethod]
        public async Task CanLoadSimpleDiscreteFacetWithoutPicks()
        {
            var builder = CreateTestWebHostBuilder2<ControllerTestStartup<TestDependencyService>>();
            using (var server = new TestServer(builder))
            {
                using (var client = server.CreateClient())
                {
                    foreach (var config in fixture.Data.DiscreteTestConfigsWithPicks)
                    {
                        FacetsConfig2 facetsConfig = fixture.GenerateByConfig(config);

                        var json = JsonConvert.SerializeObject(facetsConfig);
                        var request_content = new StringContent(json, Encoding.UTF8, "application/json");
                        var response = await client.PostAsync("/api/facets/load", request_content);
                        response.EnsureSuccessStatusCode();
                        var response_content = await response.Content.ReadAsStringAsync();
                        var jsonObject = JsonConvert.DeserializeObject<JObject>(response_content);
                        var facetContent = JsonConvert.DeserializeObject<FacetContent>(response_content);
                        Assert.AreEqual(config.ExpectedCount, facetContent.Items.Count, config.TargetCode);
                    }
                }
            }
        }

        [TestMethod]
        public async Task CanLoadDiscreteFacetConfigsWithPicks()
        {
            var builder = CreateTestWebHostBuilder2<ControllerTestStartup<TestDependencyService>>();
            using (var server = new TestServer(builder))
            {
                using (var client = server.CreateClient())
                {
                    foreach (var config in fixture.Data.DiscreteTestConfigsWithPicks)
                    {
                        FacetsConfig2 facetsConfig = fixture.GenerateByConfig(config);
                        
                        var json = JsonConvert.SerializeObject(facetsConfig);
                        var request_content = new StringContent(json, Encoding.UTF8, "application/json");
                        var response = await client.PostAsync("/api/facets/load", request_content);
                        response.EnsureSuccessStatusCode();
                        var response_content = await response.Content.ReadAsStringAsync();
                        var jsonObject = JsonConvert.DeserializeObject<JObject>(response_content);
                        var facetContent = JsonConvert.DeserializeObject<FacetContent>(response_content);
                        Assert.AreEqual(config.ExpectedCount, facetContent.Items.Count, config.UriConfig);
                    }
                }
            }
        }

        [TestMethod]
        public async Task LoadOfFinishSitesShouldEqualExpectedItems()
        {
            var builder = CreateTestWebHostBuilder2<ControllerTestStartup<TestDependencyService>>();
            using (var server = new TestServer(builder))
            {
                using (var client = server.CreateClient())
                {
                    // Arrange
                    FacetsConfig2 facetsConfig = fixture.GenerateByUri("sites@sites:country@73/sites:");
                    var json = JsonConvert.SerializeObject(facetsConfig);
                    var request_content = new StringContent(json, Encoding.UTF8, "application/json");

                    // Act
                    var response = await client.PostAsync("/api/facets/load", request_content);
                    response.EnsureSuccessStatusCode();
                    var response_content = await response.Content.ReadAsStringAsync();
                    var jsonObject = JsonConvert.DeserializeObject<JObject>(response_content);
                    var facetContent = JsonConvert.DeserializeObject<FacetContent>(response_content);

                    // Assert
                    Dictionary<string,int> items = facetContent.Items.ToDictionary(z => z.Category, z => z.Count ?? 0);
                    Dictionary<string,int> expected = fixture.Data.FinishSiteCount;
                    var isEqual = (expected == items) || (expected.Count == items.Count && !expected.Except(items).Any());
                    Assert.IsTrue(isEqual);
                }
            }
        }
    }
}
