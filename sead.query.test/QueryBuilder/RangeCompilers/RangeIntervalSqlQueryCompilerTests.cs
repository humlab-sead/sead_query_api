using Moq;
using SeadQueryCore;
using System;
using Xunit;

namespace SeadQueryTest.QueryBuilder.RangeCompilers
{
    public class RangeIntervalSqlQueryCompilerTests : IDisposable
    {
        private MockRepository mockRepository;



        public RangeIntervalSqlQueryCompilerTests()
        {
            this.mockRepository = new MockRepository(MockBehavior.Strict);


        }

        public void Dispose()
        {
            this.mockRepository.VerifyAll();
        }

        private RangeIntervalSqlQueryCompiler CreateRangeIntervalSqlQueryCompiler()
        {
            return new RangeIntervalSqlQueryCompiler();
        }

        [Fact]
        public void Compile_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var rangeIntervalSqlQueryCompiler = this.CreateRangeIntervalSqlQueryCompiler();
            int interval = 0;
            int min = 0;
            int max = 0;
            int interval_count = 0;

            // Act
            var result = rangeIntervalSqlQueryCompiler.Compile(
                interval,
                min,
                max,
                interval_count);

            // Assert
            Assert.True(false);
        }
    }
}
