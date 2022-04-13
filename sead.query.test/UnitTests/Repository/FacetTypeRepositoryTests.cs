using Moq;
using SeadQueryCore;
using SeadQueryInfra;
using SQT.Infrastructure;
using SQT.Mocks;
using System;
using Xunit;

namespace SQT.Infrastructure.Repository
{
    [Collection("JsonSeededFacetContext")]
    public class FacetTypeRepositoryTests : DisposableFacetContextContainer
    {
        public FacetTypeRepositoryTests(JsonFacetContextFixture fixture) : base(fixture)
        {
        }

        private FacetTypeRepository CreateFacetTypeRepository()
        {
            return new FacetTypeRepository(FacetContext);
        }

        [Theory]
        [InlineData(EFacetType.Discrete)]
        [InlineData(EFacetType.Range)]
        public void Find_WhenCalleWithExistingId_ReturnsType(EFacetType facetType)
        {
            // Arrange
            var repository = new Repository<FacetType, EFacetType>(FacetContext);

            // Act
            var result = repository.Get(facetType);

            // Assert
            Assert.Equal(facetType, result.FacetTypeId);
        }
    }
}
