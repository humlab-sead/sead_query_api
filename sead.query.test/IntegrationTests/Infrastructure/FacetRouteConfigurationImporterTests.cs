using System;
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

        countryFacet.Tables.Should().Contain(table => table.SequenceId == 1 && table.TableId == countrySourceTableId && table.Alias == null);
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
}
