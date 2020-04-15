using Moq;
using SeadQueryCore;
using SeadQueryCore.QueryBuilder;
using System;
using Xunit;

namespace SeadQueryTest.QueryBuilder.RangeCompilers
{
    public class RangeCategoryBoundSqlCompilerTests
    {
        public RangeCategoryBoundSqlCompilerTests()
        {
        }

        private RangeCategoryBoundSqlCompiler CreateRangeCategoryBoundSqlCompiler()
        {
            return new RangeCategoryBoundSqlCompiler();
        }

        [Fact(Skip = "Not implemented")]
        public void Compile_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var rangeCategoryBoundSqlCompiler = this.CreateRangeCategoryBoundSqlCompiler();
            QuerySetup query = null;
            Facet facet = null;
            string facetCode = null;

            // Act
            var result = rangeCategoryBoundSqlCompiler.Compile(
                query,
                facet,
                facetCode);

            // Assert
            Assert.True(false);
        }
    }
}
