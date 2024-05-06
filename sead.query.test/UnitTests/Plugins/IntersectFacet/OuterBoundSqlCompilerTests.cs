using SeadQueryCore;
using SeadQueryCore.Plugin.Intersect;
using Xunit;
using SQT.Infrastructure;

namespace SQT.Plugins.Range
{
    [Collection("SeadJsonFacetContextFixture")]
    public class OuterBoundSqlCompilerTests(SeadJsonFacetContextFixture fixture) : DisposableFacetContextContainer(fixture)
    {
        [Theory]
        [InlineData("analysis_ages:analysis_ages")]
        public void Compile_OuterBound_Succeeds(string uri)
        {
            // Arrange
            var fakeFacetsConfig = FakeFacetsConfig(uri);
            var fakeQuerySetup = FakeCountOrContentQuerySetup(fakeFacetsConfig);

            // Act
            var sqlCompiler = new IntersectOuterBoundSqlCompiler();
            var result = sqlCompiler.Compile(null /* fakeQuerySetup not used */, fakeQuerySetup.Facet);

            // Assert
            const string expectedSql = "SELECT.*MIN.*MAX.*FROM.*";
            Assert.Matches(expectedSql, result.Squeeze());
        }
    }
}
