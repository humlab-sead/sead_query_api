using Moq;
using Newtonsoft.Json.Linq;
using SeadQueryAPI.Controllers;
using SeadQueryAPI.Services;
using SeadQueryCore;
using System;
using Xunit;

namespace SeadQueryTest.Controllers
{
    public class ResultControllerTests : IDisposable
    {
        private MockRepository mockRepository;

        private Mock<IRepositoryRegistry> mockRepositoryRegistry;

        public ResultControllerTests()
        {
            this.mockRepository = new MockRepository(MockBehavior.Strict);

            this.mockRepositoryRegistry = this.mockRepository.Create<IRepositoryRegistry>();
        }

        public void Dispose()
        {
            this.mockRepository.VerifyAll();
        }

        private ResultController CreateResultController()
        {
            var mockResultService = new Mock<ILoadResultService>();
            return new ResultController(mockRepositoryRegistry.Object, mockResultService.Object);
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
