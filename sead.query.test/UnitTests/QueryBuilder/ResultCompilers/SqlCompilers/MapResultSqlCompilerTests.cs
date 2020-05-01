using SeadQueryCore;
using SQT.Fixtures;
using SQT.Infrastructure;
using SQT.SQL.Matcher;
using System.Collections.Generic;
using Xunit;

namespace SQT.SqlCompilers
{
    [Collection("JsonSeededFacetContext")]
    public class MapResultSqlCompilerTests : DisposableFacetContextContainer
    {
        public MapResultSqlCompilerTests(JsonSeededFacetContextFixture fixture) : base(fixture)
        {
        }

        [Theory]
        [InlineData("sites:sites", "result_facet")]
        [InlineData("sites:country/sites", "result_facet")]
        public void Compile_StateUnderTest_ExpectedBehavior(string uri, string resultCode)
        {
            // Arrange
            var fakeFacetsConfig = FakeFacetsConfig(uri);
            var fakeResultConfig = FakeResultConfig("site_level", "map");
            var fields = FakeResultAggregateFields("site_level", "map");
            List<string> extraTables = null;
            var fakeQuerySetup = FakeQuerySetup(fakeFacetsConfig, resultCode, extraTables);
            var fakeResultFields = FakeResultAggregateFields("site_level", "map");
            var fakeFacet = FakeRegistry().Facets.GetByCode(resultCode);

            // Act

            var mapResultSqlCompiler = new MapResultSqlCompiler();
            var result = mapResultSqlCompiler.Compile(fakeQuerySetup, fakeFacet, fakeResultFields);

            // Assert
            var matcher = new MapResultSqlCompilerMatcher();

            var match = matcher.Match(result);

            Assert.True(match.Success);
        }

    }
}
