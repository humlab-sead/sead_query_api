using Moq;
using SeadQueryCore;
using SeadQueryCore.QueryBuilder;
using SQT.Fixtures;
using SQT.Infrastructure;
using SQT.Mocks;
using SQT.SQL.Matcher;
using System;
using Xunit;

namespace SQT.SqlCompilers
{
    public class DiscreteContentSelectClauseMatcher : GenericSelectSqlMatcher
    {
        //public override string ExpectedSql { get; } =
        //        @"SELECT (?<SelectFieldsSql>.*?(?= FROM))
        //            FROM (?<TargetSql>[\w\."",\(\)]+)(?: AS \w*)?(?<JoinSql>.*)?
        //            WHERE 1 = 1\s?(?<CriteriaSql>.*)?(?:\sGROUP BY (?<GroupByFieldsSql>.*))?".Squeeze();
        public override string ExpectedSql { get; } = $@"
                SELECT cast\((?<CategoryExpr>[\w\._]+) AS varchar\) AS category, (?<ValueExpr>[\w\._]+) AS name
                FROM (?<TargetSql>[\w\."",\(\)]+)(?: AS \w*)?(?<JoinSql>.*)?
                WHERE (?<CriteriaSql>.*)
                GROUP BY \1, \2.*";

    }

    [Collection("JsonSeededFacetContext")]
    public class DiscreteContentSqlCompilerTests : DisposableFacetContextContainer
    {
        public DiscreteContentSqlCompilerTests(JsonSeededFacetContextFixture fixture) : base(fixture)
        {
        }

        [Theory]
        [InlineData("sites:sites")]
        [InlineData("country:country/sites")]
        [InlineData("sites:country@57/sites@3")]
        public void Compile_StateUnderTest_ExpectedBehavior(string uri)
        {

            // Arrange
            var fakeFacetsConfig = FakeFacetsConfig(uri);
            var fakeQuerySetup = FakeQuerySetup(uri);
            var facet = MockRegistryWithFacetRepository().Object.Facets.GetByCode(fakeFacetsConfig.TargetCode);
            string text_filter = "";

            // Act

            var result = new DiscreteContentSqlCompiler().Compile(fakeQuerySetup, facet, text_filter);

            // Assert
            var matcher = new DiscreteContentSelectClauseMatcher();
            var match = matcher.Match(result);
            Assert.True(match.Success);
            // FIXME : More asserts!

        }
    }
}
