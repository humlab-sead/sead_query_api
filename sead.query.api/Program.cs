using System;
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
        var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddJsonFile("logging.json", true)
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
