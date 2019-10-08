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
using SeadQueryTest.Infrastructure.Scaffolding;
using Xunit;

namespace SeadQueryTest
{
    public class ResultContentServiceTests
    {
        private QueryBuilderSetting mockQueryBuilderSetting;
        private RepositoryRegistry mockRegistry;
        private ScaffoldFacetsConfig facetConfigFixture;
        private ScaffoldResultConfig resultConfigFixture;

        public ResultContentServiceTests()
        {
            mockQueryBuilderSetting = new MockOptionBuilder().Build().Value;
            mockRegistry = new RepositoryRegistry(ScaffoldUtility.DefaultFacetContext());
            facetConfigFixture = new Fixtures.ScaffoldFacetsConfig(mockRegistry);
            resultConfigFixture = new Fixtures.ScaffoldResultConfig();
        }

        [Theory]
        [InlineData("tabular", "site_level", "sites@sites:country@73/sites:", 30)]
        [InlineData("tabular", "aggregate_all", "sites@sites:country@73/sites:", 1)]
        [InlineData("tabular", "sample_group_level", "sites@sites:country@73/sites:", 30)]
        //[InlineData("map", "map_result", "sites@sites:country@73/sites:", 32)]
        public void Load_WhenUsingDefaultContext(string viewTypeId, string resultKey, string uri, int expectedCount)
        {
            // using (var context = ScaffoldUtility.DefaultFacetContext())
            using (var container = new TestDependencyService().Register())
            using (var scope = container.BeginLifetimeScope()) {

                // Arrange
                var facetsConfig = facetConfigFixture.Create(uri);
                var resultConfig = resultConfigFixture.GenerateConfig(viewTypeId, resultKey);

                var dumpsFacetConfig = ObjectDumper.Dump(facetsConfig);

                var service = scope.ResolveKeyed<IResultService>(viewTypeId);

                IQuerySetupCompiler builder = scope.Resolve<IQuerySetupCompiler>();

                var resultSet = service.Load(facetsConfig, resultConfig);
                // Act

            }

        }
        /*
         */
        [Theory]
        [InlineData("tabular", "site_level", "sites@sites:country@73/sites:", 30)]
        [InlineData("tabular", "aggregate_all", "sites@sites:country@73/sites:", 1)]
        [InlineData("tabular", "sample_group_level", "sites@sites:country@73/sites:", 30)]
        //[InlineData("map", "map_result", "sites@sites:country@73/sites:", 32)]
        public void Load_ExecuteLoadContent(string viewTypeId, string resultKey, string uri, int expectedCount)
        {

            // Arrange
            var facetsConfig = facetConfigFixture.Create(uri);
            var resultConfig = resultConfigFixture.GenerateConfig(viewTypeId, resultKey);

            var aggregate = mockRegistry.Results.GetByKey(resultConfig.AggregateKeys[0]);

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

            var service = new DefaultResultService(
                mockRegistry,
                mockResultCompiler.Object
            );

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

