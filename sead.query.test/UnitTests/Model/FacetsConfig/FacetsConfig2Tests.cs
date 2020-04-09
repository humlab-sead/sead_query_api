using KellermanSoftware.CompareNetObjects;
using Newtonsoft.Json;
using SeadQueryAPI.Serializers;
using SeadQueryCore;
using SeadQueryInfra;
using SeadQueryTest.Infrastructure;
using SeadQueryTest.Mocks;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using static SeadQueryCore.FacetsConfig2;

namespace SeadQueryTest.Model.FacetsConfig
{
    public class FacetsConfig2Tests : DisposableFacetContextContainer
    {
        private readonly MockFacetsConfigFactory FacetsConfigFactory;

        public FacetsConfig2Tests(JsonSeededFacetContextFixture fixture) : base(fixture)
        {
            FacetsConfigFactory = new MockFacetsConfigFactory(Registry.Facets);
        }

        [Fact]
        public void Reconstitute_SingleFacetsConfigWithoutPicks_IsEqual()
        {
            // Arrange
            var reconstituter = new FacetConfigReconstituteService(Registry);
            FacetsConfig2 facetsConfig = FacetsConfigFactory.CreateSingleFacetsConfigWithoutPicks("sites");
            string json1 = JsonConvert.SerializeObject(facetsConfig);
            // Act
            FacetsConfig2 facetsConfig2 = reconstituter.Reconstitute(json1);
            string json2 = JsonConvert.SerializeObject(facetsConfig2);
            // Assert
            Assert.Equal(json1, json2);
        }

        [Theory]
        [InlineData("sites:sites", "sites")]
        [InlineData("ecocode:sites", "sites")]
        public void GetConfig_VariousFacetsConfig_ExpectedBehavior(string uri, string facetCode)
        {
            // Arrange
            var facetsConfig2 = new MockFacetsConfigFactory(Registry.Facets).Create(uri);

            // Act
            var result = facetsConfig2.GetConfig(facetCode);

            // Assert
            Assert.Equal(facetCode, result.FacetCode);
        }

        //"target-facet[@trigger-facet]:(facet-code[@picks])(/facet-code[@picks])*
        [Theory]
        [InlineData("sites:sites", "sites")]
        [InlineData("ecocode:sites/ecocode", "sites", "ecocode")]
        [InlineData("sites:sites@1/ecocode@2", "sites", "ecocode")]
        public void GetFacetCodes_VariousFacetsConfig_ExpectedBehavior(string uri, params string[] facetCodes)
        {
            // Arrange
            var facetsConfig2 = new MockFacetsConfigFactory(Registry.Facets).Create(uri);

            // Act
            var result = facetsConfig2.GetFacetCodes();

            // Assert
            Assert.True(new CompareLogic().Compare(result, new List<string>(facetCodes)).AreEqual);
        }

        [Theory]
        [InlineData("sites:sites@1", "sites")]
        [InlineData("ecocode:sites/ecocode")]
        [InlineData("sites:sites@1/ecocode@2", "sites", "ecocode")]
        public void GetFacetConfigsWithPicks_VariousFacetsConfig_ExpectedBehavior(string uri, params string[] facetCodes)
        {
            // Arrange
            var facetsConfig2 = new MockFacetsConfigFactory(Registry.Facets).Create(uri);

            // Act
            var result = facetsConfig2.GetFacetConfigsWithPicks();

            // Assert
            Assert.Equal(facetCodes.Length, result.Count);
        }

        //[Theory]
        //[InlineData("sites:sites@1", "sites")]
        //[InlineData("ecocode:sites/ecocode", "ecocode")]
        //[InlineData("sites:sites@1,2/ecocode@2", "sites", "ecocode")]
        //public void GetFacetConfigsAffectedByFacet_StateUnderTest_ExpectedBehavior(string uri, string targetCode, params string[] facetCodes)
        //{
        //    // Arrange
        //    var facetsConfig2 = new MockFacetsConfigFactory(Registry.Facets).Create(uri);
        //    Facet targetFacet = Registry.Facets.GetByCode(targetCode);

        //    // Act
        //    var result = facetsConfig2.GetFacetConfigsAffectedBy(targetFacet, facetCodes.ToList());

        //    // Assert
        //    Assert.True(false);
        //}

        [Theory]
        [InlineData("sites:sites@1", "sites:1")]
        [InlineData("ecocode:sites/ecocode", "")]
        [InlineData("sites:sites@1,2/ecocode@2", "sites:1,2", "ecocode:2")]
        public void CollectUserPicks_VariousFacetsConfig_ExpectedBehavior(string uri, params string[] expected)
        {
            // Arrange
            var facetsConfig2 = new MockFacetsConfigFactory(Registry.Facets).Create(uri);

            // Act
            var result = facetsConfig2.CollectUserPicks();

            // Assert
            var expectedDictionary = new List<string>(expected)
                .Where(z => z.Length > 0)
                .Select(z => z.Split(':'))
                .Select(z => (FacetCode: z[0], Picks: ToDecimals(z[1])))
                .Select(z => new UserPickData
                {
                    FacetCode = z.FacetCode,
                    FacetType = Registry.Facets.GetByCode(z.FacetCode).FacetTypeId,
                    PickValues = z.Picks,
                    Title = Registry.Facets.GetByCode(z.FacetCode).DisplayTitle
                })
                .ToDictionary(z => z.FacetCode);

            Assert.True(new CompareLogic().Compare(result, expectedDictionary).AreEqual);
        }

        private static List<decimal> ToDecimals(string z)
            => z.Split(',').Select(x => Convert.ToDecimal(x)).ToList<decimal>();

        [Theory]
        [InlineData("sites:sites@1", EFacetType.Discrete, true)]
        [InlineData("ecocode:sites/ecocode", EFacetType.Discrete, false)]
        [InlineData("sites:sites@1,2/ecocode@2", EFacetType.Discrete, true)]
        public void HasPicks_VariousFacetsConfig_ExpectedBehavior(string uri, EFacetType facetType, bool hasPicks)
        {
            // Arrange
            var facetsConfig2 = new MockFacetsConfigFactory(Registry.Facets).Create(uri);

            // Act
            var result = facetsConfig2.HasPicks(facetType);

            // Assert
            Assert.Equal(hasPicks, result);
        }

        [Theory]
        [InlineData("sites:sites@1", true)]
        [InlineData("ecocode:sites/ecocode", false)]
        [InlineData("sites:sites@1,2/ecocode@2",  true)]
        public void ClearPicks_HasOrHasNotPicks_ExpectedBehavior(string uri, bool hasPicks)
        {
            // Arrange
            var facetsConfig2 = new MockFacetsConfigFactory(Registry.Facets).Create(uri);
            Assert.Equal(hasPicks, facetsConfig2.HasPicks());

            // Act
            var result = facetsConfig2.ClearPicks();

            // Assert
            Assert.False(facetsConfig2.HasPicks());
        }
    }
}
