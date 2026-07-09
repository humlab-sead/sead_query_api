using System;
using System.IO;
using FluentAssertions;
using SeadQueryAPI.Infrastructure;
using Xunit;

namespace SQT.UnitTests.Services;

public class StartupEnvironmentTests
{
    [Fact]
    public void LoadDotEnv_WithAppSettingsFolder_LoadsDotEnvFromSameDirectory()
    {
        var tempDirectory = Directory.CreateTempSubdirectory();
        var dotenvPath = Path.Combine(tempDirectory.FullName, ".env");
        const string variableName = "SEAD_QUERY_API_STARTUP_ENV_TEST";
        var originalAppSettingsFolder = Environment.GetEnvironmentVariable("ASPNETCORE_APPSETTINGS_FOLDER");
        var originalValue = Environment.GetEnvironmentVariable(variableName);

        try
        {
            File.WriteAllText(dotenvPath, $"{variableName}=loaded-from-dotenv");
            Environment.SetEnvironmentVariable("ASPNETCORE_APPSETTINGS_FOLDER", tempDirectory.FullName);
            Environment.SetEnvironmentVariable(variableName, null);

            StartupEnvironment.LoadDotEnv();

            Environment.GetEnvironmentVariable(variableName).Should().Be("loaded-from-dotenv");
        }
        finally
        {
            Environment.SetEnvironmentVariable("ASPNETCORE_APPSETTINGS_FOLDER", originalAppSettingsFolder);
            Environment.SetEnvironmentVariable(variableName, originalValue);
            tempDirectory.Delete(recursive: true);
        }
    }
}