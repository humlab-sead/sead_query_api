using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Hosting;
using Npgsql.Logging;
using System.IO;

namespace IntegrationTests
{
    public class SeadTestHostBuilder
    {
        public IHostBuilder Create<T>() where T : class
        {
//#if DEBUG
//            NpgsqlLogManager.Provider = new ConsoleLoggingProvider(NpgsqlLogLevel.Debug);
//            NpgsqlLogManager.IsParameterLoggingEnabled = true;
//#endif
            return new HostBuilder()
               .UseServiceProviderFactory(new AutofacServiceProviderFactory())
               .ConfigureWebHost(webHost =>
               {
                   webHost.UseStartup<T>();
                   webHost.UseContentRoot(Directory.GetCurrentDirectory());
                   webHost.UseTestServer();
               });
        }
    }
}
