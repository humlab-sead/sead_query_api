using Moq;
using SeadQueryCore;
using SeadQueryCore.QueryBuilder;
using SQT.Fixtures;
using SQT.Infrastructure;
using System;
using System.Text.RegularExpressions;
using Xunit;

namespace SQT.SqlCompilers
{
    [Collection("JsonSeededFacetContext")]
    public class RangeCategoryCountSqlCompilerTests : DisposableFacetContextContainer
    {
        public RangeCategoryCountSqlCompilerTests(JsonSeededFacetContextFixture fixture) : base(fixture)
        {
        }

        [Theory]
        [InlineData("tbl_denormalized_measured_values_33_0:tbl_denormalized_measured_values_33_0")]
        public void Compile_VariousConfigs_ExpectedBehavior(string uri)
        {
            // Arrange
            var mockQuerySetupFactory = new MockQuerySetupFactory(Registry);
            var querySetup = mockQuerySetupFactory.Scaffold(uri);
            var facet = querySetup.Facet;

            var intervalQuery = "#INTERVAL-QUERY#";
            var countColumn = "#COUNT-COLUMN#";

            // Act
            var rangeCategoryCountSqlCompiler = new RangeCategoryCountSqlCompiler();
            var result = rangeCategoryCountSqlCompiler.Compile(
                querySetup,
                facet,
                intervalQuery,
                countColumn);

            // Assert
            result = result.Squeeze();

            //
            //WITH categories(category, lower, upper) AS ( #INTERVAL-QUERY# ), outerbounds(lower, upper) AS ( SELECT MIN(lower), MAX(upper) FROM categories ) SELECT c.category, c.lower, c.upper, COALESCE(r.count_column, 0) as count_column FROM categories c LEFT JOIN ( SELECT category, COUNT(DISTINCT #COUNT-COLUMN#) AS count_column FROM facet.method_measured_values(33,0) AS method_values_33 CROSS JOIN outerbounds JOIN categories ON categories.lower <= cast(method_values_33.measured_value as decimal(15, 6)) AND categories.upper >= cast(method_values_33.measured_value as decimal(15, 6)) AND (NOT (categories.upper < outerbounds.upper AND cast(method_values_33.measured_value as decimal(15, 6)) = categories.upper)) WHERE TRUE GROUP BY category ) AS r ON r.category = c.category ORDER BY c.lower"

            // SELECT c.category, c.lower, c.upper, COALESCE(r.count_column, 0) as count_column FROM categories c LEFT JOIN ( SELECT category, COUNT(DISTINCT #COUNT-COLUMN#) AS count_column FROM facet.method_measured_values(33,0) AS method_values_33 CROSS JOIN outerbounds JOIN categories ON categories.lower <= cast(method_values_33.measured_value as decimal(15, 6)) AND categories.upper >= cast(method_values_33.measured_value as decimal(15, 6)) AND (NOT (categories.upper < outerbounds.upper AND cast(method_values_33.measured_value as decimal(15, 6)) = categories.upper)) WHERE TRUE GROUP BY category ) AS r ON r.category = c.category ORDER BY c.lower"
            //var categorySql = @"categories\(category, lower, upper\) AS \( #INTERVAL-QUERY# \)";
            //var boundSql = @"outerbounds\(lower, upper\) AS \( SELECT MIN\(lower\), MAX\(upper\) FROM categories \)";
            //var outerSql = $@"SELECT c.category, c.lower, c.upper, COALESCE(r.count_column, 0) as count_column FROM categories c LEFT JOIN ( (?<innerSql>) ";
            var expectedSql = @"WITH (?<categorySql>categories.*\( #INTERVAL-QUERY# \)), (?<boundSql>\s?outerbounds.*FROM categories \)\s?)\s*(?<outerSql>SELECT.*FROM.*LEFT JOIN \((?<innerSql>.*)\)) AS r ON.*";

            var rx = Regex.Match(result, expectedSql);
            Assert.True(rx.Success);
            Assert.Matches(expectedSql, result);

            // Won't test these further (not dependent on input)
            //var categorySql = rx.Groups["categorySql"];
            //var boundSql = rx.Groups["boundSql"];
            //var outerSql = rx.Groups["outerSql"];

            var innerSql = rx.Groups["innerSql"];
            //SELECT category, COUNT(DISTINCT #COUNT-COLUMN#) AS count_column FROM facet.method_measured_values(33,0) AS method_values_33 CROSS JOIN outerbounds JOIN categories ON categories.lower <= cast(method_values_33.measured_value as decimal(15, 6)) AND categories.upper >= cast(method_values_33.measured_value as decimal(15, 6)) AND (NOT (categories.upper < outerbounds.upper AND cast(method_values_33.measured_value as decimal(15, 6)) = categories.upper)) WHERE TRUE GROUP BY category )";
            //var expectedInnerSql = @"\s?SELECT .* FROM (?<targetTable>.*) CROSS JOIN outerbounds JOIN categories ON categories.lower <= cast(method_values_33.measured_value as decimal(15, 6)) AND categories.upper >= cast(method_values_33.measured_value as decimal(15, 6)) AND (NOT (categories.upper < outerbounds.upper AND cast(method_values_33.measured_value as decimal(15, 6)) = categories.upper)) WHERE TRUE GROUP BY category )";
            // FIXME: Further matching

        }
    }
}
