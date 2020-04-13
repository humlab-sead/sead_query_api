using Moq;
using SeadQueryCore;
using System;
using Xunit;

namespace SeadQueryTest.QueryBuilder.FieldCompilers
{
    public class SqlFieldCompilerTests
    {
        [Theory]
        [InlineData("EXPR")]
        public void Compile_Expression_Success(string expr)
        {
            var fieldType = new Mock<ResultFieldType>();
            var fieldCompiler = new SqlFieldCompiler(fieldType.Object);
            var result = fieldCompiler.Compile(expr);
            Assert.Contains("EXPR", result);
        }
    }
}
