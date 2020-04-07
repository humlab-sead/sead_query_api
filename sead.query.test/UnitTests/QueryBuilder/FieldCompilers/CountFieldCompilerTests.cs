using Moq;
using SeadQueryCore;
using System;
using Xunit;

namespace SeadQueryTest.QueryBuilder.FieldCompilers
{
    public class CountFieldCompilerTests : IDisposable
    {
        private MockRepository mockRepository;

        private Mock<ResultFieldType> mockResultFieldType;

        public CountFieldCompilerTests()
        {
            this.mockRepository = new MockRepository(MockBehavior.Strict);

            this.mockResultFieldType = this.mockRepository.Create<ResultFieldType>();
        }

        public void Dispose()
        {
            this.mockRepository.VerifyAll();
        }

        private CountFieldCompiler CreateCountFieldCompiler()
        {
            return new CountFieldCompiler(
                this.mockResultFieldType.Object);
        }

        [Fact(Skip = "Not implemented")]
        public void Compile_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var countFieldCompiler = this.CreateCountFieldCompiler();
            string expr = null;

            // Act
            var result = countFieldCompiler.Compile(
                expr);

            // Assert
            Assert.True(false);
        }
    }
}
