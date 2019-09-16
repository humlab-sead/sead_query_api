using Moq;
using SeadQueryCore;
using System;
using Xunit;

namespace SeadQueryTest.Model.Entities
{
    public class FacetTableTests : IDisposable
    {
        private Moq.MockRepository mockRepository;



        public FacetTableTests()
        {
            this.mockRepository = new Moq.MockRepository(MockBehavior.Strict);


        }

        public void Dispose()
        {
            this.mockRepository.VerifyAll();
        }

        private FacetTable CreateFacetTable()
        {
            return new FacetTable();
        }

        [Fact]
        public void TestMethod1()
        {
            // Arrange
            var facetTable = this.CreateFacetTable();

            // Act


            // Assert
            Assert.True(false);
        }
    }
}
