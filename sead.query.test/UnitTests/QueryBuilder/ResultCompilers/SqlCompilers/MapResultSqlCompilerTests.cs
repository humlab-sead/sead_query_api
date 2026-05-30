using System.Collections.Generic;
using SeadQueryCore;
using SQT.Infrastructure;
using SQT.SQL.Matcher;
using Xunit;

#pragma warning disable RCS1163, IDE0060

namespace SQT.SqlCompilers
{
    [Collection("UsePostgresFixture")]
    public class MapResultSqlCompilerTests : MockerWithFacetContext
    {
        public MapResultSqlCompilerTests()
            : base() { }

        [Theory]
        [InlineData("sites:data_types@5/rdb_codes@13,21/sites", "result_facet", "site_level")]
        [InlineData("sites:sites", "result_facet", "site_level")]
        [InlineData("sites:country/sites", "result_facet", "site_level")]
        public void Compile_MapResult_Matches(string uri, string resultFacetCode, string specificationKey)
        {
            // Arrange
            var fakeFacetsConfig = FakeFacetsConfig(uri);
            var fakeQuerySetup = FakeResultQuerySetup(fakeFacetsConfig, resultFacetCode, specificationKey);
            var fakeResultFields = FakeResultConfig(resultFacetCode, specificationKey, "map").GetSortedFields();

            // Act
            var sqlCompiler = new MapResultSqlCompiler();
            var result = sqlCompiler.Compile(fakeQuerySetup, fakeQuerySetup.Facet, fakeResultFields);

            // Assert
            var matcher = new MapResultSqlCompilerMatcher();
            var match = matcher.Match(result);

            Assert.True(match.Success);
        }

        [Theory]
        [ClassData(typeof(SQT.CollectionFixtures.CompleteSetOfSingleTabularResultUriCollection))]
        public void Compile_DomainFacetsWithSingleChildFacet_HasExpectedSqlQuery(
            string uri,
            string resultFacetCode,
            string specificationKey,
            string viewType
        )
        {
            // Arrange
            var fakeFacetsConfig = FakeFacetsConfig(uri);
            var fakeQuerySetup = FakeResultQuerySetup(fakeFacetsConfig, resultFacetCode, specificationKey);
            var fakeResultFields = FakeResultConfig(resultFacetCode, specificationKey, "map").GetSortedFields();

            // Act
            var sqlCompiler = new MapResultSqlCompiler();
            var result = sqlCompiler.Compile(fakeQuerySetup, fakeQuerySetup.Facet, fakeResultFields);

            // Assert
            var matcher = new MapResultSqlCompilerMatcher();
            var match = matcher.Match(result);

            Assert.True(match.Success);

            Assert.Equal(viewType, viewType);
        }

        [Fact]
        public void Compile_WithComposedHandoffQuerySetup_PrependsLeadingSqlAndUsesHandoffJoinCriteria()
        {
            // Arrange
            var fakeFacetsConfig = FakeFacetsConfig("sites:sites");
            var fakeQuerySetup = FakeResultQuerySetup(fakeFacetsConfig, "result_facet", "site_level");
            fakeQuerySetup.LeadingSql = "with composed_filter as (select 1 as target_id)";
            fakeQuerySetup.Joins = new List<string> { " join composed_filter on composed_filter.target_id = tbl_sites.site_id" };
            fakeQuerySetup.Criterias = new List<string> { "composed_filter.target_id > 0" };
            var fakeResultFields = FakeResultConfig("result_facet", "site_level", "map").GetSortedFields();

            // Act
            var sqlCompiler = new MapResultSqlCompiler();
            var result = sqlCompiler.Compile(fakeQuerySetup, fakeQuerySetup.Facet, fakeResultFields);

            // Assert
            Assert.Contains(fakeQuerySetup.LeadingSql, result);
            Assert.Contains(fakeQuerySetup.Joins[0], result);
            Assert.Contains(fakeQuerySetup.Criterias[0], result);
        }
    }
}
