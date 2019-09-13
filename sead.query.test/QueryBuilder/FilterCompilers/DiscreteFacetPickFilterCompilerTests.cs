using Moq;
using SeadQueryCore;
using SeadQueryCore.QueryBuilder;
using System;
using Xunit;

namespace SeadQueryTest.QueryBuilder.FilterCompilers
{
    public class DiscreteFacetPickFilterCompilerTests : IDisposable
    {

        public DiscreteFacetPickFilterCompilerTests()
        {
        }

        public void Dispose()
        {
        }

        private DiscreteFacetPickFilterCompiler CreateDiscreteFacetPickFilterCompiler()
        {
            return new DiscreteFacetPickFilterCompiler();
        }

        [Fact]
        public void Compile_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var discreteFacetPickFilterCompiler = this.CreateDiscreteFacetPickFilterCompiler();
            Facet targetFacet = null;
            Facet currentFacet = null;
            FacetConfig2 config = null;

            // Act
            var result = discreteFacetPickFilterCompiler.Compile(
                targetFacet,
                currentFacet,
                config);

            // Assert
            Assert.True(false);
        }
    }
}
