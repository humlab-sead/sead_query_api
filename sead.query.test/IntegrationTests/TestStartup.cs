using Autofac;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Serialization;
using Npgsql.Logging;
using SeadQueryAPI.Controllers;
using SeadQueryCore;
using SQT.Infrastructure;

namespace IntegrationTests
{
    public class TestStartup<T> where T: TestDependencyService, new()
    {
        public IConfigurationRoot Configuration { get; }
        public IContainer Container { get; private set; }
        public ISetting Options { get; private set; }

        public TestStartup()
        {
            Options = new SQT.SettingFactory().Create().Value;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddOptions();
            //services.AddControllers();
            services.AddMvc()
                .AddApplicationPart(typeof(FacetsController).Assembly)
                .AddNewtonsoftJson(options =>
                {
                    var resolver = new SeadQueryAPI.Serializers.SeadQueryResolver();
                    options.SerializerSettings.ContractResolver = resolver as DefaultContractResolver;
                });
        }
        //( option => option.EnableEndpointRouting = false);

        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterModule(new T() { Options = Options });
        }

        public void Configure(IApplicationBuilder app)
        {
            NpgsqlLogManager.Provider = new ConsoleLoggingProvider(NpgsqlLogLevel.Trace, true, true);
            //app.UseMvcWithDefaultRoute();
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                // endpoints.MapDefaultControllerRoute();
                // endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
                // endpoints.MapHealthChecks("/health");

            });
        }
    }
}
