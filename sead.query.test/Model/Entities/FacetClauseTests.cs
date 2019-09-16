using Moq;
using SeadQueryCore;
using System;
using Xunit;

namespace SeadQueryTest.Model.Entities
{
    public class FacetClauseTests : IDisposable
    {
        private Moq.MockRepository mockRepository;



        public FacetClauseTests()
        {
            this.mockRepository = new Moq.MockRepository(MockBehavior.Strict);


        }

        public void Dispose()
        {
            this.mockRepository.VerifyAll();
        }

        private FacetClause CreateFacetClause()
        {
            return new FacetClause();
        }

        [Fact]
        public void TestMethod1()
        {
            // Arrange
            var facetClause = this.CreateFacetClause();

            // Act


            // Assert
            Assert.True(false);
        }
    }
}
