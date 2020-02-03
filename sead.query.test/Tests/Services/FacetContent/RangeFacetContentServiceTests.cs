using Autofac.Features.Indexed;
using Moq;
using SeadQueryCore;
using SeadQueryCore.Model;
using SeadQueryCore.QueryBuilder;
using SeadQueryInfra;
using System;
using System.Collections.Generic;
using Xunit;
using SeadQueryTest.Infrastructure;
using Autofac;
using SeadQueryTest.Fixtures;
using SeadQueryTest.Mocks;

namespace SeadQueryTest.Services.FacetContent
{
    public class RangeFacetContentServiceTests
    {
        //private IFacetSetting  mockSettings;
        //private IRepositoryRegistry mockRegistry;

        //public RangeFacetContentServiceTests()
        //{
        //    mockSettings = new MockOptionBuilder().Build().Value.Facet;
        //    var mockContext = JsonSeededFacetContextFactory.Create();
        //    mockRegistry = new RepositoryRegistry(mockContext);

        //}

        private static MockIndex<EFacetType, ICategoryCountService> MockCategoryCountServices()
        {
            var mockRangeCategoryCountService = new Mock<ICategoryCountService>();
            mockRangeCategoryCountService.Setup(
                x => x.Load(It.IsAny<string>(), It.IsAny<FacetsConfig2>(), It.IsAny<string>())
            ).Returns(
                new CategoryCountService.CategoryCountResult
                {
                    Data = new Dictionary<string, CategoryCountItem>() {

                    },
                    SqlQuery = ""
                }
            );
            MockIndex<EFacetType, ICategoryCountService> mockServices = new MockIndex<EFacetType, ICategoryCountService> {
                { EFacetType.Discrete, new Mock<ICategoryCountService>().Object },
                { EFacetType.Range, mockRangeCategoryCountService.Object }
            };
            return mockServices;
        }

        private static Mock<IQuerySetupCompiler> MockQuerySetupCompiler(QuerySetup querySetup)
        {
            var mockQuerySetupCompiler = new Mock<IQuerySetupCompiler>();

            mockQuerySetupCompiler.Setup(
                x => x.Build(It.IsAny<FacetsConfig2>(), It.IsAny<Facet>(), It.IsAny<List<string>>())
            ).Returns(querySetup);

            mockQuerySetupCompiler.Setup(
                x => x.Build(It.IsAny<FacetsConfig2>(), It.IsAny<Facet>(), It.IsAny<List<string>>(), It.IsAny<List<string>>())
            ).Returns(querySetup);
            return mockQuerySetupCompiler;
        }

        [Fact]
        public void Load_RangeFacetWithRangePick_IsLoaded()
        {
            // Arrange
            var uri = "tbl_denormalized_measured_values_33_0:tbl_denormalized_measured_values_33_0@(110,2904)";
            var querySetup = QuerySetupFixtures.Store[uri];
            var facetsConfig = FacetsConfigFixtures.Store[uri];

            Mock<IQuerySetupCompiler> mockQuerySetupCompiler = MockQuerySetupCompiler(querySetup);

            MockIndex<EFacetType, ICategoryCountService> mockCountServices = MockCategoryCountServices();

            var settings = new SettingFactory().Create().Value.Facet;
            var concreteRangeIntervalSqlQueryCompiler = new RangeIntervalSqlQueryCompiler();

            using (var context = JsonSeededFacetContextFactory.Create()) {

                var registry = new RepositoryRegistry(context);

                // Act

                var service = new RangeFacetContentService(
                    settings,
                    registry,
                    mockQuerySetupCompiler.Object,
                    mockCountServices,
                    concreteRangeIntervalSqlQueryCompiler
                );

                var result = service.Load(facetsConfig);

                // Assert
                Assert.NotNull(result);

            }

        }

        [Fact]
        public void Load_WhenOnlineMeasuredValues_33_00_IsTrue()
        {
            using (var context = JsonSeededFacetContextFactory.Create())
            using (var registry = new RepositoryRegistry(context))
            using (var container = TestDependencyService.CreateContainer(context, null))
            using (var scope = container.BeginLifetimeScope()) {
                // Arrange
                var uri = "tbl_denormalized_measured_values_33_0:tbl_denormalized_measured_values_33_0@(110,2904)";
                var scaffolder = new FacetsConfigFactory(registry);
                var facetsConfig = scaffolder.Create(uri);
                // var resultKeys = new List<string>() { "site_level" };
                // var resultConfig = new ScaffoldResultConfig().Scaffold("tabular", resultKeys);

                // var dumpsFacetConfig = ObjectDumper.Dump(facetsConfig);
                var service = scope.ResolveKeyed<IFacetContentService>(EFacetType.Range);

                // Act
                var resultSet = service.Load(facetsConfig);
            }
        }

        [Fact]
        public void Load_WhenOnlineMeasuredValues_33_82_IsTrue()
        {
            using (var context = JsonSeededFacetContextFactory.Create())
            using (var registry = new RepositoryRegistry(context))
            using (var container = TestDependencyService.CreateContainer(context, null))
            using (var scope = container.BeginLifetimeScope()) {

                // Arrange
                var uri = "tbl_denormalized_measured_values_33_82:tbl_denormalized_measured_values_33_82@(110,2904)";
                var scaffolder = new FacetsConfigFactory(registry);
                var facetsConfig = scaffolder.Create(uri);
                var service = scope.ResolveKeyed<IFacetContentService>(EFacetType.Range);

                // Act
                var resultSet = service.Load(facetsConfig);
            }
        }
    }
}
