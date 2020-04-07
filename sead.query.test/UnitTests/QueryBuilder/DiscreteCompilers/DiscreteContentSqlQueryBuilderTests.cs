using Moq;
using SeadQueryCore;
using SeadQueryCore.QueryBuilder;
using System;
using Xunit;

namespace SeadQueryTest.QueryBuilder.DiscreteCompilers
{
    public class DiscreteContentSqlQueryBuilderTests : IDisposable
    {
        private MockRepository mockRepository;



        public DiscreteContentSqlQueryBuilderTests()
        {
            this.mockRepository = new MockRepository(MockBehavior.Strict);


        }

        public void Dispose()
        {
            this.mockRepository.VerifyAll();
        }

        private DiscreteContentSqlQueryBuilder CreateDiscreteContentSqlQueryBuilder()
        {
            return new DiscreteContentSqlQueryBuilder();
        }

        [Fact(Skip = "Not implemented")]
        public void Compile_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var discreteContentSqlQueryBuilder = this.CreateDiscreteContentSqlQueryBuilder();
            QuerySetup query = null;
            Facet facet = null;
            string text_filter = null;

            // Act
            var result = discreteContentSqlQueryBuilder.Compile(
                query,
                facet,
                text_filter);

            // Assert
            Assert.True(false);
        }
    }
}
