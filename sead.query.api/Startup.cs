using Autofac;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json.Serialization;
using SeadQueryCore;
using System;

namespace SeadQueryAPI
{

    public class Startup
    {
        public IConfigurationRoot Configuration { get; private set; }

        public Autofac.IContainer Container { get; private set; }

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
            //Configure application's request pipeline.

            //#if DEBUG
            //            NpgsqlLogManager.Provider = new ConsoleLoggingProvider(NpgsqlLogLevel.Debug);
            //            NpgsqlLogManager.IsParameterLoggingEnabled = true;
            //#endif

            /* Add a Microsoft.AspNetCore.Routing.EndpointRoutingMiddleware middleware
             * to the IApplicationBuilder. */
            app.UseRouting();

            /* Adds a CORS middleware to application pipeline
             * to allow cross domain requests. */
            app.UseCors(builder => builder
                .AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod()
                .SetPreflightMaxAge(TimeSpan.FromMinutes(665))
            );

            /* Adds a Microsoft.AspNetCore.Routing.EndpointMiddleware middleware
             * to the specified IApplicationBuilder with the EndpointDataSource 
             * instances built from configured IEndpointRouteBuilder.
             * The Microsoft.AspNetCore.Routing.EndpointMiddleware will execute
             * the Endpoint associated with the current request.*/
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapDefaultControllerRoute();
            });

            if (env.IsDevelopment())
            {
                /* Captures synchronous and asynchronous axceptions from the pipeline
                * and generates HTML error responses.*/
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

        public void ConfigureContainer(Autofac.ContainerBuilder builder)
        {
            builder.RegisterModule(new DependencyService() { Options = GetOptions() });
        }
    }
}

