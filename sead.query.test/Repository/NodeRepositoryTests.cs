using Moq;
using SeadQueryCore;
using SeadQueryInfra;
using System;
using Xunit;

namespace SeadQueryTest.Repository
{
    public class NodeRepositoryTests : IDisposable
    {
        private Moq.MockRepository mockRepository;

        private Mock<IFacetContext> mockFacetContext;

        public NodeRepositoryTests()
        {
            this.mockRepository = new Moq.MockRepository(MockBehavior.Strict);

            this.mockFacetContext = this.mockRepository.Create<IFacetContext>();
        }

        public void Dispose()
        {
            this.mockRepository.VerifyAll();
        }

        private NodeRepository CreateNodeRepository()
        {
            return new NodeRepository(
                this.mockFacetContext.Object);
        }

        [Fact]
        public void TestMethod1()
        {
            // Arrange
            var nodeRepository = this.CreateNodeRepository();

            // Act


            // Assert
            Assert.True(false);
        }
    }
}
