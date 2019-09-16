using Moq;
using SeadQueryCore;
using SeadQueryCore.QueryBuilder;
using System;
using Xunit;

namespace SeadQueryTest.Services
{
    public class FacetContentServiceTests : IDisposable
    {
        private Moq.MockRepository mockRepository;

        private Mock<IQueryBuilderSetting> mockQueryBuilderSetting;
        private Mock<IRepositoryRegistry> mockRepositoryRegistry;
        private Mock<IQuerySetupBuilder> mockQuerySetupBuilder;

        public FacetContentServiceTests()
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

        private FacetContentService CreateService()
        {
            return new FacetContentService(
                this.mockQueryBuilderSetting.Object,
                this.mockRepositoryRegistry.Object,
                this.mockQuerySetupBuilder.Object);
        }

        [Fact]
        public void Load_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var service = this.CreateService();
            FacetsConfig2 facetsConfig = null;

            // Act
            var result = service.Load(
                facetsConfig);

            // Assert
            Assert.True(false);
        }
    }
}
