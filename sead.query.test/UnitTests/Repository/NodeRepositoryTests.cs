using Moq;
using SeadQueryCore;
using SeadQueryInfra;
using SeadQueryTest.Infrastructure;
using SeadQueryTest.Mocks;
using System;
using Xunit;

namespace SeadQueryTest.Repository
{
    [Collection("JsonSeededFacetContext")]
    public class NodeRepositoryTests : DisposableFacetContextContainer
    {
         public NodeRepositoryTests(JsonSeededFacetContextFixture fixture) : base(fixture)
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
