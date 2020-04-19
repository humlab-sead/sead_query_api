using Autofac.Features.Indexed;
using Moq;
using SeadQueryCore;
using SeadQueryCore.Model;
using SeadQueryCore.QueryBuilder;
using SQT.Infrastructure;
using System;
using Xunit;

namespace SQT.SqlCompilers
{
    public class ResultConfigCompilerTests : DisposableFacetContextContainer
    {
        public ResultConfigCompilerTests(JsonSeededFacetContextFixture fixture) : base(fixture)
        {
        }


        [Theory]
        [InlineData("sites:sites", "site_level", "tabular")]
        [InlineData("sites:sites", "site_level", "map")]
        public void Compile_StateUnderTest_ExpectedBehavior(string uri, string aggregateKey, string viewTypeId)
        {
            // Arrange
            var facetsConfig = FakeFacetsConfig(uri);
            var querySetup = FakeQuerySetup(uri);
            ResultConfig fakeResultConfig = FakeResultConfig(aggregateKey, viewTypeId);

            var mockQuerySetupBuilder = MockQuerySetupBuilder(querySetup);
            var mockResultSqlCompilerLocator = MockResultSqlCompilerLocator("#SQL#");

            string facetCode = facetsConfig.TargetCode;

            // Act
            var resultQueryCompiler = new SeadQueryCore.ResultConfigCompiler(
                base.Registry,
                mockQuerySetupBuilder.Object,
                mockResultSqlCompilerLocator.Object
            );

            var result = resultQueryCompiler.Compile(facetsConfig, fakeResultConfig, facetCode);

            // Assert
            Assert.NotEmpty(result);

            // MAP
            var expectedSql = "#SQL#";

            Assert.Equal(expectedSql, result);

        }

    }
}
