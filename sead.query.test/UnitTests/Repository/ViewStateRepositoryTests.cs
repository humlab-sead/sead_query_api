using SeadQueryCore;
using SeadQueryInfra;
using SeadQueryTest.Infrastructure;
using SeadQueryTest.Mocks;
using System;
using Xunit;

namespace SeadQueryTest.Repository
{
    [Collection("JsonSeededFacetContext")]
    public class ViewStateRepositoryTests : DisposableFacetContextContainer
    {
        public ViewStateRepositoryTests(JsonSeededFacetContextFixture fixture) : base(fixture)
        {
        }

        private ViewStateRepository CreateRepository()
        {
            return new ViewStateRepository(FacetContext);
        }

        [Fact]
        public void Find_WhenCalleWithExistingId_ReturnsType()
        {
            // Arrange
            var repository = this.CreateRepository();
            var key = "key";
            var data = "data";

            repository.Add(new ViewState() { Key = key, Data = data });

            FacetContext.SaveChanges();

            // Act
            var result = repository.Get(key);

            // Assert
            Assert.Equal(data, result.Data);
        }
    }
}
