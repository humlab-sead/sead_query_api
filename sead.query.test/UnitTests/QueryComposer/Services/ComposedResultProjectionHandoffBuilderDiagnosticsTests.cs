using System;
using System.Collections.Generic;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using SeadQueryComposer.QueryComposer.Inputs;
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

        [Fact]
        public void Build_WithTemplateSnapshot_UsesSnapshotInsteadOfLegacyTemplateLookups()
        {
            var resultFacet = CreateResultFacet();
            var aggregateFacet = CreateAggregateFacet();
            var facetsConfig = new FacetsConfig2
            {
                TargetCode = resultFacet.FacetCode,
                TargetFacet = resultFacet,
                FacetConfigs = [],
            };
            var resultConfig = new ResultConfig
            {
                FacetCode = resultFacet.FacetCode,
                Facet = resultFacet,
                ViewTypeId = "tabular",
            };

            var facetRepository = new Mock<IFacetRepository>();
            facetRepository.Setup(x => x.Get(resultFacet.AggregateFacetId)).Returns(aggregateFacet);

            var registry = new Mock<IRepositoryRegistry>();
            registry.SetupGet(x => x.Facets).Returns(facetRepository.Object);

            var querySetupFactory = new Mock<ISupportedRequestQuerySetupFactory>();
            querySetupFactory
                .Setup(x => x.CreateForResultProjection(facetsConfig, resultFacet, It.IsAny<IEnumerable<ResultSpecificationField>>()))
                .Returns(new QuerySetup { Facet = resultFacet, Joins = [] });

            var templateResolver = new Mock<IFacetTemplateRuntimeResolver>();
            templateResolver
                .Setup(x => x.GetTemplateSnapshot(resultFacet))
                .Returns(
                    new FacetTemplateRuntimeSnapshot(
                        "anchor_identity",
                        "select base_sql",
                        "discrete",
                        "analysis_entity",
                        new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
                    )
                );

            var builder = new ComposedResultProjectionHandoffBuilder(
                registry.Object,
                querySetupFactory.Object,
                Mock.Of<IPickFilterCompilerLocator>(),
                Mock.Of<IPathFinder>(),
                Mock.Of<IRouteSqlCompiler>(),
                templateResolver.Object,
                Mock.Of<IDiscreteFacetPredicateResolver>(),
                Mock.Of<IComposedFilterQueryComposer>(),
                Mock.Of<ILogger<ComposedResultProjectionHandoffBuilder>>()
            );

            var result = builder.Build(facetsConfig, resultConfig);

            result.QuerySetup.LeadingSql.Should().Contain("from tbl_analysis_entities");
            templateResolver.Verify(x => x.GetTemplateSnapshot(resultFacet), Times.Once);
            templateResolver.Verify(x => x.GetTemplateKey(It.IsAny<Facet>()), Times.Never);
            templateResolver.Verify(x => x.GetAnchorSql(It.IsAny<Facet>(), It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public void Build_WithUnsupportedTemplateKey_ThrowsInvalidOperationException()
        {
            var resultFacet = CreateResultFacet();
            var aggregateFacet = CreateAggregateFacet();
            var facetsConfig = new FacetsConfig2
            {
                TargetCode = resultFacet.FacetCode,
                TargetFacet = resultFacet,
                FacetConfigs = [],
            };
            var resultConfig = new ResultConfig
            {
                FacetCode = resultFacet.FacetCode,
                Facet = resultFacet,
                ViewTypeId = "tabular",
            };

            var facetRepository = new Mock<IFacetRepository>();
            facetRepository.Setup(x => x.Get(resultFacet.AggregateFacetId)).Returns(aggregateFacet);

            var registry = new Mock<IRepositoryRegistry>();
            registry.SetupGet(x => x.Facets).Returns(facetRepository.Object);

            var querySetupFactory = new Mock<ISupportedRequestQuerySetupFactory>();
            querySetupFactory
                .Setup(x => x.CreateForResultProjection(facetsConfig, resultFacet, It.IsAny<IEnumerable<ResultSpecificationField>>()))
                .Returns(new QuerySetup { Facet = resultFacet, Joins = [] });

            var templateResolver = new Mock<IFacetTemplateRuntimeResolver>();
            templateResolver
                .Setup(x => x.GetTemplateSnapshot(resultFacet))
                .Returns(
                    new FacetTemplateRuntimeSnapshot(
                        "unsupported_template_key",
                        "select base_sql",
                        "discrete",
                        "analysis_entity",
                        new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
                    )
                );

            var builder = new ComposedResultProjectionHandoffBuilder(
                registry.Object,
                querySetupFactory.Object,
                Mock.Of<IPickFilterCompilerLocator>(),
                Mock.Of<IPathFinder>(),
                Mock.Of<IRouteSqlCompiler>(),
                templateResolver.Object,
                Mock.Of<IDiscreteFacetPredicateResolver>(),
                Mock.Of<IComposedFilterQueryComposer>(),
                Mock.Of<ILogger<ComposedResultProjectionHandoffBuilder>>()
            );

            var action = () => builder.Build(facetsConfig, resultConfig);

            action
                .Should()
                .Throw<InvalidOperationException>()
                .WithMessage("*Unsupported template key 'unsupported_template_key' for result projection handoff.*");
        }

        [Fact]
        public void Build_WithoutTemplateMetadata_UsesRelationalFallbackPath()
        {
            var sourceFacet = CreateTaxonFacet();
            var resultFacet = CreateResultFacet();
            var aggregateFacet = CreateAggregateFacet();
            var picks = FacetConfigPick.CreateByList([1, 2]);
            var facetsConfig = new FacetsConfig2
            {
                TargetCode = resultFacet.FacetCode,
                TargetFacet = resultFacet,
                FacetConfigs = [new FacetConfig2(sourceFacet, 1, string.Empty, picks)],
            };
            var resultConfig = new ResultConfig
            {
                FacetCode = resultFacet.FacetCode,
                Facet = resultFacet,
                ViewTypeId = "tabular",
            };

            var facetRepository = new Mock<IFacetRepository>();
            facetRepository.Setup(x => x.Get(resultFacet.AggregateFacetId)).Returns(aggregateFacet);

            var registry = new Mock<IRepositoryRegistry>();
            registry.SetupGet(x => x.Facets).Returns(facetRepository.Object);

            var querySetupFactory = new Mock<ISupportedRequestQuerySetupFactory>();
            querySetupFactory
                .Setup(x => x.CreateForResultProjection(facetsConfig, resultFacet, It.IsAny<IEnumerable<ResultSpecificationField>>()))
                .Returns(new QuerySetup { Facet = resultFacet, Joins = [] });

            var templateResolver = new Mock<IFacetTemplateRuntimeResolver>();
            templateResolver
                .Setup(x => x.GetTemplateSnapshot(It.IsAny<Facet>()))
                .Returns(FacetTemplateRuntimeSnapshot.Empty);

            var predicateResolver = new Mock<IDiscreteFacetPredicateResolver>();
            predicateResolver
                .Setup(
                    x => x.ResolveSql(
                        It.IsAny<string>(),
                        It.IsAny<string>(),
                        It.IsAny<DiscreteFacetUserInput>(),
                        It.IsAny<AnchorTemplate>(),
                        It.IsAny<string>(),
                        It.IsAny<string>(),
                        It.IsAny<IReadOnlyList<string>>()
                    )
                )
                .Returns("select taxon_id as source_key, analysis_entity_id as anchor_key from facet.taxon_to_analysis_entity where taxon_id in (1, 2)");

            var pathFinder = new Mock<IPathFinder>();
            pathFinder
                .Setup(x => x.Find(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(new List<TableRelation>());

            var composedFilterComposer = new Mock<IComposedFilterQueryComposer>();
            composedFilterComposer
                .Setup(x => x.Compose(It.IsAny<IReadOnlyCollection<PredicateQueryPlan>>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(new ComposedFilterQuery { AnchorTable = "tbl_analysis_entities", AnchorKeyColumn = "anchor_key", PredicateQueries = [], Sql = "select anchor_key from composed_filter" });

            var builder = new ComposedResultProjectionHandoffBuilder(
                registry.Object,
                querySetupFactory.Object,
                Mock.Of<IPickFilterCompilerLocator>(),
                pathFinder.Object,
                Mock.Of<IRouteSqlCompiler>(),
                templateResolver.Object,
                predicateResolver.Object,
                composedFilterComposer.Object,
                Mock.Of<ILogger<ComposedResultProjectionHandoffBuilder>>()
            );

            var result = builder.Build(facetsConfig, resultConfig);

            result.Should().NotBeNull();
            result.QuerySetup.Should().NotBeNull();
            templateResolver.Verify(x => x.GetTemplateSnapshot(It.IsAny<Facet>()), Times.AtLeastOnce);
            predicateResolver.Verify(
                x => x.ResolveSql(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<DiscreteFacetUserInput>(),
                    It.Is<AnchorTemplate>(t => string.IsNullOrEmpty(t.ExplicitSql)),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<IReadOnlyList<string>>()
                ),
                Times.Once,
                "Predicate resolver should be called with empty ExplicitSql when no template metadata exists"
            );
        }

        [Fact]
        public void Build_WithDiscreteBaseTemplateAndProjectedAnchor_UsesTemplateDrivenPredicateSql()
        {
            var sourceFacet = CreateFamilyTemplateFacet();
            var resultFacet = CreateResultFacet();
            var aggregateFacet = CreateAggregateFacet();
            var picks = FacetConfigPick.CreateByList([1, 2]);
            var facetsConfig = new FacetsConfig2
            {
                TargetCode = resultFacet.FacetCode,
                TargetFacet = resultFacet,
                FacetConfigs = [new FacetConfig2(sourceFacet, 1, string.Empty, picks)],
            };
            var resultConfig = new ResultConfig
            {
                FacetCode = resultFacet.FacetCode,
                Facet = resultFacet,
                ViewTypeId = "tabular",
            };

            var facetRepository = new Mock<IFacetRepository>();
            facetRepository.Setup(x => x.Get(resultFacet.AggregateFacetId)).Returns(aggregateFacet);

            var registry = new Mock<IRepositoryRegistry>();
            registry.SetupGet(x => x.Facets).Returns(facetRepository.Object);

            var querySetupFactory = new Mock<ISupportedRequestQuerySetupFactory>();
            querySetupFactory
                .Setup(x => x.CreateForResultProjection(facetsConfig, resultFacet, It.IsAny<IEnumerable<ResultSpecificationField>>()))
                .Returns(new QuerySetup { Facet = resultFacet, Joins = [] });

            var templateResolver = new Mock<IFacetTemplateRuntimeResolver>();
            templateResolver
                .Setup(x => x.GetTemplateSnapshot(sourceFacet))
                .Returns(
                    new FacetTemplateRuntimeSnapshot(
                        string.Empty,
                        "select tf.family_id as category_id, ae.analysis_entity_id as anchor_id from tbl_taxa_tree_families tf join tbl_analysis_entities ae on 1 = 1 where {pick_filter_sql}",
                        "discrete",
                        "analysis_entity",
                        new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
                    )
                );
            templateResolver
                .Setup(x => x.GetTemplateSnapshot(resultFacet))
                .Returns(FacetTemplateRuntimeSnapshot.Empty);

            var predicateResolver = new Mock<IDiscreteFacetPredicateResolver>();
            string capturedExplicitSql = null;
            predicateResolver
                .Setup(
                    x => x.ResolveSql(
                        It.IsAny<string>(),
                        It.IsAny<string>(),
                        It.IsAny<DiscreteFacetUserInput>(),
                        It.IsAny<AnchorTemplate>(),
                        It.IsAny<string>(),
                        It.IsAny<string>(),
                        It.IsAny<IReadOnlyList<string>>()
                    )
                )
                .Callback<string, string, DiscreteFacetUserInput, AnchorTemplate, string, string, IReadOnlyList<string>>(
                    (_, _, _, anchorTemplate, _, _, _) => capturedExplicitSql = anchorTemplate.ExplicitSql
                )
                .Returns("select predicate_sql");

            var routeSqlCompiler = new Mock<IRouteSqlCompiler>();
            routeSqlCompiler
                .Setup(x => x.Compile(It.IsAny<IReadOnlyList<string>>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns("select analysis_entity_id as source_id, analysis_entity_id as target_id from tbl_analysis_entities");

            var pathFinder = new Mock<IPathFinder>();
            pathFinder.Setup(x => x.Find(It.IsAny<string>(), It.IsAny<string>())).Returns([]);

            var composedFilterComposer = new Mock<IComposedFilterQueryComposer>();
            composedFilterComposer
                .Setup(x => x.Compose(It.IsAny<IReadOnlyCollection<PredicateQueryPlan>>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(new ComposedFilterQuery { AnchorTable = "tbl_analysis_entities", AnchorKeyColumn = "anchor_key", PredicateQueries = [], Sql = "select anchor_key from composed_filter" });

            var builder = new ComposedResultProjectionHandoffBuilder(
                registry.Object,
                querySetupFactory.Object,
                Mock.Of<IPickFilterCompilerLocator>(),
                pathFinder.Object,
                routeSqlCompiler.Object,
                templateResolver.Object,
                predicateResolver.Object,
                composedFilterComposer.Object,
                Mock.Of<ILogger<ComposedResultProjectionHandoffBuilder>>()
            );

            _ = builder.Build(facetsConfig, resultConfig);

            capturedExplicitSql.Should().NotBeNullOrWhiteSpace();
            capturedExplicitSql.Should().Contain("category_id in (1, 2)");
            capturedExplicitSql.Should().Contain("base.category_id as source_id");
            predicateResolver.VerifyAll();
        }

        [Fact]
        public void Build_WithDiscreteAnchorOverride_RendersPlaceholderSql()
        {
            var sourceFacet = CreateFamilyTemplateFacet();
            var resultFacet = CreateResultFacet();
            var aggregateFacet = CreateAggregateFacet();
            var picks = FacetConfigPick.CreateByList([4, 9]);
            var facetsConfig = new FacetsConfig2
            {
                TargetCode = resultFacet.FacetCode,
                TargetFacet = resultFacet,
                FacetConfigs = [new FacetConfig2(sourceFacet, 1, string.Empty, picks)],
            };
            var resultConfig = new ResultConfig
            {
                FacetCode = resultFacet.FacetCode,
                Facet = resultFacet,
                ViewTypeId = "tabular",
            };

            var facetRepository = new Mock<IFacetRepository>();
            facetRepository.Setup(x => x.Get(resultFacet.AggregateFacetId)).Returns(aggregateFacet);

            var registry = new Mock<IRepositoryRegistry>();
            registry.SetupGet(x => x.Facets).Returns(facetRepository.Object);

            var querySetupFactory = new Mock<ISupportedRequestQuerySetupFactory>();
            querySetupFactory
                .Setup(x => x.CreateForResultProjection(facetsConfig, resultFacet, It.IsAny<IEnumerable<ResultSpecificationField>>()))
                .Returns(new QuerySetup { Facet = resultFacet, Joins = [] });

            var templateResolver = new Mock<IFacetTemplateRuntimeResolver>();
            templateResolver
                .Setup(x => x.GetTemplateSnapshot(sourceFacet))
                .Returns(
                    new FacetTemplateRuntimeSnapshot(
                        string.Empty,
                        "select tf.family_id as category_id, ae.analysis_entity_id as anchor_id from tbl_taxa_tree_families tf join tbl_analysis_entities ae on 1 = 1 where {pick_filter_sql}",
                        "discrete",
                        "analysis_entity",
                        new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
                        {
                            ["tbl_analysis_entities"] =
                                "select tf.family_id as source_id, ae.analysis_entity_id as target_id from tbl_taxa_tree_families tf join tbl_analysis_entities ae on 1 = 1 where {pick_filter_sql}",
                        }
                    )
                );
            templateResolver
                .Setup(x => x.GetTemplateSnapshot(resultFacet))
                .Returns(FacetTemplateRuntimeSnapshot.Empty);

            var predicateResolver = new Mock<IDiscreteFacetPredicateResolver>();
            string capturedExplicitSql = null;
            predicateResolver
                .Setup(
                    x => x.ResolveSql(
                        It.IsAny<string>(),
                        It.IsAny<string>(),
                        It.IsAny<DiscreteFacetUserInput>(),
                        It.IsAny<AnchorTemplate>(),
                        It.IsAny<string>(),
                        It.IsAny<string>(),
                        It.IsAny<IReadOnlyList<string>>()
                    )
                )
                .Callback<string, string, DiscreteFacetUserInput, AnchorTemplate, string, string, IReadOnlyList<string>>(
                    (_, _, _, anchorTemplate, _, _, _) => capturedExplicitSql = anchorTemplate.ExplicitSql
                )
                .Returns("select predicate_sql");

            var pathFinder = new Mock<IPathFinder>();
            pathFinder.Setup(x => x.Find(It.IsAny<string>(), It.IsAny<string>())).Returns([]);

            var composedFilterComposer = new Mock<IComposedFilterQueryComposer>();
            composedFilterComposer
                .Setup(x => x.Compose(It.IsAny<IReadOnlyCollection<PredicateQueryPlan>>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(new ComposedFilterQuery { AnchorTable = "tbl_analysis_entities", AnchorKeyColumn = "anchor_key", PredicateQueries = [], Sql = "select anchor_key from composed_filter" });

            var builder = new ComposedResultProjectionHandoffBuilder(
                registry.Object,
                querySetupFactory.Object,
                Mock.Of<IPickFilterCompilerLocator>(),
                pathFinder.Object,
                Mock.Of<IRouteSqlCompiler>(),
                templateResolver.Object,
                predicateResolver.Object,
                composedFilterComposer.Object,
                Mock.Of<ILogger<ComposedResultProjectionHandoffBuilder>>()
            );

            _ = builder.Build(facetsConfig, resultConfig);

            capturedExplicitSql.Should().NotBeNullOrWhiteSpace();
            capturedExplicitSql.Should().Contain("category_id in (4, 9)");
            capturedExplicitSql.Should().NotContain("{pick_filter_sql}");
            predicateResolver.VerifyAll();
        }

        [Fact]
        public void Build_WithDiscreteAnchorOverrideAndUnknownPlaceholder_ThrowsInvalidOperationException()
        {
            var sourceFacet = CreateFamilyTemplateFacet();
            var resultFacet = CreateResultFacet();
            var aggregateFacet = CreateAggregateFacet();
            var picks = FacetConfigPick.CreateByList([4, 9]);
            var facetsConfig = new FacetsConfig2
            {
                TargetCode = resultFacet.FacetCode,
                TargetFacet = resultFacet,
                FacetConfigs = [new FacetConfig2(sourceFacet, 1, string.Empty, picks)],
            };
            var resultConfig = new ResultConfig
            {
                FacetCode = resultFacet.FacetCode,
                Facet = resultFacet,
                ViewTypeId = "tabular",
            };

            var facetRepository = new Mock<IFacetRepository>();
            facetRepository.Setup(x => x.Get(resultFacet.AggregateFacetId)).Returns(aggregateFacet);

            var registry = new Mock<IRepositoryRegistry>();
            registry.SetupGet(x => x.Facets).Returns(facetRepository.Object);

            var querySetupFactory = new Mock<ISupportedRequestQuerySetupFactory>();
            querySetupFactory
                .Setup(x => x.CreateForResultProjection(facetsConfig, resultFacet, It.IsAny<IEnumerable<ResultSpecificationField>>()))
                .Returns(new QuerySetup { Facet = resultFacet, Joins = [] });

            var templateResolver = new Mock<IFacetTemplateRuntimeResolver>();
            templateResolver
                .Setup(x => x.GetTemplateSnapshot(sourceFacet))
                .Returns(
                    new FacetTemplateRuntimeSnapshot(
                        string.Empty,
                        "select tf.family_id as category_id, ae.analysis_entity_id as anchor_id from tbl_taxa_tree_families tf join tbl_analysis_entities ae on 1 = 1 where {pick_filter_sql}",
                        "discrete",
                        "analysis_entity",
                        new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
                        {
                            ["tbl_analysis_entities"] =
                                "select tf.family_id as source_id, ae.analysis_entity_id as target_id from tbl_taxa_tree_families tf join tbl_analysis_entities ae on 1 = 1 where {pick_filter_sql_typo}",
                        }
                    )
                );
            templateResolver
                .Setup(x => x.GetTemplateSnapshot(resultFacet))
                .Returns(FacetTemplateRuntimeSnapshot.Empty);

            var builder = new ComposedResultProjectionHandoffBuilder(
                registry.Object,
                querySetupFactory.Object,
                Mock.Of<IPickFilterCompilerLocator>(),
                Mock.Of<IPathFinder>(),
                Mock.Of<IRouteSqlCompiler>(),
                templateResolver.Object,
                Mock.Of<IDiscreteFacetPredicateResolver>(),
                Mock.Of<IComposedFilterQueryComposer>(),
                Mock.Of<ILogger<ComposedResultProjectionHandoffBuilder>>()
            );

            var action = () => builder.Build(facetsConfig, resultConfig);

            action
                .Should()
                .Throw<InvalidOperationException>()
                .WithMessage("*Template SQL contains unresolved placeholder tokens*");
        }

        [Fact]
        public void Build_WithDiscreteTemplateAndUnnormalizableClause_ComposesFromTemplatePath()
        {
            var sourceFacet = CreateFamilyTemplateFacet();
            sourceFacet.Clauses = [new FacetClause { Clause = "facet.view_sample_group_references.biblio_id is not null", EnforceConstraint = true }];

            var resultFacet = CreateResultFacet();
            var aggregateFacet = CreateAggregateFacet();
            var picks = FacetConfigPick.CreateByList([7, 8]);
            var facetsConfig = new FacetsConfig2
            {
                TargetCode = resultFacet.FacetCode,
                TargetFacet = resultFacet,
                FacetConfigs = [new FacetConfig2(sourceFacet, 1, string.Empty, picks)],
            };
            var resultConfig = new ResultConfig
            {
                FacetCode = resultFacet.FacetCode,
                Facet = resultFacet,
                ViewTypeId = "tabular",
            };

            var facetRepository = new Mock<IFacetRepository>();
            facetRepository.Setup(x => x.Get(resultFacet.AggregateFacetId)).Returns(aggregateFacet);

            var registry = new Mock<IRepositoryRegistry>();
            registry.SetupGet(x => x.Facets).Returns(facetRepository.Object);

            var querySetupFactory = new Mock<ISupportedRequestQuerySetupFactory>();
            querySetupFactory
                .Setup(x => x.CreateForResultProjection(facetsConfig, resultFacet, It.IsAny<IEnumerable<ResultSpecificationField>>()))
                .Returns(new QuerySetup { Facet = resultFacet, Joins = [] });

            var templateResolver = new Mock<IFacetTemplateRuntimeResolver>();
            templateResolver
                .Setup(x => x.GetTemplateSnapshot(sourceFacet))
                .Returns(
                    new FacetTemplateRuntimeSnapshot(
                        string.Empty,
                        "select tf.family_id as category_id, ae.analysis_entity_id as anchor_id from tbl_taxa_tree_families tf join tbl_analysis_entities ae on 1 = 1 where {pick_filter_sql}",
                        "discrete",
                        "analysis_entity",
                        new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
                    )
                );
            templateResolver
                .Setup(x => x.GetTemplateSnapshot(resultFacet))
                .Returns(FacetTemplateRuntimeSnapshot.Empty);

            var predicateResolver = new Mock<IDiscreteFacetPredicateResolver>();
            predicateResolver
                .Setup(
                    x => x.ResolveSql(
                        It.IsAny<string>(),
                        It.IsAny<string>(),
                        It.IsAny<DiscreteFacetUserInput>(),
                        It.IsAny<AnchorTemplate>(),
                        It.IsAny<string>(),
                        It.IsAny<string>(),
                        It.IsAny<IReadOnlyList<string>>()
                    )
                )
                .Returns("select predicate_sql");

            var routeSqlCompiler = new Mock<IRouteSqlCompiler>();
            routeSqlCompiler
                .Setup(x => x.Compile(It.IsAny<IReadOnlyList<string>>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns("select analysis_entity_id as source_id, analysis_entity_id as target_id from tbl_analysis_entities");

            var pathFinder = new Mock<IPathFinder>();
            pathFinder.Setup(x => x.Find(It.IsAny<string>(), It.IsAny<string>())).Returns([]);

            var composedFilterComposer = new Mock<IComposedFilterQueryComposer>();
            composedFilterComposer
                .Setup(x => x.Compose(It.IsAny<IReadOnlyCollection<PredicateQueryPlan>>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(new ComposedFilterQuery { AnchorTable = "tbl_analysis_entities", AnchorKeyColumn = "anchor_key", PredicateQueries = [], Sql = "select anchor_key from composed_filter" });

            var builder = new ComposedResultProjectionHandoffBuilder(
                registry.Object,
                querySetupFactory.Object,
                Mock.Of<IPickFilterCompilerLocator>(),
                pathFinder.Object,
                routeSqlCompiler.Object,
                templateResolver.Object,
                predicateResolver.Object,
                composedFilterComposer.Object,
                Mock.Of<ILogger<ComposedResultProjectionHandoffBuilder>>()
            );

            var act = () => builder.Build(facetsConfig, resultConfig);

            act.Should().NotThrow();
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

        private static Facet CreateResultFacet()
        {
            var analysisEntitiesTable = new Table
            {
                TableId = 4,
                TableOrUdfName = "tbl_analysis_entities",
                PrimaryKeyName = "analysis_entity_id",
            };

            return new Facet
            {
                FacetId = 20,
                FacetCode = "result_facet",
                FacetTypeId = EFacetType.Discrete,
                AggregateFacetId = 10,
                CategoryIdExpr = "tbl_analysis_entities.analysis_entity_id",
                Tables = [new FacetTable { SequenceId = 1, Table = analysisEntitiesTable }],
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

        private static Facet CreateTaxonFacet()
        {
            var taxonTable = new Table
            {
                TableId = 99,
                TableOrUdfName = "tbl_taxa_tree_master",
                PrimaryKeyName = "taxon_id",
            };

            return new Facet
            {
                FacetId = 42,
                FacetCode = "taxon",
                FacetTypeId = EFacetType.Discrete,
                CategoryIdExpr = "tbl_taxa_tree_master.taxon_id",
                Tables = [new FacetTable { SequenceId = 1, Table = taxonTable }],
            };
        }

        private static Facet CreateFamilyTemplateFacet()
        {
            var familyTable = new Table
            {
                TableId = 210,
                TableOrUdfName = "tbl_taxa_tree_families",
                PrimaryKeyName = "family_id",
            };
            var analysisEntityTable = new Table
            {
                TableId = 211,
                TableOrUdfName = "tbl_analysis_entities",
                PrimaryKeyName = "analysis_entity_id",
            };

            return new Facet
            {
                FacetId = 43,
                FacetCode = "family",
                FacetTypeId = EFacetType.Discrete,
                CategoryIdExpr = "tbl_taxa_tree_families.family_id",
                Tables = [new FacetTable { SequenceId = 1, Table = familyTable }],
                FacetAnchors =
                [
                    new FacetAnchor
                    {
                        Anchor = new Anchor { Name = "analysis_entity", Table = analysisEntityTable },
                        Route = new Route { Specification = "tbl_analysis_entities -> tbl_analysis_entities" },
                    },
                ],
            };
        }

        private sealed class TestLogger<T> : ILogger<T>
        {
            public List<LogEntry> Entries { get; } = [];

            public IDisposable BeginScope<TState>(TState state) where TState : notnull
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
