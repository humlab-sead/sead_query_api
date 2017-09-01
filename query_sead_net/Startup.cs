using Autofac;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using QuerySeadDomain;
using Microsoft.AspNetCore.Routing;
using Swashbuckle.AspNetCore.Swagger;
using Microsoft.Extensions.PlatformAbstractions;
using System.IO;

namespace QuerySeadAPI {

    public class Startup
    {
        public IConfigurationRoot Configuration { get; }
        public IContainer Container { get; private set; }
        public IQueryBuilderSetting Options { get; private set; }
  
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                //.AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

            Configuration = builder.Build();
            Options = Configuration.GetSection("QueryBuilderSetting").Get<QueryBuilderSetting>();
        }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddOptions();
            services.AddMvc();

            // See https://github.com/domaindrivendev/Swashbuckle
            // Register the Swagger generator, defining one or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info {
                    Title = "Query SEAD API",
                    Version = "v1",
                    Description = "API used by the Query SEAD client",
                    TermsOfService = "None"
                });
                string[] files = { "QuerySeadAPI.xml", "QuerySeadDomain.xml" } ;
                foreach (var file in files) {
                    c.IncludeXmlComments(Path.Combine(PlatformServices.Default.Application.ApplicationBasePath, file));
                }
            });


            Container = new DependencyService().Register(services, Options);

            return Container.Resolve<IServiceProvider>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IApplicationLifetime appLifetime)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();
 
            app.UseMvcWithDefaultRoute();

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS etc.), specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Query SEAD API V1");
                //c.EnabledValidator();
                //c.BooleanValues(new object[] { 0, 1 });
                //c.DocExpansion("full");
                //c.InjectOnCompleteJavaScript("/swagger-ui/on-complete.js");
                //c.InjectOnFailureJavaScript("/swagger-ui/on-failure.js");
                //c.SupportedSubmitMethods(new[] { "get", "post", "put", "patch" });
                c.ShowRequestHeaders();
                c.ShowJsonEditor();
            });

            appLifetime.ApplicationStopped.Register(() => this.Container.Dispose());
        }

    }
}
