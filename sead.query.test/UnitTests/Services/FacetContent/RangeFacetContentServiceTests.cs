using Moq;
using SeadQueryCore;
using SQT;
using SQT.Infrastructure;
using SQT.Mocks;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace SQT.Services
{
    [Collection("JsonSeededFacetContext")]
    public class RangeFacetContentServiceTests : DisposableFacetContextContainer
    {
        private readonly MockFacetsConfigFactory FacetsConfigFactory;

        public RangeFacetContentServiceTests(JsonSeededFacetContextFixture fixture) : base(fixture)
        {
            FacetsConfigFactory = new MockFacetsConfigFactory(Registry.Facets);
        }

        protected Mock<ICategoryCountService> MockCategoryCountService(List<CategoryCountItem> fakeCategoryCountItems)
        {
            var mockCategoryCountService = new Mock<ICategoryCountService>();
            mockCategoryCountService.Setup(
                x => x.Load(It.IsAny<string>(), It.IsAny<FacetsConfig2>(), It.IsAny<string>())
            ).Returns(
                new CategoryCountService.CategoryCountResult
                {
                    Data = fakeCategoryCountItems.ToDictionary(z => z.Category),
                    SqlQuery = "SELECT * FROM bla.bla"
                }
            );
            return mockCategoryCountService;
        }

        protected Mock<ICategoryCountServiceLocator> MockCategoryCountServiceLocator(List<CategoryCountItem> fakeCategoryCountItems)
        {
            var mockCategoryCountService = MockCategoryCountService(fakeCategoryCountItems);

            var mockCategoryCountServiceLocator = new Mock<ICategoryCountServiceLocator>();
            mockCategoryCountServiceLocator
                .Setup(z => z.Locate(It.IsAny<EFacetType>()))
                .Returns(
                    mockCategoryCountService.Object
                );
            return mockCategoryCountServiceLocator;
        }

        //private static Mock<IQuerySetupBuilder> MockQuerySetupBuilder(QuerySetup querySetup)
        //{
        //    var mockQuerySetupBuilder = new Mock<IQuerySetupBuilder>();

        //    mockQuerySetupBuilder.Setup(
        //        x => x.Build(It.IsAny<FacetsConfig2>(), It.IsAny<Facet>(), It.IsAny<List<string>>())
        //    ).Returns(querySetup);

        //    mockQuerySetupBuilder.Setup(
        //        x => x.Build(It.IsAny<FacetsConfig2>(), It.IsAny<Facet>(), It.IsAny<List<string>>(), It.IsAny<List<string>>())
        //    ).Returns(querySetup);
        //    return mockQuerySetupBuilder;
        //}

        [Theory]
        [InlineData("tbl_denormalized_measured_values_33_0:tbl_denormalized_measured_values_33_0@(110,2904)")]
        public void Load_RangeFacetWithRangePick_IsLoaded(string uri)
        {
            // Arrange
            var querySetup = FakeQuerySetup(uri);
            var facetsConfig = FakeFacetsConfig(uri);
            var mockQuerySetupBuilder = MockQuerySetupBuilder(querySetup);
            var fakeCategoryCountItems = FakeRangeCategoryCountItems(1, 10, 10);
            var mockCountServiceLocator = MockCategoryCountServiceLocator(fakeCategoryCountItems);
            var settings = FakeFacetSetting();
            var concreteRangeIntervalSqlCompiler = new RangeIntervalSqlCompiler();
            var queryProxy = MockTypedQueryProxy(fakeCategoryCountItems);
            var rangeOuterBoundExtentService = new Mock<IRangeOuterBoundExtentService>();

            rangeOuterBoundExtentService
                .Setup(z => z.GetExtent(It.IsAny<FacetConfig2>(), It.IsAny<int>()))
                .Returns(new RangeExtent {
                    Lower = 0M,
                    Upper = 10.0M,
                    Count = 100
                });

            // Act
            var service = new RangeFacetContentService(
                settings,
                Registry,
                mockQuerySetupBuilder.Object,
                mockCountServiceLocator.Object,
                concreteRangeIntervalSqlCompiler,
                rangeOuterBoundExtentService.Object,
                queryProxy.Object
            );

            var result = service.Load(facetsConfig);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(10, result.ItemCount);
        }
    }
}
