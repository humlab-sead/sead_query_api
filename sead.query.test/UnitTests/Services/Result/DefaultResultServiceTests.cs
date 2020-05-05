using Moq;
using SeadQueryCore;
using SeadQueryCore.Model;
using SeadQueryCore.Services.Result;
using SQT.Infrastructure;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Xunit;

namespace SQT.Services
{
    [Collection("JsonSeededFacetContext")]
    public class DefaultResultServiceTests : DisposableFacetContextContainer
    {

        public DefaultResultServiceTests(JsonSeededFacetContextFixture fixture) : base(fixture)
        {
        }

        public virtual DataReaderBuilder FakeResultDataBuilder(ResultAggregate aggregate, int count)
        {
            var builder = new TabularResultDataReaderBuilder(aggregate)
                .CreateNewTable()
                .GenerateBogusRows(count);
            return builder;
        }

        [Theory]
        [InlineData("sites:sites", "result_facet", "site_level", "tabular")]
        public void Load_VariousConfigs_Success(string uri, string resultCode, string aggregateKey, string viewType)
        {
            // Arrange
            var mockResultSqlCompilerLocator = MockResultSqlCompilerLocator("#RETURN-SQL#");
            var fakeFacetsConfig = FakeFacetsConfig(uri);
            var fakeResultConfig = FakeResultConfig(aggregateKey, viewType);
            var resultAggregate = FakeRegistry().Results.GetByKey(fakeResultConfig.AggregateKeys[0]);
            var fakeResultDataBuilder = FakeResultDataBuilder(resultAggregate, 10);
            var fakeDataTable = fakeResultDataBuilder.DataTable;
            var mockQueryProxy = new MockDynamicQueryProxyFactory().Create(fakeDataTable);
            var fakeQuerySetup = FakeResultQuerySetup(fakeFacetsConfig, resultCode, aggregateKey);
            var mockQuerySetupBuilder = MockQuerySetupBuilder(fakeQuerySetup);

            // Act
            var service = new DefaultResultService(
                FakeRegistry(), mockQueryProxy.Object, mockQuerySetupBuilder.Object, mockResultSqlCompilerLocator.Object
            );
            var result = service.Load(fakeFacetsConfig, fakeResultConfig);

            // Assert
            Assert.NotNull(result);
        }


    }
}
