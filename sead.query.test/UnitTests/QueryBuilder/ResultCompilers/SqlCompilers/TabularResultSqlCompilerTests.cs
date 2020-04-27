using Moq;
using SeadQueryCore;
using SeadQueryCore.QueryBuilder;
using SQT.Infrastructure;
using SQT.SQL.Matcher;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace SQT.QueryBuilder.ResultCompilers
{
    public class ResultSqlMatcher : FacetLoadSqlMatcher
    {
        public class ResultInnerSqlMatcher : SelectClauseMatcher
        {
            public override string ExpectedSql { get; } =
                    @"SELECT (?<SelectFieldsSql>.*?(?= FROM))
                      FROM (?<TargetSql>[\w\."",\(\)]+)(?: AS \w*)?(?<JoinSql>.*)?
                      WHERE 1 = 1\s?(?<CriteriaSql>.*)?(?:\sGROUP BY (?<GroupByFieldsSql>.*))?".Squeeze();
        }

        public override string ExpectedSql { get; } = @"
            SELECT (?<SelectItems>.*)(?= FROM \()
            FROM \((?<InnerSql>.*)(?=\) AS X GROUP BY)\) AS X
            GROUP BY (?<GroupByFields>.*)(?= ORDER)(?: ORDER BY (?<OrderByFields>.*))?";

        public override SelectClauseMatcher InnerSqlMatcher { get; } = new ResultInnerSqlMatcher();

    }

    public class TabularResultSqlCompilerTests : DisposableFacetContextContainer
    {
        public TabularResultSqlCompilerTests(JsonSeededFacetContextFixture fixture) : base(fixture)
        {
        }

        [Theory]
        [InlineData("sites:sites")]
        [InlineData("sites:country@10/sites")]
        [InlineData("palaeoentomology://rdb_systems:rdb_systems", "tbl_rdb_systems", "tbl_analysis_entities", "tbl_datasets", "tbl_physical_samples")]
        [InlineData("palaeoentomology://rdb_codes:rdb_codes", "tbl_rdb_codes", "tbl_analysis_entities", "tbl_datasets", "tbl_physical_samples")]
        [InlineData("palaeoentomology://sample_group_sampling_contexts:sample_group_sampling_contexts", "tbl_sample_group_sampling_contexts", "tbl_sample_groups", "tbl_physical_samples", "tbl_analysis_entities", "tbl_datasets", "tbl_physical_samples")]
        [InlineData("palaeoentomology://data_types:data_types", "tbl_data_types", "tbl_analysis_entities", "tbl_datasets", "tbl_physical_samples")]
        [InlineData("archaeobotany://ecocode_system:ecocode_system", "tbl_ecocode_systems", "tbl_ecocode_systems", "tbl_analysis_entities", "tbl_datasets", "tbl_physical_samples")]
        [InlineData("archaeobotany://ecocode:ecocode", "tbl_ecocode_definitions", "tbl_ecocode_definitions", "tbl_analysis_entities", "tbl_datasets", "tbl_physical_samples")]
        public void Compile_StateUnderTest_ExpectedBehavior(string uri, params string[] expectedJoinTables)
        {
            // Arrange
            var querySetup = FakeQuerySetup(uri);
            var facet = querySetup.Facet;
            var fields = FakeResultAggregateFields("site_level", "tabular");

            // Act
            var compiler = new TabularResultSqlCompiler();
            var result = compiler.Compile(querySetup, facet, fields);

            var match = new ResultSqlMatcher().Match(result);

            // Assert
            Assert.True(match.Success);

            Assert.True(match.InnerSelect.Success);

            Assert.NotEmpty(match.InnerSelect.Tables);
            Assert.True(expectedJoinTables.All(x => match.InnerSelect.Tables.Contains(x)));
        }
 
    }
}
