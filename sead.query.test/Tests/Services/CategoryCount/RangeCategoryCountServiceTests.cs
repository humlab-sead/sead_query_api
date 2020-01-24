using Moq;
using SeadQueryCore;
using SeadQueryCore.QueryBuilder;
using System;
using Xunit;

namespace SeadQueryTest.Services.CategoryCount
{
    public class RangeCategoryCountServiceTests
    {
        private IFacetSetting mockSetting;
        private Mock<IRepositoryRegistry> mockRepositoryRegistry;
        private Mock<IQuerySetupCompiler> mockQuerySetupBuilder;
        private Mock<IRangeCategoryCountSqlQueryCompiler> mockRangeCategoryCountSqlQueryCompiler;

        public RangeCategoryCountServiceTests()
        {
            this.mockSetting = new SettingFactory().Create().Value.Facet;
            this.mockRepositoryRegistry = new Mock<IRepositoryRegistry>();
            this.mockQuerySetupBuilder = new Mock<IQuerySetupCompiler>();
            this.mockRangeCategoryCountSqlQueryCompiler = new Mock<IRangeCategoryCountSqlQueryCompiler>();
        }

        private RangeCategoryCountService CreateService()
        {
            return new RangeCategoryCountService(
                this.mockSetting,
                this.mockRepositoryRegistry.Object,
                this.mockQuerySetupBuilder.Object,
                this.mockRangeCategoryCountSqlQueryCompiler.Object);
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
