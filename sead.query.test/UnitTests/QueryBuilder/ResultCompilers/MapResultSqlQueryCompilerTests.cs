using Moq;
using SeadQueryCore;
using SeadQueryCore.QueryBuilder;
using SeadQueryInfra;
using SeadQueryTest.Infrastructure;
using SeadQueryTest.Mocks;
using System;
using Xunit;

namespace SeadQueryTest.QueryBuilder.ResultCompilers
{
    public class MapResultSqlQueryCompilerTests : DisposableFacetContextContainer
    {
        public MapResultSqlQueryCompilerTests(JsonSeededFacetContextFixture fixture) : base(fixture)
        {
        }

        private MapResultSqlQueryCompiler CreateMapResultSqlQueryCompiler()
        {
            return new MapResultSqlQueryCompiler();
        }

        [Fact(Skip ="Not implemented")]
        public void Compile_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var mapResultSqlQueryCompiler = this.CreateMapResultSqlQueryCompiler();
            QuerySetup query = null;
            Facet facet = null;
            ResultQuerySetup config = null;

            // Act
            var result = mapResultSqlQueryCompiler.Compile(
                query,
                facet,
                config);

            // Assert
            Assert.True(false);
        }
    }
}
