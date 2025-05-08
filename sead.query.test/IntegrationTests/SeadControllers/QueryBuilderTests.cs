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

    [Collection("UsePostgresFixture")]
    public class QueryBuilderTests() : IntegrationTestBase
    {

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
