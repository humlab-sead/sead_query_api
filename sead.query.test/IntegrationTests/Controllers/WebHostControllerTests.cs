using Autofac;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Npgsql.Logging;
using SeadQueryCore;
using SeadQueryTest.Infrastructure;
using SeadQueryTest.Mocks;
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

    public class ControllerTestStartup<T> where T: TestDependencyService, new()
    {
        public IConfigurationRoot Configuration { get; }
        public IContainer Container { get; private set; }
        public ISetting Options { get; private set; }

        public ControllerTestStartup()
        {
            Options = new SeadQueryTest.SettingFactory().Create().Value;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddOptions().AddMvc();
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterModule(new T() { Options = Options });
        }

        public void Configure(IApplicationBuilder app)
        {
            NpgsqlLogManager.Provider = new ConsoleLoggingProvider(NpgsqlLogLevel.Trace, true, true);
            app.UseMvcWithDefaultRoute();
        }

    }

    public class ControllerTests
    {
        private readonly MockFacetsConfigFactory facetsConfigMockFactory;

        public ControllerTests()
        {
            facetsConfigMockFactory = new MockFacetsConfigFactory(null);
        }

        public IHostBuilder CreateTestWebHostBuilder2<T>() where T: class
        {
            NpgsqlLogManager.Provider = new ConsoleLoggingProvider(NpgsqlLogLevel.Trace, true, true);

            return new HostBuilder()
                .ConfigureWebHost(webHost =>
                {
                    webHost.UseTestServer();
                    webHost.UseContentRoot(Directory.GetCurrentDirectory());
                    webHost.UseStartup<T>();
                    //webHost.ConfigureLogging((hostingContext, logging) =>
                    //{
                    //    logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
                    //    logging.AddConsole();
                    //    logging.AddDebug();
                    //    logging.AddEventSourceLogger();
                    //});

                });

        }

        [Fact(Skip = "Not working")]
        public async Task CanTestWebServer2()
        {
            var builder = CreateTestWebHostBuilder2<ControllerTestStartup<TestDependencyService>>();
            using (var host = await builder.StartAsync())
            {
                var response = await host.GetTestClient().GetAsync(new Uri("/api/values"));
                response.EnsureSuccessStatusCode();
                Assert.Equal("[\"value1\",\"value2\"]", await response.Content.ReadAsStringAsync());
            }
        }

        [Fact(Skip = "Not working")]
        public async Task CanGetFacets()
        {
            var builder = CreateTestWebHostBuilder2<ControllerTestStartup<TestDependencyService>>();
            using (var host = await builder.StartAsync()) {
                var response = await host.GetTestClient().GetAsync(new Uri("/api/facets"));
                response.EnsureSuccessStatusCode();
                var json = await response.Content.ReadAsStringAsync();
                List<JObject> facets = JsonConvert.DeserializeObject<List<JObject>>(json.ToString());
                Assert.Equal(22, facets.Count);
            }
        }

        [Fact(Skip = "Not working")]
        public async Task CanLoadSimpleDiscreteFacetWithoutPicks()
        {
            var data = new FacetConfigsByUriFixtures();
            var builder = CreateTestWebHostBuilder2<ControllerTestStartup<TestDependencyService>>();
            using (var host = await builder.StartAsync()) {
            using (var client = host.GetTestClient()) {
                foreach (var item in data.DiscreteTestConfigsWithPicks)
                {
                    var uri = item[0].ToString();
                    var expectedCount = item[2];
                    FacetsConfig2 facetsConfig = facetsConfigMockFactory.Create(uri);

                    var json = JsonConvert.SerializeObject(facetsConfig);
                    var request_content = new StringContent(json, Encoding.UTF8, "application/json");
                    var response = await client.PostAsync(new Uri("/api/facets/load"), request_content);
                    response.EnsureSuccessStatusCode();
                    var response_content = await response.Content.ReadAsStringAsync();
                    var facetContent = JsonConvert.DeserializeObject<FacetContent>(response_content);
                    Assert.Equal(expectedCount, facetContent.Items.Count);
                }}
            }
        }

        [Fact(Skip = "Not working")]
        public async Task CanLoadDiscreteFacetConfigsWithPicks()
        {
            var data = new FacetConfigsByUriFixtures();
            var builder = CreateTestWebHostBuilder2<ControllerTestStartup<TestDependencyService>>();
            using (var host = await builder.StartAsync()) {
            using (var client = host.GetTestClient()) {
                foreach (var item in data.DiscreteTestConfigsWithPicks)
                {
                    var uri = item[0].ToString();
                    var expectedCount = item[2];
                    FacetsConfig2 facetsConfig = facetsConfigMockFactory.Create(uri);

                    var json = JsonConvert.SerializeObject(facetsConfig);
                    var request_content = new StringContent(json, Encoding.UTF8, "application/json");
                    var response = await client.PostAsync(new Uri("/api/facets/load"), request_content);
                    response.EnsureSuccessStatusCode();
                    var response_content = await response.Content.ReadAsStringAsync();
                    var facetContent = JsonConvert.DeserializeObject<FacetContent>(response_content);
                    Assert.Equal(expectedCount, facetContent.Items.Count);
                }
            }}
        }

    }
}
