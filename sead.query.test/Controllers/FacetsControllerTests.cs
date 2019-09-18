using Moq;
using Newtonsoft.Json.Linq;
using SeadQueryAPI.Controllers;
using SeadQueryAPI.Services;
using SeadQueryCore;
using System;
using Xunit;

namespace SeadQueryTest.Controllers
{
    public class FacetsControllerTests : IDisposable
    {
        private MockRepository mockRepository;

        private Mock<IRepositoryRegistry> mockRepositoryRegistry;

        public FacetsControllerTests()
        {
            this.mockRepository = new MockRepository(MockBehavior.Strict);

            this.mockRepositoryRegistry = this.mockRepository.Create<IRepositoryRegistry>();
        }

        public void Dispose()
        {
            this.mockRepository.VerifyAll();
        }

        private FacetsController CreateFacetsController()
        {
            var mockLoadFacetService = new Mock<ILoadFacetService>();

            return new FacetsController(mockRepositoryRegistry.Object, mockLoadFacetService.Object);
        }

        [Fact]
        public void Get_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var facetsController = this.CreateFacetsController();

            // Act
            var result = facetsController.Get();

            // Assert
            Assert.True(false);
        }

        [Fact]
        public void Get_StateUnderTest_ExpectedBehavior1()
        {
            // Arrange
            var facetsController = this.CreateFacetsController();
            int id = 0;

            // Act
            var result = facetsController.Get(
                id);

            // Assert
            Assert.True(false);
        }

        [Fact]
        public void Load_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var facetsController = this.CreateFacetsController();
            FacetsConfig2 facetsConfig = null;

            // Act
            var result = facetsController.Load(
                facetsConfig);

            // Assert
            Assert.True(false);
        }

        [Fact]
        public void Load2_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var facetsController = this.CreateFacetsController();
            JObject json = null;

            // Act
            var result = facetsController.Load2(
                json);

            // Assert
            Assert.True(false);
        }

        [Fact]
        public void Load3_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var facetsController = this.CreateFacetsController();
            JObject json = null;

            // Act
            var result = facetsController.Load3(
                json);

            // Assert
            Assert.True(false);
        }

        [Fact]
        public void Mirror_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var facetsController = this.CreateFacetsController();
            JObject json = null;

            // Act
            var result = facetsController.Mirror(
                json);

            // Assert
            Assert.True(false);
        }
    }
}
