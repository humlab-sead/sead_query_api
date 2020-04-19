using Moq;
using SeadQueryCore;
using SeadQueryCore.QueryBuilder;
using System;
using Xunit;

namespace SQT.SqlCompilers
{
    public class RangeFacetPickFilterCompilerTests : IDisposable
    {
        private MockRepository mockRepository;



        public RangeFacetPickFilterCompilerTests()
        {
            this.mockRepository = new MockRepository(MockBehavior.Strict);


        }

        public void Dispose()
        {
            this.mockRepository.VerifyAll();
        }

        private RangeFacetPickFilterCompiler CreateRangeFacetPickFilterCompiler()
        {
            return new RangeFacetPickFilterCompiler();
        }

        [Fact(Skip = "Not implemented")]
        public void Compile_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var rangeFacetPickFilterCompiler = this.CreateRangeFacetPickFilterCompiler();
            Facet targetFacet = null;
            Facet currentFacet = null;
            FacetConfig2 config = null;

            // Act
            var result = rangeFacetPickFilterCompiler.Compile(
                targetFacet,
                currentFacet,
                config);

            // Assert
            Assert.True(false);
        }
    }
}
