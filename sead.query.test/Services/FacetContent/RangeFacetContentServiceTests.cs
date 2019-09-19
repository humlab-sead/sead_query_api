using Autofac.Features.Indexed;
using Moq;
using SeadQueryCore;
using SeadQueryCore.QueryBuilder;
using System;
using Xunit;

namespace SeadQueryTest.Services.FacetContent
{
    public class RangeFacetContentServiceTests : IDisposable
    {
        private MockRepository mockRepository;

        private IFacetSetting  mockSettings;
        private Mock<IRepositoryRegistry> mockRepositoryRegistry;
        private Mock<IQuerySetupCompiler> mockQuerySetupBuilder;
        private Mock<IIndex<EFacetType, ICategoryCountService>> mockIndex;
        private Mock<IRangeIntervalSqlQueryCompiler> mockRangeIntervalSqlQueryCompiler;

        public RangeFacetContentServiceTests()
        {
            this.mockRepository = new MockRepository(MockBehavior.Strict);

            this.mockSettings = new MockOptionBuilder().Build().Value.Facet;
            this.mockRepositoryRegistry = this.mockRepository.Create<IRepositoryRegistry>();
            this.mockQuerySetupBuilder = this.mockRepository.Create<IQuerySetupCompiler>();
            this.mockIndex = this.mockRepository.Create<IIndex<EFacetType, ICategoryCountService>>();
            this.mockRangeIntervalSqlQueryCompiler = this.mockRepository.Create<IRangeIntervalSqlQueryCompiler>();
        }

        public void Dispose()
        {
            this.mockRepository.VerifyAll();
        }

        private RangeFacetContentService CreateService()
        {
            return new RangeFacetContentService(
                this.mockSettings,
                this.mockRepositoryRegistry.Object,
                this.mockQuerySetupBuilder.Object,
                this.mockIndex.Object,
                this.mockRangeIntervalSqlQueryCompiler.Object);
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
