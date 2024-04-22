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
    [Collection("SeadJsonFacetContextFixture")]
    public class RangeFacetContentServiceTests : DisposableFacetContextContainer
    {
        public RangeFacetContentServiceTests(SeadJsonFacetContextFixture fixture) : base(fixture)
        {
        }

        public virtual Mock<RangeCategoryInfoSqlCompiler> MockRangeIntervalSqlCompiler(string returnSql)
        {
            var mock = new Mock<RangeCategoryInfoSqlCompiler>();
            dynamic payload = new { Interval = It.IsAny<int>(), Lower = It.IsAny<int>(), Upper = It.IsAny<int>(), IntervalCount = It.IsAny<int>() };
            mock.Setup(z => z.Compile(null, null, new { Interval = It.IsAny<int>(), Lower = It.IsAny<int>(), Upper = It.IsAny<int>(), IntervalCount = It.IsAny<int>() })).Returns(returnSql);
            return mock;
        }

        public virtual Mock<IRangeOuterBoundExtentService> MockRangeOuterBoundExtentService(
            decimal lower, decimal upper
        )
        {
            var rangeOuterBoundExtentService = new Mock<IRangeOuterBoundExtentService>();

            /* GetExtent generates an extent */
            rangeOuterBoundExtentService
                .Setup(z => z.GetUpperLowerBounds(It.IsAny<Facet>()))
                .Returns((lower,upper));

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
            var mockQueryProxy = MockTypedQueryProxy(fakeCategoryCountItems);
            var mockCategoryCountService = MockCategoryCountService(fakeCategoryCountItems);

            // Act
            var service = new FacetContentService(
                fakeSettings,
                Registry,
                mockQuerySetupBuilder.Object,
                mockQueryProxy.Object,
                mockCategoryCountService.Object
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