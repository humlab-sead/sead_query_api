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
    [Collection("SeadJsonFacetContextFixture")]
    public class TabularResultSqlCompilerTests : DisposableFacetContextContainer
    {
        public TabularResultSqlCompilerTests(SeadJsonFacetContextFixture fixture) : base(fixture)
        {
        }

        [Theory]
        [InlineData("constructions:constructions", "result_facet", "site_level", "tabular")]
        [InlineData("sites:sites", "result_facet", "site_level", "tabular")]
        [InlineData("sites:country@10/sites", "result_facet", "site_level", "tabular")]
        [ClassData(typeof(CompleteSetOfSingleTabularResultUriCollection))]
        public void Compile_TabularResult_Matches(string uri, string resultFacetCode, string specificationKey, string viewType)
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
