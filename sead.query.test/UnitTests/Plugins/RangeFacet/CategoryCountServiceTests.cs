using Autofac.Features.Indexed;
using KellermanSoftware.CompareNetObjects;
using Moq;
using SeadQueryCore;
using SeadQueryCore.QueryBuilder;
using SQT.Infrastructure;
using SQT.Mocks;
using System.Linq;
using Xunit;

namespace SQT.Plugins.Range
{
    [Collection("SeadJsonFacetContextFixture")]
    public class CategoryCountServiceTests(SeadJsonFacetContextFixture fixture) : MockerWithFacetContext(fixture)
    {
        [Theory]
        [InlineData("sites:sites/tbl_denormalized_measured_values_33_0")]
        // [InlineData("tbl_denormalized_measured_values_33_0:tbl_denormalized_measured_values_33_0")]
        public void Load_OfRangeCategoryCountsForVariousFacetsConfigs_ReturnsExpectedValues(string uri)
        {
            // Arrange
            var config = new SettingFactory().Create().Value.Facet;
            var facetsConfig = FakeFacetsConfig(uri);
            var mockRegistry = MockRegistryWithFacetRepository();
            var mockQuerySetupBuilder = MockQuerySetupBuilder(new QuerySetup { /* not used */ });
            var fakeResult = FakeRangeCategoryCountItems(start: 0, size: 10, count: 3);
            var mockQueryProxy = new MockTypedQueryProxyFactory().Create<CategoryItem>(fakeResult);
            var mockHelpers = MockCategoryCountHelpers();
            var mockSqlCompilers = MockCategoryCountSqlCompilers("SELECT * FROM foot.bar");
            var mockInfoServices = MockCategoryInfoServices();

            // Act
            var service = new CategoryCountService(
                 config,
                 mockRegistry.Object,
                 mockQuerySetupBuilder.Object,
                 mockQueryProxy.Object,
                 mockHelpers.Object,
                 mockSqlCompilers.Object,
                 mockInfoServices.Object
             );
            var result = service.Load(facetsConfig.TargetFacet.FacetCode, facetsConfig);

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.CategoryCounts);
            Assert.Equal(fakeResult.Count, result.CategoryCounts.Count);

            CompareLogic compareLogic = new CompareLogic();

            Assert.True(compareLogic.Compare(fakeResult, result.CategoryCounts.Values.ToList()).AreEqual);
        }
    }
}
