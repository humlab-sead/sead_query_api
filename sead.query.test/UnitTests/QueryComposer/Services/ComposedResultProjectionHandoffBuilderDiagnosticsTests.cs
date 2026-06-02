using System;
using System.Collections.Generic;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using SeadQueryComposer.QueryComposer.Services;
using SeadQueryComposer.RouteCompiler;
using SeadQueryCore;
using SeadQueryCore.Model;
using SeadQueryCore.QueryBuilder;
using SeadQueryCore.QueryComposer;
using Xunit;

namespace SQT.UnitTests.QueryComposer.Services
{
    public class ComposedResultProjectionHandoffBuilderDiagnosticsTests
    {
        [Fact]
        public void Build_WithUnsupportedResultFacet_LogsFailureReasonAndThrows()
        {
            var aggregateFacet = CreateAggregateFacet();
            var resultFacet = CreateUnsupportedSpeciesResultFacet();
            var facetsConfig = new FacetsConfig2
            {
                TargetCode = resultFacet.FacetCode,
                TargetFacet = resultFacet,
                FacetConfigs = [new FacetConfig2(resultFacet, 1, string.Empty, [])],
            };
            var resultConfig = new ResultConfig
            {
                FacetCode = "result_facet",
                Facet = resultFacet,
                ViewTypeId = "tabular",
            };

            var facetRepository = new Mock<IFacetRepository>();
            facetRepository.Setup(x => x.Get(10)).Returns(aggregateFacet);

            var registry = new Mock<IRepositoryRegistry>();
            registry.SetupGet(x => x.Facets).Returns(facetRepository.Object);

            var querySetupFactory = new Mock<ISupportedRequestQuerySetupFactory>();

            var logger = new TestLogger<ComposedResultProjectionHandoffBuilder>();
            var builder = new ComposedResultProjectionHandoffBuilder(
                registry.Object,
                querySetupFactory.Object,
                Mock.Of<IPickFilterCompilerLocator>(),
                Mock.Of<IPathFinder>(),
                Mock.Of<IRouteSqlCompiler>(),
                Mock.Of<IFacetTemplateRuntimeResolver>(),
                Mock.Of<IDiscreteFacetPredicateResolver>(),
                Mock.Of<IComposedFilterQueryComposer>(),
                logger
            );

            var action = () => builder.Build(facetsConfig, resultConfig);

            action
                .Should()
                .Throw<InvalidOperationException>()
                .WithMessage("*routable target join column*no longer fall back to the legacy runtime*");
            logger.Entries.Should().ContainSingle();
            logger.Entries[0].Level.Should().Be(LogLevel.Information);
            logger.Entries[0].Message.Should().Contain("Rejecting unsupported composed result projection handoff");
            logger.Entries[0].Message.Should().Contain("result facet 'species'");
            logger.Entries[0].Message.Should().Contain("routable target join column");
        }

        private static Facet CreateUnsupportedSpeciesResultFacet()
        {
            var speciesTable = new Table
            {
                TableId = 8,
                TableOrUdfName = "facet.abundance_taxon_shortcut",
                PrimaryKeyName = "xxx",
            };

            return new Facet
            {
                FacetCode = "species",
                FacetTypeId = EFacetType.Discrete,
                AggregateFacetId = 10,
                CategoryIdExpr = "coalesce(facet.abundance_taxon_shortcut.taxon_id, 0)",
                Tables = [new FacetTable { SequenceId = 1, Table = speciesTable }],
            };
        }

        private static Facet CreateAggregateFacet()
        {
            var analysisEntitiesTable = new Table
            {
                TableId = 4,
                TableOrUdfName = "tbl_analysis_entities",
                PrimaryKeyName = "analysis_entity_id",
            };

            return new Facet
            {
                FacetId = 10,
                FacetCode = "analysis_entities",
                FacetTypeId = EFacetType.Discrete,
                CategoryIdExpr = "tbl_analysis_entities.analysis_entity_id",
                Tables = [new FacetTable { SequenceId = 1, Table = analysisEntitiesTable }],
            };
        }

        private sealed class TestLogger<T> : ILogger<T>
        {
            public List<LogEntry> Entries { get; } = [];

            public IDisposable BeginScope<TState>(TState state)
            {
                return NullScope.Instance;
            }

            public bool IsEnabled(LogLevel logLevel)
            {
                return true;
            }

            public void Log<TState>(
                LogLevel logLevel,
                EventId eventId,
                TState state,
                Exception exception,
                Func<TState, Exception, string> formatter
            )
            {
                Entries.Add(new LogEntry(logLevel, formatter(state, exception)));
            }
        }

        private sealed record LogEntry(LogLevel Level, string Message);

        private sealed class NullScope : IDisposable
        {
            public static NullScope Instance { get; } = new();

            public void Dispose() { }
        }
    }
}
