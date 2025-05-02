using SeadQueryCore;
using SeadQueryCore.Model.Ext;
using SQT.Infrastructure;
using SQT.SQL.Matcher;
using System.Collections.Generic;
using Xunit;

#pragma warning disable RCS1163, IDE0060

namespace SQT.SqlCompilers
{
    [Collection("SeadJsonFacetContextFixture")]
    public class MapResultSqlCompilerTests : MockerWithFacetContext
    {
        public MapResultSqlCompilerTests(SeadJsonFacetContextFixture fixture) : base(fixture)
        {
        }

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
        public void Compile_DomainFacetsWithSingleChildFacet_HasExpectedSqlQuery(string uri, string resultFacetCode, string specificationKey, string viewType)
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
    }
}
