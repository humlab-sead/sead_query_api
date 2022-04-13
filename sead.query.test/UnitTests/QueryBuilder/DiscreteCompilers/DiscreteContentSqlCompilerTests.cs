using SeadQueryCore;
using SQT.Infrastructure;
using SQT.SQL.Matcher;
using Xunit;

namespace SQT.SqlCompilers
{


    [Collection("JsonSeededFacetContext")]
    public class DiscreteContentSqlCompilerTests : DisposableFacetContextContainer
    {
        public DiscreteContentSqlCompilerTests(JsonFacetContextFixture fixture) : base(fixture)
        {
        }

        [Theory]
        [InlineData("dataset_methods:dataset_methods")]
        [InlineData("sites:sites")]
        [InlineData("country:country/sites")]
        [InlineData("sites:country@57/sites@3")]
        public void Compile_StateUnderTest_ExpectedBehavior(string uri)
        {

            // Arrange
            var fakeFacetsConfig = FakeFacetsConfig(uri);
            var fakeQuerySetup = FakeCountOrContentQuerySetup(fakeFacetsConfig);
            var facet = MockRegistryWithFacetRepository().Object.Facets.GetByCode(fakeFacetsConfig.TargetCode);
            string textFilter = "";

            // Act
            var result = new DiscreteContentSqlCompiler().Compile(fakeQuerySetup, facet, textFilter);

            // Assert
            var matcher = new DiscreteContentSqlCompilerMatcher();
            var match = matcher.Match(result);
            Assert.True(match.Success);

        }
    }
}
