using Autofac;
using Microsoft.Extensions.Configuration;
using QuerySeadAPI;
using QuerySeadDomain;

namespace QuerySeadTests
{
    public class TestDependencyService : DependencyService
    {
        public TestDependencyService()
        {

        }

        public IContainer Register()
        {
            return Register(null, OptionsHelper.GetOptions());
        }
    }

    public class OptionsHelper
    {
        public static IQueryBuilderSetting GetOptions()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(System.AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile("appsettings.test.json", optional: true);

            IConfigurationRoot configuration = builder.Build();

            IQueryBuilderSetting options = configuration.GetSection("QueryBuilderSetting").Get<QueryBuilderSetting>();

            return options;
        }
    }
}
