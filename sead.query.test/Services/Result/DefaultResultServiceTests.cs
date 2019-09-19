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
    public class DefaultResultServiceTests 
    {
        private Mock<IRepositoryRegistry> mockRepositoryRegistry;
        private Mock<IQuerySetupCompiler> mockQuerySetupBuilder;
        private Mock<IResultCompiler> mockResultCompiler;
        private Mock<IIndex<EFacetType, ICategoryCountService>> mockIndex;

        public DefaultResultServiceTests()
        {
            this.mockRepositoryRegistry = new Mock<IRepositoryRegistry>();
            this.mockQuerySetupBuilder = new Mock<IQuerySetupCompiler>();
            this.mockResultCompiler = new Mock<IResultCompiler>();
            this.mockIndex = new Mock<IIndex<EFacetType, ICategoryCountService>>();
        }

        private DefaultResultService CreateService()
        {
            return new DefaultResultService(
                this.mockRepositoryRegistry.Object,
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
