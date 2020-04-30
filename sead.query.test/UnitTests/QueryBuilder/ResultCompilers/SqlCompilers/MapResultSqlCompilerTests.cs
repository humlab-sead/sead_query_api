using SeadQueryCore;
using SQT.Fixtures;
using SQT.Infrastructure;
using SQT.SQL.Matcher;
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
        [InlineData("sites:sites", "sites")]
        [InlineData("sites:country/sites", "sites")]
        public void Compile_StateUnderTest_ExpectedBehavior(string uri, string facetCode)
        {
            // Arrange
            var querySetupMockFactory = new MockQuerySetupFactory(Registry);
            var fakeQuerySetup = new MockQuerySetupFactory(Registry).Scaffold(uri);
            var fakeResultFields = FakeResultAggregateFields("site_level", "map");
            var fakeFacet = FakeRegistry().Facets.GetByCode(facetCode);

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
