using Moq;
using SeadQueryCore;
using SeadQueryCore.QueryBuilder;
using SQT.Infrastructure;
using SQT.Mocks;
using System.Linq;
using Xunit;

namespace SQT.Services
{
    [Collection("SeadJsonFacetContextFixture")]
    public class DiscreteFacetContentServiceTests : DisposableFacetContextContainer
    {
        public DiscreteFacetContentServiceTests(SeadJsonFacetContextFixture fixture) : base(fixture)
        {
        }

        [Theory]
        [InlineData("sites:sites", false)]
        [InlineData("dataset_methods:dataset_methods", false)]
        public void Load_VariousDescreteFacets_Success(string uri, bool hasPicks)
        {
            // Arrange
            var fakeRegistry = FakeRegistry();
            var fakeSettings = FakeFacetSetting();
            var facetsConfig = FakeFacetsConfig(uri);
            var fakeQuerySetup = FakeCountOrContentQuerySetup(facetsConfig);
            var mockQuerySetupBuilder = MockQuerySetupBuilder(fakeQuerySetup);
            var fakeValues = FakeDiscreteCategoryCountItems(5);
            var mockDiscreteContentSqlCompiler = MockDiscreteContentSqlCompiler("#SQL-QUERY#");
            var mockQueryProxy = MockTypedQueryProxy(fakeValues);
            var mockCategoryCountServicesLocator = MockCategoryCountServiceLocator(fakeValues);

            // Act
            var service = new DiscreteFacetContentService(
                fakeSettings,
                fakeRegistry,
                mockQuerySetupBuilder.Object,
                mockCategoryCountServicesLocator.Object,
                mockDiscreteContentSqlCompiler.Object,
                mockQueryProxy.Object
             );

            var result = service.Load(facetsConfig);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.Items.Count > 0);
            Assert.Equal(fakeValues.Count, result.Items.Count);
            Assert.Equal(hasPicks, result.Picks.Count > 0);
        }
    }
}
