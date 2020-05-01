using Moq;
using SeadQueryCore;
using SeadQueryCore.QueryBuilder;
using SQT.Infrastructure;
using SQT.SQL.Matcher;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace SQT.SqlCompilers
{
    // FIXME Kolla vilka som saknar detta attribut!!!
    [Collection("JsonSeededFacetContext")]
    public class ValidPicksSqlQueryCompilerTests : DisposableFacetContextContainer
    {
        public ValidPicksSqlQueryCompilerTests(JsonSeededFacetContextFixture fixture) : base(fixture)
        {
        }

        [Theory]
        [InlineData("sites:sites@5")]
        [InlineData("sites:country@57/sites@3")]
        [InlineData("country:country@57/sites@3")]
        public void Compile_StateUnderTest_ExpectedBehavior(string uri)
        {
            // Arrange
            var fakeFacetsConfig = FakeFacetsConfig(uri);
            var fakeQuerySetup = FakeQuerySetup(fakeFacetsConfig);

            // Act
            var compiler = new ValidPicksSqCompiler();
            var result = compiler.Compile(
                fakeQuerySetup,
                fakeFacetsConfig.TargetFacet,
                fakeFacetsConfig.TargetConfig.GetIntegerPickValues()
            );

            // Assert
            var matcher = new ValidPicksSqlCompilerMatcher();
            var match = matcher.Match(result);

            Assert.True(match.Success);
        }
    }
}
