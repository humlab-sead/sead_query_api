using Moq;
using SeadQueryCore;
using SeadQueryCore.QueryBuilder;
using System;
using Xunit;

namespace SeadQueryTest.Services.CategoryCount
{
    public class CategoryCountServiceTests
    {
        private IFacetSetting mockSettings;
        private Mock<IRepositoryRegistry> mockRepositoryRegistry;
        private Mock<IQuerySetupCompiler> mockQuerySetupBuilder;

        public CategoryCountServiceTests()
        {
            this.mockSettings = new SettingFactory().Create().Value.Facet;
            this.mockRepositoryRegistry = new Mock<IRepositoryRegistry>();
            this.mockQuerySetupBuilder = new Mock<IQuerySetupCompiler>();
        }

        private CategoryCountService CreateService()
        {
            return new CategoryCountService(
                this.mockSettings,
                this.mockRepositoryRegistry.Object,
                this.mockQuerySetupBuilder.Object);
        }

        [Fact]
        public void Load_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var service = this.CreateService();
            string facetCode = null;
            FacetsConfig2 facetsConfig = null;
            string intervalQuery = null;

            // Act
            var result = service.Load(
                facetCode,
                facetsConfig,
                intervalQuery);

            // Assert
            Assert.True(false);
        }
    }
}
