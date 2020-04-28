using Moq;
using SeadQueryCore;
using SeadQueryCore.QueryBuilder;
using SQT.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace SQT.QueryBuilder.ResultCompilers
{

    [Collection("JsonSeededFacetContext")]
    public class TabularResultSqlCompilerTests : DisposableFacetContextContainer
    {
        public TabularResultSqlCompilerTests(JsonSeededFacetContextFixture fixture) : base(fixture)
        {
        }

        [Theory]
        [InlineData("sites:sites")]
        [InlineData("sites:country@10/sites")]
        [InlineData("palaeoentomology://rdb_systems:rdb_systems", "tbl_rdb_systems", "tbl_analysis_entities", "tbl_datasets", "tbl_physical_samples")]
        [InlineData("palaeoentomology://rdb_codes:rdb_codes", "tbl_rdb_codes", "tbl_analysis_entities", "tbl_datasets", "tbl_physical_samples")]
        [InlineData("palaeoentomology://sample_group_sampling_contexts:sample_group_sampling_contexts", "tbl_sample_group_sampling_contexts", "tbl_sample_groups", "tbl_physical_samples", "tbl_analysis_entities", "tbl_datasets", "tbl_physical_samples")]
        [InlineData("palaeoentomology://data_types:data_types", "tbl_data_types", "tbl_analysis_entities", "tbl_datasets", "tbl_physical_samples")]
        [InlineData("archaeobotany://ecocode_system:ecocode_system", "tbl_ecocode_systems", "tbl_ecocode_systems", "tbl_analysis_entities", "tbl_datasets", "tbl_physical_samples")]
        [InlineData("archaeobotany://ecocode:ecocode", "tbl_ecocode_definitions", "tbl_ecocode_definitions", "tbl_analysis_entities", "tbl_datasets", "tbl_physical_samples")]
        public void Compile_StateUnderTest_ExpectedBehavior(string uri, params string[] expectedJoinTables)
        {
            // Arrange
            var querySetup = FakeQuerySetup(uri);
            var facet = querySetup.Facet;
            var fields = FakeResultAggregateFields("site_level", "tabular");

            // Act
            var compiler = new TabularResultSqlCompiler();
            var result = compiler.Compile(querySetup, facet, fields);

            var match = new TabularResultSqlCompilerMatcher().Match(result);

            // Assert
            Assert.True(match.Success);
            Assert.True(match.InnerSelect.Success);

            Assert.NotEmpty(match.InnerSelect.Tables);
            Assert.True(expectedJoinTables.All(x => match.InnerSelect.Tables.Contains(x)));
        }

    }
}
