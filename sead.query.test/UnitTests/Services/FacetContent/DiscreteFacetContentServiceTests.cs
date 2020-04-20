using Moq;
using SeadQueryCore;
using SeadQueryCore.QueryBuilder;
using SQT.Infrastructure;
using SQT.Mocks;
using Xunit;

namespace SQT.Services
{
    [Collection("JsonSeededFacetContext")]
    public class DiscreteFacetContentServiceTests : DisposableFacetContextContainer
    {
        public DiscreteFacetContentServiceTests(JsonSeededFacetContextFixture fixture) : base(fixture)
        {
        }


        [Fact(Skip = "Not implemented")]
        public void TestMethod1()
        {
            // Arrange
            var config = new SettingFactory().Create().Value.Facet;
            var querySetupBuilder = new Mock<IQuerySetupBuilder>();
            var rangeCountSqlCompiler = new Mock<IRangeCategoryCountSqlCompiler>();
            var categoryCountServicesLocator = new Mock<ICategoryCountServiceLocator>();
            var discreteContentSqlCompiler = new Mock<IDiscreteContentSqlCompiler>();

            var queryProxy = new MockTypedQueryProxyFactory().Create<DiscreteContentDataReaderBuilder, CategoryCountItem>(3);

            //var expectedValues = new DiscreteCountDataReaderBuilder()
            //    .GenerateBogusRows(3)
            //    .ToItems<CategoryCountItem>()
            //    .ToList();

            //queryProxy.Setup(foo => foo.QueryRows(It.IsAny<string>(), It.IsAny<Func<IDataReader, CategoryCountItem>>())).Returns(
            //    expectedValues
            //);

            // Act
            var service = new DiscreteFacetContentService(
                config,
                Registry,
                querySetupBuilder.Object,
                categoryCountServicesLocator.Object,
                discreteContentSqlCompiler.Object,
                queryProxy.Object
             );

            var facetsConfig = FakeFacetsConfig("sites:sites");

            var result = service.Load(facetsConfig);

            // Assert
            Assert.True(false);
        }
    }
}
