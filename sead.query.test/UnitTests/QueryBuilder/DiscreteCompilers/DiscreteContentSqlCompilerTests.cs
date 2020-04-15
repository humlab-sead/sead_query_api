using Moq;
using SeadQueryCore;
using SeadQueryCore.QueryBuilder;
using SeadQueryTest.Infrastructure;
using SeadQueryTest.Mocks;
using System;
using Xunit;

namespace SeadQueryTest.QueryBuilder.DiscreteCompilers
{
    [Collection("JsonSeededFacetContext")]
    public class DiscreteContentSqlCompilerTests : DisposableFacetContextContainer
    {
        public DiscreteContentSqlCompilerTests(JsonSeededFacetContextFixture fixture) : base(fixture)
        {
        }

        [Theory]
        [InlineData("sites:sites")]
        [InlineData("country:country")]
        [InlineData("ecocode:ecocode")]
        public void Compile_StateUnderTest_ExpectedBehavior(string uri)
        {
            // Arrange
            var facetsConfig = new MockFacetsConfigFactory(Registry.Facets).Create(uri);
            QuerySetup query = null;
            Facet facet = null;
            string text_filter = null;

            // Act
            var discreteContentSqlQueryBuilder = new DiscreteContentSqlCompiler();

            var result = discreteContentSqlQueryBuilder.Compile(
                query,
                facet,
                text_filter);

            // Assert
            Assert.True(false);
        }
    }
}
