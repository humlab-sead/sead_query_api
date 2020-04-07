using Moq;
using SeadQueryCore;
using System;
using Xunit;

namespace SeadQueryTest.QueryBuilder.FieldCompilers
{
    public class TextAggFieldCompilerTests : IDisposable
    {
        private MockRepository mockRepository;

        private Mock<ResultFieldType> mockResultFieldType;

        public TextAggFieldCompilerTests()
        {
            this.mockRepository = new MockRepository(MockBehavior.Strict);

            this.mockResultFieldType = this.mockRepository.Create<ResultFieldType>();
        }

        public void Dispose()
        {
            this.mockRepository.VerifyAll();
        }

        private TextAggFieldCompiler CreateTextAggFieldCompiler()
        {
            return new TextAggFieldCompiler(
                this.mockResultFieldType.Object);
        }

        [Fact(Skip = "Not implemented")]
        public void Compile_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var textAggFieldCompiler = this.CreateTextAggFieldCompiler();
            string expr = null;

            // Act
            var result = textAggFieldCompiler.Compile(
                expr);

            // Assert
            Assert.True(false);
        }
    }
}
