using Moq;
using Newtonsoft.Json.Linq;
using SeadQueryAPI.Serializers;
using SeadQueryCore;
using SeadQueryInfra;
using SeadQueryTest.Infrastructure.Scaffolding;
using System;
using Xunit;

namespace SeadQueryTest.Serializers
{
    public class FacetConfigReconstituteServiceTests
    {
        private IRepositoryRegistry Registry;

        public FacetConfigReconstituteServiceTests()
        {
            var mockSettings = new MockOptionBuilder().Build().Value.Facet;
            var mockContext = ScaffoldUtility.DefaultFacetContext();
            Registry = new RepositoryRegistry(mockContext);
        }


        private IFacetConfigReconstituteService CreateService()
        {
            FacetConfigReconstituteService service = new FacetConfigReconstituteService(Registry);
            return service;

        }

        //[Fact]
        //public void Reconstitute_StateUnderTest_ExpectedBehavior()
        //{
        //    // Arrange
        //    var service = this.CreateService();
        //    FacetConfig2 facetConfig = null;

        //    // Act
        //    var result = service.Reconstitute(
        //        facetConfig);

        //    // Assert
        //    Assert.True(false);
        //}

        //[Fact]
        //public void Reconstitute_StateUnderTest_ExpectedBehavior1()
        //{
        //    // Arrange
        //    var service = this.CreateService();
        //    FacetsConfig2 facetsConfig = null;

        //    // Act
        //    var result = service.Reconstitute(
        //        facetsConfig);

        //    // Assert
        //    Assert.True(false);
        //}

        //const string json = @"
        //        {
        //          ""facetsConfig"": {
        //            ""requestId"": 1,
        //            ""requestType"": ""populate"",
        //            ""targetCode"": ""sites"",
        //            ""triggerCode"": ""sites"",
        //            ""facetConfigs"": [
        //              {
        //                ""facetCode"": ""sites"",
        //                ""position"": 1,
        //                ""picks"": [],
        //                ""textFilter"": """"
        //              }
        //            ]
        //          },
        //          ""resultConfig"": {
        //            ""requestId"": 1,
        //            ""sessionId"": ""1"",
        //            ""viewTypeId"": ""map"",
        //            ""aggregateKeys"": [ ""site_level"" ]
        //          }
        //        }
        //    ";
        [Fact]
        public void Reconstitute_UsingLowerCaseJsonFacetsConfig_ReturnsExpectedResult()
        {
            // Arrange
            var service = this.CreateService();
            const string json = @" {
                ""requestId"": 1,
                ""requestType"": ""populate"",
                ""targetCode"": ""sites"",
                ""triggerCode"": ""sites"",
                ""facetConfigs"": [
                    {
                    ""facetCode"": ""sites"",
                    ""position"": 1,
                    ""picks"": [],
                    ""textFilter"": """"
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
            const string json = @"{ ""FacetsConfig"": { ""RequestId"": 1, ""RequestType"": ""populate"", ""TargetCode"": ""sites"", ""TriggerCode"": ""sites"", ""FacetConfigs"": [ { ""FacetCode"": ""sites"", ""Position"": 1, ""Picks"": [], ""TextFilter"": """" }]}, ""ResultConfig"": { ""RequestId"": 1, ""SessionId"": ""1"", ""ViewTypeId"": ""map"", ""AggregateKeys"": [""site_level""]}}";

            // Act
            var result = service.Reconstitute(json);

            FacetsConfig2 facetsConfig = service.Reconstitute(json);

            // Assert
            Assert.NotNull(facetsConfig);
        }

        [Fact]
        public void Reconstitute_WithoutTriggerCode_ReturnsExpectedResult()
        {
            // Arrange
            var service = this.CreateService();
            const string json = @"{ ""FacetsConfig"": { ""RequestId"": 1, ""RequestType"": ""populate"", ""TargetCode"": ""sites"", ""FacetConfigs"": [ { ""FacetCode"": ""sites"", ""Position"": 1, ""Picks"": [], ""TextFilter"": """" }]}, ""ResultConfig"": { ""RequestId"": 1, ""SessionId"": ""1"", ""ViewTypeId"": ""map"", ""AggregateKeys"": [""site_level""]}}";

            // Act
            var result = service.Reconstitute(json);

            FacetsConfig2 facetsConfig = service.Reconstitute(json);

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
            var result = service.Reconstitute(json);

            FacetsConfig2 facetsConfig = service.Reconstitute(json);

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
            var result = service.Reconstitute(json);

            FacetsConfig2 facetsConfig = service.Reconstitute(json);

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
            var result = service.Reconstitute(json);

            FacetsConfig2 facetsConfig = service.Reconstitute(json);

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
            var result = service.Reconstitute(json);

            FacetsConfig2 facetsConfig = service.Reconstitute(json);

            // Assert
            Assert.NotNull(facetsConfig);
        }

        //[Fact]
        //public void Reconstitute_StateUnderTest_ExpectedBehavior3()
        //{
        //    // Arrange
        //    var service = this.CreateService();
        //    JObject json = null;

        //    // Act
        //    var result = service.Reconstitute(
        //        json);

        //    // Assert
        //    Assert.True(false);
        //}
    }
}
