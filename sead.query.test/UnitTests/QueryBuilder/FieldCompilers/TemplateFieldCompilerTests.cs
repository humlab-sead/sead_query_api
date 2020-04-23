using Moq;
using SeadQueryCore;
using System;
using Xunit;

namespace SQT.SqlCompilers
{
    public class TemplateFieldCompilerTests
    {
        [Theory]
        [InlineData("EXPR")]
        public void Compile_Expression_Success(string expr)
        {
            var fieldType = new Mock<ResultFieldType>();
            fieldType.Setup(z => z.SqlTemplate).Returns("X{0}Z");
            var fieldCompiler = new TemplateFieldCompiler(fieldType.Object);
            var result = fieldCompiler.Compile(expr);
            Assert.Contains("XEXPRZ", result);
        }
    }
}
