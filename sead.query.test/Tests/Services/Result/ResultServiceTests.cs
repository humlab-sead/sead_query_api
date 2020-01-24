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
    public class ResultContentServiceTests
    {
        [Fact]
        public void Scaffolding_CanCreateContextByJsonSeededFacetContext()
        {
            using (var context = JsonSeededFacetContextFactory.Create())
            using (var registry = new RepositoryRegistry(context)) {

                Assert.NotNull(registry);
                Assert.NotNull(registry.Facets);

                Assert.True(registry.Facets.GetAll().Any());

            }
        }

        [Theory]
        [InlineData("tabular", "site_level", "sites@sites:country@73/sites:", 30)]
        [InlineData("tabular", "aggregate_all", "sites@sites:country@73/sites:", 1)]
        [InlineData("tabular", "sample_group_level", "sites@sites:country@73/sites:", 30)]
        //[InlineData("map", "map_result", "sites@sites:country@73/sites:", 32)]
        public void Load_ExecuteLoadContent(string viewTypeId, string resultKey, string uri, int expectedCount)
        {
            var registry = JsonSeededRepositoryRegistryFactory.Create();

            // Arrange
            var facetsConfig = new FacetsConfigFactory(registry).Create(uri);
            var resultConfig = ResultConfigFactory.Create(viewTypeId, resultKey);

            var aggregate = registry.Results.GetByKey(resultConfig.AggregateKeys[0]);

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

            var service = new DefaultResultService(registry, mockResultCompiler.Object);

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

