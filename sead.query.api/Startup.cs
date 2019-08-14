using Autofac;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using SeadQueryCore;
using Microsoft.AspNetCore.Routing;
using Swashbuckle.AspNetCore.Swagger;
using Microsoft.Extensions.PlatformAbstractions;
using System.IO;
using Npgsql.Logging;

namespace SeadQueryAPI {

    public class Startup
    {
        public IConfigurationRoot Configuration { get; }
        public IContainer Container { get; private set; }
        public IQueryBuilderSetting Options { get; private set; }

        public Startup(IHostingEnvironment env)
        {
            // https://docs.microsoft.com/en-us/aspnet/core/fundamentals/configuration/index?tabs=basicconfiguration&view=aspnetcore-2.1#simple-configuration
            // https://docs.microsoft.com/en-us/aspnet/core/fundamentals/environments?view=aspnetcore-2.1:
            // ASP.NET Core reads the environment variable ASPNETCORE_ENVIRONMENT at app startup and stores the value in IHostingEnvironment.EnvironmentName.
            // You can set ASPNETCORE_ENVIRONMENT to any value, but three values are supported by the framework:
            // Development, Staging, and Production. If ASPNETCORE_ENVIRONMENT isn't set, it defaults to Production.

            var builder = new ConfigurationBuilder()
                //.SetBasePath(env.ContentRootPath)  // Directory.GetCurrentDirectory()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables();

            Configuration = builder.Build();
            Options = Configuration.GetSection("QueryBuilderSetting").Get<QueryBuilderSetting>();

        }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            // Configuration.GetSection("Logging")
            services.AddOptions();
            services.AddCors();
            services.AddMvc();
            services.AddLogging(builder => builder.AddConsole());

            //AddSwagger(services);

            Container = new DependencyService().Register(services, Options);

            return Container.Resolve<IServiceProvider>();
        }

        private static void AddSwagger(IServiceCollection services)
        {
            //See https://github.com/domaindrivendev/Swashbuckle
            // Register the Swagger generator, defining one or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info
                {
                    Title = "Query SEAD API",
                    Version = "v1",
                    Description = "API used by the Query SEAD client",
                    TermsOfService = "None"
                });
                string[] files = { "SeadQueryAPI.xml", "SeadQueryCore.xml" };
                foreach (var file in files)
                {
                    c.IncludeXmlComments(Path.Combine(PlatformServices.Default.Application.ApplicationBasePath, file));
                }
            });
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IApplicationLifetime appLifetime)
        {
            NpgsqlLogManager.Provider = new ConsoleLoggingProvider(NpgsqlLogLevel.Trace, true, true);

            app.UseMvcWithDefaultRoute();
            app.UseResponseBuffering();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            appLifetime.ApplicationStopped.Register(() => this.Container.Dispose());
        }

    }
}

