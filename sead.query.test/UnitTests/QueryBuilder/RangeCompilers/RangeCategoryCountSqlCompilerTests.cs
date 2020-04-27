using Moq;
using SeadQueryCore;
using SQT.SQL.Matcher;
using SQT.Fixtures;
using SQT.Infrastructure;
using System;
using System.Text.RegularExpressions;
using Xunit;
using System.Linq;

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
        //[InlineData("palaeoentomology://geochronology:geochronology", "tbl_geochronology")]
        //[InlineData("palaeoentomology://abundances_all:abundances_all", "facet.view_abundance")]
        //[InlineData("archaeobotany://geochronology:geochronology", "tbl_geochronology")]
        //[InlineData("archaeobotany://abundances_all:abundances_all", "facet.view_abundance")]
        //[InlineData("pollen://abundances_all:abundances_all", "facet.view_abundance")]
        //[InlineData("pollen://geochronology:geochronology", "tbl_geochronology")]
        //[InlineData("geoarchaeology://tbl_denormalized_measured_values_32:tbl_denormalized_measured_values_32", "facet.method_measured_values(32,0)")]
        //[InlineData("geoarchaeology://tbl_denormalized_measured_values_37:tbl_denormalized_measured_values_37", "facet.method_measured_values(37,0)")]
        //[InlineData("geoarchaeology://tbl_denormalized_measured_values_33_0:tbl_denormalized_measured_values_33_0", "facet.method_measured_values(33,0)")]
        //[InlineData("geoarchaeology://geochronology:geochronology", "tbl_geochronology")]
        //[InlineData("geoarchaeology://tbl_denormalized_measured_values_33_82:tbl_denormalized_measured_values_33_82", "facet.method_measured_values(33,82)")]
        //[InlineData("dendrochronology://geochronology:geochronology", "tbl_geochronology")]
        //[InlineData("ceramic://geochronology:geochronology", "tbl_geochronology")]
        public void Compile_VariousConfigs_ExpectedBehavior(string uri, params string[] expectedJoinTables)
        {
            // Arrange
            var mockQuerySetupFactory = new MockQuerySetupFactory(Registry);
            var fakeQuerySetup = FakeQuerySetup(uri);
            var facet = fakeQuerySetup.Facet;
            // FIXME:::
            var intervalQuery = "( #INTERVAL-QUERY# )";
            var countColumn = "#COUNT-COLUMN#";

            // Act
            var rangeCategoryCountSqlCompiler = new RangeCategoryCountSqlCompiler();
            var result = rangeCategoryCountSqlCompiler.Compile(
                fakeQuerySetup,
                facet,
                intervalQuery,
                countColumn);

            // Assert
            result = result.Squeeze();
            var match = FacetLoadSqlMatcher
                .Create(facet.FacetTypeId).Match(result);

            Assert.True(match.Success);

            Assert.True(match.InnerSelect.Success);

            Assert.NotEmpty(match.InnerSelect.Tables);

            Assert.True(expectedJoinTables.All(x => match.InnerSelect.Tables.Contains(x)));

        }
    }
}
