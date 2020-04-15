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
using System.Data;

namespace SeadQueryTest.Services.FacetContent
{
    [Collection("JsonSeededFacetContext")]
    public class RangeFacetContentServiceTests : DisposableFacetContextContainer
    {
        private readonly MockFacetsConfigFactory FacetsConfigFactory;

        public RangeFacetContentServiceTests(JsonSeededFacetContextFixture fixture) : base(fixture)
        {
            FacetsConfigFactory = new MockFacetsConfigFactory(Registry.Facets);
        }

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

        private static Mock<IQuerySetupBuilder> MockQuerySetupBuilder(QuerySetup querySetup)
        {
            var mockQuerySetupBuilder = new Mock<IQuerySetupBuilder>();

            mockQuerySetupBuilder.Setup(
                x => x.Build(It.IsAny<FacetsConfig2>(), It.IsAny<Facet>(), It.IsAny<List<string>>())
            ).Returns(querySetup);

            mockQuerySetupBuilder.Setup(
                x => x.Build(It.IsAny<FacetsConfig2>(), It.IsAny<Facet>(), It.IsAny<List<string>>(), It.IsAny<List<string>>())
            ).Returns(querySetup);
            return mockQuerySetupBuilder;
        }

        [Fact]
        public void Load_RangeFacetWithRangePick_IsLoaded()
        {
            // Arrange
            var uri = "tbl_denormalized_measured_values_33_0:tbl_denormalized_measured_values_33_0@(110,2904)";
            var querySetup = QuerySetupFixtures.Store[uri];
            var facetsConfig = FacetsConfigFactory.Create(uri);

            Mock<IQuerySetupBuilder> mockQuerySetupBuilder = MockQuerySetupBuilder(querySetup);

            MockIndex<EFacetType, ICategoryCountService> mockCountServices = MockCategoryCountServices();

            var settings = new SettingFactory().Create().Value.Facet;
            var concreteRangeIntervalSqlQueryCompiler = new RangeIntervalSqlCompiler();
            var queryProxy = new Mock<ITypedQueryProxy>();

            queryProxy.Setup(foo => foo.QueryRows<CategoryCountItem>(It.IsAny<string>(), It.IsAny<Func<IDataReader, CategoryCountItem >>())).Returns(
                new List<CategoryCountItem>
                {
                    new CategoryCountItem { Category = "", Count = 0, Extent = null }
                }
            );

            // Act

            var service = new RangeFacetContentService(
                settings,
                Registry,
                mockQuerySetupBuilder.Object,
                mockCountServices,
                concreteRangeIntervalSqlQueryCompiler,
                queryProxy.Object
            );

            var result = service.Load(facetsConfig);

            // Assert
            Assert.NotNull(result);

        }

        [Fact(Skip = "Needs rework")]
        public void Load_WhenOnlineMeasuredValues_33_00_IsTrue()
        {
            using (var container = TestDependencyService.CreateContainer(FacetContext, null))
            using (var scope = container.BeginLifetimeScope()) {
                // Arrange
                var uri = "tbl_denormalized_measured_values_33_0:tbl_denormalized_measured_values_33_0@(110,2904)";
                var scaffolder = new MockFacetsConfigFactory(Registry.Facets);
                var facetsConfig = scaffolder.Create(uri);
                // var resultKeys = new List<string>() { "site_level" };
                // var resultConfig = new ScaffoldResultConfig().Scaffold("tabular", resultKeys);

                // var dumpsFacetConfig = ObjectDumper.Dump(facetsConfig);
                var service = scope.ResolveKeyed<IFacetContentService>(EFacetType.Range);

                // Act
                var resultSet = service.Load(facetsConfig);
            }
        }

        [Fact(Skip = "Needs rework")]
        public void Load_WhenOnlineMeasuredValues_33_82_IsTrue()
        {
            using (var container = TestDependencyService.CreateContainer(FacetContext, null))
            using (var scope = container.BeginLifetimeScope()) {

                // Arrange
                var uri = "tbl_denormalized_measured_values_33_82:tbl_denormalized_measured_values_33_82@(110,2904)";
                var scaffolder = new MockFacetsConfigFactory(Registry.Facets);
                var facetsConfig = scaffolder.Create(uri);
                var service = scope.ResolveKeyed<IFacetContentService>(EFacetType.Range);

                // Act
                var resultSet = service.Load(facetsConfig);
            }
        }
    }
}
