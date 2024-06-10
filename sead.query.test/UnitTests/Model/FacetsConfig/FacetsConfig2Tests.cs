using KellermanSoftware.CompareNetObjects;
using Newtonsoft.Json;
using SeadQueryAPI.Serializers;
using SeadQueryCore;
using SeadQueryInfra;
using SQT.Infrastructure;
using SQT.Mocks;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using static SeadQueryCore.FacetsConfig2;

namespace SQT.Model
{
    [Collection("SeadJsonFacetContextFixture")]
    public class FacetsConfig2Tests : DisposableFacetContextContainer
    {
        private readonly MockFacetsConfigFactory FacetsConfigFactory;

        public FacetsConfig2Tests(SeadJsonFacetContextFixture fixture) : base(fixture)
        {
            FacetsConfigFactory = new MockFacetsConfigFactory(Registry.Facets);
        }

        [Theory]
        [InlineData("country:country", "country:country.json")]
        [InlineData("country:country@1", "country:country@1.json")]
        public void UriToJSON(string uri, string filename)
        {
            var reconstituter = new FacetConfigReconstituteService(Registry);
            FacetsConfig2 facetsConfig = FacetsConfigFactory.Create(uri);
            // Serialize to JSON with formatting:
            facetsConfig.TargetFacet = null;
            string json = JsonConvert.SerializeObject(facetsConfig, Formatting.Indented);

            // string json = JsonConvert.SerializeObject(facetsConfig);
            // string tmpFolder = Environment.GetEnvironmentVariable("SEAD_QUERY_API_PROJECT_ROOT") + "/tmp";

            // Comine folder and filename:
            string path = System.IO.Path.Combine("/home/roger/source/sead_query_api/tmp", filename);

            // Write string json to text file:
            System.IO.File.WriteAllText(path, json);

        }

        [Fact]
        public void Reconstitute_SingleFacetsConfigWithoutPicks_IsEqual()
        {
            // Arrange
            var reconstituter = new FacetConfigReconstituteService(Registry);
            FacetsConfig2 facetsConfig = FacetsConfigFactory.Create("sites:sites");
            string json1 = JsonConvert.SerializeObject(facetsConfig);
            // Act
            FacetsConfig2 facetsConfig2 = reconstituter.Reconstitute(json1);
            string json2 = JsonConvert.SerializeObject(facetsConfig2);
            // Assert
            Assert.Equal(json1, json2);
        }


        [Theory]
        [InlineData("sites:sites", "sites")]
        [InlineData("country:country/sites", "sites")]
        [InlineData("country:country/sites", "country")]
        [InlineData("sites:country@5/sites", "sites")]
        [InlineData("sites:country@5/sites@4,5", "sites")]
        [InlineData("sites:dataset_master@2/dataset_methods@10/country@44/sites@4,5/", "country")]
        [InlineData("sites:dataset_master@2/dataset_methods@10/country@44/sites@4,5/geochronology@(0,100)", "country")]
        public void GetConfig_WhenConfigExists_ExpectedBehavior(string uri, string facetCode)
        {
            // Arrange
            var facetsConfig = FakeFacetsConfig(uri);

            // Act
            var result = facetsConfig.GetConfig(facetCode);

            // Assert
            Assert.Equal(facetCode, result.FacetCode);
        }

        //"target-facet[@trigger-facet]:(facet-code[@picks])(/facet-code[@picks])*
        [Theory]
        [InlineData("sites:sites", "sites")]
        [InlineData("country:country/sites", "country", "sites")]
        [InlineData("country:country/region/sites", "country", "region", "sites")]
        [InlineData("sites:country@5/sites", "country", "sites")]
        [InlineData("sites:sites@5/country@4,5", "sites", "country")]
        [InlineData("sites:dataset_master@2/dataset_methods@10/country@44/sites@4,5/", "dataset_master", "dataset_methods", "country", "sites")]
        [InlineData("sites:dataset_master@2/dataset_methods@10/country@44/sites@4,5/geochronology@(0,100)", "dataset_master", "dataset_methods", "country", "sites", "geochronology")]
        public void GetFacetCodes_VariousFacetsConfig_ExpectedBehavior(string uri, params string[] facetCodes)
        {
            // Arrange
            var facetsConfig = FakeFacetsConfig(uri);

            // Act
            var result = facetsConfig.GetFacetCodes();

            // Assert
            Assert.True(new CompareLogic().Compare(result, new List<string>(facetCodes)).AreEqual);
        }

        [Theory]
        [InlineData("sites:sites")]
        [InlineData("country:country/sites")]
        [InlineData("sites:country@5/sites", "country")]
        [InlineData("sites:country@5/sites@4,5", "country", "sites")]
        [InlineData("sites:dataset_master/dataset_methods@10/country@44/sites@4,5/", "dataset_methods", "country", "sites")]
        public void GetFacetConfigsWithPicks_VariousFacetsConfig_ExpectedBehavior(string uri, params string[] facetCodes)
        {
            // Arrange
            var facetsConfig = FakeFacetsConfig(uri);

            // Act
            var result = facetsConfig.GetFacetConfigsWithPicks();

            // Assert
            Assert.Equal(facetCodes.Length, result.Select(x => x.FacetCode).Intersect(facetCodes).Count());
        }

        public bool IsPriorTo(FacetConfig2 facetConfig, List<string> facetCodes, Facet targetFacet)
        {
            if (!facetConfig.HasConstraints())
            {
                // FIXME Is this a relevant condition?
                return false;
            }

            if (targetFacet.FacetCode == facetConfig.FacetCode)
                return targetFacet.FacetType.ReloadAsTarget;

            return facetCodes.IndexOf(targetFacet.FacetCode) > facetCodes.IndexOf(facetConfig.FacetCode);
        }

        /*
        SAMPLE TRACES FROM query_sead PHP APPLICATION

        PICK ADDITIONAL COUNTRY:

        sites:country@240,58,64/sites@1013,957,958/species/ecocode@28,12/           // facetCode: sites, facetCodes: country sites species ecocode
        => country: HAS-PICKS, INVOLVED
        => sites: HAS-PICKS, SKIPPING/POSITION
        => species: SKIPPING/NO-PICKS
        => ecocode: HAS-PICKS, SKIPPING/POSITION

        sites:country@240,58,64/sites@1013,957,958/species/ecocode@28,12/           // facetCode: result_facet, facetCodes: country result_facet sites species ecocode
        => country: HAS-PICKS, INVOLVED
        => result_facet: SKIPPING/NO-PICKS
        => sites: HAS-PICKS, SKIPPING/POSITION
        => species: SKIPPING/NO-PICKS
        => ecocode: HAS-PICKS, SKIPPING/POSITION

        species:country@240,58,64/sites@1013,957,958/species/ecocode@28,12/         // facetCode: species, facetCodes: country sites species ecocode
        => country: HAS-PICKS, INVOLVED
        => sites: HAS-PICKS, INVOLVED
        => species: SKIPPING/NO-PICKS
        => ecocode: HAS-PICKS, SKIPPING/POSITION

        species:country@240,58,64/sites@1013,957,958/species/ecocode@28,12/         // facetCode: abundances_all_helper, facetCodes: country sites abundances_all_helper species ecocode
        => country: HAS-PICKS, INVOLVED
        => sites: HAS-PICKS, INVOLVED
        => abundances_all_helper: SKIPPING/NO-PICKS
        => species: SKIPPING/NO-PICKS)
        => ecocode: HAS-PICKS, SKIPPING/POSITION

        ecocode:country@240,58,64/sites@1013,957,958/species/ecocode@28,12/         // facetCode: ecocode, facetCodes: country sites species ecocode
        => country: HAS-PICKS, INVOLVED
        => sites: HAS-PICKS, INVOLVED
        => species: SKIPPING/NO-PICKS
        => ecocode: HAS-PICKS, SKIPPING/POSITION

        ecocode:country@240,58,64/sites@1013,957,958/species/ecocode@28,12/         // facetCode: result_facet, facetCodes: country sites species result_facet ecocode
        => country: HAS-PICKS, INVOLVED
        => sites: HAS-PICKS, INVOLVED
        => species: SKIPPING/NO-PICKS
        => result_facet: SKIPPING/NO-PICKS
        => ecocode: HAS-PICKS, SKIPPING/POSITION

        PICK ADDITIONAL SITE:

        species:country@240,58,64/sites@1013,957,958,401/species/ecocode@28,12/       // facetCode: species, facetCodes: country sites species ecocode
        => country: HAS-PICKS, INVOLVED
        => sites: HAS-PICKS, INVOLVED
        => species: SKIPPING/NO-PICKS)
        => ecocode: HAS-PICKS, SKIPPING/POSITION

        species:country@240,58,64/sites@1013,957,958,401/species/ecocode@28,12/       // facetCode: abundances_all_helper, facetCodes: country sites abundances_all_helper species ecocode
        => country: HAS-PICKS, INVOLVED
        => sites: HAS-PICKS, INVOLVED
        => abundances_all_helper: SKIPPING/NO-PICKS
        => species: SKIPPING/NO-PICKS
        => ecocode: HAS-PICKS, SKIPPING/POSITION

        ecocode:country@240,58,64/sites@1013,957,958,401/species/ecocode@28,12/       // facetCode: ecocode, facetCodes: country sites species ecocode
        => country: HAS-PICKS, INVOLVED
        => sites: HAS-PICKS, INVOLVED
        => species: SKIPPING/NO-PICKS
        => ecocode: HAS-PICKS, SKIPPING/POSITION

        ecocode:country@240,58,64/sites@1013,957,958,401/species/ecocode@28,12/       // facetCode: result_facet, facetCodes: country sites species result_facet ecocode
        => country: HAS-PICKS, INVOLVED
        => sites: HAS-PICKS, INVOLVED
        => species: SKIPPING/NO-PICKS
        => result_facet: SKIPPING/NO-PICKS
        => ecocode: HAS-PICKS, SKIPPING/POSITION

        @:country@240,58,64/sites@1013,957,958,401/species/ecocode@28,12/                   // facetCode: result_facet, facetCodes: country sites species ecocode result_facet
        => country: HAS-PICKS, INVOLVED
        => sites: HAS-PICKS, INVOLVED
        => species: SKIPPING/NO-PICKS
        => ecocode: HAS-PICKS, INVOLVED
        => result_facet: SKIPPING/NO-PICKS

        PICK ADDITINAL ECOCODES:

        @:country@240,58,64/sites@1013,957,958,401/species/ecocode@28,12,24/                // facetCode: result_facet, facetCodes: country sites species ecocode result_facet
        => country: HAS-PICKS, INVOLVED
        => sites: HAS-PICKS, INVOLVED
        => species: SKIPPING/NO-PICKS
        => ecocode: HAS-PICKS, INVOLVED
        => result_facet: SKIPPING/NO-PICKS



        */

        [Theory]
        [InlineData(
            "sites:country@240,58,64/sites@1013,957,958/species/ecocode@28,12/",
            "country"
         )]
        [InlineData(
            "sites:country/sites@1013,957,958/species/ecocode@28,12/", "country"
         )]
        public void GetConfigsThatAffectsTarget_StateUnderTest_ExpectedBehavior(string uri, params string[] expectedInvolvedCodes)
        {
            // Arrange
            var facetsConfig = FakeFacetsConfig(uri);
            var facetCodes = facetsConfig.GetFacetCodes();
            var targetCode = facetsConfig.TargetCode;

            // Act
            var result = facetsConfig.GetConfigsThatAffectsTarget(targetCode, facetCodes.ToList());

            // Assert
            var involvedCodes = result.Select(x => x.FacetCode);
            Assert.True(new CompareLogic().Compare(involvedCodes.ToList(), expectedInvolvedCodes.ToList()).AreEqual);
        }

        [Theory]
        [InlineData("sites:sites@1", "sites:1")]
        [InlineData("ecocode:sites/ecocode")]
        [InlineData("sites:sites@1,2/ecocode@2", "sites:1,2", "ecocode:2")]
        [InlineData("sites:sites@1,2/geochronology@(0,100)", "sites:1,2", "geochronology:0,100")]
        public void CollectUserPicks_VariousFacetsConfig_ExpectedBehavior(string uri, params string[] expected)
        {
            // Arrange
            var facetsConfig = FakeFacetsConfig(uri);

            // Act
            var result = facetsConfig.CollectUserPicks();

            // Assert
            var expectedDictionary = new List<string>(expected)
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
            var facetsConfig = FakeFacetsConfig(uri);

            // Act
            var result = facetsConfig.HasPicks(facetType);

            // Assert
            Assert.Equal(hasPicks, result);
        }

        [Theory]
        [InlineData("sites:sites@1", true)]
        [InlineData("ecocode:sites/ecocode", false)]
        [InlineData("sites:sites@1,2/ecocode@2", true)]
        public void ClearPicks_HasOrHasNotPicks_ExpectedBehavior(string uri, bool hasPicks)
        {
            // Arrange
            var facetsConfig = FakeFacetsConfig(uri);
            Assert.Equal(hasPicks, facetsConfig.HasPicks());

            // Act
            facetsConfig.ClearPicks();

            // Assert
            Assert.False(facetsConfig.HasPicks());
        }
    }
}
