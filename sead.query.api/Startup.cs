using Autofac;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
//using Microsoft.Extensions.PlatformAbstractions;
using Newtonsoft.Json.Serialization;
using Npgsql.Logging;
using SeadQueryCore;
//using Swashbuckle.AspNetCore.Swagger;
using System;
using System.IO;

namespace SeadQueryAPI
{

    public class Startup
    {
        public IConfigurationRoot Configuration { get; private set; }

        public IContainer Container { get; private set; }

        public Startup()
        {
            Configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();
        }

        private Setting GetOptions()
        {
            return Configuration.GetSection("QueryBuilderSetting").Get<Setting>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IHostApplicationLifetime appLifetime)
        {
            NpgsqlLogManager.Provider = new ConsoleLoggingProvider(NpgsqlLogLevel.Trace, true, true);

            // app.UseResponseBuffering();

            app.UseRouting();
            app.UseCors(builder => builder
                .AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod()
                .SetPreflightMaxAge(TimeSpan.FromMinutes(665))
            );

            app.UseEndpoints(endpoints => {
                endpoints.MapControllers();
                endpoints.MapDefaultControllerRoute();
                // endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
                // endpoints.MapHealthChecks("/health");
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            appLifetime.ApplicationStopped.Register(() => this.Container.Dispose());
        }

        public void ConfigureServices(IServiceCollection services)
        {
            _ = services
                .AddOptions()
                .AddCors()
                .AddControllers()
                .AddNewtonsoftJson(options =>
                {
                    var resolver = new Serializers.SeadQueryResolver();
                    options.SerializerSettings.ContractResolver = resolver as DefaultContractResolver;
                });
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterModule(new DependencyService() { Options = GetOptions() });
        }
    }
}

