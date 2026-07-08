using System;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using SeadQueryAPI.Cli;
using SeadQueryAPI.Infrastructure;
using Serilog;

namespace SeadQueryAPI;

public static class Program
{
    public static int Main(string[] args)
    {
        Log.Logger = BootstrapLogger.Create();

        Log.Information("Starting application");

        try
        {
            var parser = new StartupCommandParser();
            var command = parser.Parse(args);
            return StartupCommandDispatcher.Run(command, CreateHost);
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "Application terminated unexpectedly");
            return 1;
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }

    public static IHost CreateHost(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .UseServiceProviderFactory(new AutofacServiceProviderFactory())
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.ConfigureKestrel(_ => { }).UseStartup<Startup>();
            })
            .UseSerilog()
            .Build();
}
