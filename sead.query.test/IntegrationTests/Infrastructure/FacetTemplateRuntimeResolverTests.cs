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
public class FacetTemplateRuntimeResolverTests : MockerWithFacetContext
{
    [Fact]
    public void GetTemplateSnapshot_AfterImport_LoadsBaseTemplateAndAnchorOverrideMetadata()
    {
        var dbContext = (FacetContext)FacetContext;
        var importer = new FacetRouteConfigurationImporter(dbContext);
        var resolver = new FacetTemplateRuntimeResolver(dbContext);
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

            var featureTypeFacet = dbContext
                .Facets.Include(facet => facet.FacetAnchors)
                .ThenInclude(facetAnchor => facetAnchor.Anchor)
                .ThenInclude(anchor => anchor.Table)
                .Single(facet => facet.FacetCode == "feature_type");

            var snapshot = resolver.GetTemplateSnapshot(featureTypeFacet);

            snapshot.TemplateKey.Should().BeEmpty();
            snapshot.TemplateContract.Should().Be("discrete");
            snapshot.BaseAnchor.Should().Be("sample");
            snapshot.BaseSql.Should().Contain("category_id");
            snapshot.AnchorSqlByTable.Should().ContainKey("tbl_datasets");
            snapshot.AnchorSqlByTable["tbl_datasets"].Should().Contain("source_id");
            resolver.GetAnchorSql(featureTypeFacet, "tbl_datasets").Should().Contain("source_id");
        }
        finally
        {
            File.Delete(temporaryFilePath);
        }
    }

    private static string GetConfigurationFilePath()
    {
        return Path.GetFullPath(
            Path.Combine(ScaffoldUtility.GetProjectRoot(), "..", "sead.query.composer", "Templates", "route_v1.yaml")
        );
    }

    private static string CreateTemporaryConfigurationFile(string fileContent)
    {
        var temporaryFilePath = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid():N}.yaml");
        File.WriteAllText(temporaryFilePath, fileContent);
        return temporaryFilePath;
    }
}
