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
        [InlineData("sites:sites", "sites", "count")]
        public void Compile_StateUnderTest_ExpectedBehavior(string uri, string facetCode, string aggType)
        {
            // Arrange
            var querySetupMockFactory = new MockQuerySetupFactory(Registry);
            var querySetup = querySetupMockFactory.Scaffold(uri);
            var facet = Registry.Facets.GetByCode(facetCode);
            var countFacet = facet;

            // Act

            var discreteCategoryCountSqlQueryCompiler = new DiscreteCategoryCountSqlQueryCompiler();
            var result = discreteCategoryCountSqlQueryCompiler.Compile(querySetup, facet, countFacet, aggType);

            result = Regex.Replace(result, @"\s+", " ").Trim();

            // Assert
            //@"select\s+(.*?)\s*from\s+(.*?)\s*(where\s(.*?)\s*)?;"
            var outerExpected = $@"SELECT category, {aggType}(value) AS count FROM \(.*\) AS x GROUP BY category;";
            Assert.Matches(outerExpected, result);

            var expected = $@"\s*SELECT category, {aggType}\(value\).*AS count\s+FROM.*\(\s*SELECT.*AS category,.*AS value\s*FROM\s*.*"; // WHERE.*"; // GROUP BY.+\) AS x\s*GROUP BY category;\s*";
            Assert.Matches(expected, result);

        }
    }
}
