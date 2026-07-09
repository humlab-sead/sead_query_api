using System;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace SeadQueryAPI.Infrastructure;

public static class BootstrapLogger
{
    public static ILogger Create()
    {
        var appSettingsPath = StartupEnvironment.ResolveAppSettingsPath();

        var configuration = new ConfigurationBuilder().AddJsonFile(appSettingsPath, optional: true).Build();

        return new LoggerConfiguration().ReadFrom.Configuration(configuration).CreateLogger();
    }
}
