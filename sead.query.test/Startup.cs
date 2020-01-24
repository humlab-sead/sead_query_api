using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Npgsql.Logging;
using SeadQueryCore;
using System.Diagnostics;

namespace SeadQueryTest
{

    [TestClass]
    public static class Startup
    {
        public static IConfigurationRoot Configuration;
        public static Setting Options;

        [AssemblyInitialize()]
        public static void AssemblyInit()
        {
            //Configure();
            //ConfigureServices();
        }

        //public static void Configure()
        //{
        //    NpgsqlLogManager.Provider = new ConsoleLoggingProvider(NpgsqlLogLevel.Debug, false, false);
        //    var builder = new ConfigurationBuilder()
        //        .SetBasePath(System.AppContext.BaseDirectory)
        //        .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
        //        .AddJsonFile("appsettings.test.json", optional: true)
        //        .AddEnvironmentVariables();
        //    Configuration = builder.Build();
        //    Options = Configuration.GetSection("QueryBuilderSetting").Get<QueryBuilderSetting>();
        //}

        //public static void ConfigureServices()
        //{
        //}

        [AssemblyCleanup()]
        public static void AssemblyCleanup()
        {
            Debug.WriteLine("AssemblyCleanup");
        }
    }
}
