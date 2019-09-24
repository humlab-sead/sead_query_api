using System;
using System.Collections.Generic;
using Autofac;
using Autofac.Features.Indexed;
using Moq;
using SeadQueryCore;
using SeadQueryCore.Model;
using SeadQueryCore.QueryBuilder;
using SeadQueryCore.Services.Result;
using SeadQueryTest.Fixtures;
using SeadQueryTest.Infrastructure;
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

        [Fact]
        public void Load_WhenOnline_IsTrue()
        {
            using (var container = new TestDependencyService().Register())
            using (var scope = container.BeginLifetimeScope()) {

                // Arrange
                var uri = "tbl_denormalized_measured_values_33_0:tbl_denormalized_measured_values_33_0@(110,2904)";
                var registry = scope.Resolve<IRepositoryRegistry>();
                var scaffolder = new ScaffoldFacetsConfig(registry);
                var facetsConfig = scaffolder.Create(uri);
                var resultKeys = new List<string>() { "site_level" };
                var resultConfig = new ScaffoldResultConfig().Scaffold("tabular", resultKeys);
                var service = scope.ResolveKeyed<IResultService>(EFacetType.Range);

                // Act
                var resultSet = service.Load(facetsConfig, resultConfig);

            }
        }
    }
}
