using Moq;
using SeadQueryCore;
using SeadQueryCore.QueryBuilder;
using System;
using Xunit;

namespace SQT.QueryBuilder.ResultCompilers
{
    public class TabularResultSqlCompilerTests : IDisposable
    {
        private MockRepository mockRepository;



        public TabularResultSqlCompilerTests()
        {
            this.mockRepository = new MockRepository(MockBehavior.Strict);


        }

        public void Dispose()
        {
            this.mockRepository.VerifyAll();
        }

        private TabularResultSqlCompiler CreateTabularResultSqlCompiler()
        {
            return new TabularResultSqlCompiler();
        }

        [Fact(Skip = "Not implemented")]
        public void Compile_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var tabularResultSqlCompiler = this.CreateTabularResultSqlCompiler();
            QuerySetup query = null;
            Facet facet = null;
            ResultQuerySetup config = null;

            // Act
            var result = tabularResultSqlCompiler.Compile(
                query,
                facet,
                config);

            // Assert
            Assert.True(false);
        }
    }
}
