using SeadQueryCore;
using SeadQueryInfra;
using Xunit;

namespace SQT.Infrastructure.Repository
{
    [Collection("SqliteFacetContext")]
    public class FacetTypeRepositoryTests : MockerWithFacetContext
    {
        public FacetTypeRepositoryTests(SqliteFacetContext facetContext) : base(facetContext)
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
