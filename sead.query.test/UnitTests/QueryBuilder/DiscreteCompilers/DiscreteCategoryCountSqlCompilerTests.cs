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
    public class DiscreteCategoryCountSqlCompilerTests : DisposableFacetContextContainer
    {
        public DiscreteCategoryCountSqlCompilerTests(JsonSeededFacetContextFixture fixture) : base(fixture)
        {
        }

        [Theory]
        [InlineData("sites:sites", "sites", "count", 0)]
        [InlineData("country:sites", "sites", "count", 1)]
        [InlineData("country@57:sites@3", "sites", "count", 1)]
        public void Compile_VariousConfigs_ExpectedBehavior(string uri, string facetCode, string aggType, int expectedJoinCount)
        {
            // Arrange
            var mockQuerySetupFactory = new MockQuerySetupFactory(Registry);
            var querySetup = mockQuerySetupFactory.Scaffold(uri);
            var facet = Registry.Facets.GetByCode(facetCode);
            var countFacet = facet;

            // Act

            var discreteCategoryCountSqlCompiler = new DiscreteCategoryCountSqlCompiler();
            var result = discreteCategoryCountSqlCompiler.Compile(querySetup, facet, countFacet, aggType);

            result = result.Squeeze();

            // Assert

            var outerExpected = $@"
                SELECT category, {aggType}\(value\) AS count
                FROM \((?<innerSql>.*)\) AS x
                GROUP BY category;".Squeeze();
            Assert.Matches(outerExpected, result);

            var rx = Regex.Match(result, outerExpected);

            var innerSql = rx.Groups["innerSql"].Value.Squeeze();
            var innerExpected = @"
                SELECT (?<tableName>\w+).(?<catName>\w+) AS category, (?:[\w\.]+).(?:[\w\.]+) AS value
                FROM (?:[\w\.]+)(?: AS \w*)?(?<joins>.*)?
                WHERE 1 = 1\s?(?<criterias>.*)?
                GROUP BY \1.\2, \1.\2".Squeeze();
            Assert.Matches(innerExpected, innerSql);

            rx = Regex.Match(result, innerExpected);

            var joinSql = (rx.Groups["joins"]?.Value ?? "").Squeeze();
            if (!joinSql.IsEmpty()) {
                var joinExpected = @"(?<joins>(?:INNER|LEFT|RIGHT|OUTER)?\s?JOIN\s[\w\.""]+\sON\s[\w\.""]+\s=\s[\w\.""]+\s?)+";
                Assert.Matches(joinExpected, joinSql);
                rx = Regex.Match(joinSql, joinExpected);
                Assert.Equal(expectedJoinCount, rx.Captures.Count);
            }

            var criteriaSql = (rx.Groups["criterias"]?.Value ?? "").Squeeze();
            if (!criteriaSql.IsEmpty()) {
                var criteriaExpected = ".*AND*.";
                Assert.Matches(criteriaExpected, criteriaSql);
            }
         }
    }
}
