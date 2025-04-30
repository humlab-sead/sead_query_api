using IntegrationTests.Sead;
using Newtonsoft.Json;
using SeadQueryCore;
using SeadQueryCore.Model;
using SQT;
using SQT.Infrastructure;
using SQT.SQL.Matcher;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using System.Text;
using System.Collections.Generic;

namespace IntegrationTests.Debug
{
    [Collection("SeadJsonFacetContextFixture")]
    public class DebugTests : ControllerTest<TestHostWithContainer>, IClassFixture<TestHostWithContainer>, IClassFixture<PostgresTestcontainerFixture>
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
        public JsonFacetContextFixture FacetContextFixture { get; }
        public DisposableFacetContextContainer MockService { get; }

        public DebugTests(TestHostWithContainer hostBuilderFixture, SeadJsonFacetContextFixture facetContextFixture) : base(hostBuilderFixture)
        {
            FacetContextFixture = facetContextFixture;
            MockService = new DisposableFacetContextContainer(facetContextFixture);
        }

        protected StringContent FakeLoadResultPayload(FacetsConfig2 facetsConfig, ResultConfig resultConfig)
        {
            var bothConfigs = new ResultControllerTests.LoadPayload { facetsConfig = facetsConfig, resultConfig = resultConfig };
            var payload = new StringContent(JsonConvert.SerializeObject(bothConfigs), Encoding.UTF8, "application/json");
            return payload;
        }

        // [Fact]
        // public void BuggTestByService()
        // {
        //     var data = JObject.Parse(jsonThatFails);
        //     var facetsConfigDTO = data["facetsConfig"].ToObject<FacetsConfig2>();
        //     var resultConfigDTO = data["resultConfig"].ToObject<ResultConfigDTO>();
        //     var facetsConfig = new FacetConfigReconstituteService(MockService.Registry).Reconstitute(facetsConfigDTO);
        //     var resultConfig = new ResultConfigReconstituteService(MockService.Registry).Reconstitute(resultConfigDTO);

        //     IContainer container = DependencyService.CreateContainer(MockService.FacetContext, "PAP", MockService.Settings);

        //     ILoadResultService service = container.Resolve<ILoadResultService>();
        //     service.Load(facetsConfig, resultConfig).Nullify();
        // }

        [Fact]
        public async Task BuggTest2()
        {
                // /home/sead/supersead.humlab.umu.se/postgresql/mounts/pg-data-volume/pg_hba.conf
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

        [Fact]
        public async Task BuggTest()
        {
            const string resultFacetCode = "map_result";
            const string specificationKey = "site_level";
            const string viewType = "map";

            var facetsConfig2 = new FacetsConfig2
            {
                RequestId = "3",
                RequestType = "populate",
                DomainCode = "",
                TargetCode = "ecocode",
                FacetConfigs = new List<FacetConfig2>
                {
                    new FacetConfig2
                    {
                        FacetCode = "ecocode",
                        Position = 1,
                        TextFilter = "",
                        Picks = new List<FacetConfigPick>
                        {
                            new FacetConfigPick { PickValue="36", Text="36"}
                        }
                    }
                }
            };

            var facet = MockService.Facets.GetByCode(resultFacetCode);
            var resultSpecification = MockService.Results.GetByKey(specificationKey);

            var resultConfig = new ResultConfig()
            {
                Facet = facet,
                FacetCode = facet.FacetCode,
                RequestId = "1",
                SessionId = "1",
                Specifications = new List<ResultSpecification> { resultSpecification },
                SpecificationKeys = new List<string> { specificationKey },
                ViewTypeId = viewType
            };

            var payload = FakeLoadResultPayload(facetsConfig2, resultConfig);

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
