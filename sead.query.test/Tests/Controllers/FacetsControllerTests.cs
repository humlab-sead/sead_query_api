using Moq;
using Newtonsoft.Json.Linq;
using SeadQueryAPI.Controllers;
using SeadQueryAPI.Serializers;
using SeadQueryAPI.Services;
using SeadQueryCore;
using SeadQueryInfra;
using SeadQueryTest.Infrastructure.Scaffolding;
using System;
using Xunit;

namespace SeadQueryTest.Controllers
{
    public class FacetsControllerTests
    {
        private RepositoryRegistry mockRegistry;
        private QueryBuilderSetting mockQueryBuilderSetting;

        public FacetsControllerTests()
        {
            mockQueryBuilderSetting = new MockOptionBuilder().Build().Value;
            mockRegistry = new RepositoryRegistry(ScaffoldUtility.JsonSeededFacetContext());
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
