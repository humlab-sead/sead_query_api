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
using SeadQueryTest.Infrastructure.Scaffolding;
using Xunit;

namespace SeadQueryTest.Services.Result
{
    public class MapResultServiceTests
    {
        //private Mock<IRepositoryRegistry> mockRepositoryRegistry;
        //private Mock<IResultCompiler> mockResultCompiler;
        //private Mock<IIndex<EFacetType, ICategoryCountService>> mockIndex;

        //public MapResultServiceTests()
        //{
        //    this.mockRepositoryRegistry = this.mockRepository.Create<IRepositoryRegistry>();
        //    this.mockResultCompiler = this.mockRepository.Create<IResultCompiler>();
        //    this.mockIndex = this.mockRepository.Create<IIndex<EFacetType, ICategoryCountService>>();
        //}

        //private MapResultService CreateService()
        //{
        //    return new MapResultService(
        //        this.mockRepositoryRegistry.Object,
        //        this.mockResultCompiler.Object,
        //        this.mockIndex.Object);
        //}
        private IIndex<int, IPickFilterCompiler> ConcretePickCompilers(string returnValue = "")
        {
            return new MockIndex<int, IPickFilterCompiler>
            {
                    { 1, new DiscreteFacetPickFilterCompiler() },
                    { 2, new RangeFacetPickFilterCompiler() }
            };
        }

        private IIndex<int, IPickFilterCompiler> MockPickCompilers(string returnValue = "")
        {
            var mockPickCompiler = new Mock<IPickFilterCompiler>();
            mockPickCompiler.Setup(foo => foo.Compile(It.IsAny<Facet>(), It.IsAny<Facet>(), It.IsAny<FacetConfig2>())).Returns(returnValue);

            return new MockIndex<int, IPickFilterCompiler>
            {
                    { 1, mockPickCompiler.Object },
                    { 2, mockPickCompiler.Object }
            };
        }

        private IQuerySetupCompiler CreateQuerySetupCompiler(IRepositoryRegistry registry)
        {
            IFacetsGraph facetsGraph = ScaffoldUtility.DefaultFacetsGraph(registry);
            var pickCompilers = ConcretePickCompilers();
            IQuerySetupCompiler querySetupCompiler = new QuerySetupCompiler(facetsGraph, pickCompilers, new EdgeSqlCompiler());
            return querySetupCompiler;
        }
        [Fact]
        public void Load_StateUnderTest_ExpectedBehavior()
        {
            using (var container = new TestDependencyService().Register())
            using (var scope = container.BeginLifetimeScope()) {

                // Arrange
                var uri = "sites:sites";
                var registry = scope.Resolve<IRepositoryRegistry>();
                var facetsConfig = new ScaffoldFacetsConfig(registry).Create(uri);
                var resultKeys = new List<string>() { "site_level" };
                var resultConfig = new ScaffoldResultConfig().Scaffold("map", resultKeys);

                // var categoryCountServices = ConcreteDiscreteCategoryCountService(registry);
                // IResultCompiler resultCompiler = ConcreteResultCompiler(registry);

                var mockCategoryCountServices = new Mock<IDiscreteCategoryCountService>();
                mockCategoryCountServices
                    .Setup(x => x.Load(It.IsAny<string>(), It.IsAny<FacetsConfig2>(), It.IsAny<string>()))
                    .Returns(
                        new Dictionary<string, CategoryCountItem> {
                            { "A", new CategoryCountItem { Category = "A", Count = 10, Extent = new List<decimal>() } },
                            { "B", new CategoryCountItem { Category = "B", Count = 11, Extent = new List<decimal>() } },
                            { "C", new CategoryCountItem { Category = "C", Count = 12, Extent = new List<decimal>() } }
                        }
                    );

                var mockResultCompiler = new Mock<IResultCompiler>();
                mockResultCompiler
                    .Setup(x => x.Compile(It.IsAny<FacetsConfig2>(), It.IsAny<ResultConfig>(), It.IsAny<string>()))
                    .Returns(
                        "SELECT DISTINCT tbl_sites.site_id AS id_column, tbl_sites.site_name AS name, coalesce(latitude_dd, 0.0) AS latitude_dd, coalesce(longitude_dd, 0) AS longitude_dd FROM tbl_sites WHERE 1 = 1 "
                    );

                var service = new MapResultService(registry, mockResultCompiler.Object, mockCategoryCountServices.Object);

                // Act
                var resultSet = service.Load(facetsConfig, resultConfig);

                Assert.NotNull(resultSet);
            }
        }

        private IResultCompiler ConcreteResultCompiler(IRepositoryRegistry registry)
        {
            var resultSqlQueryCompilers = new MockIndex<string, IResultSqlQueryCompiler> {
                {  "map", new MapResultSqlQueryCompiler() }
            };
            IQuerySetupCompiler querySetupCompiler = CreateQuerySetupCompiler(registry);
            var resultQueryCompiler = new ResultCompiler(registry, querySetupCompiler, resultSqlQueryCompilers);
            return resultQueryCompiler;
        }

        private IDiscreteCategoryCountService ConcreteDiscreteCategoryCountService(IRepositoryRegistry registry)
        {
            IFacetSetting facetSettings = new MockOptionBuilder().DefaultFacetSettings();
            IQuerySetupCompiler querySetupCompiler = CreateQuerySetupCompiler(registry);
            IDiscreteCategoryCountSqlQueryCompiler categoryCountSqlCompiler = new DiscreteCategoryCountSqlQueryCompiler();
            return new DiscreteCategoryCountService(facetSettings, registry, querySetupCompiler, categoryCountSqlCompiler);
        }
    }
}
