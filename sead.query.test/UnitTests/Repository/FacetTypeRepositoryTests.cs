using Moq;
using SeadQueryCore;
using SeadQueryInfra;
using SQT.Infrastructure;
using SQT.Mocks;
using System;
using Xunit;

namespace SQT.Infrastructure.Repository
{
    [Collection("SeadJsonFacetContextFixture")]
    public class FacetTypeRepositoryTests : MockerWithFacetContext
    {
        public FacetTypeRepositoryTests(SeadJsonFacetContextFixture fixture) : base(fixture)
        {
        }

        // private FacetTypeRepository CreateFacetTypeRepository()
        // {
        //     return new FacetTypeRepository(FacetContext);
        // }

        [Theory]
        [InlineData(EFacetType.Discrete)]
        [InlineData(EFacetType.Range)]
        public void Find_WhenCalleWithExistingId_ReturnsType(EFacetType facetType)
        {
            // Arrange
            var repository = new Repository<FacetType, EFacetType>(Registry);

            // Act
            var result = repository.Get(facetType);

            // Assert
            Assert.Equal(facetType, result.FacetTypeId);
        }
    }
}
