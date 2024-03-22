using Moq;
using SeadQueryCore;
using System;
using Xunit;

namespace SQT.SqlCompilers
{
    public class TextAggFieldCompilerTests
    {
        [Fact]
        public void Compile_TextAggregateField_Matches()
        {
            var fieldType = new Mock<ResultFieldType>();
            fieldType.Setup(z => z.SqlTemplate).Returns("X{0}Z");
            var fieldCompiler = new TextAggFieldCompiler(fieldType.Object);
            var result = fieldCompiler.Compile("FOO");
            Assert.Contains("array_to_string", result);
            Assert.Contains("FOO", result);
        }
    }
}
