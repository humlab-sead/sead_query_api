using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SeadQueryComposer.RouteCompiler;
using SeadQueryCore;

namespace SeadQueryAPI.Services
{
    public sealed class RouteConfigurationStartupValidationService : IHostedService
    {
        private readonly IRouteResolver _routeResolver;
        private readonly IRouteRepository _routeRepository;
        private readonly IArrowRouteParser _routeParser;
        private readonly ILogger<RouteConfigurationStartupValidationService> _logger;

        public RouteConfigurationStartupValidationService(
            IRouteResolver routeResolver,
            IRouteRepository routeRepository,
            IArrowRouteParser routeParser,
            ILogger<RouteConfigurationStartupValidationService> logger
        )
        {
            _routeResolver = routeResolver;
            _routeRepository = routeRepository;
            _routeParser = routeParser;
            _logger = logger;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            _ = _routeResolver.Relations.Count;

            foreach (var route in _routeRepository.GetAll().ToList())
            {
                ValidateRouteKey(route, route.Name, "name");

                if (!string.IsNullOrWhiteSpace(route.Alias))
                {
                    ValidateRouteKey(route, route.Alias, "alias");
                }
            }

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        private void ValidateRouteKey(Route route, string key, string keyType)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new InvalidOperationException(
                    $"Route configuration contains a route with an empty {keyType}. Specification: '{route?.Specification ?? string.Empty}'."
                );
            }

            try
            {
                _routeParser.ResolveRoute(key);
            }
            catch (Exception ex) when (ex is InvalidOperationException || ex is ArgumentException || ex is KeyNotFoundException)
            {
                _logger.LogError(
                    ex,
                    "Route startup validation failed for route {KeyType} '{Key}' with specification '{Specification}'.",
                    keyType,
                    key,
                    route?.Specification ?? string.Empty
                );

                throw new InvalidOperationException(
                    $"Route configuration validation failed for route {keyType} '{key}' with specification '{route?.Specification ?? string.Empty}'.",
                    ex
                );
            }
        }
    }
}
