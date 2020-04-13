using Moq;
using SeadQueryCore;
using System;
using Xunit;

namespace SeadQueryTest.QueryBuilder.FieldCompilers
{
    public class AvgFieldCompilerTests
    {
        [Theory]
        [InlineData("EXPR")]
        public void Compile_Expression_Success(string expr)
        {
            var fieldType = new Mock<ResultFieldType>();
            var fieldCompiler = new AvgFieldCompiler(fieldType.Object);
            var result = fieldCompiler.Compile(expr);
            Assert.Contains("AVG", result);
        }
    }
}
