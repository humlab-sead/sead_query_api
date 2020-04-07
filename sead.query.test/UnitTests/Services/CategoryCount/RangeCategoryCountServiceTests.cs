using Moq;
using SeadQueryCore;
using SeadQueryCore.QueryBuilder;
using SeadQueryTest.Infrastructure;
using System;
using System.Data.Common;
using Xunit;

namespace SeadQueryTest.Services.CategoryCount
{
    public class RangeCategoryCountServiceTests : DisposableFacetContextContainer
    {

        public RangeCategoryCountServiceTests(JsonSeededFacetContextFixture fixture) : base(fixture)
        {
        }

        [Fact(Skip = "Not implemented")]
        public void TestMethod1()
        {
            // Arrange
            var config = new SettingFactory().Create().Value.Facet;
            var querySetupBuilder = new Mock<IQuerySetupCompiler>();
            var rangeCountSqlCompiler = new Mock<IRangeCategoryCountSqlQueryCompiler>();
            var queryProxy = new Mock<IDatabaseQueryProxy>();

            queryProxy
                .Setup(foo => foo.Query(It.IsAny<string>())).Returns(
                    new DiscreteCountDataReaderBuilder().GenerateBogusRows(3).ToDataReader()
                );

            var service = new RangeCategoryCountService(
                config,
                Registry,
                querySetupBuilder.Object,
                rangeCountSqlCompiler.Object,
                queryProxy.Object 
            );
            // Act
            //var result = service.Load(facetCode, facetsConfig);

            // Assert
            Assert.True(false);
        }
    }
}
