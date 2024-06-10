using System;
using System.IO;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace SeadQueryAPI;

public static class Program
{
    public static int Main(string[] args)
    {
        Log.Logger = CreateSerilogger();

        Log.Information("Starting web host");

        try
        {
            CreateHostBuilder(args).Run();
            return 0;
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "Host terminated unexpectedly");
            return 1;
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }

    private static ILogger CreateSerilogger()
    {
        var appSettingsFolder = Environment.GetEnvironmentVariable("ASPNETCORE_APPSETTINGS_FOLDER");
        var appSettingsPath = string.IsNullOrEmpty(appSettingsFolder) ? "appsettings.json" : Path.Combine(appSettingsFolder, "appsettings.json");
        var configuration = new ConfigurationBuilder()
                .AddJsonFile(appSettingsPath, true)
                .Build();

        return new LoggerConfiguration()
            .ReadFrom.Configuration(configuration)
            .CreateLogger();
    }

    public static IHost CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .UseServiceProviderFactory(new AutofacServiceProviderFactory())
            .ConfigureWebHostDefaults(
                webBuilder => {
                    webBuilder.ConfigureKestrel(_ => { })
                    .UseStartup<Startup>();
                })
            .UseSerilog()
            .Build();
}
