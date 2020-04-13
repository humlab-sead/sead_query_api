using Moq;
using SeadQueryCore;
using System;
using Xunit;

namespace SeadQueryTest.QueryBuilder.FieldCompilers
{
    public class SumFieldCompilerTests
    {
        [Theory]
        [InlineData("A")]
        public void Compile_Expression_Success(string expr)
        {
            var fieldType = new Mock<ResultFieldType>();
            var fieldCompiler = new SumFieldCompiler(fieldType.Object);
            var result = fieldCompiler.Compile(expr);
            Assert.Contains("SUM", result);
        }
    }
}
