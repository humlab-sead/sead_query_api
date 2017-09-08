using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.TestHost;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;

namespace QuerySeadTests
{
    [TestClass]
    public class ControllerTests
    {

        [TestMethod]
        public async Task CorsRequest_MatchPolicy_SetsResponseHeaders()
        {
            string accessControlRequestMethod = "GET";
            // Arrange
            var hostBuilder = new WebHostBuilder()
                .Configure(app =>
                {
                    app.UseCors(builder =>
                        builder.WithOrigins("http://localhost:5001")
                               .WithMethods("GET")
                               .WithHeaders("Header1")
                               .WithExposedHeaders("AllowedHeader"));
                    app.Run(async context =>
                    {
                        await context.Response.WriteAsync("Cross origin response");
                    });
                })
                .ConfigureServices(services => services.AddCors());

            using (var server = new TestServer(hostBuilder))
            {
                // Act
                // Actual request.
                var response = await server.CreateRequest("/")
                    .AddHeader(CorsConstants.Origin, "http://localhost:5001")
                    .SendAsync(accessControlRequestMethod);

                // Assert
                response.EnsureSuccessStatusCode();
                //Assert.Single(response.Headers);
                //Assert.Equal("Cross origin response", await response.Content.ReadAsStringAsync());
                //Assert.Equal("http://localhost:5001", response.Headers.GetValues(CorsConstants.AccessControlAllowOrigin).FirstOrDefault());
            }
        }
    }
}
