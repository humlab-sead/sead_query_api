using Autofac.Features.Indexed;
using Moq;
using SeadQueryCore;
using SeadQueryCore.Model;
using SeadQueryCore.QueryBuilder;
using SeadQueryCore.Services.Result;
using SQT.Infrastructure;
using SQT.Mocks;
using System.Collections.Generic;
using Xunit;

namespace SQT.Services.Result
{
    [Collection("JsonSeededFacetContext")]
    public class MapResultServiceTests : DisposableFacetContextContainer
    {
        public MapResultServiceTests(JsonSeededFacetContextFixture fixture) : base(fixture)
        {
        }

        private IQuerySetupBuilder MockQuerySetupBuilder()
        {
            IFacetsGraph facetsGraph = ScaffoldUtility.DefaultFacetsGraph(Registry);

            var mockPickCompiler = new Mock<IPickFilterCompiler>();
            mockPickCompiler
                .Setup(x => x.Compile(It.IsAny<Facet>(), It.IsAny<Facet>(), It.IsAny<FacetConfig2>()))
                .Returns("");

            var pickCompilers = new Mock<IPickFilterCompilerLocator>();
            pickCompilers.Setup(x => x.Locate(It.IsAny<EFacetType>())).Returns(mockPickCompiler.Object);

            IQuerySetupBuilder querySetupBuilder = new QuerySetupBuilder(facetsGraph, pickCompilers.Object, new JoinSqlCompiler());
            return querySetupBuilder;
        }

        //[Fact]
        //public void Load_StateUnderTest_ExpectedBehavior()
        //{
        //    using (var registry = FakeFacetsGetByCodeRepositoryFactory.Create()) {

        //        // Arrange
        //        var uri = "sites:sites";
        //        var facetsConfig = FakeFacetsConfig(uri);
        //        var resultKeys = new List<string>() { "site_level" };
        //        var resultConfig = ResultConfigFactory.Create("map", resultKeys);

        //        var mockCategoryCountServices = new Mock<IDiscreteCategoryCountService>();
        //        mockCategoryCountServices
        //            .Setup(x => x.Load(It.IsAny<string>(), It.IsAny<FacetsConfig2>(), It.IsAny<string>()))
        //            .Returns(
        //                new CategoryCountService.CategoryCountResult
        //                {
        //                    Data = new Dictionary<string, CategoryCountItem> {
        //                        { "A", new CategoryCountItem { Category = "A", Count = 10, Extent = new List<decimal>() } },
        //                        { "B", new CategoryCountItem { Category = "B", Count = 11, Extent = new List<decimal>() } },
        //                        { "C", new CategoryCountItem { Category = "C", Count = 12, Extent = new List<decimal>() } }
        //                    },
        //                    SqlQuery = ""
        //                }
        //            );

        //        var mockResultCompiler = new Mock<IResultCompiler>();

        //        mockResultCompiler
        //            .Setup(x => x.Compile(It.IsAny<FacetsConfig2>(), It.IsAny<ResultConfig>(), It.IsAny<string>()))
        //            .Returns(
        //                "SELECT DISTINCT tbl_sites.site_id AS id_column, tbl_sites.site_name AS name, coalesce(latitude_dd, 0.0) AS latitude_dd, coalesce(longitude_dd, 0) AS longitude_dd FROM tbl_sites WHERE 1 = 1 "
        //            );

        //        var service = new MapResultService(registry, mockResultCompiler.Object, mockCategoryCountServices.Object);

        //        // Act
        //        var resultSet = service.Load(facetsConfig, resultConfig);

        //        Assert.NotNull(resultSet);
        //    }
        //}

        //private IResultSqlCompiler ConcreteResultCompiler(IRepositoryRegistry registry)
        //{
        //    var resultSqlCompilers = new MockIndex<string, IResultSqlCompiler> {
        //        {  "map", new MapResultSqlCompiler() }
        //    };
        //    IQuerySetupBuilder querySetupBuilder = MockQuerySetupBuilder();
        //    var resultQueryCompiler = new ResultConfigCompiler(registry, querySetupBuilder, resultSqlCompilers);
        //    return resultQueryCompiler;
        //}

        //private IDiscreteCategoryCountService ConcreteDiscreteCategoryCountService(IRepositoryRegistry registry)
        //{
        //    IFacetSetting facetSettings = new SettingFactory().DefaultFacetSettings();
        //    IQuerySetupBuilder querySetupBuilder = CreateQuerySetupBuilder(registry);
        //    IDiscreteCategoryCountSqlCompiler categoryCountSqlCompiler = new DiscreteCategoryCountSqlCompiler();
        //    return new DiscreteCategoryCountService(
        //        facetSettings,
        //        registry,
        //        querySetupBuilder,
        //        categoryCountSqlCompiler
        //    );
        //}
    }
}
