using Moq;
using SeadQueryCore;
using SeadQueryCore.QueryBuilder;
using System;
using Xunit;

namespace SeadQueryTest.QueryBuilder.RangeCompilers
{
    public class RangeCategoryBoundSqlQueryCompilerTests
    {
        public RangeCategoryBoundSqlQueryCompilerTests()
        {
        }

        private RangeCategoryBoundSqlQueryCompiler CreateRangeCategoryBoundSqlQueryCompiler()
        {
            return new RangeCategoryBoundSqlQueryCompiler();
        }

        [Fact(Skip = "Not implemented")]
        public void Compile_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var rangeCategoryBoundSqlQueryCompiler = this.CreateRangeCategoryBoundSqlQueryCompiler();
            QuerySetup query = null;
            Facet facet = null;
            string facetCode = null;

            // Act
            var result = rangeCategoryBoundSqlQueryCompiler.Compile(
                query,
                facet,
                facetCode);

            // Assert
            Assert.True(false);
        }
    }
}
