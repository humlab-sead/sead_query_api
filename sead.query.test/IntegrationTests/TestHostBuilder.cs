using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Npgsql.Logging;
using System.Collections.Generic;
using System.IO;

namespace IntegrationTests
{
    public class TestHostBuilder
    {
        public IHostBuilder Create<TStartup>(string jsonFolder) where TStartup : class
        {
            //#if DEBUG
            //            NpgsqlLogManager.Provider = new ConsoleLoggingProvider(NpgsqlLogLevel.Debug);
            //            NpgsqlLogManager.IsParameterLoggingEnabled = true;
            //#endif
            return new HostBuilder()
               .UseServiceProviderFactory(new AutofacServiceProviderFactory())
               .ConfigureHostConfiguration(
                    builder =>
                    {
                        builder.AddInMemoryCollection(new Dictionary<string, string>
                        {
                            { "jsonFolder", jsonFolder }
                        });
                    }
                )
               .ConfigureWebHost(webHost =>
               {
                   webHost.UseStartup<TStartup>();
                   webHost.UseContentRoot(Directory.GetCurrentDirectory());
                   webHost.UseTestServer();
               });
        }
    }
}
