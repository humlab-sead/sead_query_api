using SeadQueryCore;
using SeadQueryCore.Plugin.Discrete;
using SQT.Infrastructure;
using SQT.SQL.Matcher;
using Xunit;

namespace SQT.Plugins.Discrete
{
    [Collection("UsePostgresDockerSession")]
    public class CategoryCountSqlCompilerTests() : MockerWithFacetContext()
    {
        [Theory]
        [InlineData("sites:sites", "sites", "count")]
        [InlineData("country:sites", "sites", "count")]
        [InlineData("country@57:sites@3", "sites", "count")]
        public void Compile_VariousConfigs_ExpectedBehavior(string uri, string facetCode, string aggType)
        {
            // Arrange
            var fakeFacetsConfig = FakeFacetsConfig(uri);
            var fakeQuerySetup = FakeCountOrContentQuerySetup(fakeFacetsConfig);
            var facet = Registry.Facets.GetByCode(facetCode);
            var countFacet = facet;

            // Act

            CompilePayload compilePayload = new CompilePayload()
            {
                AggregateType = aggType,
                AggregateFacet = countFacet,
                TargetFacet = facet
            };
            var discreteCategoryCountSqlCompiler = new DiscreteCategoryCountSqlCompiler();
            var sqlQuery = discreteCategoryCountSqlCompiler.Compile(fakeQuerySetup, facet, compilePayload);

            // Assert

            var matcher = new DiscreteCategoryCountSqlCompilerMatcher();
            var match = matcher.Match(sqlQuery.Squeeze());

            Assert.True(match.Success);
        }
    }
}
