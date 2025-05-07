using SeadQueryInfra;
using Xunit;

namespace SQT.Infrastructure.Repository
{
    [Collection("UsePostgresFixture")]
    public class NodeRepositoryTests : MockerWithFacetContext
    {
        public NodeRepositoryTests() : base()
        {
        }

        private NodeRepository CreateRepository()
        {
            return new NodeRepository(Registry);
        }

        [Fact]
        public void Find_WhenCalleWithExistingId_ReturnsType()
        {
            // Arrange
            var repository = this.CreateRepository();

            // Act
            var result = repository.Get(1);

            // Assert
            Assert.Equal(1, result.TableId);
        }
    }
}
