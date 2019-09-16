using Moq;
using SeadQueryCore;
using SeadQueryCore.QueryBuilder;
using SeadQueryCore.Services;
using System;
using Xunit;

namespace SeadQueryTest.Services
{
    public class ReportServiceTests : IDisposable
    {
        private Moq.MockRepository mockRepository;

        private Mock<IQueryBuilderSetting> mockQueryBuilderSetting;
        private Mock<IRepositoryRegistry> mockRepositoryRegistry;
        private Mock<IQuerySetupBuilder> mockQuerySetupBuilder;

        public ReportServiceTests()
        {
            this.mockRepository = new Moq.MockRepository(MockBehavior.Strict);

            this.mockQueryBuilderSetting = this.mockRepository.Create<IQueryBuilderSetting>();
            this.mockRepositoryRegistry = this.mockRepository.Create<IRepositoryRegistry>();
            this.mockQuerySetupBuilder = this.mockRepository.Create<IQuerySetupBuilder>();
        }

        public void Dispose()
        {
            this.mockRepository.VerifyAll();
        }

        private ReportService CreateService()
        {
            return new ReportService(
                this.mockQueryBuilderSetting.Object,
                this.mockRepositoryRegistry.Object,
                this.mockQuerySetupBuilder.Object);
        }

        [Fact]
        public void TestMethod1()
        {
            // Arrange
            var service = this.CreateService();

            // Act


            // Assert
            Assert.True(false);
        }
    }
}
