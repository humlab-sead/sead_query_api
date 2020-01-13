using Moq;
using SeadQueryCore;
using System;
using Xunit;

namespace SeadQueryTest.QueryBuilder.FieldCompilers
{
    public class SqlFieldCompilerTests : IDisposable
    {
        private MockRepository mockRepository;

        private Mock<ResultFieldType> mockResultFieldType;

        public SqlFieldCompilerTests()
        {
            this.mockRepository = new MockRepository(MockBehavior.Strict);

            this.mockResultFieldType = this.mockRepository.Create<ResultFieldType>();
        }

        public void Dispose()
        {
            this.mockRepository.VerifyAll();
        }

        private SqlFieldCompiler CreateSqlFieldCompiler()
        {
            return new SqlFieldCompiler(
                this.mockResultFieldType.Object);
        }

        [Fact]
        public void Compile_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var sqlFieldCompiler = this.CreateSqlFieldCompiler();
            string expr = null;

            // Act
            var result = sqlFieldCompiler.Compile(
                expr);

            // Assert
            Assert.True(false);
        }
    }
}
