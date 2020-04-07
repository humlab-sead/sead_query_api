using Moq;
using SeadQueryCore;
using System;
using Xunit;

namespace SeadQueryTest.QueryBuilder.FieldCompilers
{
    public class DefaultFieldCompilerTests : IDisposable
    {
        private MockRepository mockRepository;

        private Mock<ResultFieldType> mockResultFieldType;

        public DefaultFieldCompilerTests()
        {
            this.mockRepository = new MockRepository(MockBehavior.Strict);

            this.mockResultFieldType = this.mockRepository.Create<ResultFieldType>();
        }

        public void Dispose()
        {
            this.mockRepository.VerifyAll();
        }

        private DefaultFieldCompiler CreateDefaultFieldCompiler()
        {
            return new DefaultFieldCompiler(
                this.mockResultFieldType.Object);
        }

        [Fact(Skip = "Not implemented")]
        public void Compile_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var defaultFieldCompiler = this.CreateDefaultFieldCompiler();
            string expr = null;

            // Act
            var result = defaultFieldCompiler.Compile(
                expr);

            // Assert
            Assert.True(false);
        }
    }
}
