using Moq;
using SeadQueryCore;
using SeadQueryCore.Model;
using SeadQueryCore.QueryBuilder;
using SeadQueryInfra;
using SQT.Fixtures;
using SQT.Infrastructure;
using SQT.Mocks;
using System;
using System.Text.RegularExpressions;
using Xunit;

namespace SQT.SqlCompilers
{
    [Collection("JsonSeededFacetContext")]
    public class MapResultSqlCompilerTests : DisposableFacetContextContainer
    {
        public MapResultSqlCompilerTests(JsonSeededFacetContextFixture fixture) : base(fixture)
        {
        }

        [Theory]
        [InlineData("sites:sites", "sites", "count")]
        [InlineData("sites:country/sites", "sites", "count")]
        public void Compile_StateUnderTest_ExpectedBehavior(string uri, string facetCode, string aggType)
        {
            // Arrange
            var querySetupMockFactory = new MockQuerySetupFactory(Registry);
            var querySetup = querySetupMockFactory.Scaffold(uri);
            var resultConfig = CreateResultSetup();
            var facet = Registry.Facets.GetByCode(facetCode);

            // Act

            var mapResultSqlCompiler = new MapResultSqlCompiler();
            var result = mapResultSqlCompiler.Compile(querySetup, facet, resultConfig);

            // Assert

            string expectedSql = $@"
                SELECT DISTINCT (?<idExpr>\w+) AS id_column, (?<nameExpr>\w+) AS name, coalesce(latitude_dd, 0.0) AS latitude_dd, coalesce(longitude_dd, 0) AS longitude_dd
                FROM (?<targetTable>\w+)
                     (?<additionalJoins>)
                WHERE 1 = 1
                (?<additionalCriteria>.*)
            ";
            
            expectedSql = expectedSql.Squeeze();
            result = result.Squeeze();

            var matches = Regex.Match(result, expectedSql);

            Assert.NotNull(matches);
        }

        private ResultQuerySetup CreateResultSetup()
        {
            ResultConfig resultConfig = CreateResultConfig();
            var resultFields = Registry.Results.GetFieldsByKeys(resultConfig.AggregateKeys);
            ResultQuerySetup resultQuerySetup = new ResultQuerySetup(resultFields);
            return resultQuerySetup;
        }

        private static ResultConfig CreateResultConfig() => new ResultConfig()
        {
            ViewTypeId = "map",
            RequestId = "1",
            SessionId = "1",
            AggregateKeys = new System.Collections.Generic.List<string> { "site_level" }
        };

        /*
    public class ResultQuerySetup
    {
        public List<ResultAggregateField> Fields { get; set; }
        public List<ResultField> ResultFields => Fields.Select(z => z.ResultField).ToList();
        public List<string> DataTables => Fields.Select(z => z.ResultField.TableName).Where(t => t != null).ToList();

        public List<(string, string)> AliasPairs { get; set; }
        public List<string> DataFields { get; set; }
        public List<string> GroupByFields { get; set; }
        public List<string> InnerGroupByFields { get; set; }
        public List<string> SortFields { get; set; }

        public ResultQuerySetup(List<ResultAggregateField> fields)
        {
            var aliasedFields = fields.Select((field, i) => new { Field = field, Alias = "alias_" + (i+1).ToString() });
            Fields = fields;
            InnerGroupByFields = aliasedFields.Select(p => p.Alias).ToList();
            GroupByFields = aliasedFields.Where(z => z.Field.FieldType.IsGroupByField).Select(z => z.Alias).ToList();
            AliasPairs = aliasedFields.Select(z => ((z.Field.ResultField.ColumnName, z.Alias))).ToList();
            SortFields = aliasedFields.Where(z => z.Field.FieldType.IsSortField).Select(z => z.Alias).ToList();
            DataFields = aliasedFields.Where(z => z.Field.FieldType.IsResultValue).Select(z => z.Field.FieldType.Compiler.Compile(z.Alias)).ToList();
        }

        public bool IsEmpty => (Fields?.Count ?? 0) == 0;
    }
         */
    }
}
