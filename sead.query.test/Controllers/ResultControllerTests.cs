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
    public class ResultControllerTests
    {
        private RepositoryRegistry mockRepositoryRegistry;

        public ResultControllerTests()
        {
            mockRepositoryRegistry = new RepositoryRegistry(ScaffoldUtility.DefaultFacetContext());
        }

        private ResultController CreateResultController()
        {
            var mockResultService = new Mock<ILoadResultService>();
            var mockReconstituteService = new Mock<IFacetConfigReconstituteService>();
            return new ResultController(
                mockRepositoryRegistry,
                mockReconstituteService.Object,
                mockResultService.Object
            );
        }

        [Fact]
        public void Get_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var resultController = this.CreateResultController();

            // Act
            var result = resultController.Get();

            // Assert
            Assert.True(false);
        }

        [Fact]
        public void Get_StateUnderTest_ExpectedBehavior1()
        {
            // Arrange
            var resultController = this.CreateResultController();
            int id = 0;

            // Act
            var result = resultController.Get(
                id);

            // Assert
            Assert.True(false);
        }

        [Fact]
        public void Load_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var resultController = this.CreateResultController();
            JObject data = null;

            // Act
            var result = resultController.Load(
                data);

            // Assert
            Assert.True(false);
        }
    }
}
