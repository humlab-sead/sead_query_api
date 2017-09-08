using Autofac;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Npgsql.Logging;
using QuerySeadAPI;
using QuerySeadDomain;
using System.Diagnostics;

namespace QuerySeadTests
{
    public class TestDependencyService : DependencyService
    {
        public TestDependencyService()
        {
        }

        public IContainer Register()
        {
            // This is a good place to injext stubs and mockups!
            return Register(null, Startup.Options);
        }
    }

    [TestClass]
    public class Startup
    {
        public static IConfigurationRoot Configuration;
        public static QueryBuilderSetting Options;

        [AssemblyInitialize()]
        public static void AssemblyInit(TestContext context)
        {
            Configure(context);
            ConfigureServices(context);
        }

        public static void Configure(TestContext context)
        {
            NpgsqlLogManager.Provider = new ConsoleLoggingProvider(NpgsqlLogLevel.Debug, false, false);
            var builder = new ConfigurationBuilder()
                .SetBasePath(System.AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile("appsettings.test.json", optional: true);
            Configuration = builder.Build();
            Options = Configuration.GetSection("QueryBuilderSetting").Get<QueryBuilderSetting>();
        }

        public static void ConfigureServices(TestContext context)
        {
        }

        [AssemblyCleanup()]
        public static void AssemblyCleanup()
        {
            Debug.WriteLine("AssemblyCleanup");
        }
    }
}
