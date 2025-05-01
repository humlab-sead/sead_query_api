using SeadQueryAPI.Serializers;
using SeadQueryCore;
using Xunit;

namespace SQT.Infrastructure
{
    [Collection("SeadJsonFacetContextFixture")]
    public class FacetConfigReconstituteServiceTests : JsonSeededFacetContextContainer
    {
        public FacetConfigReconstituteServiceTests(SeadJsonFacetContextFixture fixture) : base(fixture)
        {
        }

        private IFacetConfigReconstituteService CreateService()
        {
            FacetConfigReconstituteService service = new FacetConfigReconstituteService(Registry);
            return service;
        }

        [Fact]
        public void Reconstitute_UsingLowerCaseJsonFacetsConfig_ReturnsExpectedResult()
        {
            // Arrange
            var service = this.CreateService();
            const string json = @" {
                ""requestId"": 1,
                ""requestType"": ""populate"",
                ""targetCode"": ""sites"",
                ""facetConfigs"": [
                    {
                    ""facetCode"": ""sites"",
                    ""position"": 1,
                    ""picks"": []
                    }
                ]
            }
            ";

            // Act

            FacetsConfig2 facetsConfig = service.Reconstitute(json);

            // Assert
            Assert.NotNull(facetsConfig);
        }

        [Fact]
        public void Reconstitute_UsingUpperCaseJsonFacetsConfig_ReturnsExpectedResult()
        {
            // Arrange
            var service = this.CreateService();
            const string json = @"{ ""FacetsConfig"": { ""RequestId"": 1, ""RequestType"": ""populate"", ""TargetCode"": ""sites"", ""FacetConfigs"": [ { ""FacetCode"": ""sites"", ""Position"": 1, ""Picks"": [], ""TextFilter"": """" }]}, ""ResultConfig"": { ""RequestId"": 1, ""SessionId"": ""1"", ""ViewTypeId"": ""map"", ""AggregateKeys"": [""site_level""]}}";

            // Act
            var facetsConfig = service.Reconstitute(json);

            // Assert
            Assert.NotNull(facetsConfig);
        }


        [Fact]
        public void Reconstitute_OfSingleDiscreteFacetWithEnvelope_ReturnsExpectedResult()
        {
            // Arrange
            var service = this.CreateService();
            const string json = @"{
                ""FacetsConfig"": {
                    ""RequestId"": 1,
                    ""RequestType"": ""populate"",
                    ""TargetCode"": ""sites"",
                    ""FacetConfigs"": [ {
                        ""FacetCode"": ""sites"",
                        ""Position"": 1,
                        ""Picks"": [],
                        ""TextFilter"": """"
                    } ]
                }
            }";

            // Act
            var facetsConfig = service.Reconstitute(json);

            // Assert
            Assert.NotNull(facetsConfig);
        }

        [Fact]
        public void Reconstitute_OfSingleDiscreteFacetWithoutEnvelope_ReturnsExpectedResult()
        {
            // Arrange
            var service = this.CreateService();
            const string json = @"{
                ""RequestId"": 1,
                ""RequestType"": ""populate"",
                ""TargetCode"": ""sites"",
                ""FacetConfigs"": [ {
                    ""FacetCode"": ""sites"",
                    ""Position"": 1,
                    ""Picks"": [],
                    ""TextFilter"": """"
                } ]
            }";

            // Act
            var facetsConfig = service.Reconstitute(json);

            // Assert
            Assert.NotNull(facetsConfig);
        }

        [Fact]
        public void Reconstitute_OfSingleRangeFacetWithEnvelope_ReturnsExpectedResult()
        {
            // Arrange
            var service = this.CreateService();
            const string json = @"{
                ""FacetsConfig"": {
                    ""RequestId"": 1,
                    ""RequestType"": ""populate"",
                    ""TargetCode"": ""tbl_denormalized_measured_values_33_0"",
                    ""FacetConfigs"": [ {
                        ""FacetCode"": ""tbl_denormalized_measured_values_33_0"",
                        ""Position"": 1,
                        ""Picks"": [],
                        ""TextFilter"": """"
                    } ]
                }
            }";

            // Act
            var facetsConfig = service.Reconstitute(json);

            // Assert
            Assert.NotNull(facetsConfig);
        }

        [Fact]
        public void Reconstitute_OfSingleRangeFacetWithoutEnvelope_ReturnsExpectedResult()
        {
            // Arrange
            var service = this.CreateService();
            const string json = @"{
                ""RequestId"": 1,
                ""RequestType"": ""populate"",
                ""TargetCode"": ""tbl_denormalized_measured_values_33_0"",
                ""FacetConfigs"": [ {
                    ""FacetCode"": ""tbl_denormalized_measured_values_33_0"",
                    ""Position"": 1,
                    ""Picks"": [],
                    ""TextFilter"": """"
                } ]
            }";

            // Act
            var facetsConfig = service.Reconstitute(json);

            // Assert
            Assert.NotNull(facetsConfig);
        }
    }
}
