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

        private Mock<IQueryBuilderSetting> mockQueryBuilderSetting;
        private Mock<IRepositoryRegistry> mockRepositoryRegistry;
        private Mock<IQuerySetupBuilder> mockQuerySetupBuilder;
        private Mock<IIndex<EFacetType, ICategoryCountService>> mockIndex;
        private Mock<IRangeIntervalSqlQueryCompiler> mockRangeIntervalSqlQueryCompiler;

        public RangeFacetContentServiceTests()
        {
            this.mockRepository = new MockRepository(MockBehavior.Strict);

            this.mockQueryBuilderSetting = this.mockRepository.Create<IQueryBuilderSetting>();
            this.mockRepositoryRegistry = this.mockRepository.Create<IRepositoryRegistry>();
            this.mockQuerySetupBuilder = this.mockRepository.Create<IQuerySetupBuilder>();
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
                this.mockQueryBuilderSetting.Object,
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
