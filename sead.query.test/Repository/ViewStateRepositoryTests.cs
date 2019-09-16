using Moq;
using SeadQueryCore;
using SeadQueryInfra;
using System;
using Xunit;

namespace SeadQueryTest.Repository
{
    public class ViewStateRepositoryTests : IDisposable
    {
        private Moq.MockRepository mockRepository;

        private Mock<IFacetContext> mockFacetContext;

        public ViewStateRepositoryTests()
        {
            this.mockRepository = new Moq.MockRepository(MockBehavior.Strict);

            this.mockFacetContext = this.mockRepository.Create<IFacetContext>();
        }

        public void Dispose()
        {
            this.mockRepository.VerifyAll();
        }

        private ViewStateRepository CreateViewStateRepository()
        {
            return new ViewStateRepository(
                this.mockFacetContext.Object);
        }

        [Fact]
        public void TestMethod1()
        {
            // Arrange
            var viewStateRepository = this.CreateViewStateRepository();

            // Act


            // Assert
            Assert.True(false);
        }
    }
}
