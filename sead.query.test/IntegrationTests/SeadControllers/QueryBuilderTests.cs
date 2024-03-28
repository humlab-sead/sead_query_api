using Newtonsoft.Json;
using SeadQueryCore;
using SQT;
using SQT.Infrastructure;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Autofac;
using SeadQueryCore.QueryBuilder;
using SQT.Mocks;
using Microsoft.Win32;
using SeadQueryCore.Model;

namespace IntegrationTests.Sead
{

    [Collection("SeadJsonFacetContextFixture")]
    public class QueryBuilderTests
    {
        public JsonFacetContextFixture FacetContextFixture { get; }
        public DependencyService DependencyService { get; }
        public IContainer Container { get; private set; }
        public IResultSqlCompilerLocator SqlCompilerLocator { get; private set; }
        public IQuerySetupBuilder QuerySetupBuilder { get; private set; }
        public IRepositoryRegistry Registry { get; private set; }
        
        public QueryBuilderTests(SeadJsonFacetContextFixture facetContextFixture)
        {
            FacetContextFixture = facetContextFixture;
            DependencyService = new DependencyService(FacetContextFixture) { Options = SettingFactory.GetSettings() };
            var builder = new ContainerBuilder();
            builder.RegisterModule(DependencyService);
            Container = builder.Build();
            QuerySetupBuilder = Container.Resolve<IQuerySetupBuilder>();
            SqlCompilerLocator = Container.Resolve<IResultSqlCompilerLocator>();
            Registry = Container.Resolve<IRepositoryRegistry>();
        }
        public FacetsConfig2 FakeFacetsConfig(string uri)
            => new MockFacetsConfigFactory(Registry.Facets).Create(uri);

        public virtual ResultConfig FakeResultConfig(string facetCode, string specificationKey, string viewTypeId)
            => ResultConfigFactory.Create(Registry.Facets.GetByCode(facetCode), Registry.Results.GetByKey(specificationKey), viewTypeId);

        [Theory]
        [InlineData("species:species")]
        public void Load_VariousFacetConfigs_HasExpectedSqlQuery(string uri)
        {
            var facetsConfig = FakeFacetsConfig(uri);
            var resultConfig = FakeResultConfig("result_facet", "site_level", "tabular");

            var queryFields = resultConfig.GetSortedFields();

            var querySetup = QuerySetupBuilder
                .Build(facetsConfig, resultConfig.Facet, queryFields);

            var sqlQuery = SqlCompilerLocator
                .Locate(resultConfig.ViewTypeId)
                    .Compile(querySetup, resultConfig.Facet, queryFields);

            Assert.NotNull(sqlQuery);

            // return new TabularResultContentSet(
            //     resultConfig: resultConfig,
            //     resultFields: queryFields.GetResultValueFields().ToList(),
            //     reader: QueryProxy.Query(sqlQuery) /* This is (for now) only call to generic QueryProxy.Query */
            // )
            // {
            //     Payload = GetPayload(facetsConfig, resultConfig),
            //     Query = sqlQuery
            // };

        }

    }
}
