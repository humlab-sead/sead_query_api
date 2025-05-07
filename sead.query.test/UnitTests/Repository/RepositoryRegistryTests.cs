using Xunit;

namespace SQT.Infrastructure.Repository
{
    [Collection("UsePostgresDockerSession")]
    public class RepositoryRegistryTests : MockerWithFacetContext
    {
        public RepositoryRegistryTests() : base()
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
