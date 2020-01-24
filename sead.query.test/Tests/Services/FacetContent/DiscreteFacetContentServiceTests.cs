using Autofac.Features.Indexed;
using Moq;
using SeadQueryCore;
using SeadQueryCore.QueryBuilder;
using System;
using Xunit;

namespace SeadQueryTest.Services.FacetContent
{
    public class DiscreteFacetContentServiceTests : IDisposable
    {
        private MockRepository mockRepository;

        private IFacetSetting mockSettings;
        private Mock<IRepositoryRegistry> mockRepositoryRegistry;
        private Mock<IQuerySetupCompiler> mockQuerySetupBuilder;
        private Mock<IIndex<EFacetType, ICategoryCountService>> mockIndex;
        private Mock<IDiscreteContentSqlQueryCompiler> mockDiscreteContentSqlQueryCompiler;

        public DiscreteFacetContentServiceTests()
        {
            this.mockRepository = new MockRepository(MockBehavior.Strict);

            this.mockSettings = new SettingFactory().Create().Value.Facet;
            this.mockRepositoryRegistry = this.mockRepository.Create<IRepositoryRegistry>();
            this.mockQuerySetupBuilder = this.mockRepository.Create<IQuerySetupCompiler>();
            this.mockIndex = this.mockRepository.Create<IIndex<EFacetType, ICategoryCountService>>();
            this.mockDiscreteContentSqlQueryCompiler = this.mockRepository.Create<IDiscreteContentSqlQueryCompiler>();
        }

        public void Dispose()
        {
            this.mockRepository.VerifyAll();
        }

        private DiscreteFacetContentService CreateService()
        {
            return new DiscreteFacetContentService(
                mockSettings,
                this.mockRepositoryRegistry.Object,
                this.mockQuerySetupBuilder.Object,
                this.mockIndex.Object,
                this.mockDiscreteContentSqlQueryCompiler.Object);
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
