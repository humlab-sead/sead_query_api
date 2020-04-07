using Moq;
using SeadQueryCore;
using SeadQueryCore.QueryBuilder;
using System;
using Xunit;

namespace SeadQueryTest.QueryBuilder.DiscreteCompilers
{
    public class DiscreteCategoryCountSqlQueryCompilerTests : IDisposable
    {
        private MockRepository mockRepository;



        public DiscreteCategoryCountSqlQueryCompilerTests()
        {
            this.mockRepository = new MockRepository(MockBehavior.Strict);


        }

        public void Dispose()
        {
            this.mockRepository.VerifyAll();
        }

        private DiscreteCategoryCountSqlQueryCompiler CreateDiscreteCategoryCountSqlQueryCompiler()
        {
            return new DiscreteCategoryCountSqlQueryCompiler();
        }

        [Fact(Skip = "Not implemented")]
        public void Compile_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var discreteCategoryCountSqlQueryCompiler = this.CreateDiscreteCategoryCountSqlQueryCompiler();
            QuerySetup query = null;
            Facet facet = null;
            Facet countFacet = null;
            string aggType = null;

            // Act
            var result = discreteCategoryCountSqlQueryCompiler.Compile(
                query,
                facet,
                countFacet,
                aggType);

            // Assert
            Assert.True(false);
        }
    }
}
