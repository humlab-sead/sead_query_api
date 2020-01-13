using Moq;
using SeadQueryCore;
using System;
using Xunit;

namespace SeadQueryTest.QueryBuilder.FieldCompilers
{
    public class AvgFieldCompilerTests : IDisposable
    {
        private MockRepository mockRepository;

        private Mock<ResultFieldType> mockResultFieldType;

        public AvgFieldCompilerTests()
        {
            this.mockRepository = new MockRepository(MockBehavior.Strict);

            this.mockResultFieldType = this.mockRepository.Create<ResultFieldType>();
        }

        public void Dispose()
        {
            this.mockRepository.VerifyAll();
        }

        private AvgFieldCompiler CreateAvgFieldCompiler()
        {
            return new AvgFieldCompiler(
                this.mockResultFieldType.Object);
        }

        [Fact]
        public void Compile_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var avgFieldCompiler = this.CreateAvgFieldCompiler();
            string expr = null;

            // Act
            var result = avgFieldCompiler.Compile(
                expr);

            // Assert
            Assert.True(false);
        }
    }
}
