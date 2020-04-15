using Moq;
using SeadQueryCore;
using SeadQueryCore.QueryBuilder;
using System;
using Xunit;

namespace SeadQueryTest.QueryBuilder.RangeCompilers
{
    public class RangeCategoryCountSqlQueryCompilerTests
    {
        private RangeCategoryCountSqlCompiler CreateRangeCategoryCountSqlQueryCompiler()
        {
            return new RangeCategoryCountSqlCompiler();
        }

        [Fact(Skip = "Not implemented")]
        public void Compile_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var rangeCategoryCountSqlQueryCompiler = this.CreateRangeCategoryCountSqlQueryCompiler();
            QuerySetup querySetup = null;
            Facet facet = null;
            string intervalQuery = null;
            string countColumn = null;

            // Act
            var result = rangeCategoryCountSqlQueryCompiler.Compile(
                querySetup,
                facet,
                intervalQuery,
                countColumn);

            // Assert
            Assert.True(false);
        }
    }
}
