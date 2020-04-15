using Autofac.Features.Indexed;
using Moq;
using SeadQueryCore;
using SeadQueryCore.QueryBuilder;
using SeadQueryTest.Infrastructure;
using System;
using System.Linq;
using System.Data;
using Xunit;
using SeadQueryTest.Mocks;

namespace SeadQueryTest.Services.FacetContent
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
            var categoryCountServices = new Mock<IIndex<EFacetType, ICategoryCountService>>();
            var discreteContentSqlQueryCompiler = new Mock<IDiscreteContentSqlCompiler>();

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
                categoryCountServices.Object,
                discreteContentSqlQueryCompiler.Object,
                queryProxy.Object
             );

            var facetsConfig = new MockFacetsConfigFactory(Registry.Facets).Create("sites:sites");

            var result = service.Load(facetsConfig);

            // Assert
            Assert.True(false);
        }
    }
}
