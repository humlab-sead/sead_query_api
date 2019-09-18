using System;
using Autofac.Features.Indexed;
using Moq;
using SeadQueryCore;
using SeadQueryCore.Model;
using SeadQueryCore.QueryBuilder;
using SeadQueryCore.Services.Result;
using Xunit;

namespace SeadQueryTest.Services.Result
{
    public class MapResultServiceTests : IDisposable
    {
        private MockRepository mockRepository;

        private Mock<IQueryBuilderSetting> mockQueryBuilderSetting;
        private Mock<IRepositoryRegistry> mockRepositoryRegistry;
        private Mock<IQuerySetupBuilder> mockQuerySetupBuilder;
        private Mock<IResultCompiler> mockResultCompiler;
        private Mock<IIndex<EFacetType, ICategoryCountService>> mockIndex;

        public MapResultServiceTests()
        {
            this.mockRepository = new MockRepository(MockBehavior.Strict);

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

        private MapResultService CreateService()
        {
            return new MapResultService(
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
