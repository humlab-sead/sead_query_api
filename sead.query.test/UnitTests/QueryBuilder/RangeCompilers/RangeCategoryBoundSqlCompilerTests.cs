using Moq;
using SeadQueryCore;
using SeadQueryCore.QueryBuilder;
using SQT.Fixtures;
using SQT.Infrastructure;
using System;
using Xunit;

namespace SQT.SqlCompilers
{
    [Collection("JsonSeededFacetContext")]
    public class RangeCategoryBoundSqlCompilerTests : DisposableFacetContextContainer
    {

        public RangeCategoryBoundSqlCompilerTests(JsonSeededFacetContextFixture fixture) : base(fixture)
        {
        }

        [Theory]
        [InlineData("tbl_denormalized_measured_values_33_0:tbl_denormalized_measured_values_33_0")]
        public void Compile_RangeFacet_Success(string uri)
        {
            // Arrange
            var mockQuerySetupFactory = new MockQuerySetupFactory(Registry);
            var querySetup = mockQuerySetupFactory.Scaffold(uri);
            var facetCode = querySetup.Facet.FacetCode;

            // Act
            var compiler = new RangeCategoryBoundSqlCompiler();
            var result = compiler.Compile(querySetup, querySetup.Facet, facetCode);

            // Assert
            string expectedSql = $@"
               SELECT '{RX.ID}' AS facet_code, MIN\((?<IdExpr>.+)::real\) AS min, MAX\(\1::real\) AS max
               FROM (?:{RX.ID_AS})(?<joins> .*)?\s?(?<whereClause>WHERE .*)?";

            Assert.Matches(expectedSql.Squeeze(), result.Squeeze());
        }
    }

}
