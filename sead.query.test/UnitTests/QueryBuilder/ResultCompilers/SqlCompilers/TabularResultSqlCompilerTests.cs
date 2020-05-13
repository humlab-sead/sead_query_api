using Moq;
using SeadQueryCore;
using SeadQueryCore.QueryBuilder;
using SQT.ClassData;
using SQT.Infrastructure;
using SQT.SQL.Matcher;
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
        [InlineData("sites:sites", "result_facet", "site_level", "tabular")]
        [InlineData("sites:country@10/sites", "result_facet", "site_level", "tabular")]
        [InlineData("palaeoentomology://rdb_systems:rdb_systems", "result_facet", "site_level", "tabular")]
        [InlineData("palaeoentomology://rdb_codes:rdb_codes", "result_facet", "site_level", "tabular")]
        [InlineData("palaeoentomology://sample_group_sampling_contexts:sample_group_sampling_contexts", "result_facet", "site_level", "tabular")]
        [InlineData("palaeoentomology://data_types:data_types", "result_facet", "site_level", "tabular")]
        [InlineData("archaeobotany://ecocode:ecocode", "result_facet", "site_level", "tabular")]
        [ClassData(typeof(CompleteSetOfSingleTabularResultUriCollection))]
        public void Compile_StateUnderTest_ExpectedBehavior(string uri, string resultFacetCode, string specificationKey, string viewType)
        {
            // Arrange
            var fakeFacetsConfig = FakeFacetsConfig(uri);
            var fakeQuerySetup = FakeResultQuerySetup(fakeFacetsConfig, resultFacetCode, specificationKey);
            var facet = fakeQuerySetup.Facet;
            var fields = FakeResultConfig(resultFacetCode, specificationKey, viewType).GetSortedFields();

            // Act
            var compiler = new TabularResultSqlCompiler();
            var result = compiler.Compile(fakeQuerySetup, facet, fields);

            var match = new TabularResultSqlCompilerMatcher().Match(result);

            // Assert
            Assert.True(match.Success);
            Assert.True(match.InnerSelect.Success);
            Assert.NotEmpty(match.InnerSelect.Tables);

        }
    }
}
