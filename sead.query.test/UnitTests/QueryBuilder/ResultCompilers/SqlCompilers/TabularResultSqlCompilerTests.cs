using SeadQueryCore;
using SQT.CollectionFixtures;
using SQT.Infrastructure;
using SQT.SQL.Matcher;
using System.Linq;
using Xunit;

namespace SQT.QueryBuilder.ResultCompilers
{
    [Collection("UsePostgresFixture")]
    public class TabularResultSqlCompilerTests : MockerWithFacetContext
    {
        public TabularResultSqlCompilerTests() : base()
        {
        }

        [Theory]
        [InlineData("constructions:constructions", "result_facet", "site_level", "tabular")]
        [InlineData("sites:sites", "result_facet", "site_level", "tabular")]
        [InlineData("sites:country@10/sites", "result_facet", "site_level", "tabular")]
        [ClassData(typeof(CompleteSetOfSingleTabularResultUriCollection))]
        public void Compile_TabularResult_Matches(string uri, string resultFacetCode, string specificationKey, string viewType)
        {
            // Arrange
            var fakeFacetsConfig = FakeFacetsConfig(uri);
            var fakeQuerySetup = FakeResultQuerySetup(fakeFacetsConfig, resultFacetCode, specificationKey);
            var facet = fakeQuerySetup.Facet;
            var fields = FakeResultConfig(resultFacetCode, specificationKey, viewType).GetSortedFields();

            // Act
            var compiler = new TabularResultSqlCompiler();
            var result = compiler.Compile(fakeQuerySetup, facet, fields);

            var match = new TabularResultSqlCompilerMatcher().Match(result);

            // Assert
            Assert.True(match.Success);
            Assert.True(match.InnerSelect.Success);
            Assert.NotEmpty(match.InnerSelect.Tables);
        }

        [Fact]
        public void Compile_WithCountryField_AddsLeftJoinCountryLookup()
        {
            // Arrange
            var fakeFacetsConfig = FakeFacetsConfig("sites:sites");
            var fakeQuerySetup = FakeResultQuerySetup(fakeFacetsConfig, "result_facet", "site_level");
            var fields = FakeResultConfig("result_facet", "site_level", "tabular").GetSortedFields().ToList();
            var singleItemType = fields.First(z => z.FieldType?.FieldTypeId == "single_item").FieldType;
            fields.Add(new ResultSpecificationField
            {
                FieldType = singleItemType,
                FieldTypeId = singleItemType.FieldTypeId,
                ResultField = new ResultField
                {
                    ResultFieldKey = "country",
                    ColumnName = "site_country.country_name",
                    DisplayText = "Country",
                    FieldType = singleItemType
                }
            });

            // Act
            var compiler = new TabularResultSqlCompiler();
            var result = compiler.Compile(fakeQuerySetup, fakeQuerySetup.Facet, fields);

            // Assert
            Assert.Contains("LEFT JOIN (", result);
            Assert.Contains("country_locations.location_type_id = 1", result);
            Assert.Contains("site_country.site_id = tbl_sites.site_id", result);
        }
    }
}
