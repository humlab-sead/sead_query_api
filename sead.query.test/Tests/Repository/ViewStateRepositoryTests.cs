using SeadQueryCore;
using SeadQueryInfra;
using SeadQueryTest.Mocks;
using System;
using Xunit;

namespace SeadQueryTest.Repository
{
    public class ViewStateRepositoryTests : IDisposable
    {
        private IFacetContext mockFacetContext;

        public ViewStateRepositoryTests()
        {
            this.mockFacetContext = JsonSeededFacetContextFactory.Create();
        }

        public void Dispose()
        {
            mockFacetContext.Dispose();
        }

        private ViewStateRepository CreateRepository()
        {
            return new ViewStateRepository(this.mockFacetContext);
        }

        [Fact]
        public void Find_WhenCalleWithExistingId_ReturnsType()
        {
            // Arrange
            var repository = this.CreateRepository();
            var key = "key";
            var data = "data";

            repository.Add(new ViewState() { Key = key, Data = data });

            mockFacetContext.SaveChanges();

            // Act
            var result = repository.Get(key);

            // Assert
            Assert.Equal(data, result.Data);
        }
    }
}
