using Moq;
using SeadQueryCore;
using System;
using Xunit;

namespace SeadQueryTest.Model.Entities
{
    public class FacetTypeTests : IDisposable
    {
        private Moq.MockRepository mockRepository;



        public FacetTypeTests()
        {
            this.mockRepository = new Moq.MockRepository(MockBehavior.Strict);


        }

        public void Dispose()
        {
            this.mockRepository.VerifyAll();
        }

        private FacetType CreateFacetType()
        {
            return new FacetType();
        }

        [Fact]
        public void TestMethod1()
        {
            // Arrange
            var facetType = this.CreateFacetType();

            // Act


            // Assert
            Assert.True(false);
        }
    }
}
