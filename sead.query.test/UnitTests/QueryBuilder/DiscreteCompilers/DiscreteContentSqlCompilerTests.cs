using Moq;
using SeadQueryCore;
using SeadQueryCore.QueryBuilder;
using SQT.Fixtures;
using SQT.Infrastructure;
using SQT.Mocks;
using System;
using Xunit;

namespace SQT.SqlCompilers
{
    [Collection("JsonSeededFacetContext")]
    public class DiscreteContentSqlCompilerTests : DisposableFacetContextContainer
    {
        public DiscreteContentSqlCompilerTests(JsonSeededFacetContextFixture fixture) : base(fixture)
        {
        }

        [Theory]
        [InlineData("sites:sites", 0)]
        [InlineData("country:country/sites", 1)]
        [InlineData("sites:country@57/sites@3", 1)]
        public void Compile_StateUnderTest_ExpectedBehavior(string uri, int expectedJoinCount)
        {
            // Arrange
            var facetsConfig = FakeFacetsConfig(uri);
            var mockQuerySetupFactory = new MockQuerySetupFactory(Registry);
            var querySetup = mockQuerySetupFactory.Scaffold(uri);
            var facet = MockRegistryWithFacetRepository().Object.Facets.GetByCode(facetsConfig.TargetCode);
            string text_filter = "";

            // Act

            var result = new DiscreteContentSqlCompiler().Compile(querySetup, facet, text_filter);

            // Assert
            string expectedSql = $@"
                SELECT cast\((?<IdExpr>[\w\._]+) AS varchar\) AS category, (?<NameExpr>[\w\._]+) AS name
                FROM (?:[\w\.,_]+)(?: AS \w*)?(?<joins> .*)?
                WHERE (?<Criterias>.*)
                GROUP BY \1, \2.*";

            Assert.Matches(expectedSql.Squeeze(), result.Squeeze());
        }
    }
}
