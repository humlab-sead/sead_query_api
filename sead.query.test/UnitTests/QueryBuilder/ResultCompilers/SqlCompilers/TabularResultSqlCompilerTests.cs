using System.Collections.Generic;
using System.Linq;
using SeadQueryCore;
using SQT.CollectionFixtures;
using SQT.Infrastructure;
using SQT.SQL.Matcher;
using Xunit;

namespace SQT.QueryBuilder.ResultCompilers
{
    [Collection("UsePostgresFixture")]
    public class TabularResultSqlCompilerTests : MockerWithFacetContext
    {
        public TabularResultSqlCompilerTests()
            : base() { }

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
        public void Compile_WithComposedHandoffQuerySetup_PrependsLeadingSqlAndUsesHandoffJoinCriteria()
        {
            // Arrange
            var fakeFacetsConfig = FakeFacetsConfig("sites:sites");
            var fakeQuerySetup = FakeResultQuerySetup(fakeFacetsConfig, "result_facet", "site_level");
            fakeQuerySetup.LeadingSql = "with composed_filter as (select 1 as target_id)";
            fakeQuerySetup.Joins = new List<string> { " join composed_filter on composed_filter.target_id = tbl_sites.site_id" };
            fakeQuerySetup.Criterias = new List<string> { "composed_filter.target_id > 0" };
            var fields = FakeResultConfig("result_facet", "site_level", "tabular").GetSortedFields();

            // Act
            var compiler = new TabularResultSqlCompiler();
            var result = compiler.Compile(fakeQuerySetup, fakeQuerySetup.Facet, fields);

            // Assert
            Assert.Contains(fakeQuerySetup.LeadingSql, result);
            Assert.Contains(fakeQuerySetup.Joins[0], result);
            Assert.Contains(fakeQuerySetup.Criterias[0], result);
        }

        [Fact]
        public void Compile_WithOrderedResultFields_PreservesAliasCompiledValueAndGroupingSemantics()
        {
            // Arrange
            var fakeFacetsConfig = FakeFacetsConfig("sites:sites");
            var fakeQuerySetup = FakeResultQuerySetup(fakeFacetsConfig, "result_facet", "site_level");
            var fields = FakeOrderedResultFields();

            // Act
            var compiler = new TabularResultSqlCompiler();
            var result = compiler.Compile(fakeQuerySetup, fakeQuerySetup.Facet, fields);

            // Assert
            Assert.Contains(
                "SELECT alias_1, ARRAY_TO_STRING(ARRAY_AGG(DISTINCT alias_2),',') AS text_agg_of_alias_2, COUNT(alias_3) AS count_of_alias_3, alias_4, alias_5",
                result
            );
            Assert.Contains(
                "SELECT tbl_sites.site_name AS alias_1, tbl_record_types.record_type_name AS alias_2, tbl_analysis_entities.analysis_entity_id AS alias_3, tbl_sites.site_id AS alias_4, tbl_sites.site_id AS alias_5, tbl_sites.site_name AS alias_6",
                result
            );
            Assert.Contains("GROUP BY alias_1, alias_2, alias_3, alias_4, alias_5, alias_6", result);
            Assert.Contains("GROUP BY alias_1, alias_4, alias_5, alias_6", result);
            Assert.Contains("ORDER BY alias_6", result);
        }

        private static List<ResultSpecificationField> FakeOrderedResultFields()
        {
            var specification = new ResultSpecification
            {
                SpecificationId = 1,
                SpecificationKey = "site_level",
                DisplayText = "Site level",
                IsActivated = true,
                Fields = new List<ResultSpecificationField>
                {
                    FakeResultSpecificationField(4, "single_item", 1, 1),
                    FakeResultSpecificationField(5, "text_agg_item", 2, 2),
                    FakeResultSpecificationField(8, "count_item", 3, 3),
                    FakeResultSpecificationField(10, "link_item", 4, 4),
                    FakeResultSpecificationField(13, "link_item_filtered", 5, 5),
                    FakeResultSpecificationField(16, "sort_item", 1, 99),
                },
            };

            return specification.GetSortedFields().ToList();
        }

        private static ResultSpecificationField FakeResultSpecificationField(
            int id,
            string fieldTypeId,
            int resultFieldId,
            int sequenceId
        ) =>
            new ResultSpecificationField
            {
                SpecificationFieldId = id,
                SpecificationId = 1,
                FieldTypeId = fieldTypeId,
                ResultFieldId = resultFieldId,
                SequenceId = sequenceId,
                FieldType = ResultSpecificationFieldExtensionTests.ResultFieldTypes[fieldTypeId],
                ResultField = ResultSpecificationFieldExtensionTests.ResultFields[resultFieldId],
            };
    }
}
