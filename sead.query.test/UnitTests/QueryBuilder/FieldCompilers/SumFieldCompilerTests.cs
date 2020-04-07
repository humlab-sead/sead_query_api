using Moq;
using SeadQueryCore;
using System;
using Xunit;

namespace SeadQueryTest.QueryBuilder.FieldCompilers
{
    public class SumFieldCompilerTests : IDisposable
    {
        private MockRepository mockRepository;

        private Mock<ResultFieldType> mockResultFieldType;

        public SumFieldCompilerTests()
        {
            this.mockRepository = new MockRepository(MockBehavior.Strict);

            this.mockResultFieldType = this.mockRepository.Create<ResultFieldType>();
        }

        public void Dispose()
        {
            this.mockRepository.VerifyAll();
        }

        private SumFieldCompiler CreateSumFieldCompiler()
        {
            return new SumFieldCompiler(
                this.mockResultFieldType.Object);
        }

        [Fact(Skip = "Not implemented")]
        public void Compile_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var sumFieldCompiler = this.CreateSumFieldCompiler();
            string expr = null;

            // Act
            var result = sumFieldCompiler.Compile(
                expr);

            // Assert
            Assert.True(false);
        }
    }
}
