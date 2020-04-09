using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using Autofac;
using Autofac.Features.Indexed;
using Moq;
using SeadQueryCore;
using SeadQueryCore.QueryBuilder;
using SeadQueryCore.Services.Result;
using SeadQueryInfra;
using SeadQueryTest.Fixtures;
using SeadQueryTest.Infrastructure;
using SeadQueryTest.Mocks;
using Xunit;

namespace SeadQueryTest
{
    public class ResultContentServiceTests : DisposableFacetContextContainer
    {
        public ResultContentServiceTests(JsonSeededFacetContextFixture fixture) : base(fixture)
        {
        }

        public ExpandoObject FakeResultItem(ResultAggregate aggregate, object[] data) {
            var value = new ExpandoObject() as IDictionary<string, object>;
            var fields = aggregate.GetResultFields();
            var i = 0;
            foreach (var field in fields) {
                value.Add(field.ResultField.ResultFieldKey, data[i++]);
                //switch (field.ResultField.DataType) {
                //    case "int":
                //        break;
                //    case "text":
                //        break;
                //    case "decimal":
                //        break;
                //}
            }
            return (ExpandoObject)value;
        }
        public List<ExpandoObject> FakeResultItems(ResultAggregate aggregate, List<object[]> items)
        {
            var values = new List<ExpandoObject>();
            foreach (var item in items)
                values.Add(FakeResultItem(aggregate, item));
            return values;
        }

        [Theory]
        [InlineData("tabular", "site_level", "sites@sites:country@73/sites:", 30)]
        [InlineData("tabular", "aggregate_all", "sites@sites:country@73/sites:", 1)]
        [InlineData("tabular", "sample_group_level", "sites@sites:country@73/sites:", 30)]
        //[InlineData("map", "map_result", "sites@sites:country@73/sites:", 32)]
        public void Load_WhenTabularData_Success(string viewTypeId, string resultKey, string uri, int expectedCount)
        {
            // Arrange
            var facetsConfig = new MockFacetsConfigFactory(Registry.Facets).Create(uri);
            var resultConfig = ResultConfigFactory.Create(viewTypeId, resultKey);


            var mockResultCompiler = new Mock<IResultCompiler>();
            mockResultCompiler.Setup(
                c => c.Compile(facetsConfig, resultConfig, "result_facet")
            ).Returns("");

            var mockCountService = new Mock<ICategoryCountService>();
            IIndex<EFacetType, ICategoryCountService> mockCountServices = new MockIndex<EFacetType, ICategoryCountService>
            {
                { EFacetType.Discrete, mockCountService.Object },
                { EFacetType.Range, mockCountService.Object }
            };
            var tabularSqlCompiler = new TabularResultSqlQueryCompiler();

            var aggregate = Registry.Results.GetByKey(resultConfig.AggregateKeys[0]);
            var resultFields = aggregate
                .GetResultFields()
                .Select((field, i) => new { Field = field, Alias = $"alias_{i + 1}" });

            var queryProxy = new MockDynamicQueryProxyFactory().Create<ExpandoObject>(
                FakeResultItems(aggregate, new List<object[]>  {
                    new object[] { 1 }
                })
            ); ;

            var service = new DefaultResultService(Registry, mockResultCompiler.Object, queryProxy.Object);

            // Act
            var resultSet = service.Load(facetsConfig, resultConfig);

            // Assert
            Assert.NotNull(resultSet);
            var expectedFields = aggregate.GetResultFields();
            var items = resultSet.Data.DataCollection.ToList();
            Assert.True(items.All(x => x.Length == expectedFields.Count));
            Assert.Equal(expectedCount, items.Count);

            var columns = resultSet.Meta.Columns;

            Assert.Equal(expectedFields.Count, columns.Count);

        }
    }
}

