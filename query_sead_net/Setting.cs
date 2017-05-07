using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using QuerySeadDomain;

namespace QuerySeadAPI {

    public interface ISettingFactory {
        IQueryBuilderSetting Create();
    }

    public class SettingFactory : ISettingFactory {

        public IQueryBuilderSetting Create()
        {
            var builder = new ConfigurationBuilder()
                //.SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables();
                //.AddJsonFile("appsettings.Development.json", optional: false, reloadOnChange: true);

            IConfigurationRoot configuration = builder.Build();

            IQueryBuilderSetting setting = configuration
               .GetSection("QueryBuilderSetting")
               .Get<QueryBuilderSetting>();

            return setting;

        }
    }
}
