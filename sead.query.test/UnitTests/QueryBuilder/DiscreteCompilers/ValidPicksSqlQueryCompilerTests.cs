using Moq;
using SeadQueryCore;
using SeadQueryCore.QueryBuilder;
using System;
using System.Collections.Generic;
using Xunit;

namespace SeadQueryTest.QueryBuilder.DiscreteCompilers
{
    public class ValidPicksSqlCompilerTests : IDisposable
    {
        private MockRepository mockRepository;



        public ValidPicksSqlCompilerTests()
        {
            this.mockRepository = new MockRepository(MockBehavior.Strict);


        }

        public void Dispose()
        {
            this.mockRepository.VerifyAll();
        }

        private ValidPicksSqCompiler CreateValidPicksSqlCompiler()
        {
            return new ValidPicksSqCompiler();
        }

        [Fact(Skip = "Not implemented")]
        public void Compile_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var validPicksSqlCompiler = this.CreateValidPicksSqlCompiler();
            QuerySetup query = null;
            Facet facet = null;
            List<int> picks = null;

            // Act
            var result = validPicksSqlCompiler.Compile(
                query,
                facet,
                picks);

            // Assert
            Assert.True(false);
        }
    }
}
