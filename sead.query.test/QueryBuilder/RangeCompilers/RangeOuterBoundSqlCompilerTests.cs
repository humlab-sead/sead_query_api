using Moq;
using SeadQueryCore;
using SeadQueryCore.QueryBuilder;
using System;
using Xunit;

namespace SeadQueryTest.QueryBuilder.RangeCompilers
{
    public class RangeOuterBoundSqlCompilerTests : IDisposable
    {
        private MockRepository mockRepository;



        public RangeOuterBoundSqlCompilerTests()
        {
            this.mockRepository = new MockRepository(MockBehavior.Strict);


        }

        public void Dispose()
        {
            this.mockRepository.VerifyAll();
        }

        private RangeOuterBoundSqlCompiler CreateRangeOuterBoundSqlCompiler()
        {
            return new RangeOuterBoundSqlCompiler();
        }

        [Fact]
        public void Compile_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var rangeOuterBoundSqlCompiler = this.CreateRangeOuterBoundSqlCompiler();
            QuerySetup query = null;
            Facet facet = null;

            // Act
            var result = rangeOuterBoundSqlCompiler.Compile(
                query,
                facet);

            // Assert
            Assert.True(false);
        }
    }
}
