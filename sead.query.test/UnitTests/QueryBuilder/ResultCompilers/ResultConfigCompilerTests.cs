using SQT.Infrastructure;
using Xunit;

namespace SQT.SqlCompilers
{
    [Collection("JsonSeededFacetContext")]
    public class ResultConfigCompilerTests : DisposableFacetContextContainer
    {
        public ResultConfigCompilerTests(JsonSeededFacetContextFixture fixture) : base(fixture)
        {
        }

        [Theory]
        [InlineData("sites:sites", "site_level", "tabular")]
        [InlineData("sites:sites", "site_level", "map")]
        public void Compile_StateUnderTest_ExpectedBehavior(string uri, string aggregateKey, string viewTypeId)
        {
            // Arrange
            var fakeFacetsConfig = FakeFacetsConfig(uri);
            var fakeQuerySetup = FakeQuerySetup(fakeFacetsConfig);
            var fakeResultConfig = FakeResultConfig(aggregateKey, viewTypeId);

            var mockQuerySetupBuilder = MockQuerySetupBuilder(fakeQuerySetup);
            var mockResultSqlCompilerLocator = MockResultSqlCompilerLocator("#SQL#");

            string facetCode = fakeFacetsConfig.TargetCode;

            // Act
            var resultQueryCompiler = new SeadQueryCore.ResultConfigCompiler(
                Registry,
                mockQuerySetupBuilder.Object,
                mockResultSqlCompilerLocator.Object
            );

            var result = resultQueryCompiler.Compile(fakeFacetsConfig, fakeResultConfig, facetCode);

            // Assert
            Assert.NotEmpty(result);

            // MAP
            var expectedSql = "#SQL#";

            Assert.Equal(expectedSql, result);

        }

    }
}
