using System;
using System.Collections.Generic;
using System.Linq;
using System.CommandLine;
using System.CommandLine.Parsing;

namespace SeadQueryAPI.Cli;

public sealed class StartupCommandParser
{
    private const string ImportFacetConfigArgument = "--import-facet-config";
    private const string ValidateFacetConfigArgument = "--validate-facet-config";
    private const string ValidateFacetConfigOfflineArgument = "--validate-facet-config-offline";
    private const string PrintFacetSqlArgument = "--print-facet-sql";
    private const string PrintResultSqlArgument = "--print-result-sql";
    private const string ViewTypeArgument = "--view-type";
    private const string ResultCodeArgument = "--result-code";

    private readonly RootCommand _rootCommand;
    private readonly Option<string> _importFacetConfigOption;
    private readonly Option<string> _validateFacetConfigOption;
    private readonly Option<bool> _validateFacetConfigOfflineOption;
    private readonly Option<string> _printFacetSqlOption;
    private readonly Option<string> _printResultSqlOption;
    private readonly Option<string> _viewTypeOption;
    private readonly Option<string> _resultCodeOption;

    public StartupCommandParser()
    {
        _importFacetConfigOption = new Option<string>(ImportFacetConfigArgument);
        _validateFacetConfigOption = new Option<string>(ValidateFacetConfigArgument);
        _validateFacetConfigOfflineOption = new Option<bool>(ValidateFacetConfigOfflineArgument);
        _printFacetSqlOption = new Option<string>(PrintFacetSqlArgument);
        _printResultSqlOption = new Option<string>(PrintResultSqlArgument);
        _viewTypeOption = new Option<string>(ViewTypeArgument);
        _resultCodeOption = new Option<string>(ResultCodeArgument);

        _rootCommand = new RootCommand
        {
            TreatUnmatchedTokensAsErrors = false,
        };

        _rootCommand.Options.Add(_importFacetConfigOption);
        _rootCommand.Options.Add(_validateFacetConfigOption);
        _rootCommand.Options.Add(_validateFacetConfigOfflineOption);
        _rootCommand.Options.Add(_printFacetSqlOption);
        _rootCommand.Options.Add(_printResultSqlOption);
        _rootCommand.Options.Add(_viewTypeOption);
        _rootCommand.Options.Add(_resultCodeOption);
    }

    public StartupCommand Parse(string[] args)
    {
        ArgumentNullException.ThrowIfNull(args);

        var parseResult = _rootCommand.Parse(args);

        var hasFacetRouteCommand = TryGetFacetRouteCommand(args, parseResult, out var routeCommand, out var routeParseError);
        var hasFacetSqlCommand = TryGetFacetSqlCommand(args, parseResult, out var facetSqlCommand, out var facetSqlParseError);
        var hasResultSqlCommand = TryGetResultSqlCommand(args, parseResult, out var resultSqlCommand, out var resultSqlParseError);

        var parseError = routeParseError ?? facetSqlParseError ?? resultSqlParseError;

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
            return routeCommand;
        }

        if (hasFacetSqlCommand)
        {
            return facetSqlCommand;
        }

        if (hasResultSqlCommand)
        {
            return resultSqlCommand;
        }

        if (!string.IsNullOrWhiteSpace(parseError))
        {
            throw new ArgumentException(parseError);
        }

        return new RunWebHostCommand(args);
    }

    internal bool TryGetFacetRouteCommand(string[] args, ParseResult parseResult, out StartupCommand command, out string parseError)
    {
        command = null;
        parseError = null;

        if (args.Length == 0)
        {
            return false;
        }

        var importOptionResult = parseResult.GetResult(_importFacetConfigOption);
        var validateOptionResult = parseResult.GetResult(_validateFacetConfigOption);
        var importArgumentIndex = Array.IndexOf(args, ImportFacetConfigArgument);
        var validateArgumentIndex = Array.IndexOf(args, ValidateFacetConfigArgument);
        var validateOfflineArgumentIndex = Array.IndexOf(args, ValidateFacetConfigOfflineArgument);
        var hasValidateOfflineOption = validateOfflineArgumentIndex >= 0;
        var hasImportOption = importOptionResult is not null;
        var hasValidateOption = validateOptionResult is not null;

        if (hasValidateOfflineOption && !hasValidateOption)
        {
            parseError = $"{ValidateFacetConfigOfflineArgument} can only be used together with {ValidateFacetConfigArgument}.";
            return false;
        }

        if (hasImportOption && hasValidateOption)
        {
            parseError = $"{ImportFacetConfigArgument} and {ValidateFacetConfigArgument} cannot be used together.";
            return false;
        }

        if (hasImportOption && hasValidateOfflineOption)
        {
            parseError = $"{ValidateFacetConfigOfflineArgument} cannot be used together with {ImportFacetConfigArgument}.";
            return false;
        }

        var commandArgumentIndex = hasImportOption ? importArgumentIndex : validateArgumentIndex;
        if (commandArgumentIndex < 0)
        {
            return false;
        }

        var commandArgument = hasImportOption ? ImportFacetConfigArgument : ValidateFacetConfigArgument;

        var commandOptionResult = hasImportOption ? importOptionResult : validateOptionResult;
        if (commandOptionResult.Tokens.Count == 0)
        {
            parseError = $"{commandArgument} requires a configuration file path.";
            return false;
        }

        var configurationFilePath = commandOptionResult.GetValueOrDefault<string>();

        if (string.IsNullOrWhiteSpace(configurationFilePath))
        {
            parseError = $"{commandArgument} requires a non-empty configuration file path.";
            return false;
        }

        var hostArgs = RemoveFacetRouteConfigurationCommandArguments(args);
        command = hasImportOption
            ? new ImportFacetConfigCommand(configurationFilePath, hostArgs)
            : new ValidateFacetConfigCommand(configurationFilePath, hasValidateOfflineOption, hostArgs);

        return true;
    }

    internal bool TryGetFacetSqlCommand(string[] args, ParseResult parseResult, out StartupCommand command, out string parseError)
    {
        command = null;
        parseError = null;

        if (args.Length == 0)
        {
            return false;
        }

        var commandOptionResult = parseResult.GetResult(_printFacetSqlOption);
        if (commandOptionResult is null)
        {
            return false;
        }

        if (commandOptionResult.Tokens.Count == 0)
        {
            parseError = $"{PrintFacetSqlArgument} requires a facet URL.";
            return false;
        }

        var facetUrl = commandOptionResult.GetValueOrDefault<string>();
        if (string.IsNullOrWhiteSpace(facetUrl))
        {
            parseError = $"{PrintFacetSqlArgument} requires a non-empty facet URL.";
            return false;
        }

        command = new PrintFacetSqlCommand(facetUrl, RemoveFacetSqlCommandArguments(args));
        return true;
    }

    internal bool TryGetResultSqlCommand(string[] args, ParseResult parseResult, out StartupCommand command, out string parseError)
    {
        command = null;
        parseError = null;

        if (args.Length == 0)
        {
            return false;
        }

        var commandOptionResult = parseResult.GetResult(_printResultSqlOption);
        if (commandOptionResult is null)
        {
            return false;
        }

        if (commandOptionResult.Tokens.Count == 0)
        {
            parseError = $"{PrintResultSqlArgument} requires a facet URL.";
            return false;
        }

        var facetUrl = commandOptionResult.GetValueOrDefault<string>();
        if (string.IsNullOrWhiteSpace(facetUrl))
        {
            parseError = $"{PrintResultSqlArgument} requires a non-empty facet URL.";
            return false;
        }

        var viewTypeOptionResult = parseResult.GetResult(_viewTypeOption);
        if (viewTypeOptionResult is not null && viewTypeOptionResult.Tokens.Count == 0)
        {
            parseError = $"{ViewTypeArgument} requires a non-empty view type when provided.";
            return false;
        }

        var viewTypeId = viewTypeOptionResult?.GetValueOrDefault<string>() ?? "tabular";
        if (string.IsNullOrWhiteSpace(viewTypeId))
        {
            parseError = $"{ViewTypeArgument} requires a non-empty view type when provided.";
            return false;
        }

        var resultCodeOptionResult = parseResult.GetResult(_resultCodeOption);
        if (resultCodeOptionResult is not null && resultCodeOptionResult.Tokens.Count == 0)
        {
            parseError = $"{ResultCodeArgument} requires a non-empty result code when provided.";
            return false;
        }

        var resultCode = resultCodeOptionResult?.GetValueOrDefault<string>();
        if (resultCode is not null && string.IsNullOrWhiteSpace(resultCode))
        {
            parseError = $"{ResultCodeArgument} requires a non-empty result code when provided.";
            return false;
        }

        command = new PrintResultSqlCommand(facetUrl, viewTypeId, resultCode, RemoveResultSqlCommandArguments(args));
        return true;
    }

    private static string[] RemoveFacetRouteConfigurationCommandArguments(string[] args)
    {
        var importArgumentIndex = Array.IndexOf(args, ImportFacetConfigArgument);
        var validateArgumentIndex = Array.IndexOf(args, ValidateFacetConfigArgument);
        var validateOfflineArgumentIndex = Array.IndexOf(args, ValidateFacetConfigOfflineArgument);

        return RemoveCommandArguments(
            args,
            importArgumentIndex,
            importArgumentIndex + 1,
            validateArgumentIndex,
            validateArgumentIndex + 1,
            validateOfflineArgumentIndex
        );
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
        var indexes = new HashSet<int>(indexesToRemove.Where(index => index >= 0));
        return args.Where((_, index) => !indexes.Contains(index)).ToArray();
    }
}
