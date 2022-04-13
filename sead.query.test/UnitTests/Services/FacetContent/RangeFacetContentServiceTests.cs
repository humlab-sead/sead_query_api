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

        public RangeFacetContentServiceTests(JsonFacetContextFixture fixture) : base(fixture)
        {
        }

        public virtual Mock<RangeIntervalSqlCompiler> MockRangeIntervalSqlCompiler(string returnSql)
        {
            var mock = new Mock<RangeIntervalSqlCompiler>();
            mock.Setup(z => z.Compile(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()))
                .Returns(returnSql);
            return mock;
        }

        public virtual Mock<IRangeOuterBoundExtentService> MockRangeOuterBoundExtentService(
            decimal lower, decimal upper, int count = 0
        )
        {
            var rangeOuterBoundExtentService = new Mock<IRangeOuterBoundExtentService>();

            /* GetExtent generates an extent */
            rangeOuterBoundExtentService
                .Setup(z => z.GetExtent(It.IsAny<FacetConfig2>(), It.IsAny<int>()))
                .Returns(new RangeExtent{Lower = lower, Upper = upper, Count = count });

            /* GetUpperLowerBounds hits the database */
            rangeOuterBoundExtentService
                .Setup(z => z.GetUpperLowerBounds(It.IsAny<Facet>()))
                .Returns((lower, upper));

            return rangeOuterBoundExtentService;
        }

        [Theory]
        [InlineData("tbl_denormalized_measured_values_33_0:tbl_denormalized_measured_values_33_0@(110,2904)")]
        public void Load_RangeFacetWithRangePick_IsLoaded(string uri)
        {
            // Arrange
            var fakeSettings = FakeFacetSetting();
            var fakeFacetsConfig = FakeFacetsConfig(uri);
            var fakeQuerySetup = FakeCountOrContentQuerySetup(fakeFacetsConfig);
            var fakeCategoryCountItems = FakeRangeCategoryCountItems(1, 10, 10);
            var mockQuerySetupBuilder = MockQuerySetupBuilder(fakeQuerySetup);
            var mockCountServiceLocator = MockCategoryCountServiceLocator(fakeCategoryCountItems);
            var mockRangeIntervalSqlCompiler = MockRangeIntervalSqlCompiler("#SQL#");
            var mockQueryProxy = MockTypedQueryProxy(fakeCategoryCountItems);
            var mockRangeOuterBoundExtentService = MockRangeOuterBoundExtentService(0M, 100M, 100);

            // Act
            var service = new RangeFacetContentService(
                fakeSettings,
                Registry,
                mockQuerySetupBuilder.Object,
                mockCountServiceLocator.Object,
                mockRangeIntervalSqlCompiler.Object,
                mockRangeOuterBoundExtentService.Object,
                mockQueryProxy.Object
            );

            var result = service.Load(fakeFacetsConfig);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(fakeCategoryCountItems.Count, result.Distribution.Count);
            Assert.Equal(fakeCategoryCountItems.Count, result.Items.Count);
            Assert.Equal(fakeCategoryCountItems.Count, result.ItemCount);
        }
    }
}
