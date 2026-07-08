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
        var appSettingsPath = ResolveAppSettingsPath(appSettingsFolder);

        var configuration = new ConfigurationBuilder().AddJsonFile(appSettingsPath, optional: true).Build();

        return new LoggerConfiguration().ReadFrom.Configuration(configuration).CreateLogger();
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
}
