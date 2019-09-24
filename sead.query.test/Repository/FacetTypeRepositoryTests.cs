using Moq;
using SeadQueryCore;
using SeadQueryInfra;
using SeadQueryTest.Infrastructure.Scaffolding;
using System;
using Xunit;

namespace SeadQueryTest.Repository
{
    public class FacetTypeRepositoryTests : IDisposable
    {
        private IFacetContext mockFacetContext;

        public FacetTypeRepositoryTests()
        {
            this.mockFacetContext = ScaffoldUtility.DefaultFacetContext();
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
