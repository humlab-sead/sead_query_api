using SeadQueryCore;
using SeadQueryCore.Plugin.Intersect;
using SQT.SQL.Matcher;
using SQT.Infrastructure;
using Xunit;

namespace SQT.Plugins.Intersect
{
    [Collection("SeadJsonFacetContextFixture")]
    public class CategoryCountSqlCompilerTests(SeadJsonFacetContextFixture fixture) : MockerWithFacetContext(fixture)
    {
        [Theory]
        [InlineData("analysis_entity_ages:analysis_entity_ages")]
        public void Compile_VariousConfigs_ExpectedBehavior(string uri)
        {
            // Arrange
            var fakeFacetsConfig = FakeFacetsConfig(uri);
            var fakeQuerySetup = FakeCountOrContentQuerySetup(fakeFacetsConfig);
            var facet = fakeQuerySetup.Facet;
            const string intervalQuery = "#INTERVAL-QUERY#";
            const string countColumn = "dummy_column";

            CompilePayload compilePayload = new CompilePayload()
            {
                IntervalQuery = intervalQuery,
                CountColumn = countColumn,
                AggregateType = null,
                AggregateFacet = null,
                TargetFacet = null
            };
            // Act
            var sqlCompiler = new IntersectCategoryCountSqlCompiler();
            var result = sqlCompiler.Compile(fakeQuerySetup, facet, compilePayload);

            // Assert
            result = result.Squeeze();
            var match = CategoryCountSqlCompilerMatcher
                .Create(facet.FacetTypeId).Match(result);

            Assert.True(match.Success);

            Assert.True(match.InnerSelect.Success);

            Assert.NotEmpty(match.InnerSelect.Tables);
        }
    }
}
