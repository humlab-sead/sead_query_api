using Moq;
using SeadQueryCore;
using System;
using Xunit;

namespace SeadQueryTest.QueryBuilder.FieldCompilers
{
    public class TemplateFieldCompilerTests : IDisposable
    {
        private MockRepository mockRepository;

        private Mock<ResultFieldType> mockResultFieldType;

        public TemplateFieldCompilerTests()
        {
            this.mockRepository = new MockRepository(MockBehavior.Strict);

            this.mockResultFieldType = this.mockRepository.Create<ResultFieldType>();
        }

        public void Dispose()
        {
            this.mockRepository.VerifyAll();
        }

        private TemplateFieldCompiler CreateTemplateFieldCompiler()
        {
            return new TemplateFieldCompiler(
                this.mockResultFieldType.Object);
        }

        [Fact]
        public void Compile_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var templateFieldCompiler = this.CreateTemplateFieldCompiler();
            string expr = null;

            // Act
            var result = templateFieldCompiler.Compile(
                expr);

            // Assert
            Assert.True(false);
        }
    }
}
