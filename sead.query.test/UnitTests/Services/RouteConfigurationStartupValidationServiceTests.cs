using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using SeadQueryAPI.Services;
using SeadQueryComposer.RouteCompiler;
using SeadQueryCore;
using Xunit;

namespace SQT.Services
{
    public class RouteConfigurationStartupValidationServiceTests
    {
        [Fact]
        public async Task StartAsync_WithConfiguredRoutes_ValidatesGraphAndRouteKeys()
        {
            var routeResolver = new Mock<IRouteResolver>();
            routeResolver.SetupGet(x => x.Relations).Returns(new Dictionary<(string SourceTable, string TargetTable), TableRelation>());

            var routeRepository = new Mock<IRouteRepository>();
            routeRepository
                .Setup(x => x.GetAll())
                .Returns(
                    new[]
                    {
                        new Route
                        {
                            Name = "MAIN_ROUTE",
                            Alias = "MAIN_ALIAS",
                            Specification = "tbl_sites -> tbl_sample_groups",
                        },
                        new Route { Name = "SECONDARY_ROUTE", Specification = "tbl_sample_groups -> tbl_analysis_entities" },
                    }
                );

            var routeParser = new Mock<IArrowRouteParser>();
            routeParser.Setup(x => x.ResolveRoute(It.IsAny<string>())).Returns(new[] { "tbl_sites", "tbl_sample_groups" });

            var service = new RouteConfigurationStartupValidationService(
                routeResolver.Object,
                routeRepository.Object,
                routeParser.Object,
                NullLogger<RouteConfigurationStartupValidationService>.Instance
            );

            await service.StartAsync(CancellationToken.None);

            routeResolver.VerifyGet(x => x.Relations, Times.Once);
            routeParser.Verify(x => x.ResolveRoute("MAIN_ROUTE"), Times.Once);
            routeParser.Verify(x => x.ResolveRoute("MAIN_ALIAS"), Times.Once);
            routeParser.Verify(x => x.ResolveRoute("SECONDARY_ROUTE"), Times.Once);
        }

        [Fact]
        public async Task StartAsync_WithEmptyRouteName_ThrowsInvalidOperationException()
        {
            var routeResolver = new Mock<IRouteResolver>();
            routeResolver.SetupGet(x => x.Relations).Returns(new Dictionary<(string SourceTable, string TargetTable), TableRelation>());

            var routeRepository = new Mock<IRouteRepository>();
            routeRepository
                .Setup(x => x.GetAll())
                .Returns(
                    new[]
                    {
                        new Route { Name = "", Specification = "tbl_sites -> tbl_sample_groups" },
                    }
                );

            var routeParser = new Mock<IArrowRouteParser>();
            var service = new RouteConfigurationStartupValidationService(
                routeResolver.Object,
                routeRepository.Object,
                routeParser.Object,
                NullLogger<RouteConfigurationStartupValidationService>.Instance
            );

            Func<Task> act = () => service.StartAsync(CancellationToken.None);

            await act.Should().ThrowAsync<InvalidOperationException>().WithMessage("*contains a route with an empty name*");
        }

        [Fact]
        public async Task StartAsync_WithBrokenRouteDefinition_WrapsStartupValidationFailure()
        {
            var routeResolver = new Mock<IRouteResolver>();
            routeResolver.SetupGet(x => x.Relations).Returns(new Dictionary<(string SourceTable, string TargetTable), TableRelation>());

            var routeRepository = new Mock<IRouteRepository>();
            routeRepository
                .Setup(x => x.GetAll())
                .Returns(
                    new[]
                    {
                        new Route { Name = "BROKEN_ROUTE", Specification = "{missing}" },
                    }
                );

            var routeParser = new Mock<IArrowRouteParser>();
            routeParser
                .Setup(x => x.ResolveRoute("BROKEN_ROUTE"))
                .Throws(new InvalidOperationException("Route macro '{missing}' is not defined."));

            var service = new RouteConfigurationStartupValidationService(
                routeResolver.Object,
                routeRepository.Object,
                routeParser.Object,
                NullLogger<RouteConfigurationStartupValidationService>.Instance
            );

            Func<Task> act = () => service.StartAsync(CancellationToken.None);

            var assertion = await act.Should()
                .ThrowAsync<InvalidOperationException>()
                .WithMessage("*route name 'BROKEN_ROUTE'*specification '{missing}'*");

            assertion.Which.InnerException.Should().BeOfType<InvalidOperationException>();
            assertion.Which.InnerException!.Message.Should().Contain("Route macro '{missing}' is not defined.");
        }

        [Fact]
        public async Task StartAsync_WithBrokenRouteAlias_WrapsStartupValidationFailureWithAliasContext()
        {
            var routeResolver = new Mock<IRouteResolver>();
            routeResolver.SetupGet(x => x.Relations).Returns(new Dictionary<(string SourceTable, string TargetTable), TableRelation>());

            var routeRepository = new Mock<IRouteRepository>();
            routeRepository
                .Setup(x => x.GetAll())
                .Returns(
                    new[]
                    {
                        new Route
                        {
                            Name = "VALID_ROUTE",
                            Alias = "BROKEN_ALIAS",
                            Specification = "{missing}",
                        },
                    }
                );

            var routeParser = new Mock<IArrowRouteParser>();
            routeParser.Setup(x => x.ResolveRoute("VALID_ROUTE")).Returns(new[] { "tbl_sites", "tbl_sample_groups" });
            routeParser
                .Setup(x => x.ResolveRoute("BROKEN_ALIAS"))
                .Throws(new InvalidOperationException("Route macro '{missing}' is not defined."));

            var service = new RouteConfigurationStartupValidationService(
                routeResolver.Object,
                routeRepository.Object,
                routeParser.Object,
                NullLogger<RouteConfigurationStartupValidationService>.Instance
            );

            Func<Task> act = () => service.StartAsync(CancellationToken.None);

            var assertion = await act.Should()
                .ThrowAsync<InvalidOperationException>()
                .WithMessage("*route alias 'BROKEN_ALIAS'*specification '{missing}'*");

            assertion.Which.InnerException.Should().BeOfType<InvalidOperationException>();
            assertion.Which.InnerException!.Message.Should().Contain("Route macro '{missing}' is not defined.");
        }
    }
}
