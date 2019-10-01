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
using Newtonsoft.Json.Serialization;

namespace SeadQueryAPI {

    public class Startup
    {
        public IContainer Container { get; private set; }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IApplicationLifetime appLifetime)
        {
            NpgsqlLogManager.Provider = new ConsoleLoggingProvider(NpgsqlLogLevel.Trace, true, true);

            app.UseMvcWithDefaultRoute();
            app.UseResponseBuffering();
            app.UseCors(builder => builder
                .AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod()
                .SetPreflightMaxAge(TimeSpan.FromMinutes(665))
            );

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            appLifetime.ApplicationStopped.Register(() => this.Container.Dispose());
        }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            _ = services
                .AddOptions()
                .AddCors()
                .AddMvc()
                .AddJsonOptions(opts =>
                {
                    var settings = opts.SerializerSettings;
                    var resolver = new SeadQueryAPI.Serializers.SeadQueryResolver();
                    settings.ContractResolver = resolver as DefaultContractResolver;
                });

            services.AddLogging(builder => builder.AddConsole());
            //AddSwagger(services);

            var options = GetOptions();
            Container = new DependencyService().Register(services, options);

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
                    Title = "SEAD Query API",
                    Version = "v1",
                    Description = "API used by the SEAD Query clients",
                    TermsOfService = "None"
                });
                string[] files = { "SeadQueryAPI.xml", "SeadQueryCore.xml" };
                foreach (var file in files)
                {
                    c.IncludeXmlComments(Path.Combine(PlatformServices.Default.Application.ApplicationBasePath, file));
                }
            });
        }

        private QueryBuilderSetting GetOptions()
        {
            var options = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build()
                .GetSection("QueryBuilderSetting")
                .Get<QueryBuilderSetting>();
            return options;
        }
    }
}

