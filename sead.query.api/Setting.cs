using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using SeadQueryCore;

namespace SeadQueryAPI
{

    public interface ISettingFactory
    {
        IQueryBuilderSetting Create(IConfigurationRoot env);
    }

    public class SettingFactory : ISettingFactory
    {

        public IQueryBuilderSetting Create(IConfigurationRoot configuration)
        {
            //var builder = new ConfigurationBuilder()
            //    .SetBasePath(env.ContentRootPath)
            //    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            //    .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
            //    .AddEnvironmentVariables();

            //IConfigurationRoot configuration = builder.Build();

            IQueryBuilderSetting setting = configuration.GetSection("QueryBuilderSetting").Get<QueryBuilderSetting>();

            return setting;

        }

        //public Startup(IHostingEnvironment env)
        //{
        //    var builder = new ConfigurationBuilder()
        //        .SetBasePath(env.ContentRootPath)
        //        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
        //        .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
        //        .AddEnvironmentVariables();

        //    return builder.Build();
        //}
    }
}
