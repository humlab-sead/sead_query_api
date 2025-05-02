using Xunit;

namespace SQT.Infrastructure.Repository
{
    [Collection("SeadJsonFacetContextFixture")]
    public class RepositoryRegistryTests : MockerWithJsonFacetContext
    {
        public RepositoryRegistryTests(SeadJsonFacetContextFixture fixture) : base(fixture)
        {
        }

        //[Fact]
        //public void GetRepositoryTypes_LoadsTypes()
        //{
        //    // Arrange
        //    var registry = new RepositoryRegistry(FacetContext);
        //    // Act
        //    var result = registry.GetRepositoryTypes().ToList();

        //    // Assert
        //    Assert.NotNull(result);
        //    Assert.True(result.Any());
        //}

    }
}
