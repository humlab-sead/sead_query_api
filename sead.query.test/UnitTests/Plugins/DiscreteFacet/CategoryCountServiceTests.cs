using Autofac.Features.Indexed;
using Moq;
using SeadQueryCore;
using SeadQueryCore.QueryBuilder;
using SQT.Infrastructure;
using SQT.Mocks;
using Xunit;

namespace SQT.Services.Plugins.Discrete
{
    [Collection("SeadJsonFacetContextFixture")]
    public class CategoryCountServiceTests(SeadJsonFacetContextFixture fixture) : DisposableFacetContextContainer(fixture)
    {
        [Theory]
        [InlineData("sites:sites", 5)]
        public void Load_UsingVariousConfigs_LoadsSuccessfully(string uri, int nCount)
        {
            // Arrange
            var fakeSettings = FakeFacetSetting();
            var fakeRegistry = FakeRegistry();
            var fakeFacetsConfig = FakeFacetsConfig(uri);
            var mockQuerySetupBuilder = MockQuerySetupBuilder(new QuerySetup { /* not used */ });
            var fakeCategoryCountItems = FakeDiscreteCategoryCountItems(nCount);
            var queryProxy = MockTypedQueryProxy(fakeCategoryCountItems);

            var mockHelpers = MockCategoryCountHelpers();
            var mockSqlCompilers = MockCategoryCountSqlCompilers("SELECT * FROM foot.bar");
            var mockCategoryInfoServices = MockCategoryInfoServices();
            // Act
            var service = new CategoryCountService(
                fakeSettings,
                fakeRegistry,
                mockQuerySetupBuilder.Object,
                queryProxy.Object,
                mockHelpers.Object,
                mockSqlCompilers.Object,
                mockCategoryInfoServices.Object
            );

            var result = service.Load(fakeFacetsConfig.TargetCode, fakeFacetsConfig);

            // Assert
            Assert.NotNull(result);
        }
    }
}
