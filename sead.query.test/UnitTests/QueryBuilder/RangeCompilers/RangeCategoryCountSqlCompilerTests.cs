using Moq;
using SeadQueryCore;
using SeadQueryCore.QueryBuilder;
using System;
using Xunit;

namespace SeadQueryTest.QueryBuilder.RangeCompilers
{
    public class RangeCategoryCountSqlCompilerTests
    {
        private RangeCategoryCountSqlCompiler CreateRangeCategoryCountSqlCompiler()
        {
            return new RangeCategoryCountSqlCompiler();
        }

        [Fact(Skip = "Not implemented")]
        public void Compile_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var rangeCategoryCountSqlCompiler = this.CreateRangeCategoryCountSqlCompiler();
            QuerySetup querySetup = null;
            Facet facet = null;
            string intervalQuery = null;
            string countColumn = null;

            // Act
            var result = rangeCategoryCountSqlCompiler.Compile(
                querySetup,
                facet,
                intervalQuery,
                countColumn);

            // Assert
            Assert.True(false);
        }
    }
}
