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
        private fixtures.SetupFacetsConfig fixture;

        [TestInitialize()]
        public void Initialize()
        {
            fixture = new fixtures.SetupFacetsConfig();
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
        public async Task CanLoadSimpleFacet()
        {
            var builder = CreateTestWebHostBuilder2<ControllerTestStartup<TestDependencyService>>();
            FacetsConfig2 facetsConfig = fixture.GenerateFacetsConfig(
                "sites", "sites",
                new List<FacetConfig2>() {
                    fixture.GenerateFacetConfig("sites", 0, new List<FacetConfigPick>())
                }
            );
            var json = JsonConvert.SerializeObject(facetsConfig);
            using (var server = new TestServer(builder))
            {
                var request_content = new StringContent(json, Encoding.UTF8, "application/json");
                using (var client = server.CreateClient()) {
                    var response = await client.PostAsync("/api/facets/load", request_content);
                    response.EnsureSuccessStatusCode();
                    var response_content = await response.Content.ReadAsStringAsync();
                    var jsonObject = JsonConvert.DeserializeObject<JObject>(response_content);
                    var facetContent = JsonConvert.DeserializeObject<FacetContent>(response_content);
                    Assert.IsTrue(facetContent.Items.Count > 0);
                    Assert.Fail("Add additional tests");
                }
            }
        }

        //[TestMethod]
        //public async Task CorsRequest_MatchPolicy_SetsResponseHeaders()
        //{
        //    string accessControlRequestMethod = "GET";
        //    // Arrange
        //    var hostBuilder = new WebHostBuilder()
        //        .Configure(app =>
        //        {
        //            app.UseCors(builder =>
        //                builder.WithOrigins("http://localhost:5001")
        //                       .WithMethods("GET")
        //                       .WithHeaders("Header1")
        //                       .WithExposedHeaders("AllowedHeader"));
        //            app.Run(async context =>
        //            {
        //                await context.Response.WriteAsync("Cross origin response");
        //            });
        //        })
        //        .ConfigureServices(services => services.AddCors());

        //    using (var server = new TestServer(hostBuilder))
        //    {
        //        // Act
        //        // Actual request.
        //        var response = await server.CreateRequest("/")
        //            .AddHeader(CorsConstants.Origin, "http://localhost:5001")
        //            .SendAsync(accessControlRequestMethod);

        //        // Assert
        //        response.EnsureSuccessStatusCode();
        //        //Assert.Single(response.Headers);
        //        //Assert.Equal("Cross origin response", await response.Content.ReadAsStringAsync());
        //        //Assert.Equal("http://localhost:5001", response.Headers.GetValues(CorsConstants.AccessControlAllowOrigin).FirstOrDefault());
        //    }
        //}
    }
}
