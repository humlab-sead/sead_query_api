using Moq;
using SeadQueryCore;
using SeadQueryCore.QueryBuilder;
using SeadQueryTest.Fixtures;
using SeadQueryTest.Infrastructure;
using System;
using System.Text.RegularExpressions;
using Xunit;

namespace SeadQueryTest.QueryBuilder.DiscreteCompilers
{
    [Collection("JsonSeededFacetContext")]
    public class DiscreteCategoryCountSqlQueryCompilerTests : DisposableFacetContextContainer
    {
        public DiscreteCategoryCountSqlQueryCompilerTests(JsonSeededFacetContextFixture fixture) : base(fixture)
        {
        }

        [Theory]
        [InlineData("sites:sites", "sites", "count", 0)]
        [InlineData("country:sites", "sites", "count", 1)]
        [InlineData("country@57:sites@3", "sites", "count", 1)]
        public void Compile_StateUnderTest_ExpectedBehavior(string uri, string facetCode, string aggType, int expectedJoinCount)
        {
            // Arrange
            var querySetupMockFactory = new MockQuerySetupFactory(Registry);
            var querySetup = querySetupMockFactory.Scaffold(uri);
            var facet = Registry.Facets.GetByCode(facetCode);
            var countFacet = facet;

            // Act

            var discreteCategoryCountSqlQueryCompiler = new DiscreteCategoryCountSqlQueryCompiler();
            var result = discreteCategoryCountSqlQueryCompiler.Compile(querySetup, facet, countFacet, aggType);

            result = result.Squeeze();

            // Assert

            var outerExpected = $@"SELECT category, {aggType}\(value\) AS count FROM \((?<innerSql>.*)\) AS x GROUP BY category;";
            Assert.Matches(outerExpected, result);

            var rx = Regex.Match(result, outerExpected);

            var innerSql = rx.Groups["innerSql"].Value.Squeeze();
            var innerExpected = @"SELECT (?<tableName>\w+).(?<catName>\w+) AS category, (?:[\w\.]+).(?:[\w\.]+) AS value FROM (?:[\w\.]+) (?:AS \w*)?(?<joins>.*)?\s?WHERE 1 = 1 (?<criterias>.*)?\s?GROUP BY \1.\2, \1.\2";
            Assert.Matches(innerExpected, innerSql);

            rx = Regex.Match(result, innerExpected);

            var joinSql = (rx.Groups["joins"]?.Value ?? "").Squeeze();
            if (!joinSql.IsEmpty()) {
                var joinExpected = @"(?<joins>(?:INNER|LEFT|RIGHT|OUTER)?\s?JOIN\s[\w\.""]+\sON\s[\w\.""]+\s=\s[\w\.""]+\s?)+";
                Assert.Matches(joinExpected, joinSql);
                rx = Regex.Match(joinSql, joinExpected);
                Assert.Equal(expectedJoinCount, rx.Groups.Count);
            }

            var criteriaSql = (rx.Groups["criterias"]?.Value ?? "").Squeeze();
            if (!criteriaSql.IsEmpty()) {
                var criteriaExpected = ".*AND*.";
                Assert.Matches(criteriaExpected, criteriaSql);
            }
         }
    }
}
