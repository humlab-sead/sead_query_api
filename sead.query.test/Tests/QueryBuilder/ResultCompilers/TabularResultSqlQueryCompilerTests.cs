using Moq;
using SeadQueryCore;
using SeadQueryCore.QueryBuilder;
using System;
using Xunit;

namespace SeadQueryTest.QueryBuilder.ResultCompilers
{
    public class TabularResultSqlQueryCompilerTests : IDisposable
    {
        private MockRepository mockRepository;



        public TabularResultSqlQueryCompilerTests()
        {
            this.mockRepository = new MockRepository(MockBehavior.Strict);


        }

        public void Dispose()
        {
            this.mockRepository.VerifyAll();
        }

        private TabularResultSqlQueryCompiler CreateTabularResultSqlQueryCompiler()
        {
            return new TabularResultSqlQueryCompiler();
        }

        [Fact]
        public void Compile_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var tabularResultSqlQueryCompiler = this.CreateTabularResultSqlQueryCompiler();
            QuerySetup query = null;
            Facet facet = null;
            ResultQuerySetup config = null;

            // Act
            var result = tabularResultSqlQueryCompiler.Compile(
                query,
                facet,
                config);

            // Assert
            Assert.True(false);
        }
    }
}
