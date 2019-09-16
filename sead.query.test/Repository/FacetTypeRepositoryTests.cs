using Moq;
using SeadQueryCore;
using SeadQueryInfra;
using System;
using Xunit;

namespace SeadQueryTest.Repository
{
    public class FacetTypeRepositoryTests : IDisposable
    {
        private Moq.MockRepository mockRepository;

        private Mock<IFacetContext> mockFacetContext;

        public FacetTypeRepositoryTests()
        {
            this.mockRepository = new Moq.MockRepository(MockBehavior.Strict);

            this.mockFacetContext = this.mockRepository.Create<IFacetContext>();
        }

        public void Dispose()
        {
            this.mockRepository.VerifyAll();
        }

        private FacetTypeRepository CreateFacetTypeRepository()
        {
            return new FacetTypeRepository(
                this.mockFacetContext.Object);
        }

        [Fact]
        public void TestMethod1()
        {
            // Arrange
            var facetTypeRepository = this.CreateFacetTypeRepository();

            // Act


            // Assert
            Assert.True(false);
        }
    }
}
