using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace SeadQueryAPI.Infrastructure;

public static class BootstrapLogger
{
    public static ILogger Create()
    {
        var appSettingsFolder = Environment.GetEnvironmentVariable("ASPNETCORE_APPSETTINGS_FOLDER");
        var appSettingsPath = string.IsNullOrEmpty(appSettingsFolder)
            ? "appsettings.json"
            : Path.Combine(appSettingsFolder, "appsettings.json");

        var configuration = new ConfigurationBuilder().AddJsonFile(appSettingsPath, optional: true).Build();

        return new LoggerConfiguration().ReadFrom.Configuration(configuration).CreateLogger();
    }
}
