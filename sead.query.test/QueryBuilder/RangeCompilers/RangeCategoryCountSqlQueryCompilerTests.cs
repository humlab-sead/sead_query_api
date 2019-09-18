using Moq;
using SeadQueryCore;
using SeadQueryCore.QueryBuilder;
using System;
using Xunit;

namespace SeadQueryTest.QueryBuilder.RangeCompilers
{
    public class RangeCategoryCountSqlQueryCompilerTests : IDisposable
    {
        private MockRepository mockRepository;



        public RangeCategoryCountSqlQueryCompilerTests()
        {
            this.mockRepository = new MockRepository(MockBehavior.Strict);


        }

        public void Dispose()
        {
            this.mockRepository.VerifyAll();
        }

        private RangeCategoryCountSqlQueryCompiler CreateRangeCategoryCountSqlQueryCompiler()
        {
            return new RangeCategoryCountSqlQueryCompiler();
        }

        [Fact]
        public void Compile_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var rangeCategoryCountSqlQueryCompiler = this.CreateRangeCategoryCountSqlQueryCompiler();
            QuerySetup query = null;
            Facet facet = null;
            string intervalQuery = null;
            string countColumn = null;

            // Act
            var result = rangeCategoryCountSqlQueryCompiler.Compile(
                query,
                facet,
                intervalQuery,
                countColumn);

            // Assert
            Assert.True(false);
        }
    }
}
