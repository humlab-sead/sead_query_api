using Moq;
using SeadQueryCore;
using SeadQueryCore.QueryBuilder;
using SeadQueryTest.Infrastructure;
using SeadQueryTest.Mocks;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using Xunit;
using KellermanSoftware.CompareNetObjects;

namespace SeadQueryTest.Services.CategoryCount
{

    [Collection("JsonSeededFacetContext")]
    public class RangeCategoryCountServiceTests : DisposableFacetContextContainer
    {

        public RangeCategoryCountServiceTests(JsonSeededFacetContextFixture fixture) : base(fixture)
        {
        }

        [Theory]
        [InlineData("sites:tbl_denormalized_measured_values_33_0")]
        [InlineData("tbl_denormalized_measured_values_33_0:tbl_denormalized_measured_values_33_0")]
        public void Load_OfRangeCategoryCountsForVariousFacetsConfigs_ReturnsExpectedValues(string uri)
        {
            // Arrange
            var config = new SettingFactory().Create().Value.Facet;
            var mockRegistry = MockFacetRepository();
            var mockFacetsConfig = new MockFacetsConfigFactory(Registry.Facets).Create(uri);
            var mockQuerySetupBuilder = MockQuerySetupBuilder(new QuerySetup { /* not used */ });
            var mockRangeCountSqlCompiler = MockRangeCategoryCountSqlCompiler(returnSql: "SELECT * FROM foot.bar");
            var fakeResult = MockRangeCategoryCountItems(start: 0, step: 10, count: 3);
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
            Assert.NotNull(result.Data);
            Assert.Equal(fakeResult.Count, result.Data.Count);

            CompareLogic compareLogic = new CompareLogic();

            Assert.True(compareLogic.Compare(fakeResult, result.Data.Values.ToList()).AreEqual);
        }
    }
}
