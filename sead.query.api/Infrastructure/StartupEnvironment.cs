using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SeadQueryInfra;

namespace SeadQueryAPI.Infrastructure;

internal static class StartupEnvironment
{
    private const string AppSettingsFolderVariable = "ASPNETCORE_APPSETTINGS_FOLDER";
    private const string AppSettingsFileName = "appsettings.json";
    private const string DotEnvFileName = ".env";

    public static void LoadDotEnv()
    {
        DotEnv.Load(GetDotEnvSearchPaths().ToArray());
    }

    public static string ResolveAppSettingsPath()
    {
        var appSettingsFolder = Environment.GetEnvironmentVariable(AppSettingsFolderVariable);
        return ResolveAppSettingsPath(appSettingsFolder);
    }

    internal static IReadOnlyList<string> GetDotEnvSearchPaths()
    {
        var candidates = new List<string>();
        var appSettingsDirectory = Path.GetDirectoryName(ResolveAppSettingsPath());

        if (!string.IsNullOrEmpty(appSettingsDirectory))
        {
            candidates.Add(Path.Combine(appSettingsDirectory, DotEnvFileName));
        }

        var currentDirectory = Directory.GetCurrentDirectory();
        candidates.Add(Path.Combine(currentDirectory, DotEnvFileName));
        candidates.Add(Path.Combine(currentDirectory, "conf", DotEnvFileName));
        candidates.Add(Path.Combine(currentDirectory, "..", DotEnvFileName));

        return candidates
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToArray();
    }

    private static string ResolveAppSettingsPath(string appSettingsFolder)
    {
        if (!string.IsNullOrEmpty(appSettingsFolder))
        {
            return Path.GetFullPath(Path.Combine(appSettingsFolder, AppSettingsFileName));
        }

        var localPath = AppSettingsFileName;
        if (File.Exists(localPath))
        {
            return Path.GetFullPath(localPath);
        }

        var repositoryRootPath = Path.Combine("..", AppSettingsFileName);
        if (File.Exists(repositoryRootPath))
        {
            return Path.GetFullPath(repositoryRootPath);
        }

        return Path.GetFullPath(localPath);
    }
}