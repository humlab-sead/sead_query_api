using SeadQueryCore;
using SeadQueryCore.Plugin.Intersect;
using Xunit;
using SQT.Infrastructure;

namespace SQT.Plugins.Range
{
    [Collection("SeadJsonFacetContextFixture")]
    public class OuterBoundSqlCompilerTests(SeadJsonFacetContextFixture fixture) : MockerWithFacetContext(fixture)
    {
        [Theory]
        [InlineData("analysis_entity_ages:analysis_entity_ages")]
        public void Compile_OuterBound_Succeeds(string uri)
        {
            // Arrange
            var fakeFacetsConfig = FakeFacetsConfig(uri);
            var fakeQuerySetup = FakeCountOrContentQuerySetup(fakeFacetsConfig);

            // Act
            var sqlCompiler = new IntersectOuterBoundSqlCompiler();
            var result = sqlCompiler.Compile(null /* fakeQuerySetup not used */, fakeQuerySetup.Facet);

            // Assert
            const string expectedSql = @"select min\(lower\(.+\)\)::.+ as lower, max\(upper\(.+\)\)::.+ as upper from .+";
            Assert.Matches(expectedSql, result.Squeeze().ToLower());
        }
    }
}
