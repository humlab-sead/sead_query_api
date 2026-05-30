using System;
using System.IO;
using System.Linq;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SeadQueryAPI.Services;
using Serilog;

namespace SeadQueryAPI;

public static class Program
{
    private const string ImportFacetConfigArgument = "--import-facet-config";
    private const string ValidateFacetConfigArgument = "--validate-facet-config";

    public static int Main(string[] args)
    {
        Log.Logger = CreateSerilogger();

        Log.Information("Starting web host");

        try
        {
            if (TryGetFacetRouteCommand(args, out var commandType, out var configurationFilePath, out var parseError))
            {
                return commandType switch
                {
                    FacetRouteConfigurationCommandType.Import => RunFacetRouteImport(configurationFilePath, args),
                    FacetRouteConfigurationCommandType.Validate => RunFacetRouteValidation(configurationFilePath, args),
                    _ => throw new InvalidOperationException($"Unsupported facet route configuration command '{commandType}'."),
                };
            }

            if (!string.IsNullOrWhiteSpace(parseError))
            {
                throw new ArgumentException(parseError);
            }

            CreateHostBuilder(args).Run();
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

    private static int RunFacetRouteValidation(string configurationFilePath, string[] args)
    {
        var hostArgs = RemoveFacetRouteConfigurationCommandArguments(args);
        using var host = CreateHostBuilder(hostArgs);
        using var scope = host.Services.CreateScope();

        var command = scope.ServiceProvider.GetRequiredService<FacetRouteConfigurationValidationCommand>();
        command.Run(configurationFilePath);

        Log.Information("Validated facet route configuration from {ConfigurationFilePath}", configurationFilePath);
        return 0;
    }

    private static int RunFacetRouteImport(string configurationFilePath, string[] args)
    {
        var hostArgs = RemoveFacetRouteConfigurationCommandArguments(args);
        using var host = CreateHostBuilder(hostArgs);
        using var scope = host.Services.CreateScope();

        var command = scope.ServiceProvider.GetRequiredService<FacetRouteConfigurationImportCommand>();
        command.Run(configurationFilePath);

        Log.Information("Imported facet route configuration from {ConfigurationFilePath}", configurationFilePath);
        return 0;
    }

    internal static bool TryGetFacetRouteCommand(
        string[] args,
        out FacetRouteConfigurationCommandType commandType,
        out string configurationFilePath,
        out string parseError
    )
    {
        commandType = FacetRouteConfigurationCommandType.None;
        configurationFilePath = null;
        parseError = null;

        if (args.Length == 0)
        {
            return false;
        }

        var importArgumentIndex = Array.IndexOf(args, ImportFacetConfigArgument);
        var validateArgumentIndex = Array.IndexOf(args, ValidateFacetConfigArgument);

        if (importArgumentIndex >= 0 && validateArgumentIndex >= 0)
        {
            parseError = $"{ImportFacetConfigArgument} and {ValidateFacetConfigArgument} cannot be used together.";
            return false;
        }

        var commandArgumentIndex = importArgumentIndex >= 0 ? importArgumentIndex : validateArgumentIndex;
        if (commandArgumentIndex < 0)
        {
            return false;
        }

        commandType = importArgumentIndex >= 0 ? FacetRouteConfigurationCommandType.Import : FacetRouteConfigurationCommandType.Validate;

        var commandArgument =
            commandType == FacetRouteConfigurationCommandType.Import ? ImportFacetConfigArgument : ValidateFacetConfigArgument;

        if (commandArgumentIndex == args.Length - 1)
        {
            parseError = $"{commandArgument} requires a configuration file path.";
            return false;
        }

        configurationFilePath = args[commandArgumentIndex + 1];

        if (string.IsNullOrWhiteSpace(configurationFilePath))
        {
            parseError = $"{commandArgument} requires a non-empty configuration file path.";
            configurationFilePath = null;
            return false;
        }

        return true;
    }

    private static string[] RemoveFacetRouteConfigurationCommandArguments(string[] args)
    {
        var importArgumentIndex = Array.IndexOf(args, ImportFacetConfigArgument);
        var validateArgumentIndex = Array.IndexOf(args, ValidateFacetConfigArgument);

        return args.Where((_, index) => index != importArgumentIndex && index != importArgumentIndex + 1)
            .Where((_, index) => index != validateArgumentIndex && index != validateArgumentIndex + 1)
            .ToArray();
    }

    private static ILogger CreateSerilogger()
    {
        var appSettingsFolder = Environment.GetEnvironmentVariable("ASPNETCORE_APPSETTINGS_FOLDER");
        var appSettingsPath = string.IsNullOrEmpty(appSettingsFolder)
            ? "appsettings.json"
            : Path.Combine(appSettingsFolder, "appsettings.json");
        var configuration = new ConfigurationBuilder().AddJsonFile(appSettingsPath, true).Build();

        return new LoggerConfiguration().ReadFrom.Configuration(configuration).CreateLogger();
    }

    public static IHost CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .UseServiceProviderFactory(new AutofacServiceProviderFactory())
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.ConfigureKestrel(_ => { }).UseStartup<Startup>();
            })
            .UseSerilog()
            .Build();

    internal enum FacetRouteConfigurationCommandType
    {
        None = 0,
        Import = 1,
        Validate = 2,
    }
}
