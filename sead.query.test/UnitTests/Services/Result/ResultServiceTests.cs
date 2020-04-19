using Moq;
using SeadQueryCore;
using SeadQueryCore.Model;
using SeadQueryCore.Services.Result;
using SQT.Infrastructure;
using SQT.Mocks;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace SQT
{
    [Collection("JsonSeededFacetContext")]
    public class ResultContentServiceTests : DisposableFacetContextContainer
    {
        public ResultContentServiceTests(JsonSeededFacetContextFixture fixture) : base(fixture)
        {
        }

        [Fact]
        public void Example_HowToSetup_DynamicQueryProxyMock()
        {
            var fields = Registry.Results.GetByKey("site_level").GetResultFieldTypes().ToList();
            object[,] values = new object[,] {
                { "ipsum", "lorum", 1, 2, 3 },
                { "ipsus", "lorus", 4, 5, 6 }
            };
            var queryProxy = new MockDynamicQueryProxyFactory().CreateWithData(fields, values);

            Assert.NotNull(queryProxy);

            queryProxy = new MockDynamicQueryProxyFactory().CreateWithFakeData(fields, 10);

            Assert.NotNull(queryProxy);
        }

        [Theory]
        [InlineData("tabular", "site_level", "sites@sites:country@73/sites:", 30)]
        [InlineData("tabular", "aggregate_all", "sites@sites:country@73/sites:", 1)]
        [InlineData("tabular", "sample_group_level", "sites@sites:country@73/sites:", 30)]
        [InlineData("map", "map_result", "sites@sites:country@73/sites:", 32)]
        public void Load_WithVariousSetups_Success(string viewTypeId, string resultKey, string uri, int expectedCount)
        {
            // Arrange
            var facetsConfig = new MockFacetsConfigFactory(Registry.Facets).Create(uri);
            var resultConfig = new ResultConfig()
            {
                ViewTypeId = viewTypeId,
                RequestId = "1",
                SessionId = "1",
                AggregateKeys = new List<string> { resultKey }
            };
            var mockResultQueryCompiler = MockResultConfigCompiler("SELECT * FROM dummy");
            var aggregate = Registry.Results.GetByKey(resultKey);
            var fields = aggregate.GetResultFieldTypes().ToList();
            var queryProxy = new MockDynamicQueryProxyFactory().CreateWithFakeData(fields, expectedCount);

            // Act
            var service = new DefaultResultService(Registry, mockResultQueryCompiler.Object, queryProxy.Object);
            var result = service.Load(facetsConfig, resultConfig);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.Data.DataCollection.All(x => x.Length == fields.Count));
            Assert.Equal(expectedCount, result.Data.DataCollection.Count);
            Assert.Equal(fields.Count, result.Meta.Columns.Count);

        }

        //private static IIndex<EFacetType, ICategoryCountService> MockCategoryCountService()
        //{
        //    var mockCountService = new Mock<ICategoryCountService>();
        //    var mockCountServices = new MockIndex<EFacetType, ICategoryCountService>
        //    {
        //        { EFacetType.Discrete, mockCountService.Object },
        //        { EFacetType.Range, mockCountService.Object }
        //    };
        //    return mockCountServices;
        //}

    }
}

