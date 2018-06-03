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
using Npgsql.Logging;

namespace QuerySeadAPI {

    public class Startup
    {
        public IConfigurationRoot Configuration { get; }
        public IContainer Container { get; private set; }
        public IQueryBuilderSetting Options { get; private set; }
  
        public Startup(IHostingEnvironment env)
        {
            var configFile = "appsettings.json"; //  Path.Combine("some_hidden_place", "appsettings.json");
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile(configFile, optional: false, reloadOnChange: true)
                .AddEnvironmentVariables();

            Configuration = builder.Build();
            Options = Configuration.GetSection("QueryBuilderSetting").Get<QueryBuilderSetting>();
        }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddOptions();
            services.AddCors();
            services.AddMvc();

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
                string[] files = { "QuerySeadAPI.xml", "QuerySeadDomain.xml" };
                foreach (var file in files)
                {
                    c.IncludeXmlComments(Path.Combine(PlatformServices.Default.Application.ApplicationBasePath, file));
                }
            });
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IApplicationLifetime appLifetime)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

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

//app.UseCors(options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader().AllowCredentials());

//// Enable middleware to serve generated Swagger as a JSON endpoint.
//app.UseSwagger();

//// Enable middleware to serve swagger-ui (HTML, JS, CSS etc.), specifying the Swagger JSON endpoint.
//app.UseSwaggerUI(c =>
//{
//    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Query SEAD API V1");
//    //c.EnabledValidator();
//    //c.BooleanValues(new object[] { 0, 1 });
//    //c.DocExpansion("full");
//    //c.InjectOnCompleteJavaScript("/swagger-ui/on-complete.js");
//    //c.InjectOnFailureJavaScript("/swagger-ui/on-failure.js");
//    //c.SupportedSubmitMethods(new[] { "get", "post", "put", "patch" });
//    c.ShowRequestHeaders();
//    c.ShowJsonEditor();
//});

//services.AddCors(options =>
//{
//    options.AddPolicy("QuerySeadCorsPolicy",
//        builder => builder
//            .AllowAnyOrigin()
//            .AllowAnyMethod()
//            .AllowAnyHeader()
//            .AllowCredentials());
//});

//app.UseCors("QuerySeadCorsPolicy");
