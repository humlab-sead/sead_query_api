using Moq;
using SeadQueryCore;
using SeadQueryCore.QueryBuilder;
using System;
using System.Collections.Generic;
using Xunit;

namespace SeadQueryTest.QueryBuilder.DiscreteCompilers
{
    public class ValidPicksSqlQueryCompilerTests : IDisposable
    {
        private MockRepository mockRepository;



        public ValidPicksSqlQueryCompilerTests()
        {
            this.mockRepository = new MockRepository(MockBehavior.Strict);


        }

        public void Dispose()
        {
            this.mockRepository.VerifyAll();
        }

        private ValidPicksSqCompiler CreateValidPicksSqlQueryCompiler()
        {
            return new ValidPicksSqCompiler();
        }

        [Fact(Skip = "Not implemented")]
        public void Compile_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var validPicksSqlQueryCompiler = this.CreateValidPicksSqlQueryCompiler();
            QuerySetup query = null;
            Facet facet = null;
            List<int> picks = null;

            // Act
            var result = validPicksSqlQueryCompiler.Compile(
                query,
                facet,
                picks);

            // Assert
            Assert.True(false);
        }
    }
}
