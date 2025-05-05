using SeadQueryCore;
using SeadQueryCore.Plugin.Range;
using SQT.SQL.Matcher;
using SQT.Infrastructure;
using Xunit;

namespace SQT.Plugins.Range
{
    [Collection("SqliteFacetContext")]
    public class CategoryCountSqlCompilerTests(SqliteFacetContext fixture) : MockerWithFacetContext(fixture)
    {
        [Theory]
        [InlineData("tbl_denormalized_measured_values_33_0:tbl_denormalized_measured_values_33_0")]
        [InlineData("palaeoentomology://geochronology:geochronology")]
        [InlineData("palaeoentomology://abundances_all:abundances_all")]
        [InlineData("archaeobotany://geochronology:geochronology")]
        [InlineData("archaeobotany://abundances_all:abundances_all")]
        [InlineData("pollen://abundances_all:abundances_all")]
        [InlineData("pollen://geochronology:geochronology")]
        [InlineData("geoarchaeology://tbl_denormalized_measured_values_32:tbl_denormalized_measured_values_32")]
        [InlineData("geoarchaeology://tbl_denormalized_measured_values_37:tbl_denormalized_measured_values_37")]
        [InlineData("geoarchaeology://tbl_denormalized_measured_values_33_0:tbl_denormalized_measured_values_33_0")]
        [InlineData("geoarchaeology://geochronology:geochronology")]
        [InlineData("geoarchaeology://tbl_denormalized_measured_values_33_82:tbl_denormalized_measured_values_33_82")]
        [InlineData("dendrochronology://geochronology:geochronology")]
        [InlineData("ceramic://geochronology:geochronology")]
        public void Compile_VariousConfigs_ExpectedBehavior(string uri)
        {
            // Arrange
            var fakeFacetsConfig = FakeFacetsConfig(uri);
            var fakeQuerySetup = FakeCountOrContentQuerySetup(fakeFacetsConfig);
            var facet = fakeQuerySetup.Facet;
            const string intervalQuery = "( #INTERVAL-QUERY# )";
            const string countColumn = "dummy_column";

            CompilePayload compilePayload = new CompilePayload()
            {
                IntervalQuery = intervalQuery,
                CountColumn = countColumn,
                AggregateType = null,
                AggregateFacet = null,
                TargetFacet = null
            };
            // Act
            var rangeCategoryCountSqlCompiler = new RangeCategoryCountSqlCompiler();
            var result = rangeCategoryCountSqlCompiler.Compile(fakeQuerySetup, facet, compilePayload);

            // Assert
            result = result.Squeeze();
            var match = CategoryCountSqlCompilerMatcher
                .Create(facet.FacetTypeId).Match(result);

            Assert.True(match.Success);

            Assert.True(match.InnerSelect.Success);

            Assert.NotEmpty(match.InnerSelect.Tables);
        }
    }
}
