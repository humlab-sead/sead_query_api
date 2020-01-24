using Moq;
using SeadQueryCore;
using SeadQueryCore.QueryBuilder;
using System;
using Xunit;

namespace SeadQueryTest.Services
{
    public class DeleteBogusPickServiceTests : IDisposable
    {
        private Moq.MockRepository mockRepository;

        private Mock<ISetting> mockQueryBuilderSetting;
        private Mock<IRepositoryRegistry> mockRepositoryRegistry;
        private Mock<IQuerySetupCompiler> mockQuerySetupBuilder;
        private Mock<IValidPicksSqlQueryCompiler> mockValidPicksSqlQueryCompiler;

        public DeleteBogusPickServiceTests()
        {
            this.mockRepository = new Moq.MockRepository(MockBehavior.Strict);

            this.mockQueryBuilderSetting = this.mockRepository.Create<ISetting>();
            this.mockRepositoryRegistry = this.mockRepository.Create<IRepositoryRegistry>();
            this.mockQuerySetupBuilder = this.mockRepository.Create<IQuerySetupCompiler>();
            this.mockValidPicksSqlQueryCompiler = this.mockRepository.Create<IValidPicksSqlQueryCompiler>();
        }

        public void Dispose()
        {
            this.mockRepository.VerifyAll();
        }

        private DiscreteBogusPickService CreateService()
        {
            return new DiscreteBogusPickService(
                this.mockRepositoryRegistry.Object,
                this.mockQuerySetupBuilder.Object,
                this.mockValidPicksSqlQueryCompiler.Object);
        }

        [Fact]
        public void Delete_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var service = this.CreateService();
            FacetsConfig2 facetsConfig = null;

            // Act
            var result = service.Delete(
                facetsConfig);

            // Assert
            Assert.True(false);
        }
    }
}
