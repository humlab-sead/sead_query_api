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
    private const string PrintFacetSqlArgument = "--print-facet-sql";
    private const string PrintResultSqlArgument = "--print-result-sql";
    private const string ViewTypeArgument = "--view-type";
    private const string ResultCodeArgument = "--result-code";

    public static int Main(string[] args)
    {
        Log.Logger = CreateSerilogger();

        Log.Information("Starting web host");

        try
        {
            var hasFacetRouteCommand = TryGetFacetRouteCommand(
                args,
                out var commandType,
                out var configurationFilePath,
                out var routeParseError
            );
            var hasFacetSqlCommand = TryGetFacetSqlCommand(args, out var facetUrl, out var sqlParseError);
            var hasResultSqlCommand = TryGetResultSqlCommand(
                args,
                out var resultFacetUrl,
                out var viewTypeId,
                out var resultCode,
                out var resultSqlParseError
            );

            var parseError = routeParseError ?? sqlParseError ?? resultSqlParseError;

            if ((hasFacetRouteCommand && hasFacetSqlCommand) || (hasFacetRouteCommand && hasResultSqlCommand))
            {
                throw new ArgumentException(
                    $"SQL probe commands cannot be used together with {ImportFacetConfigArgument} or {ValidateFacetConfigArgument}."
                );
            }

            if (hasFacetSqlCommand && hasResultSqlCommand)
            {
                throw new ArgumentException($"{PrintFacetSqlArgument} and {PrintResultSqlArgument} cannot be used together.");
            }

            if (hasFacetRouteCommand)
            {
                return commandType switch
                {
                    FacetRouteConfigurationCommandType.Import => RunFacetRouteImport(configurationFilePath, args),
                    FacetRouteConfigurationCommandType.Validate => RunFacetRouteValidation(configurationFilePath, args),
                    _ => throw new InvalidOperationException($"Unsupported facet route configuration command '{commandType}'."),
                };
            }

            if (hasFacetSqlCommand)
            {
                return RunFacetSqlProbe(facetUrl, args);
            }

            if (hasResultSqlCommand)
            {
                return RunResultSqlProbe(resultFacetUrl, viewTypeId, resultCode, args);
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

    private static int RunFacetSqlProbe(string facetUrl, string[] args)
    {
        var hostArgs = RemoveFacetSqlCommandArguments(args);
        using var host = CreateHostBuilder(hostArgs);
        using var scope = host.Services.CreateScope();

        var command = scope.ServiceProvider.GetRequiredService<FacetUrlSqlProbeCommand>();
        command.Run(facetUrl);

        Log.Information("Printed facet SQL for {FacetUrl}", facetUrl);
        return 0;
    }

    private static int RunResultSqlProbe(string facetUrl, string viewTypeId, string resultCode, string[] args)
    {
        var hostArgs = RemoveResultSqlCommandArguments(args);
        using var host = CreateHostBuilder(hostArgs);
        using var scope = host.Services.CreateScope();

        var command = scope.ServiceProvider.GetRequiredService<ResultUrlSqlProbeCommand>();
        command.Run(facetUrl, viewTypeId, resultCode);

        Log.Information(
            "Printed result SQL for {FacetUrl} with view type {ViewTypeId} and result code {ResultCode}",
            facetUrl,
            viewTypeId,
            resultCode ?? "(default)"
        );
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

    internal static bool TryGetFacetSqlCommand(string[] args, out string facetUrl, out string parseError)
    {
        facetUrl = null;
        parseError = null;

        if (args.Length == 0)
        {
            return false;
        }

        var commandArgumentIndex = Array.IndexOf(args, PrintFacetSqlArgument);
        if (commandArgumentIndex < 0)
        {
            return false;
        }

        if (commandArgumentIndex == args.Length - 1)
        {
            parseError = $"{PrintFacetSqlArgument} requires a facet URL.";
            return false;
        }

        facetUrl = args[commandArgumentIndex + 1];
        if (string.IsNullOrWhiteSpace(facetUrl))
        {
            parseError = $"{PrintFacetSqlArgument} requires a non-empty facet URL.";
            facetUrl = null;
            return false;
        }

        return true;
    }

    internal static bool TryGetResultSqlCommand(
        string[] args,
        out string facetUrl,
        out string viewTypeId,
        out string resultCode,
        out string parseError
    )
    {
        facetUrl = null;
        viewTypeId = "tabular";
        resultCode = null;
        parseError = null;

        if (args.Length == 0)
        {
            return false;
        }

        var commandArgumentIndex = Array.IndexOf(args, PrintResultSqlArgument);
        if (commandArgumentIndex < 0)
        {
            return false;
        }

        if (commandArgumentIndex == args.Length - 1)
        {
            parseError = $"{PrintResultSqlArgument} requires a facet URL.";
            return false;
        }

        facetUrl = args[commandArgumentIndex + 1];
        if (string.IsNullOrWhiteSpace(facetUrl))
        {
            parseError = $"{PrintResultSqlArgument} requires a non-empty facet URL.";
            facetUrl = null;
            return false;
        }

        viewTypeId = GetOptionValue(args, ViewTypeArgument) ?? "tabular";
        if (string.IsNullOrWhiteSpace(viewTypeId))
        {
            parseError = $"{ViewTypeArgument} requires a non-empty view type when provided.";
            return false;
        }

        resultCode = GetOptionValue(args, ResultCodeArgument);
        if (resultCode is not null && string.IsNullOrWhiteSpace(resultCode))
        {
            parseError = $"{ResultCodeArgument} requires a non-empty result code when provided.";
            resultCode = null;
            return false;
        }

        return true;
    }

    private static string[] RemoveFacetRouteConfigurationCommandArguments(string[] args)
    {
        var importArgumentIndex = Array.IndexOf(args, ImportFacetConfigArgument);
        var validateArgumentIndex = Array.IndexOf(args, ValidateFacetConfigArgument);

        return RemoveCommandArguments(args, importArgumentIndex, importArgumentIndex + 1, validateArgumentIndex, validateArgumentIndex + 1);
    }

    private static string[] RemoveFacetSqlCommandArguments(string[] args)
    {
        var printArgumentIndex = Array.IndexOf(args, PrintFacetSqlArgument);
        return RemoveCommandArguments(args, printArgumentIndex, printArgumentIndex + 1);
    }

    private static string[] RemoveResultSqlCommandArguments(string[] args)
    {
        var printArgumentIndex = Array.IndexOf(args, PrintResultSqlArgument);
        var viewTypeArgumentIndex = Array.IndexOf(args, ViewTypeArgument);
        var resultCodeArgumentIndex = Array.IndexOf(args, ResultCodeArgument);

        return RemoveCommandArguments(
            args,
            printArgumentIndex,
            printArgumentIndex + 1,
            viewTypeArgumentIndex,
            viewTypeArgumentIndex + 1,
            resultCodeArgumentIndex,
            resultCodeArgumentIndex + 1
        );
    }

    private static string[] RemoveCommandArguments(string[] args, params int[] indexesToRemove)
    {
        var indexes = indexesToRemove.Where(index => index >= 0).ToHashSet();
        return args.Where((_, index) => !indexes.Contains(index)).ToArray();
    }

    private static string GetOptionValue(string[] args, string optionName)
    {
        var optionIndex = Array.IndexOf(args, optionName);
        if (optionIndex < 0 || optionIndex == args.Length - 1)
        {
            return null;
        }

        return args[optionIndex + 1];
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
