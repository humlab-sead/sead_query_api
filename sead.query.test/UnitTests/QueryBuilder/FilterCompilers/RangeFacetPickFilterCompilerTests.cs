using Moq;
using SeadQueryCore;
using SeadQueryCore.QueryBuilder;
using SQT.Infrastructure;
using System;
using Xunit;

namespace SQT.SqlCompilers
{
    [Collection("SeadJsonFacetContextFixture")]
    public class RangeFacetPickFilterCompilerTests : DisposableFacetContextContainer
    {
        public RangeFacetPickFilterCompilerTests(SeadJsonFacetContextFixture fixture) : base(fixture)
        {
        }

        [Theory]
        [InlineData("tbl_denormalized_measured_values_33_0:tbl_denormalized_measured_values_33_0")]
        [InlineData("tbl_denormalized_measured_values_33_0:tbl_denormalized_measured_values_33_0@0,10")]
        [InlineData("tbl_denormalized_measured_values_33_0:tbl_denormalized_measured_values_33_0@10,10")]
        public void Compile_VariousConfigs_ExpectedBehavior(string uri)
        {
            // Arrange
            var facetsConfig = FakeFacetsConfig(uri);
            var facetConfig = facetsConfig.TargetConfig;
            var facet = facetConfig.Facet;

            // Act
            var rangeFacetPickFilterCompiler = new RangeFacetPickFilterCompiler();
            var result = rangeFacetPickFilterCompiler.Compile(null, facet, facetConfig);

            // Assert

            if (facetConfig.HasPicks())
            {
                var picks = facetConfig.Picks;
                var sqlEqualExpected = (picks[0].PickValue == picks[1].PickValue) ?
                        @"\(floor\(.+\) = [\d+-,]+\)" : @"\(.+ >= [\d+-,]+ and .+ <= [\d+-,]+\)";
                var sqlWhere = facetConfig.HasCriterias() ? "AND .+" : "";
                var sqlExpected = $@"{sqlEqualExpected}\s?{sqlWhere}";

                Assert.Matches(sqlExpected, result.Squeeze());
            }
            else
            {
                Assert.Matches("", result.Squeeze());
            }
        }
    }
}
