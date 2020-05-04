using SeadQueryCore;
using SQT.Infrastructure;
using SQT.SQL.Matcher;
using Xunit;

namespace SQT.SqlCompilers
{
    [Collection("JsonSeededFacetContext")]
    public class ValidPicksSqlQueryCompilerTests : DisposableFacetContextContainer
    {
        public ValidPicksSqlQueryCompilerTests(JsonSeededFacetContextFixture fixture) : base(fixture)
        {
        }

        [Theory]
        [InlineData("sites:sites@5")]
        [InlineData("sites:country@57/sites@3")]
        [InlineData("country:country@57/sites@3")]
        public void Compile_VariousConfigs_ExpectedBehavior(string uri)
        {
            // Arrange
            var fakeFacetsConfig = FakeFacetsConfig(uri);
            var fakeQuerySetup = FakeCountOrContentQuerySetup(fakeFacetsConfig);

            // Act
            var compiler = new ValidPicksSqCompiler();
            var result = compiler.Compile(
                fakeQuerySetup,
                fakeFacetsConfig.TargetConfig.GetIntegerPickValues()
            );

            // Assert
            var matcher = new ValidPicksSqlCompilerMatcher();
            var match = matcher.Match(result);

            Assert.True(match.Success);
        }
    }
}
