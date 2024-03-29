﻿using System;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace SeadQueryAPI
{
    public static class Program
    {
        public static int Main(string[] args)
        {
            Log.Logger = Logger.CreateSerilogger();

            try
            {
                Log.Information("Starting web host");
                var host = CreateHostBuilder(args).Build();
                host.Run();
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

        public static IHostBuilder CreateHostBuilder(string[] args) =>
           Host.CreateDefaultBuilder(args)
               .UseServiceProviderFactory(new AutofacServiceProviderFactory())
               .ConfigureWebHostDefaults(webBuilder =>
               {
                   webBuilder.ConfigureKestrel(_ =>
                   {
                        // Set properties and call methods on options
                    })
                   .UseStartup<Startup>();
               }).UseSerilog()
           ;
    }
}
