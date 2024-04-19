using SeadQueryCore;
using SQT.Infrastructure;
using SQT.SQL.Matcher;
using Xunit;
using System.Diagnostics;

namespace SQT.SqlCompilers
{
    [Collection("SeadJsonFacetContextFixture")]
    public class DiscreteContentSqlCompilerTests : DisposableFacetContextContainer
    {
        public DiscreteContentSqlCompilerTests(SeadJsonFacetContextFixture fixture) : base(fixture)
        {
        }

        [Theory]
        [InlineData("dataset_methods:dataset_methods")]
        [InlineData("sites:sites")]
        [InlineData("country:country/sites")]
        [InlineData("sites:country@57/sites@3")]
        public void Compile_DiscreteContent_Matches(string uri)
        {
            // Arrange
            var fakeFacetsConfig = FakeFacetsConfig(uri);
            var fakeQuerySetup = FakeCountOrContentQuerySetup(fakeFacetsConfig);
            var facet = MockRegistryWithFacetRepository().Object.Facets.GetByCode(fakeFacetsConfig.TargetCode);
            const string textFilter = "";

            // Act
            var result = new DiscreteCategoryInfoSqlCompiler().Compile(fakeQuerySetup, facet, textFilter);

            // Assert
            var matcher = new DiscreteContentSqlCompilerMatcher();
            var match = matcher.Match(result);
            Assert.True(match.Success);
        }

        [Theory]
        [InlineData("sites_polygon:sites_polygon@63.872484,20.093291,63.947006,20.501316,63.878949,20.673213,63.748021,20.252953,63.793983,20.095738")]
        public void Compile_GeoPolygon_Matches(string uri)
        {
            // Arrange
            var fakeFacetsConfig = FakeFacetsConfig(uri);
            var fakeQuerySetup = FakeCountOrContentQuerySetup(fakeFacetsConfig);
            var facet = MockRegistryWithFacetRepository().Object.Facets.GetByCode(fakeFacetsConfig.TargetCode);
            const string textFilter = "";

            // Act
            var result = new DiscreteCategoryInfoSqlCompiler().Compile(fakeQuerySetup, facet, textFilter);

            // Assert
            var matcher = new DiscreteContentSqlCompilerMatcher();
            var match = matcher.Match(result);
            Assert.True(match.Success);
        }

        [Theory]
        [InlineData("construction_type:construction_type")]
        public void Compile_ConstructionType_Matches(string uri)
        {
            // Arrange
            var fakeFacetsConfig = FakeFacetsConfig(uri);
            var fakeQuerySetup = FakeCountOrContentQuerySetup(fakeFacetsConfig);
            var facet = MockRegistryWithFacetRepository().Object.Facets.GetByCode(fakeFacetsConfig.TargetCode);
            const string textFilter = "";

            // Act
            var result = new GeoPolygonCategoryInfoSqlCompiler().Compile(fakeQuerySetup, facet, textFilter);

            // Assert
            var matcher = new DiscreteContentSqlCompilerMatcher();
            var match = matcher.Match(result);
            Assert.True(match.Success);
        }
    }
}
