using SeadQueryCore;
using SeadQueryCore.Plugin.Range;
using Xunit;
using SQT.Infrastructure;

namespace SQT.Plugins.Range
{
    [Collection("SeadJsonFacetContextFixture")]
    public class RangeOuterBoundSqlCompilerTests(SeadJsonFacetContextFixture fixture) : DisposableFacetContextContainer(fixture)
    {
        [Theory]
        [InlineData("tbl_denormalized_measured_values_33_0:tbl_denormalized_measured_values_33_0")]
        public void Compile_VariousConfigs_ExpectedBehavior(string uri)
        {
            // Arrange
            var fakeFacetsConfig = FakeFacetsConfig(uri);
            var fakeQuerySetup = FakeCountOrContentQuerySetup(fakeFacetsConfig);

            // Act
            var rangeOuterBoundSqlCompiler = new RangeOuterBoundSqlCompiler();
            var result = rangeOuterBoundSqlCompiler.Compile(null /* fakeQuerySetup not used */, fakeQuerySetup.Facet);

            // Assert
            const string expectedSql = "SELECT.*MIN.*MAX.*FROM.*";
            Assert.Matches(expectedSql, result.Squeeze());
        }
    }
}
