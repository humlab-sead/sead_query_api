using SeadQueryCore;
using SeadQueryCore.Model.Ext;
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
        [InlineData("sites:sites", "result_facet", "site_level")]
        [InlineData("sites:country/sites", "result_facet", "site_level")]
        public void Compile_StateUnderTest_ExpectedBehavior(string uri, string resultFacetCode, string aggregateKey)
        {
            // Arrange
            var fakeFacetsConfig = FakeFacetsConfig(uri);
            var fakeQuerySetup = FakeResultQuerySetup(fakeFacetsConfig, resultFacetCode, aggregateKey);
            var fakeResultFields = FakeResultConfig(resultFacetCode, aggregateKey, "map").GetSortedFields();

            // Act

            var sqlCompiler = new MapResultSqlCompiler();
            var result = sqlCompiler.Compile(fakeQuerySetup, fakeQuerySetup.Facet, fakeResultFields);

            // Assert
            var matcher = new MapResultSqlCompilerMatcher();

            var match = matcher.Match(result);

            Assert.True(match.Success);
        }

    }
}
