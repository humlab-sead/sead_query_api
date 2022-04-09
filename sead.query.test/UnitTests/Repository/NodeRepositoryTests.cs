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
    public class NodeRepositoryTests : DisposableFacetContextContainer
    {
         public NodeRepositoryTests(SeadJsonFacetContextFixture fixture) : base(fixture)
        {
        }

        private TableRepository CreateRepository()
        {
            return new TableRepository(FacetContext);
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
