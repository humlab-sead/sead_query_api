using IntegrationTests.Sead;
using Newtonsoft.Json;
using SeadQueryCore;
using SeadQueryCore.Model;
using SQT.SQL.Matcher;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using System.Text;

namespace IntegrationTests.Debug
{
    [Collection("UsePostgresFixture")]
    public class DebugTests : ControllerTest<TestHostWithContainer>, IClassFixture<TestHostWithContainer>
    {
        readonly string jsonThatFails = @"{
                ""facetsConfig"": {
                    ""requestId"": 3,
                    ""requestType"": ""populate"",
                    ""domainCode"": """",
                    ""targetCode"": ""ecocode"",
                    ""facetConfigs"": [
                        {
                            ""facetCode"": ""ecocode"",
                            ""position"": 1,
                            ""picks"": [
                                {
                                    ""pickValue"": 36,
                                    ""text"": 36
                                }
                            ]
                        }
                    ]
                },
                ""resultConfig"": {
                    ""requestId"": 3,
                    ""sessionId"": ""1"",
                    ""viewTypeId"": ""map"",
                    ""aggregateKeys"": [
                        ""site_level""
                    ]
                }
            }";

        public DebugTests(TestHostWithContainer hostBuilderFixture) : base(hostBuilderFixture)
        {
        }

        protected StringContent FakeLoadResultPayload(FacetsConfig2 facetsConfig, ResultConfig resultConfig)
        {
            var bothConfigs = new ResultControllerTests.LoadPayload { facetsConfig = facetsConfig, resultConfig = resultConfig };
            var payload = new StringContent(JsonConvert.SerializeObject(bothConfigs), Encoding.UTF8, "application/json");
            return payload;
        }


        [Fact]
        public async Task BuggTest2()
        {
            var payload = new StringContent(jsonThatFails, Encoding.UTF8, "application/json");
            var response = await Fixture.Client.PostAsync("api/result/load", payload);

            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<ResultContentSet>(responseContent);

            Assert.NotNull(result);
            Assert.NotNull(result.Data);
            Assert.NotNull(result.Data.DataCollection);
            Assert.NotNull(result.Meta);
            Assert.NotNull(result.Meta.Columns);
            Assert.True(result.Meta.Columns.Count > 0);
            Assert.NotEmpty(result.Query);

            var sqlQuery = result.Query.Squeeze();

            var matcher = new MapResultSqlCompilerMatcher();
            var match = matcher.Match(sqlQuery);

            Assert.True(match.Success);
        }

    }
}
