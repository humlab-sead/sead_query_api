using Moq;
using Newtonsoft.Json.Linq;
using SeadQueryAPI.Controllers;
using SeadQueryAPI.Serializers;
using SeadQueryAPI.Services;
using SeadQueryCore;
using SeadQueryInfra;
using SeadQueryTest.Mocks;
using System;
using Xunit;
using FluentAssertions;
using SeadQueryTest.Infrastructure;

namespace SeadQueryTest.Controllers
{
    public class FacetsControllerTests : IDisposable
    {
        private FacetContext mockContext;
        private RepositoryRegistry mockRegistry;

        public FacetsControllerTests()
        {
            mockContext = JsonSeededFacetContextFactory.Create();
            mockRegistry = new RepositoryRegistry(mockContext);
        }

        public void Dispose()
        {
            mockContext.Dispose();
            mockRegistry.Dispose();
        }

        private FacetsController CreateFacetsController()
        {
            var mockLoadFacetService = new Mock<IFacetReconstituteService>();
            var mockReconstituteService = new Mock<IFacetConfigReconstituteService>();

            return new FacetsController(
                mockRegistry,
                mockReconstituteService.Object,
                mockLoadFacetService.Object
            );

        }

        [Fact]
        public void Get_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var facetsController = CreateFacetsController();

            // Act
            var result = facetsController.Get();

            // Assert
            Assert.True(false);
        }

        [Fact]
        public void Get_StateUnderTest_ExpectedBehavior1()
        {
            // Arrange
            var facetsController = CreateFacetsController();
            int id = 0;

            // Act
            var result = facetsController.Get(
                id);

            // Assert
            result.Should().Be(true);
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
            result.Should().Be(true);

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
            var result = facetsController.Mirror(json);

            // Assert
            Assert.True(false);
        }

    }
}
