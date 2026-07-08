using System;
using System.IO;
using Autofac;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json.Serialization;
using SeadQueryCore;

namespace SeadQueryAPI;

public class Startup
{
    public IConfigurationRoot Configuration { get; private set; }

    public Autofac.IContainer Container { get; private set; }

    public Startup()
    {
        var appSettingsFolder = Environment.GetEnvironmentVariable("ASPNETCORE_APPSETTINGS_FOLDER");
        var appSettingsPath = ResolveAppSettingsPath(appSettingsFolder);

        Configuration = new ConfigurationBuilder()
            .AddJsonFile(appSettingsPath, optional: false, reloadOnChange: true)
            .AddEnvironmentVariables()
            .Build();
    }

    private static string ResolveAppSettingsPath(string appSettingsFolder)
    {
        if (!string.IsNullOrEmpty(appSettingsFolder))
        {
            return Path.GetFullPath(Path.Combine(appSettingsFolder, "appsettings.json"));
        }

        const string appSettingsFileName = "appsettings.json";
        var localPath = appSettingsFileName;

        if (File.Exists(localPath))
        {
            return Path.GetFullPath(localPath);
        }

        var repositoryRootPath = Path.Combine("..", appSettingsFileName);
        if (File.Exists(repositoryRootPath))
        {
            return Path.GetFullPath(repositoryRootPath);
        }

        return Path.GetFullPath(localPath);
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

        app.UseCors(builder => builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod().SetPreflightMaxAge(TimeSpan.FromMinutes(665)));

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
            .AddHostedService<Services.RouteConfigurationStartupValidationService>()
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
