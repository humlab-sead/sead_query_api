using SeadQueryCore.Plugin;
using SQT.Infrastructure;
using SQT.SQL.Matcher;
using Xunit;
using System.Diagnostics;

namespace SQT.SqlCompilers
{
    [Collection("SeadJsonFacetContextFixture")]
    public class GeoPolygonContentSqlCompilerTests : DisposableFacetContextContainer
    {
        public GeoPolygonContentSqlCompilerTests(SeadJsonFacetContextFixture fixture) : base(fixture)
        {
        }


        [Theory]
        [InlineData("sites_polygon:sites_polygon@63.872484,20.093291,63.947006,20.501316,63.878949,20.673213,63.748021,20.252953,63.793983,20.095738")]
        public void Compile_GeoPolygon_Matches(string uri)
        {
            // Arrange
            var facetsConfig = FakeFacetsConfig(uri);
            var facetConfig = facetsConfig.TargetConfig;
            var facet = MockRegistryWithFacetRepository().Object.Facets.GetByCode(facetsConfig.TargetCode);
            var pickCriteria = new GeoPolygonPickFilterCompiler().Compile(facet, facet, facetConfig);

            var fakeQuerySetup = FakeCountOrContentQuerySetup(facetsConfig, pickCriteria);

            // Act
            var result = new GeoPolygonCategoryInfoSqlCompiler().Compile(fakeQuerySetup, facet, null);

            // Assert
            var matcher = new GeoPolygonCategoryCountSqlCompilerMatcher();
            var match = matcher.Match(result);
            Assert.True(match.Success);
        }
    }
}
