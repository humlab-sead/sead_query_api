using Moq;
using Newtonsoft.Json.Linq;
using SeadQueryAPI.Controllers;
using SeadQueryAPI.Serializers;
using SeadQueryAPI.Services;
using SeadQueryInfra;
using SeadQueryTest.Mocks;
using System;
using Xunit;

namespace SeadQueryTest.Controllers
{
    public class ResultControllerTests : IDisposable
    {
        private FacetContext mockContext;
        private RepositoryRegistry mockRepositoryRegistry;

        public void Dispose()
        {
            mockContext.Dispose();
            mockRepositoryRegistry.Dispose();
        }

        public ResultControllerTests()
        {
            mockContext = JsonSeededFacetContextFactory.Create();
            mockRepositoryRegistry = new RepositoryRegistry(mockContext);
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

        [Fact(Skip="Not implemented")]
        public void Get_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            using (var resultController = this.CreateResultController()) {

                // Act
                var result = resultController.Get();

                // Assert
                Assert.True(false);
            }

        }

        [Fact(Skip = "Not implemented")]
        public void Get_StateUnderTest_ExpectedBehavior1()
        {
            // Arrange
            using (var resultController = this.CreateResultController()) {

                int id = 0;

                // Act
                var result = resultController.Get(id);

                // Assert
                Assert.True(false);
            }
        }

        [Fact(Skip = "Not implemented")]
        public void Load_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            // Arrange
            using (var resultController = this.CreateResultController()) {
                JObject data = null;

                // Act
                var result = resultController.Load(data);

                // Assert
                Assert.True(false);
            }
        }

    }
}
