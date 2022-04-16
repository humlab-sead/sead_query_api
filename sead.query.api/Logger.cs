using Microsoft.Extensions.Configuration;
using Serilog;

namespace SeadQueryAPI
{
    public static class Logger
    {
        public static Serilog.ILogger CreateSerilogger()
        {
            // FIXME: Move settings to appsettings
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

    }
}

