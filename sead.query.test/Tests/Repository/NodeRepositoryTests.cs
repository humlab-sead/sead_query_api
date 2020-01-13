using Moq;
using SeadQueryCore;
using SeadQueryInfra;
using SeadQueryTest.Infrastructure.Scaffolding;
using System;
using Xunit;

namespace SeadQueryTest.Repository
{
    public class NodeRepositoryTests : IDisposable
    {
        private IFacetContext mockFacetContext;

        public NodeRepositoryTests()
        {
            this.mockFacetContext = ScaffoldUtility.JsonSeededFacetContext();
        }

        public void Dispose()
        {
        }

        private TableRepository CreateRepository()
        {
            return new TableRepository(this.mockFacetContext);
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
