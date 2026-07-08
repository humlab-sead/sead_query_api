using System;
using Microsoft.Extensions.Hosting;
using SeadQueryAPI.Services;
using Serilog;

namespace SeadQueryAPI.Cli;

public static class StartupCommandDispatcher
{
    public static int Run(StartupCommand command, Func<string[], IHost> hostFactory)
    {
        ArgumentNullException.ThrowIfNull(command);

        var runner = new CommandHostRunner(hostFactory);

        return command switch
        {
            RunWebHostCommand runWebHostCommand => RunWebHost(runWebHostCommand, hostFactory),
            ImportFacetConfigCommand importFacetConfigCommand => runner.RunScoped<FacetRouteConfigurationImportCommand>(
                importFacetConfigCommand.HostArgs,
                service =>
                {
                    service.Run(importFacetConfigCommand.ConfigurationFilePath);
                    Log.Information(
                        "Imported facet route configuration from {ConfigurationFilePath}",
                        importFacetConfigCommand.ConfigurationFilePath
                    );
                }
            ),
            ValidateFacetConfigCommand validateFacetConfigCommand => runner.RunScoped<FacetRouteConfigurationValidationCommand>(
                validateFacetConfigCommand.HostArgs,
                service =>
                {
                    service.Run(validateFacetConfigCommand.ConfigurationFilePath);
                    Log.Information(
                        "Validated facet route configuration from {ConfigurationFilePath}",
                        validateFacetConfigCommand.ConfigurationFilePath
                    );
                }
            ),
            PrintFacetSqlCommand printFacetSqlCommand => runner.RunScoped<FacetUrlSqlProbeCommand>(
                printFacetSqlCommand.HostArgs,
                service =>
                {
                    service.Run(printFacetSqlCommand.FacetUrl);
                    Log.Information("Printed facet SQL for {FacetUrl}", printFacetSqlCommand.FacetUrl);
                }
            ),
            PrintResultSqlCommand printResultSqlCommand => runner.RunScoped<ResultUrlSqlProbeCommand>(
                printResultSqlCommand.HostArgs,
                service =>
                {
                    service.Run(
                        printResultSqlCommand.FacetUrl,
                        printResultSqlCommand.ViewTypeId,
                        printResultSqlCommand.ResultCode
                    );
                    Log.Information(
                        "Printed result SQL for {FacetUrl} with view type {ViewTypeId} and result code {ResultCode}",
                        printResultSqlCommand.FacetUrl,
                        printResultSqlCommand.ViewTypeId,
                        printResultSqlCommand.ResultCode ?? "(default)"
                    );
                }
            ),
            _ => throw new InvalidOperationException($"Unsupported startup command type '{command.GetType().Name}'."),
        };
    }

    private static int RunWebHost(RunWebHostCommand command, Func<string[], IHost> hostFactory)
    {
        using var host = hostFactory(command.HostArgs);
        host.Run();
        return 0;
    }
}
