using Autofac;
using Autofac.Core;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Serialization;
using Npgsql.Logging;
using SeadQueryAPI.Controllers;
using SeadQueryCore;
using SQT.Infrastructure;
using System.Configuration;

namespace IntegrationTests
{
    public class Startup<T> where T: DependencyService, new()
    {
        public Startup()
        {
        }

        /// <summary>
        /// ConfigureServices is where you register dependencies. Gets called by the runtime before
        /// the ConfigureContainer method, below.
        /// </summary>
        public virtual void ConfigureServices(IServiceCollection services)
        {
            services.AddOptions();
            services.AddMvc()
                .AddApplicationPart(typeof(FacetsController).Assembly)
                .AddNewtonsoftJson(options =>
                {
                    var resolver = new SeadQueryAPI.Serializers.SeadQueryResolver();
                    options.SerializerSettings.ContractResolver = resolver as DefaultContractResolver;
                });
        }

        /// <summary>
        /// ConfigureContainer is where you can register things directly with Autofac.
        /// Runs after ConfigureServices so the things here will override registrations made in ConfigureServices.
        /// Don't build the container; that gets done for you by the factory.
        /// </summary>
        /// <param name="builder"></param>
        public virtual void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterModule(
                new T()
                {
                    Options = new SQT.SettingFactory().Create().Value
                }
            );
        }

        /// <summary>
        /// Configure is where you add middleware and is called after ConfigureContainer.
        /// You can use IApplicationBuilder.ApplicationServices  here if you need to resolve things from the container.
        /// </summary>
        /// <param name="app"></param>
        public void Configure(IApplicationBuilder app)
        {
            NpgsqlLogManager.Provider = new ConsoleLoggingProvider(NpgsqlLogLevel.Trace, true, true);
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
