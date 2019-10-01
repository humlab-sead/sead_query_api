using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace SeadQueryAPI {

    public static class Program
    {

        public static void Main(string[] args)
        {
            //var config = new ConfigurationBuilder()
            //    .SetBasePath(Directory.GetCurrentDirectory())
            //    .AddJsonFile("hosting.json", optional: true)
            //    .AddCommandLine(args)
            //    .Build();

            //var host = new WebHostBuilder()

            //    .UseKestrel()
            //    .UseConfiguration(config)
            //    .UseContentRoot(Directory.GetCurrentDirectory())
            //    .ConfigureLogging((hostingContext, logging) =>
            //    {
            //        // Requires `using Microsoft.Extensions.Logging;`
            //        logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
            //        logging.AddConsole();
            //        logging.AddDebug();
            //        logging.AddEventSourceLogger();
            //    })
            //    .UseStartup<Startup>();

            CreateWebHostBuilder(args)
                .ConfigureLogging((hostingContext, logging) => { })
                .Build()
                .Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();

    }
}

