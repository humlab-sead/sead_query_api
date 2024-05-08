using Moq;
using SeadQueryCore;
using System;
using Xunit;

namespace SQT.SqlCompilers
{
    public class CountFieldCompilerTests
    {
        [Theory]
        [InlineData("A")]
        public void Compile_Expression_Success(string expr)
        {
            var fieldType = new Mock<ResultFieldType>();
            var fieldCompiler = new CountFieldCompiler(fieldType.Object);
            var result = fieldCompiler.Compile(expr);
            Assert.Contains("COUNT(A)", result);
        }

        [Theory]
        [InlineData("A")]
        public void Compile_DistinctExpression_Success(string expr)
        {
            var fieldType = new Mock<ResultFieldType>();
            var fieldCompiler = new CountDistinctFieldCompiler(fieldType.Object);
            var result = fieldCompiler.Compile(expr);
            Assert.Contains("COUNT(DISTINCT A)", result);
        }
    }


}
