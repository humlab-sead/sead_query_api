using Moq;
using SeadQueryCore;
using SeadQueryInfra;
using SeadQueryTest.Mocks;
using System;
using Xunit;

namespace SeadQueryTest.Repository
{
    public class FacetTypeRepositoryTests : IDisposable
    {
        private IFacetContext mockFacetContext;

        public FacetTypeRepositoryTests()
        {
            this.mockFacetContext = JsonSeededFacetContextFactory.Create();
        }

        public void Dispose()
        {
        }

        private FacetTypeRepository CreateFacetTypeRepository()
        {
            return new FacetTypeRepository(this.mockFacetContext);
        }

        [Fact]
        public void Find_WhenCalleWithExistingId_ReturnsType()
        {
            // Arrange
            var facetTypeRepository = this.CreateFacetTypeRepository();

            // Act
            var result = facetTypeRepository.Get(1);

            // Assert
            Assert.Equal(EFacetType.Discrete, result.FacetTypeId);
        }
    }
}
