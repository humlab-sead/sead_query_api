using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Hosting;
using System.IO;

namespace IntegrationTests
{
    public class SeadTestHostBuilder
    {
        public IHostBuilder Create<T>() where T : class
        {
            //NpgsqlLogManager.Provider = new ConsoleLoggingProvider(NpgsqlLogLevel.Trace, true, true);
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
