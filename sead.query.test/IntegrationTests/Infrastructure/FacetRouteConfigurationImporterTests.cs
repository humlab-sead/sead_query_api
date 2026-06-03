using System;
using System.Data;
using System.IO;
using System.Linq;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using SeadQueryCore;
using SeadQueryInfra;
using SQT.Scaffolding;
using Xunit;

namespace SQT.IntegrationTests.Infrastructure;

[Collection("UsePostgresFixture")]
public class FacetRouteConfigurationImporterTests : MockerWithFacetContext
{
    [Fact]
    public void ImportFromFile_WithDraftSlices_UpsertsCurrentFacetSchemaRows()
    {
        var dbContext = (FacetContext)FacetContext;
        var importer = new FacetRouteConfigurationImporter(dbContext);
        var configurationFilePath = Path.GetFullPath(
            Path.Combine(ScaffoldUtility.GetProjectRoot(), "..", "sead.query.composer", "Templates", "route_v1.yaml")
        );
        var previousSourceCommit = Environment.GetEnvironmentVariable("SEAD_QUERY_FACET_CONFIG_SOURCE_COMMIT");
        var previousImportedBy = Environment.GetEnvironmentVariable("SEAD_QUERY_FACET_CONFIG_IMPORTED_BY");

        Environment.SetEnvironmentVariable("SEAD_QUERY_FACET_CONFIG_SOURCE_COMMIT", "test-commit-01");
        Environment.SetEnvironmentVariable("SEAD_QUERY_FACET_CONFIG_IMPORTED_BY", "facet-import-test");

        try
        {
            importer.ImportFromFile(configurationFilePath);
        }
        finally
        {
            Environment.SetEnvironmentVariable("SEAD_QUERY_FACET_CONFIG_SOURCE_COMMIT", previousSourceCommit);
            Environment.SetEnvironmentVariable("SEAD_QUERY_FACET_CONFIG_IMPORTED_BY", previousImportedBy);
        }

        var featureTypeSourceTableId = dbContext.Tables.Single(table => table.TableOrUdfName == "tbl_feature_types").TableId;
        var datasetMethodsSourceTableId = dbContext.Tables.Single(table => table.TableOrUdfName == "tbl_dataset_methods").TableId;
        var dataTypesSourceTableId = dbContext.Tables.Single(table => table.TableOrUdfName == "tbl_data_types").TableId;
        var datasetProviderSourceTableId = dbContext.Tables.Single(table => table.TableOrUdfName == "tbl_dataset_masters").TableId;
        var recordTypesSourceTableId = dbContext.Tables.Single(table => table.TableOrUdfName == "tbl_record_types").TableId;
        var countrySourceTableId = dbContext.Tables.Single(table => table.TableOrUdfName == "facet.site_location_shortcut").TableId;

        var sampleRoute = dbContext.Routes.Include(route => route.Steps).Single(route => route.Name == "feature_type__sample");
        var datasetRoute = dbContext.Routes.Include(route => route.Steps).Single(route => route.Name == "feature_type__dataset");
        var datasetMethodsSampleRoute = dbContext
            .Routes.Include(route => route.Steps)
            .Single(route => route.Name == "dataset_methods__sample");
        var datasetMethodsDatasetRoute = dbContext
            .Routes.Include(route => route.Steps)
            .Single(route => route.Name == "dataset_methods__dataset");
        var dataTypesSampleRoute = dbContext.Routes.Include(route => route.Steps).Single(route => route.Name == "data_types__sample");
        var dataTypesDatasetRoute = dbContext.Routes.Include(route => route.Steps).Single(route => route.Name == "data_types__dataset");
        var datasetProviderSampleRoute = dbContext
            .Routes.Include(route => route.Steps)
            .Single(route => route.Name == "dataset_provider__sample");
        var datasetProviderDatasetRoute = dbContext
            .Routes.Include(route => route.Steps)
            .Single(route => route.Name == "dataset_provider__dataset");
        var recordTypesSampleRoute = dbContext.Routes.Include(route => route.Steps).Single(route => route.Name == "record_types__sample");
        var recordTypesDatasetRoute = dbContext.Routes.Include(route => route.Steps).Single(route => route.Name == "record_types__dataset");

        sampleRoute.Specification.Should().Be("tbl_feature_types -> tbl_features -> tbl_physical_sample_features -> tbl_physical_samples");
        datasetRoute
            .Specification.Should()
            .Be(
                "tbl_feature_types -> tbl_features -> tbl_physical_sample_features -> tbl_physical_samples -> tbl_analysis_entities -> tbl_datasets"
            );
        datasetMethodsSampleRoute
            .Specification.Should()
            .Be("tbl_dataset_methods -> tbl_methods -> tbl_datasets -> tbl_analysis_entities -> tbl_physical_samples");
        datasetMethodsDatasetRoute.Specification.Should().Be("tbl_dataset_methods -> tbl_methods -> tbl_datasets");
        dataTypesSampleRoute.Specification.Should().Be("tbl_data_types -> tbl_datasets -> tbl_analysis_entities -> tbl_physical_samples");
        dataTypesDatasetRoute.Specification.Should().Be("tbl_data_types -> tbl_datasets");
        datasetProviderSampleRoute
            .Specification.Should()
            .Be("tbl_dataset_masters -> tbl_datasets -> tbl_analysis_entities -> tbl_physical_samples");
        datasetProviderDatasetRoute.Specification.Should().Be("tbl_dataset_masters -> tbl_datasets");
        recordTypesSampleRoute
            .Specification.Should()
            .Be("tbl_record_types -> tbl_methods -> tbl_datasets -> tbl_analysis_entities -> tbl_physical_samples");
        recordTypesDatasetRoute.Specification.Should().Be("tbl_record_types -> tbl_methods -> tbl_datasets");

        sampleRoute.Steps.Should().BeEmpty();
        datasetRoute.Steps.Should().BeEmpty();
        datasetMethodsSampleRoute.Steps.Should().BeEmpty();
        datasetMethodsDatasetRoute.Steps.Should().BeEmpty();
        dataTypesSampleRoute.Steps.Should().BeEmpty();
        dataTypesDatasetRoute.Steps.Should().BeEmpty();
        datasetProviderSampleRoute.Steps.Should().BeEmpty();
        datasetProviderDatasetRoute.Steps.Should().BeEmpty();
        recordTypesSampleRoute.Steps.Should().BeEmpty();
        recordTypesDatasetRoute.Steps.Should().BeEmpty();

        var featureTypeFacet = dbContext
            .Facets.Include(facet => facet.Tables)
            .Include(facet => facet.FacetAnchors)
            .ThenInclude(facetAnchor => facetAnchor.Anchor)
            .Include(facet => facet.FacetAnchors)
            .ThenInclude(facetAnchor => facetAnchor.Route)
            .Single(facet => facet.FacetCode == "feature_type");

        var datasetMethodsFacet = dbContext
            .Facets.Include(facet => facet.Tables)
            .Include(facet => facet.FacetAnchors)
            .ThenInclude(facetAnchor => facetAnchor.Anchor)
            .Include(facet => facet.FacetAnchors)
            .ThenInclude(facetAnchor => facetAnchor.Route)
            .Single(facet => facet.FacetCode == "dataset_methods");

        var dataTypesFacet = dbContext
            .Facets.Include(facet => facet.Tables)
            .Include(facet => facet.FacetAnchors)
            .ThenInclude(facetAnchor => facetAnchor.Anchor)
            .Include(facet => facet.FacetAnchors)
            .ThenInclude(facetAnchor => facetAnchor.Route)
            .Single(facet => facet.FacetCode == "data_types");

        var datasetProviderFacet = dbContext
            .Facets.Include(facet => facet.Tables)
            .Include(facet => facet.FacetAnchors)
            .ThenInclude(facetAnchor => facetAnchor.Anchor)
            .Include(facet => facet.FacetAnchors)
            .ThenInclude(facetAnchor => facetAnchor.Route)
            .Single(facet => facet.FacetCode == "dataset_provider");

        var recordTypesFacet = dbContext
            .Facets.Include(facet => facet.Tables)
            .Include(facet => facet.FacetAnchors)
            .ThenInclude(facetAnchor => facetAnchor.Anchor)
            .Include(facet => facet.FacetAnchors)
            .ThenInclude(facetAnchor => facetAnchor.Route)
            .Single(facet => facet.FacetCode == "record_types");

        var countryFacet = dbContext
            .Facets.Include(facet => facet.Tables)
            .Include(facet => facet.Clauses)
            .Include(facet => facet.FacetAnchors)
            .ThenInclude(facetAnchor => facetAnchor.Anchor)
            .Include(facet => facet.FacetAnchors)
            .ThenInclude(facetAnchor => facetAnchor.Route)
            .Single(facet => facet.FacetCode == "country");

        var activeRevision = dbContext.Set<FacetConfigRevision>().Single(revision => revision.IsActive);

        featureTypeFacet.FacetGroupId.Should().Be(dbContext.FacetGroups.Single(group => group.FacetGroupKey == "others").FacetGroupId);
        featureTypeFacet.FacetTypeId.Should().Be(EFacetType.Discrete);
        featureTypeFacet.CategoryIdExpr.Should().Be("tbl_feature_types.feature_type_id");
        featureTypeFacet.CategoryIdOperator.Should().Be("=");
        featureTypeFacet.AggregateFacetId.Should().Be(dbContext.Facets.Single(facet => facet.FacetCode == "result_facet").FacetId);
        featureTypeFacet.Tables.Should().Contain(table => table.SequenceId == 1 && table.TableId == featureTypeSourceTableId);

        featureTypeFacet
            .FacetAnchors.Should()
            .Contain(facetAnchor => facetAnchor.Anchor.Name == "sample" && facetAnchor.Route.Name == "feature_type__sample");
        featureTypeFacet
            .FacetAnchors.Should()
            .Contain(facetAnchor => facetAnchor.Anchor.Name == "dataset" && facetAnchor.Route.Name == "feature_type__dataset");

        datasetMethodsFacet
            .FacetGroupId.Should()
            .Be(dbContext.FacetGroups.Single(group => group.FacetGroupKey == "space_time").FacetGroupId);
        datasetMethodsFacet.FacetTypeId.Should().Be(EFacetType.Discrete);
        datasetMethodsFacet.CategoryIdExpr.Should().Be("tbl_methods.method_id");
        datasetMethodsFacet.CategoryIdOperator.Should().Be("=");
        datasetMethodsFacet.AggregateFacetId.Should().Be(dbContext.Facets.Single(facet => facet.FacetCode == "result_facet").FacetId);
        datasetMethodsFacet.Tables.Should().Contain(table => table.SequenceId == 1 && table.TableId == datasetMethodsSourceTableId);
        datasetMethodsFacet
            .FacetAnchors.Should()
            .Contain(facetAnchor => facetAnchor.Anchor.Name == "sample" && facetAnchor.Route.Name == "dataset_methods__sample");
        datasetMethodsFacet
            .FacetAnchors.Should()
            .Contain(facetAnchor => facetAnchor.Anchor.Name == "dataset" && facetAnchor.Route.Name == "dataset_methods__dataset");

        dataTypesFacet.FacetGroupId.Should().Be(dbContext.FacetGroups.Single(group => group.FacetGroupKey == "others").FacetGroupId);
        dataTypesFacet.FacetTypeId.Should().Be(EFacetType.Discrete);
        dataTypesFacet.CategoryIdExpr.Should().Be("tbl_data_types.data_type_id");
        dataTypesFacet.CategoryIdOperator.Should().Be("=");
        dataTypesFacet.AggregateFacetId.Should().Be(dbContext.Facets.Single(facet => facet.FacetCode == "result_facet").FacetId);
        dataTypesFacet.Tables.Should().Contain(table => table.SequenceId == 1 && table.TableId == dataTypesSourceTableId);
        dataTypesFacet
            .FacetAnchors.Should()
            .Contain(facetAnchor => facetAnchor.Anchor.Name == "sample" && facetAnchor.Route.Name == "data_types__sample");
        dataTypesFacet
            .FacetAnchors.Should()
            .Contain(facetAnchor => facetAnchor.Anchor.Name == "dataset" && facetAnchor.Route.Name == "data_types__dataset");

        datasetProviderFacet
            .FacetGroupId.Should()
            .Be(dbContext.FacetGroups.Single(group => group.FacetGroupKey == "space_time").FacetGroupId);
        datasetProviderFacet.FacetTypeId.Should().Be(EFacetType.Discrete);
        datasetProviderFacet.CategoryIdExpr.Should().Be("tbl_dataset_masters.master_set_id");
        datasetProviderFacet.CategoryIdOperator.Should().Be("=");
        datasetProviderFacet.AggregateFacetId.Should().Be(dbContext.Facets.Single(facet => facet.FacetCode == "result_facet").FacetId);
        datasetProviderFacet.Tables.Should().Contain(table => table.SequenceId == 1 && table.TableId == datasetProviderSourceTableId);
        datasetProviderFacet
            .FacetAnchors.Should()
            .Contain(facetAnchor => facetAnchor.Anchor.Name == "sample" && facetAnchor.Route.Name == "dataset_provider__sample");
        datasetProviderFacet
            .FacetAnchors.Should()
            .Contain(facetAnchor => facetAnchor.Anchor.Name == "dataset" && facetAnchor.Route.Name == "dataset_provider__dataset");

        recordTypesFacet.FacetGroupId.Should().Be(dbContext.FacetGroups.Single(group => group.FacetGroupKey == "others").FacetGroupId);
        recordTypesFacet.FacetTypeId.Should().Be(EFacetType.Discrete);
        recordTypesFacet.CategoryIdExpr.Should().Be("tbl_record_types.record_type_id");
        recordTypesFacet.CategoryIdOperator.Should().Be("=");
        recordTypesFacet.AggregateFacetId.Should().Be(dbContext.Facets.Single(facet => facet.FacetCode == "result_facet").FacetId);
        recordTypesFacet.Tables.Should().Contain(table => table.SequenceId == 1 && table.TableId == recordTypesSourceTableId);
        recordTypesFacet
            .FacetAnchors.Should()
            .Contain(facetAnchor => facetAnchor.Anchor.Name == "sample" && facetAnchor.Route.Name == "record_types__sample");
        recordTypesFacet
            .FacetAnchors.Should()
            .Contain(facetAnchor => facetAnchor.Anchor.Name == "dataset" && facetAnchor.Route.Name == "record_types__dataset");

        countryFacet
            .Tables.Should()
            .Contain(table => table.SequenceId == 1 && table.TableId == countrySourceTableId && table.Alias == null);
        countryFacet.Clauses.Select(clause => clause.Clause).Should().Equal("facet.site_location_shortcut.location_type_id=1");
        countryFacet
            .FacetAnchors.Should()
            .Contain(facetAnchor => facetAnchor.Anchor.Name == "analysis_entity" && facetAnchor.Route.Name == "country__analysis_entity");

        activeRevision.ConfigRevision.Should().Be("phase5-runtime-slices-draft-06");
        activeRevision.SourceCommit.Should().Be("test-commit-01");
        activeRevision.ImportedBy.Should().Be("facet-import-test");
        activeRevision.ContentHash.Should().HaveLength(64);
        activeRevision.ImportedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromMinutes(1));
        dbContext.Set<FacetConfigRevision>().Count(revision => revision.IsActive).Should().Be(1);
    }

    [Fact]
    public void ValidateFile_WithRouteAnchorTargetMismatch_ThrowsInvalidOperationException()
    {
        var dbContext = (FacetContext)FacetContext;
        var importer = new FacetRouteConfigurationImporter(dbContext);
        var configurationFilePath = GetConfigurationFilePath();
        var fileContent = File.ReadAllText(configurationFilePath);
        var invalidContent = fileContent.Replace(
            "  - key: sample\n    table: tbl_physical_samples\n    key_column: physical_sample_id",
            "  - key: sample\n    table: tbl_datasets\n    key_column: dataset_id",
            StringComparison.Ordinal
        );
        var temporaryFilePath = CreateTemporaryConfigurationFile(invalidContent);

        try
        {
            var act = () => importer.ValidateFile(temporaryFilePath);

            act.Should()
                .Throw<InvalidOperationException>()
                .WithMessage("*Generated route 'feature_type' for anchor 'sample' ends on table 'tbl_physical_samples'*");
        }
        finally
        {
            File.Delete(temporaryFilePath);
        }
    }

    [Fact]
    public void ValidateFile_WithMissingFacetAnchorRoute_ThrowsInvalidOperationException()
    {
        var dbContext = (FacetContext)FacetContext;
        var importer = new FacetRouteConfigurationImporter(dbContext);
        var configurationFilePath = GetConfigurationFilePath();
        var fileContent = File.ReadAllText(configurationFilePath);
        var invalidContent = fileContent.Replace(
            "route: feature_type__sample",
            "route: feature_type__missing_sample",
            StringComparison.Ordinal
        );
        var temporaryFilePath = CreateTemporaryConfigurationFile(invalidContent);

        try
        {
            var act = () => importer.ValidateFile(temporaryFilePath);

            act.Should()
                .Throw<InvalidOperationException>()
                .WithMessage("*facet 'feature_type' route 'feature_type__missing_sample' could not be resolved*");
        }
        finally
        {
            File.Delete(temporaryFilePath);
        }
    }

    [Fact]
    public void ValidateFile_WithUnsupportedTemplateKeyOnRegularFacet_ThrowsInvalidOperationException()
    {
        var dbContext = (FacetContext)FacetContext;
        var importer = new FacetRouteConfigurationImporter(dbContext);
        var configurationFilePath = GetConfigurationFilePath();
        var fileContent = File.ReadAllText(configurationFilePath);
        var featureTypeFacetStart = fileContent.IndexOf("  - key: feature_type\n", StringComparison.Ordinal);
        featureTypeFacetStart.Should().BeGreaterThanOrEqualTo(0);

        var featureTypeClausesStart = fileContent.IndexOf("    clauses: []\n", featureTypeFacetStart, StringComparison.Ordinal);
        featureTypeClausesStart.Should().BeGreaterThanOrEqualTo(featureTypeFacetStart);

        var invalidContent =
            fileContent[..featureTypeClausesStart] + "    template_key: anchor_identity\n" + fileContent[featureTypeClausesStart..];
        var temporaryFilePath = CreateTemporaryConfigurationFile(invalidContent);

        try
        {
            var act = () => importer.ValidateFile(temporaryFilePath);

            act.Should()
                .Throw<InvalidOperationException>()
                .WithMessage("*Facet 'feature_type' cannot declare template_key 'anchor_identity'*");
        }
        finally
        {
            File.Delete(temporaryFilePath);
        }
    }

    [Fact]
    public void ValidateFile_WithBaseAnchorNotInFacetAnchors_ThrowsInvalidOperationException()
    {
        var dbContext = (FacetContext)FacetContext;
        var importer = new FacetRouteConfigurationImporter(dbContext);
        var configurationFilePath = GetConfigurationFilePath();
        var fileContent = File.ReadAllText(configurationFilePath);
        var featureTypeFacetStart = fileContent.IndexOf("  - key: feature_type\n", StringComparison.Ordinal);
        featureTypeFacetStart.Should().BeGreaterThanOrEqualTo(0);

        var baseAnchorLineStart = fileContent.IndexOf("      base_anchor: sample\n", featureTypeFacetStart, StringComparison.Ordinal);
        baseAnchorLineStart.Should().BeGreaterThanOrEqualTo(featureTypeFacetStart);

        var invalidContent =
            fileContent[..baseAnchorLineStart]
            + "      base_anchor: analysis_entity\n"
            + fileContent[(baseAnchorLineStart + "      base_anchor: sample\n".Length)..];
        var temporaryFilePath = CreateTemporaryConfigurationFile(invalidContent);

        try
        {
            var act = () => importer.ValidateFile(temporaryFilePath);

            act.Should()
                .Throw<InvalidOperationException>()
                .WithMessage("*Facet 'feature_type' declares sql base_anchor 'analysis_entity' which is not listed in its anchors.*");
        }
        finally
        {
            File.Delete(temporaryFilePath);
        }
    }

    [Fact]
    public void ValidateFile_WithSqlOverrideTargetingBaseAnchor_ThrowsInvalidOperationException()
    {
        var dbContext = (FacetContext)FacetContext;
        var importer = new FacetRouteConfigurationImporter(dbContext);
        var configurationFilePath = GetConfigurationFilePath();
        var fileContent = File.ReadAllText(configurationFilePath);
        var featureTypeFacetStart = fileContent.IndexOf("  - key: feature_type\n", StringComparison.Ordinal);
        featureTypeFacetStart.Should().BeGreaterThanOrEqualTo(0);

        var sampleAnchorEntryStart = fileContent.IndexOf(
            "      - anchor: sample\n        route: feature_type__sample\n",
            featureTypeFacetStart,
            StringComparison.Ordinal
        );
        sampleAnchorEntryStart.Should().BeGreaterThanOrEqualTo(featureTypeFacetStart);

        var replacementAnchorEntry =
            "      - anchor: sample\n        route: feature_type__sample\n"
            + "        sql_override: |\n"
            + "          select\n"
            + "            tbl_feature_types.feature_type_id as source_id,\n"
            + "            tbl_physical_samples.physical_sample_id as target_id\n"
            + "          from tbl_feature_types\n"
            + "          join tbl_features on tbl_features.feature_type_id = tbl_feature_types.feature_type_id\n"
            + "          join tbl_physical_sample_features on tbl_physical_sample_features.feature_id = tbl_features.feature_id\n"
            + "          join tbl_physical_samples on tbl_physical_samples.physical_sample_id = tbl_physical_sample_features.physical_sample_id\n";
        var anchorEntryEnd = sampleAnchorEntryStart + "      - anchor: sample\n        route: feature_type__sample\n".Length;
        var invalidContent = fileContent[..sampleAnchorEntryStart] + replacementAnchorEntry + fileContent[anchorEntryEnd..];
        var temporaryFilePath = CreateTemporaryConfigurationFile(invalidContent);

        try
        {
            var act = () => importer.ValidateFile(temporaryFilePath);

            act.Should()
                .Throw<InvalidOperationException>()
                .WithMessage(
                    "*Facet 'feature_type' has an explicit anchor-to-SQL override for anchor 'sample' which is also the base_anchor*"
                );
        }
        finally
        {
            File.Delete(temporaryFilePath);
        }
    }

    [Fact]
    public void ValidateFile_WithUnsupportedPlaceholderInBaseTemplate_ThrowsInvalidOperationException()
    {
        var dbContext = (FacetContext)FacetContext;
        var importer = new FacetRouteConfigurationImporter(dbContext);
        var configurationFilePath = GetConfigurationFilePath();
        var fileContent = File.ReadAllText(configurationFilePath);
        var featureTypeFacetStart = fileContent.IndexOf("  - key: feature_type\n", StringComparison.Ordinal);
        featureTypeFacetStart.Should().BeGreaterThanOrEqualTo(0);

        var bodyStart = fileContent.IndexOf("      body: |\n", featureTypeFacetStart, StringComparison.Ordinal);
        bodyStart.Should().BeGreaterThanOrEqualTo(featureTypeFacetStart);

        var bodyEnd = fileContent.IndexOf("\n    clauses:", bodyStart, StringComparison.Ordinal);
        bodyEnd.Should().BeGreaterThanOrEqualTo(bodyStart);

        var originalBody = fileContent[bodyStart..bodyEnd];
        var modifiedBody = originalBody.Replace(
            "        select\n",
            "        select\n          where {unsupported_placeholder}\n",
            StringComparison.Ordinal
        );
        var invalidContent = fileContent[..bodyStart] + modifiedBody + fileContent[bodyEnd..];
        var temporaryFilePath = CreateTemporaryConfigurationFile(invalidContent);

        try
        {
            var act = () => importer.ValidateFile(temporaryFilePath);

            act.Should()
                .Throw<InvalidOperationException>()
                .WithMessage(
                    "*Facet 'feature_type' base template uses unsupported placeholder(s) 'unsupported_placeholder' for contract 'discrete'*"
                );
        }
        finally
        {
            File.Delete(temporaryFilePath);
        }
    }

    [Fact]
    public void ValidateFile_WithUnsupportedPlaceholderInSqlOverride_ThrowsInvalidOperationException()
    {
        var dbContext = (FacetContext)FacetContext;
        var importer = new FacetRouteConfigurationImporter(dbContext);
        var configurationFilePath = GetConfigurationFilePath();
        var fileContent = File.ReadAllText(configurationFilePath);
        var featureTypeFacetStart = fileContent.IndexOf("  - key: feature_type\n", StringComparison.Ordinal);
        featureTypeFacetStart.Should().BeGreaterThanOrEqualTo(0);

        var datasetAnchorEntryStart = fileContent.IndexOf(
            "      - anchor: dataset\n        route: feature_type__dataset\n",
            featureTypeFacetStart,
            StringComparison.Ordinal
        );
        datasetAnchorEntryStart.Should().BeGreaterThanOrEqualTo(featureTypeFacetStart);

        var replacementAnchorEntry =
            "      - anchor: dataset\n        route: feature_type__dataset\n"
            + "        sql_override: |\n"
            + "          select\n"
            + "            tbl_feature_types.feature_type_id as source_id,\n"
            + "            tbl_datasets.dataset_id as target_id\n"
            + "          from tbl_feature_types\n"
            + "          where {unsupported_placeholder}\n";
        var anchorEntryEnd = datasetAnchorEntryStart + "      - anchor: dataset\n        route: feature_type__dataset\n".Length;
        var invalidContent = fileContent[..datasetAnchorEntryStart] + replacementAnchorEntry + fileContent[anchorEntryEnd..];
        var temporaryFilePath = CreateTemporaryConfigurationFile(invalidContent);

        try
        {
            var act = () => importer.ValidateFile(temporaryFilePath);

            act.Should()
                .Throw<InvalidOperationException>()
                .WithMessage(
                    "*Facet 'feature_type' anchor 'dataset' sql_override uses unsupported placeholder(s) 'unsupported_placeholder' for contract 'discrete'*"
                );
        }
        finally
        {
            File.Delete(temporaryFilePath);
        }
    }

    [Fact]
    public void ImportFromFile_WithInlineTemplateMetadata_PersistsFacetTemplateRows()
    {
        var dbContext = (FacetContext)FacetContext;
        var importer = new FacetRouteConfigurationImporter(dbContext);
        var configurationFilePath = GetConfigurationFilePath();
        var fileContent = File.ReadAllText(configurationFilePath);
        var updatedContent = fileContent.Replace(
            "    clauses: []\n    anchors:\n      - anchor: sample\n        route: feature_type__sample\n      - anchor: dataset\n        route: feature_type__dataset",
            "    clauses: []\n    anchors:\n      - anchor: sample\n        route: feature_type__sample\n      - anchor: dataset\n        route: feature_type__dataset\n        sql_override: |\n          select\n            tbl_feature_types.feature_type_id as source_id,\n            tbl_datasets.dataset_id as target_id\n          from tbl_feature_types\n          join tbl_features on tbl_features.feature_type_id = tbl_feature_types.feature_type_id\n          join tbl_physical_sample_features on tbl_physical_sample_features.feature_id = tbl_features.feature_id\n          join tbl_physical_samples on tbl_physical_samples.physical_sample_id = tbl_physical_sample_features.physical_sample_id\n          join tbl_analysis_entities on tbl_analysis_entities.physical_sample_id = tbl_physical_samples.physical_sample_id\n          join tbl_datasets on tbl_datasets.dataset_id = tbl_analysis_entities.dataset_id",
            StringComparison.Ordinal
        );
        var temporaryFilePath = CreateTemporaryConfigurationFile(updatedContent);

        try
        {
            importer.ImportFromFile(temporaryFilePath);

            var featureTypeFacetId = dbContext.Facets.Single(facet => facet.FacetCode == "feature_type").FacetId;
            var datasetAnchorId = dbContext.Anchors.Single(anchor => anchor.Name == "dataset").AnchorId;

            QueryScalar(
                    dbContext,
                    "select template_contract from facet.facet_template where facet_id = @facet_id and template_role = 'base_sql' limit 1",
                    ("@facet_id", featureTypeFacetId)
                )
                .Should()
                .Be("discrete");
            QueryScalar(
                    dbContext,
                    "select base_anchor from facet.facet_template where facet_id = @facet_id and template_role = 'base_sql' limit 1",
                    ("@facet_id", featureTypeFacetId)
                )
                .Should()
                .Be("sample");
            QueryScalar(
                    dbContext,
                    "select sql_text from facet.facet_template where facet_id = @facet_id and template_role = 'base_sql' limit 1",
                    ("@facet_id", featureTypeFacetId)
                )
                .Should()
                .Contain("category_id");
            QueryScalar(
                    dbContext,
                    "select sql_text from facet.facet_template where facet_id = @facet_id and anchor_id = @anchor_id and template_role = 'anchor_sql' limit 1",
                    ("@facet_id", featureTypeFacetId),
                    ("@anchor_id", datasetAnchorId)
                )
                .Should()
                .Contain("source_id");
        }
        finally
        {
            File.Delete(temporaryFilePath);
        }
    }

    [Fact]
    public void ImportFromFile_WithTemplateKeyAndInlineSql_PersistsFacetTemplateRows()
    {
        var dbContext = (FacetContext)FacetContext;
        var importer = new FacetRouteConfigurationImporter(dbContext);
        var configurationFilePath = GetConfigurationFilePath();
        var fileContent = File.ReadAllText(configurationFilePath);

        var updatedContent = fileContent.Replace(
            "    aggregate:\n      type: count\n      title: Number of samples\n    template_key: anchor_identity\n    clauses: []\n    anchors:\n      - anchor: analysis_entity\n        route: analysis_entity_ages__analysis_entity",
            "    aggregate:\n      type: count\n      title: Number of samples\n    template_key: anchor_identity\n    sql:\n      mode: inline-template\n      contract: discrete\n      base_anchor: analysis_entity\n      body: |\n        select\n          tbl_analysis_entities.analysis_entity_id as category_id,\n          tbl_analysis_entities.analysis_entity_id as anchor_id\n    clauses: []\n    anchors:\n      - anchor: analysis_entity\n        route: analysis_entity_ages__analysis_entity",
            StringComparison.Ordinal
        );
        var temporaryFilePath = CreateTemporaryConfigurationFile(updatedContent);

        try
        {
            importer.ImportFromFile(temporaryFilePath);

            var resultFacetId = dbContext.Facets.Single(facet => facet.FacetCode == "result_facet").FacetId;

            QueryScalar(
                    dbContext,
                    "select template_key from facet.facet_template where facet_id = @facet_id and template_role = 'template_key' limit 1",
                    ("@facet_id", resultFacetId)
                )
                .Should()
                .Be("anchor_identity");
            QueryScalar(
                    dbContext,
                    "select template_contract from facet.facet_template where facet_id = @facet_id and template_role = 'base_sql' limit 1",
                    ("@facet_id", resultFacetId)
                )
                .Should()
                .Be("discrete");
            QueryScalar(
                    dbContext,
                    "select base_anchor from facet.facet_template where facet_id = @facet_id and template_role = 'base_sql' limit 1",
                    ("@facet_id", resultFacetId)
                )
                .Should()
                .Be("analysis_entity");
            QueryScalar(
                    dbContext,
                    "select sql_text from facet.facet_template where facet_id = @facet_id and template_role = 'base_sql' limit 1",
                    ("@facet_id", resultFacetId)
                )
                .Should()
                .Contain("category_id");
        }
        finally
        {
            File.Delete(temporaryFilePath);
        }
    }

    [Fact]
    public void ValidateFile_WithRetainedResultShapeFacetMissingTemplateKey_ThrowsInvalidOperationException()
    {
        var dbContext = (FacetContext)FacetContext;
        var importer = new FacetRouteConfigurationImporter(dbContext);
        var configurationFilePath = GetConfigurationFilePath();
        var fileContent = File.ReadAllText(configurationFilePath);

        var resultFacetDefinition =
            "\n  - key: result_facet\n"
            + "    display_title: Result facet\n"
            + "    description: Retained result-shape facet\n"
            + "    group_key: others\n"
            + "    type: discrete\n"
            + "    source_table: tbl_analysis_entities\n"
            + "    category:\n"
            + "      id_expr: tbl_analysis_entities.analysis_entity_id\n"
            + "      name_expr: tbl_analysis_entities.analysis_entity_id::text\n"
            + "      data_type: integer\n"
            + "      operator: \"=\"\n"
            + "    sort_expr: tbl_analysis_entities.analysis_entity_id::text\n"
            + "    flags:\n"
            + "      is_applicable: true\n"
            + "      is_default: false\n"
            + "    aggregate:\n"
            + "      type: count\n"
            + "      title: Number of samples\n"
            + "    clauses: []\n"
            + "    anchors:\n"
            + "      - anchor: analysis_entity\n"
            + "        route: analysis_entity_ages__analysis_entity\n";

        var invalidContent = fileContent + resultFacetDefinition;
        var temporaryFilePath = CreateTemporaryConfigurationFile(invalidContent);

        try
        {
            var act = () => importer.ValidateFile(temporaryFilePath);

            act.Should()
                .Throw<InvalidOperationException>()
                .WithMessage("*Facet 'result_facet' is a retained result-shape facet and must declare a template_key*");
        }
        finally
        {
            File.Delete(temporaryFilePath);
        }
    }

    [Fact]
    public void ValidateFile_WithTemplateKeyAndInlineSqlTogether_Succeeds()
    {
        var dbContext = (FacetContext)FacetContext;
        var importer = new FacetRouteConfigurationImporter(dbContext);
        var configurationFilePath = GetConfigurationFilePath();
        var fileContent = File.ReadAllText(configurationFilePath);

        var resultFacetDefinition =
            "\n  - key: result_facet\n"
            + "    display_title: Result facet\n"
            + "    description: Retained result-shape facet\n"
            + "    group_key: others\n"
            + "    type: discrete\n"
            + "    source_table: tbl_analysis_entities\n"
            + "    category:\n"
            + "      id_expr: tbl_analysis_entities.analysis_entity_id\n"
            + "      name_expr: tbl_analysis_entities.analysis_entity_id::text\n"
            + "      data_type: integer\n"
            + "      operator: \"=\"\n"
            + "    sort_expr: tbl_analysis_entities.analysis_entity_id::text\n"
            + "    flags:\n"
            + "      is_applicable: true\n"
            + "      is_default: false\n"
            + "    aggregate:\n"
            + "      type: count\n"
            + "      title: Number of samples\n"
            + "    template_key: anchor_identity\n"
            + "    sql:\n"
            + "      mode: inline-template\n"
            + "      contract: discrete\n"
            + "      base_anchor: analysis_entity\n"
            + "      body: |\n"
            + "        select 1 as category_id\n"
            + "    clauses: []\n"
            + "    anchors:\n"
            + "      - anchor: analysis_entity\n"
            + "        route: analysis_entity_ages__analysis_entity\n";

        var invalidContent = fileContent + resultFacetDefinition;
        var temporaryFilePath = CreateTemporaryConfigurationFile(invalidContent);

        try
        {
            var act = () => importer.ValidateFile(temporaryFilePath);

            act.Should().NotThrow();
        }
        finally
        {
            File.Delete(temporaryFilePath);
        }
    }

    [Fact]
    public void ValidateFile_WithValidTemplateKeyOnRetainedResultShapeFacet_Succeeds()
    {
        var dbContext = (FacetContext)FacetContext;
        var importer = new FacetRouteConfigurationImporter(dbContext);
        var configurationFilePath = GetConfigurationFilePath();
        var fileContent = File.ReadAllText(configurationFilePath);

        var resultFacetDefinition =
            "\n  - key: result_facet\n"
            + "    display_title: Result facet\n"
            + "    description: Retained result-shape facet\n"
            + "    group_key: others\n"
            + "    type: discrete\n"
            + "    source_table: tbl_analysis_entities\n"
            + "    category:\n"
            + "      id_expr: tbl_analysis_entities.analysis_entity_id\n"
            + "      name_expr: tbl_analysis_entities.analysis_entity_id::text\n"
            + "      data_type: integer\n"
            + "      operator: \"=\"\n"
            + "    sort_expr: tbl_analysis_entities.analysis_entity_id::text\n"
            + "    flags:\n"
            + "      is_applicable: true\n"
            + "      is_default: false\n"
            + "    aggregate:\n"
            + "      type: count\n"
            + "      title: Number of samples\n"
            + "    template_key: anchor_identity\n"
            + "    clauses: []\n"
            + "    anchors:\n"
            + "      - anchor: analysis_entity\n"
            + "        route: analysis_entity_ages__analysis_entity\n";

        var validContent = fileContent + resultFacetDefinition;
        var temporaryFilePath = CreateTemporaryConfigurationFile(validContent);

        try
        {
            var act = () => importer.ValidateFile(temporaryFilePath);

            act.Should().NotThrow();
        }
        finally
        {
            File.Delete(temporaryFilePath);
        }
    }

    [Fact]
    public void ValidateFile_WithUnsupportedSqlContract_ThrowsInvalidOperationException()
    {
        var dbContext = (FacetContext)FacetContext;
        var importer = new FacetRouteConfigurationImporter(dbContext);
        var configurationFilePath = GetConfigurationFilePath();
        var fileContent = File.ReadAllText(configurationFilePath);
        var featureTypeFacetStart = fileContent.IndexOf("  - key: feature_type\n", StringComparison.Ordinal);
        featureTypeFacetStart.Should().BeGreaterThanOrEqualTo(0);

        var contractLineStart = fileContent.IndexOf("      contract: discrete\n", featureTypeFacetStart, StringComparison.Ordinal);
        contractLineStart.Should().BeGreaterThanOrEqualTo(featureTypeFacetStart);

        var invalidContent =
            fileContent[..contractLineStart]
            + "      contract: intersect\n"
            + fileContent[(contractLineStart + "      contract: discrete\n".Length)..];
        var temporaryFilePath = CreateTemporaryConfigurationFile(invalidContent);

        try
        {
            var act = () => importer.ValidateFile(temporaryFilePath);

            act.Should()
                .Throw<InvalidOperationException>()
                .WithMessage("*Facet 'feature_type' uses unsupported sql contract 'intersect'*");
        }
        finally
        {
            File.Delete(temporaryFilePath);
        }
    }

    [Fact]
    public void ValidateFile_WithTypeContractMismatch_ThrowsInvalidOperationException()
    {
        var dbContext = (FacetContext)FacetContext;
        var importer = new FacetRouteConfigurationImporter(dbContext);
        var configurationFilePath = GetConfigurationFilePath();
        var fileContent = File.ReadAllText(configurationFilePath);
        var featureTypeFacetStart = fileContent.IndexOf("  - key: feature_type\n", StringComparison.Ordinal);
        featureTypeFacetStart.Should().BeGreaterThanOrEqualTo(0);

        var contractLineStart = fileContent.IndexOf("      contract: discrete\n", featureTypeFacetStart, StringComparison.Ordinal);
        contractLineStart.Should().BeGreaterThanOrEqualTo(featureTypeFacetStart);

        var invalidContent =
            fileContent[..contractLineStart]
            + "      contract: range\n"
            + fileContent[(contractLineStart + "      contract: discrete\n".Length)..];
        var temporaryFilePath = CreateTemporaryConfigurationFile(invalidContent);

        try
        {
            var act = () => importer.ValidateFile(temporaryFilePath);

            act.Should()
                .Throw<InvalidOperationException>()
                .WithMessage("*Facet 'feature_type' type 'discrete' is incompatible with sql contract 'range'*");
        }
        finally
        {
            File.Delete(temporaryFilePath);
        }
    }

    [Fact]
    public void ValidateFile_WithMissingBaseAnchorInDocument_ThrowsInvalidOperationException()
    {
        var dbContext = (FacetContext)FacetContext;
        var importer = new FacetRouteConfigurationImporter(dbContext);
        var configurationFilePath = GetConfigurationFilePath();
        var fileContent = File.ReadAllText(configurationFilePath);
        var featureTypeFacetStart = fileContent.IndexOf("  - key: feature_type\n", StringComparison.Ordinal);
        featureTypeFacetStart.Should().BeGreaterThanOrEqualTo(0);

        var baseAnchorLineStart = fileContent.IndexOf("      base_anchor: sample\n", featureTypeFacetStart, StringComparison.Ordinal);
        baseAnchorLineStart.Should().BeGreaterThanOrEqualTo(featureTypeFacetStart);

        var invalidContent =
            fileContent[..baseAnchorLineStart]
            + "      base_anchor: nonexistent_anchor\n"
            + fileContent[(baseAnchorLineStart + "      base_anchor: sample\n".Length)..];
        var temporaryFilePath = CreateTemporaryConfigurationFile(invalidContent);

        try
        {
            var act = () => importer.ValidateFile(temporaryFilePath);

            act.Should()
                .Throw<InvalidOperationException>()
                .WithMessage("*facet 'feature_type' sql base anchor 'nonexistent_anchor' could not be resolved*");
        }
        finally
        {
            File.Delete(temporaryFilePath);
        }
    }

    [Fact]
    public void ValidateFile_WithMissingSqlBody_ThrowsInvalidOperationException()
    {
        var dbContext = (FacetContext)FacetContext;
        var importer = new FacetRouteConfigurationImporter(dbContext);
        var configurationFilePath = GetConfigurationFilePath();
        var fileContent = File.ReadAllText(configurationFilePath);
        var featureTypeFacetStart = fileContent.IndexOf("  - key: feature_type\n", StringComparison.Ordinal);
        featureTypeFacetStart.Should().BeGreaterThanOrEqualTo(0);

        var bodyStart = fileContent.IndexOf("      body: |\n", featureTypeFacetStart, StringComparison.Ordinal);
        bodyStart.Should().BeGreaterThanOrEqualTo(featureTypeFacetStart);

        var bodyEnd = fileContent.IndexOf("\n    clauses:", bodyStart, StringComparison.Ordinal);
        bodyEnd.Should().BeGreaterThanOrEqualTo(bodyStart);

        var originalBody = fileContent[bodyStart..bodyEnd];
        var bodyLineCount = originalBody.Split('\n').Length;
        var bodyLineStart = fileContent.IndexOf("        select\n", bodyStart, StringComparison.Ordinal);
        bodyLineStart.Should().BeGreaterThanOrEqualTo(bodyStart);

        var bodyContentEnd = fileContent.IndexOf("\n    clauses:", bodyLineStart, StringComparison.Ordinal);
        bodyContentEnd.Should().BeGreaterThan(bodyLineStart);

        var invalidContent = fileContent[..bodyLineStart] + "      body: \"\"\n" + fileContent[bodyContentEnd..];
        var temporaryFilePath = CreateTemporaryConfigurationFile(invalidContent);

        try
        {
            var act = () => importer.ValidateFile(temporaryFilePath);

            act.Should()
                .Throw<InvalidOperationException>()
                .WithMessage("*facet 'feature_type' sql is missing required field 'Body'*");
        }
        finally
        {
            File.Delete(temporaryFilePath);
        }
    }

    [Fact]
    public void ValidateFile_WithUnsupportedSqlMode_ThrowsInvalidOperationException()
    {
        var dbContext = (FacetContext)FacetContext;
        var importer = new FacetRouteConfigurationImporter(dbContext);
        var configurationFilePath = GetConfigurationFilePath();
        var fileContent = File.ReadAllText(configurationFilePath);
        var featureTypeFacetStart = fileContent.IndexOf("  - key: feature_type\n", StringComparison.Ordinal);
        featureTypeFacetStart.Should().BeGreaterThanOrEqualTo(0);

        var modeLineStart = fileContent.IndexOf("      mode: inline-template\n", featureTypeFacetStart, StringComparison.Ordinal);
        modeLineStart.Should().BeGreaterThanOrEqualTo(featureTypeFacetStart);

        var invalidContent =
            fileContent[..modeLineStart]
            + "      mode: legacy-override\n"
            + fileContent[(modeLineStart + "      mode: inline-template\n".Length)..];
        var temporaryFilePath = CreateTemporaryConfigurationFile(invalidContent);

        try
        {
            var act = () => importer.ValidateFile(temporaryFilePath);

            act.Should()
                .Throw<InvalidOperationException>()
                .WithMessage("*Facet 'feature_type' uses unsupported sql mode 'legacy-override'*");
        }
        finally
        {
            File.Delete(temporaryFilePath);
        }
    }

    private static string GetConfigurationFilePath()
    {
        return Path.GetFullPath(Path.Combine(ScaffoldUtility.GetProjectRoot(), "..", "sead.query.composer", "Templates", "route_v1.yaml"));
    }

    private static string CreateTemporaryConfigurationFile(string fileContent)
    {
        var temporaryFilePath = Path.Combine(Path.GetTempPath(), $"facet-route-config-{Guid.NewGuid():N}.yaml");
        File.WriteAllText(temporaryFilePath, fileContent);
        return temporaryFilePath;
    }

    private static string QueryScalar(FacetContext dbContext, string sql, params (string Name, object Value)[] parameters)
    {
        using var command = dbContext.Database.GetDbConnection().CreateCommand();
        command.CommandText = sql;

        foreach (var parameter in parameters)
        {
            var dbParameter = command.CreateParameter();
            dbParameter.ParameterName = parameter.Name;
            dbParameter.Value = parameter.Value;
            command.Parameters.Add(dbParameter);
        }

        if (command.Connection?.State != ConnectionState.Open)
        {
            command.Connection?.Open();
        }

        var value = command.ExecuteScalar();
        return value?.ToString() ?? string.Empty;
    }
}
