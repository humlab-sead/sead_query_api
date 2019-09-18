using Moq;
using SeadQueryCore;
using SeadQueryCore.QueryBuilder;
using System;
using Xunit;

namespace SeadQueryTest.QueryBuilder.ResultCompilers
{
    public class MapResultSqlQueryCompilerTests : IDisposable
    {
        private MockRepository mockRepository;



        public MapResultSqlQueryCompilerTests()
        {
            this.mockRepository = new MockRepository(MockBehavior.Strict);


        }

        public void Dispose()
        {
            this.mockRepository.VerifyAll();
        }

        private MapResultSqlQueryCompiler CreateMapResultSqlQueryCompiler()
        {
            return new MapResultSqlQueryCompiler();
        }

        [Fact]
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
