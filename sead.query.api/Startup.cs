using Autofac;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json.Serialization;
using SeadQueryCore;
using System;
using System.IO;

namespace SeadQueryAPI;

public class Startup
{
    public IConfigurationRoot Configuration { get; private set; }

    public Autofac.IContainer Container { get; private set; }

    public Startup()
    {
        var appSettingsFolder = Environment.GetEnvironmentVariable("ASPNETCORE_APPSETTINGS_FOLDER");
        var appSettingsPath = string.IsNullOrEmpty(appSettingsFolder) ? "appsettings.json" : Path.Combine(appSettingsFolder, "appsettings.json");

        Configuration = new ConfigurationBuilder()
            .AddJsonFile(appSettingsPath, optional: false, reloadOnChange: true)
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

        app.UseRouting();

        if (env.IsDevelopment())
        {
            app.UseMiddleware<RequestLoggingMiddleware>();
        }

        app.UseCors(builder => builder
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod()
            .SetPreflightMaxAge(TimeSpan.FromMinutes(665))
        );

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
            endpoints.MapDefaultControllerRoute();
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

    public void ConfigureContainer(Autofac.ContainerBuilder builder)
    {
        builder.RegisterModule(new DependencyService() { Options = GetOptions() });
    }
}
