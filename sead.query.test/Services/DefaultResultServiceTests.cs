using Autofac.Features.Indexed;
using Moq;
using SeadQueryCore;
using SeadQueryCore.QueryBuilder;
using System;
using Xunit;

namespace SeadQueryTest.Services
{
    public class DefaultResultServiceTests : IDisposable
    {
        private Moq.MockRepository mockRepository;

        private Mock<IQueryBuilderSetting> mockQueryBuilderSetting;
        private Mock<IRepositoryRegistry> mockRepositoryRegistry;
        private Mock<IQuerySetupBuilder> mockQuerySetupBuilder;
        private Mock<IResultCompiler> mockResultCompiler;
        private Mock<IIndex<EFacetType, ICategoryCountService>> mockIndex;

        public DefaultResultServiceTests()
        {
            this.mockRepository = new Moq.MockRepository(MockBehavior.Strict);

            this.mockQueryBuilderSetting = this.mockRepository.Create<IQueryBuilderSetting>();
            this.mockRepositoryRegistry = this.mockRepository.Create<IRepositoryRegistry>();
            this.mockQuerySetupBuilder = this.mockRepository.Create<IQuerySetupBuilder>();
            this.mockResultCompiler = this.mockRepository.Create<IResultCompiler>();
            this.mockIndex = this.mockRepository.Create<IIndex<EFacetType, ICategoryCountService>>();
        }

        public void Dispose()
        {
            this.mockRepository.VerifyAll();
        }

        private DefaultResultService CreateService()
        {
            return new DefaultResultService(
                this.mockQueryBuilderSetting.Object,
                this.mockRepositoryRegistry.Object,
                this.mockQuerySetupBuilder.Object,
                this.mockResultCompiler.Object,
                this.mockIndex.Object);
        }

        [Fact]
        public void Load_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var service = this.CreateService();
            FacetsConfig2 facetsConfig = null;
            ResultConfig resultConfig = null;

            // Act
            var result = service.Load(
                facetsConfig,
                resultConfig);

            // Assert
            Assert.True(false);
        }
    }
}
