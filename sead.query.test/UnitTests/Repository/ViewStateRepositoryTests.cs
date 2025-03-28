using SeadQueryCore;
using SeadQueryInfra;
using SQT.Infrastructure;
using SQT.Mocks;
using System;
using Xunit;

namespace SQT.Infrastructure.Repository
{
    [Collection("SeadJsonFacetContextFixture")]
    public class ViewStateRepositoryTests : DisposableFacetContextContainer
    {
        public ViewStateRepositoryTests(SeadJsonFacetContextFixture fixture) : base(fixture)
        {
        }

        private ViewStateRepository CreateRepository()
        {
            return new ViewStateRepository(Registry);
        }

        [Fact]
        public void Find_WhenCalleWithExistingId_ReturnsType()
        {
            // Arrange
            var repository = this.CreateRepository();
            const string key = "key";
            const string data = "data";

            repository.Add(new ViewState() { Key = key, Data = data });

            FacetContext.SaveChanges();

            // Act
            var result = repository.Get(key);

            // Assert
            Assert.Equal(data, result.Data);
        }
    }
}
