using Moq;
using SeadQueryCore;
using SeadQueryCore.QueryBuilder;
using System;
using Xunit;

namespace SeadQueryTest.QueryBuilder.FilterCompilers
{
    public class UndefinedFacetPickFilterCompilerTests : IDisposable
    {
        private MockRepository mockRepository;



        public UndefinedFacetPickFilterCompilerTests()
        {
            this.mockRepository = new MockRepository(MockBehavior.Strict);


        }

        public void Dispose()
        {
            this.mockRepository.VerifyAll();
        }

        private UndefinedFacetPickFilterCompiler CreateUndefinedFacetPickFilterCompiler()
        {
            return new UndefinedFacetPickFilterCompiler();
        }

        [Fact]
        public void Compile_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var undefinedFacetPickFilterCompiler = this.CreateUndefinedFacetPickFilterCompiler();
            Facet targetFacet = null;
            Facet currentFacet = null;
            FacetConfig2 config = null;

            // Act
            var result = undefinedFacetPickFilterCompiler.Compile(
                targetFacet,
                currentFacet,
                config);

            // Assert
            Assert.True(false);
        }
    }
}
