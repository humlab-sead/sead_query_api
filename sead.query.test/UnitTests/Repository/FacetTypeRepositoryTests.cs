using Moq;
using SeadQueryCore;
using SeadQueryInfra;
using SeadQueryTest.Infrastructure;
using SeadQueryTest.Mocks;
using System;
using Xunit;

namespace SeadQueryTest.Repository
{
    public class FacetTypeRepositoryTests : DisposableFacetContextContainer
    {
        public FacetTypeRepositoryTests(JsonSeededFacetContextFixture fixture) : base(fixture)
        {
        }

        private FacetTypeRepository CreateFacetTypeRepository()
        {
            return new FacetTypeRepository(FacetContext);
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
