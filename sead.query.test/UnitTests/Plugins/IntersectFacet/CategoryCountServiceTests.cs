using Autofac.Features.Indexed;
using KellermanSoftware.CompareNetObjects;
using Moq;
using SeadQueryCore;
using SeadQueryCore.QueryBuilder;
using SQT.Infrastructure;
using SQT.Mocks;
using System.Linq;
using Xunit;

namespace SQT.Plugins.Intersect
{
    [Collection("UsePostgresFixture")]
    public class CategoryCountServiceTests() : MockerWithFacetContext()
    {
        [Theory]
        [InlineData("analysis_entity_ages:analysis_entity_ages")]
        // [InlineData("sites:sites/analysis_entity_ages")]
        public void Load_CategoryCount_ReturnsExpectedValues(string uri)
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
