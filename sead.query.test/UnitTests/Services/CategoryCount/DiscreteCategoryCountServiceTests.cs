using SeadQueryCore;
using SeadQueryCore.QueryBuilder;
using SQT.Infrastructure;
using SQT.Mocks;
using Xunit;

namespace SQT.Services
{

    [Collection("JsonSeededFacetContext")]
    public class CategoryCountServiceTests : DisposableFacetContextContainer
    {
        public CategoryCountServiceTests(JsonSeededFacetContextFixture fixture) : base(fixture)
        {
        }

        [Theory]
        [InlineData("sites:sites", 5)]
        public void Load_UsingVariousConfigs_LoadsSuccessfully(string uri, int nCount)
        {
            // Arrange
            var fakeSettings = FakeFacetSetting();
            var fakeRegistry = FakeRegistry();
            var fakeFacetsConfig = FakeFacetsConfig(uri);
            var mockQuerySetupBuilder = MockQuerySetupBuilder(new QuerySetup { /* not used */ });
            var mockCategoryCountSqlCompiler = MockDiscreteCategoryCountSqlCompiler(returnSql: "SELECT * FROM foo.bar");
            var fakeCategoryCountItems = FakeDiscreteCategoryCountItems(nCount);
            var queryProxy = MockTypedQueryProxy(fakeCategoryCountItems);

            // Act
            var service = new DiscreteCategoryCountService(
                fakeSettings,
                fakeRegistry,
                mockQuerySetupBuilder.Object,
                mockCategoryCountSqlCompiler.Object,
                queryProxy.Object
            );

            var result = service.Load(fakeFacetsConfig.TargetCode, fakeFacetsConfig);

            // Assert
            Assert.NotNull(result);
        }

    }
}
