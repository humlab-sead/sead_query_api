using Autofac;
using SeadQueryCore;
using SeadQueryCore.Plugin.Intersect;
using SQT.Infrastructure;
using SQT.SQL.Matcher;
using Xunit;

namespace SQT.Plugins.Intersect
{


    [Collection("SeadJsonFacetContextFixture")]
    public class QueryBuilderTests(SeadJsonFacetContextFixture facetContextFixture) : IntegrationTestBase(facetContextFixture)
    {

        [Theory]
        [InlineData("analysis_entity_ages:analysis_entity_ages")]
        public void Load_VariousFacetConfigs_HasExpectedSqlQuery(string uri)
        {
            var facetsConfig = FakeFacetsConfig(uri);
            var resultConfig = FakeResultConfig("result_facet", "site_level", "tabular");

            var queryFields = resultConfig.GetSortedFields();

            var querySetup = QuerySetupBuilder
                .Build(facetsConfig, resultConfig.Facet, queryFields);

            var sqlQuery = SqlCompilerLocator
                .Locate(resultConfig.ViewTypeId)
                    .Compile(querySetup, resultConfig.Facet, queryFields);

            Assert.NotNull(sqlQuery);

        }

        [Theory]
        [InlineData("analysis_entity_ages:analysis_entity_ages@850000,2350000")]
        // [InlineData("analysis_entity_ages:analysis_entity_ages")]
        public void FacetContentService_Compile_ToCorrectSql(string uri)
        {
            var (dataLow, dataHigh, desiredCount) = (1.0m, 10.0m, 3);

            var facetsConfig = FakeFacetsConfig(uri);

            var facet = facetsConfig.TargetFacet;
            var targetFacet = facet;
            var aggregateFacet = facet;

            var plugin = Container.ResolveKeyed<IFacetPlugin>(facet.FacetTypeId);

            var categoryInfo = plugin.CategoryInfoService.GetCategoryInfo(facetsConfig, facetsConfig.TargetCode);

            CompilePayload compilePayload = new CompilePayload()
            {
                ResultFacet = facet,
                TargetFacet = targetFacet,
                AggregateFacet = aggregateFacet,
                IntervalQuery = categoryInfo.Query,
                CountColumn = "tbl_analysis_entities.analysis_entity_id",
                AggregateType = facet.AggregateType ?? "count"
            };

            var extraTableNames = plugin.CategoryCountHelper.GetTables(compilePayload);
            var facetCodes = plugin.CategoryCountHelper.GetFacetCodes(facetsConfig, compilePayload);

            var querySetup = QuerySetupBuilder.Build(facetsConfig, facet, extraTableNames, facetCodes);
            var sqlQuery = plugin.CategoryCountSqlCompiler.Compile(querySetup, facet, compilePayload);

            // Assert
            sqlQuery = sqlQuery.Squeeze();
            var match = CategoryCountSqlCompilerMatcher
                .Create(facet.FacetTypeId).Match(sqlQuery);

            Assert.True(match.Success);

            Assert.True(match.InnerSelect.Success);

            Assert.NotEmpty(match.InnerSelect.Tables);
        }

        // [Theory]
        // [InlineData("analysis_entity_ages:analysis_entity_ages")]
        // public void Compile_VariousConfigs_ExpectedBehavior(string uri)
        // {
        //     var (dataLow, dataHigh, desiredCount) = (1.0m, 10.0m, 3);

        //     // Arrange
        //     var facetsConfig = FakeFacetsConfig(uri);
        //     var resultConfig = FakeResultConfig("result_facet", "site_level", "tabular");
        //     var queryFields = resultConfig.GetSortedFields();

        //     var querySetup = QuerySetupBuilder
        //         .Build(facetsConfig, resultConfig.Facet, queryFields);

        //     var facet = querySetup.Facet;

        //     var infoService = Container.ResolveKeyed<ICategoryInfoService>(facet.FacetTypeId);

        //     var payload = new TickerInfo(dataLow, dataHigh, desiredCount);

        //     var categoryInfo = infoService.GetCategoryInfo(facetsConfig, facet.FacetCode, payload);
        //     // Act
        //     var sqlCompiler = new IntersectCategoryCountSqlCompiler();

        //     var compilePayload = new CompilePayload {
        //         IntervalQuery = categoryInfo.Query,
        //         ResultFacet = resultConfig.Facet,
        //         TargetFacet = facet,
        //         AggregateFacet = null,
        //         AggregateType = null,
        //         CountColumn = null
        //     };
        //     var result = sqlCompiler.Compile(querySetup, facet, compilePayload);

        //     // Assert
        //     result = result.Squeeze();
        //     var match = CategoryCountSqlCompilerMatcher
        //         .Create(facet.FacetTypeId).Match(result);

        //     Assert.True(match.Success);

        //     Assert.True(match.InnerSelect.Success);

        //     Assert.NotEmpty(match.InnerSelect.Tables);
        // }
    }
}
