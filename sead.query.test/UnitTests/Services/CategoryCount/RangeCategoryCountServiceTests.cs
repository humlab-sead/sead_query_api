using KellermanSoftware.CompareNetObjects;
using SeadQueryCore;
using SeadQueryCore.QueryBuilder;
using SQT.Infrastructure;
using SQT.Mocks;
using System.Linq;
using Xunit;

namespace SQT.Services
{

    [Collection("SeadJsonFacetContextFixture")]
    public class RangeCategoryCountServiceTests : DisposableFacetContextContainer
    {

        public RangeCategoryCountServiceTests(SeadJsonFacetContextFixture fixture) : base(fixture)
        {
        }

        [Theory]
        [InlineData("sites:tbl_denormalized_measured_values_33_0")]
        [InlineData("tbl_denormalized_measured_values_33_0:tbl_denormalized_measured_values_33_0")]
        public void Load_OfRangeCategoryCountsForVariousFacetsConfigs_ReturnsExpectedValues(string uri)
        {
            // Arrange
            var config = new SettingFactory().Create().Value.Facet;
            var mockRegistry = MockRegistryWithFacetRepository();
            var mockFacetsConfig = FakeFacetsConfig(uri);
            var mockQuerySetupBuilder = MockQuerySetupBuilder(new QuerySetup { /* not used */ });
            var mockRangeCountSqlCompiler = MockRangeCategoryCountSqlCompiler(returnSql: "SELECT * FROM foot.bar");
            var fakeResult = FakeRangeCategoryCountItems(start: 0, size: 10, count: 3);
            var mockQueryProxy = new MockTypedQueryProxyFactory()
                .Create<CategoryCountItem>(fakeResult);

            // Act
            var service = new RangeCategoryCountService(
                 config,
                 mockRegistry.Object,
                 mockQuerySetupBuilder.Object,
                 mockRangeCountSqlCompiler.Object,
                 mockQueryProxy.Object
             );
            var result = service.Load(mockFacetsConfig.TargetFacet.FacetCode, mockFacetsConfig);

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.CategoryCounts);
            Assert.Equal(fakeResult.Count, result.CategoryCounts.Count);

            CompareLogic compareLogic = new CompareLogic();

            Assert.True(compareLogic.Compare(fakeResult, result.CategoryCounts.Values.ToList()).AreEqual);
        }
    }
}
