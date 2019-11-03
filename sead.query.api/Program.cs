using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;

namespace SeadQueryAPI {

    public static class Program
    {

        public static int Main(string[] args)
        {
            Log.Logger = CreateSerilogger();

            try {
                Log.Information("Starting web host");
                CreateWebHostBuilder(args).Build().Run();
                return 0;
            } catch (Exception ex) {
                Log.Fatal(ex, "Host terminated unexpectedly");
                return 1;
            } finally {
                Log.CloseAndFlush();
            }
        }

        private static Serilog.ILogger CreateSerilogger()
        {
            return new LoggerConfiguration()
                .ReadFrom.Configuration(new ConfigurationBuilder()
                    .AddJsonFile("logging.json", true)
                    .Build())
                //.Enrich.FromLogContext()
                //.MinimumLevel.Debug()
                //.MinimumLevel.Override("Microsoft", LogEventLevel.Information).WriteTo.Debug()
                //.WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj} {Properties:j}{NewLine}{Exception}")
                .CreateLogger();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .UseSerilog();

    }
}

