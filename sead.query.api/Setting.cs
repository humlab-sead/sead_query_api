using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System.Runtime.CompilerServices;
using SeadQueryCore;

[assembly: InternalsVisibleTo("sead.query.test")]

namespace SeadQueryAPI
{

    public interface ISettingFactory
    {
        ISetting Create(IConfigurationRoot env);
    }

    public class SettingFactory : ISettingFactory
    {

        public ISetting Create(IConfigurationRoot configuration)
        {
            //var builder = new ConfigurationBuilder()
            //    .SetBasePath(env.ContentRootPath)
            //    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            //    .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
            //    .AddEnvironmentVariables();

            //IConfigurationRoot configuration = builder.Build();

            ISetting setting = configuration.GetSection("QueryBuilderSetting").Get<Setting>();

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
