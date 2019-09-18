using Moq;
using SeadQueryCore;
using SeadQueryCore.QueryBuilder;
using System;
using Xunit;

namespace SeadQueryTest.QueryBuilder.RangeCompilers
{
    public class RangeCategoryBoundSqlQueryCompilerTests : IDisposable
    {
        private MockRepository mockRepository;



        public RangeCategoryBoundSqlQueryCompilerTests()
        {
            this.mockRepository = new MockRepository(MockBehavior.Strict);


        }

        public void Dispose()
        {
            this.mockRepository.VerifyAll();
        }

        private RangeCategoryBoundSqlQueryCompiler CreateRangeCategoryBoundSqlQueryCompiler()
        {
            return new RangeCategoryBoundSqlQueryCompiler();
        }

        [Fact]
        public void Compile_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var rangeCategoryBoundSqlQueryCompiler = this.CreateRangeCategoryBoundSqlQueryCompiler();
            QuerySetup query = null;
            Facet facet = null;
            string facetCode = null;

            // Act
            var result = rangeCategoryBoundSqlQueryCompiler.Compile(
                query,
                facet,
                facetCode);

            // Assert
            Assert.True(false);
        }
    }
}
