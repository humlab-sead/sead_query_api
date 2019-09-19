using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
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
using SeadQueryCore;
using SeadQueryAPI;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Text;
using SeadQueryTest.Infrastructure;
using SeadQueryInfra;
using SeadQueryTest.Infrastructure.Scaffolding;
using Xunit;

namespace SeadQueryTest
{

    public class ControllerTestStartup<T> where T: DependencyService, new()
    {
        public IConfigurationRoot Configuration { get; }
        public IContainer Container { get; private set; }
        public IQueryBuilderSetting Options { get; private set; }

        public ControllerTestStartup(IHostingEnvironment env)
        {
            Options = new MockOptionBuilder().Build().Value;
        }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddOptions().AddMvc();
            Container = new T().Register(services, Options);
            return Container.Resolve<IServiceProvider>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IApplicationLifetime appLifetime)
        {
            NpgsqlLogManager.Provider = new ConsoleLoggingProvider(NpgsqlLogLevel.Trace, true, true);
            app.UseMvcWithDefaultRoute();
        }

    }
    
    public class ControllerTests
    {
        private fixtures.FacetConfigGenerator fixture;

        public ControllerTests()
        {
            var registry = new RepositoryRegistry(ScaffoldUtility.DefaultFacetContext());
            fixture = new fixtures.FacetConfigGenerator(null);
        }

        public IWebHostBuilder CreateTestWebHostBuilder2<T>() where T: class
        {
            return new WebHostBuilder()
                .UseKestrel()
                //.UseConfiguration(config)
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseStartup<T>()
                .ConfigureLogging((hostingContext, logging) =>
                {
                    // Requires `using Microsoft.Extensions.Logging;`
                    logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
                    logging.AddConsole();
                    logging.AddDebug();
                    logging.AddEventSourceLogger();
                });
        }

        [Fact]
        public async Task CanTestWebServer2()
        {
            var builder = CreateTestWebHostBuilder2<ControllerTestStartup<TestDependencyService>>();
            using (var server = new TestServer(builder))
            {
                var response = await server.CreateRequest("/api/values")
                    .SendAsync("GET");
                response.EnsureSuccessStatusCode();
                Assert.Equal("[\"value1\",\"value2\"]", await response.Content.ReadAsStringAsync());
            }
        }

        [Fact]
        public async Task CanGetFacets()
        {
            var builder = CreateTestWebHostBuilder2<ControllerTestStartup<TestDependencyService>>();
            using (var server = new TestServer(builder))
            {
                var response = await server.CreateRequest("/api/facets").SendAsync("GET");
                response.EnsureSuccessStatusCode();
                var json = await response.Content.ReadAsStringAsync();
                List<JObject> facets = JsonConvert.DeserializeObject<List<JObject>>(json.ToString());
                Assert.Equal(22, facets.Count());
            }
        }

        [Fact]
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
                        Assert.Equal(config.ExpectedCount, facetContent.Items.Count);
                    }
                }
            }
        }

        [Fact]
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
                        Assert.Equal(config.ExpectedCount, facetContent.Items.Count);
                    }
                }
            }
        }

        [Fact]
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
                    Assert.True(isEqual);
                }
            }
        }
    }
}
