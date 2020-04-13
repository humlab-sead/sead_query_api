using Moq;
using SeadQueryCore;
using System;
using Xunit;

namespace SeadQueryTest.QueryBuilder.FieldCompilers
{
    public class DefaultFieldCompilerTests
    {
        [Fact]
        public void Compile_Expression_Success()
        {
            var fieldType = new Mock<ResultFieldType>();
            var avgFieldCompiler = new DefaultFieldCompiler(fieldType.Object);
            var result = avgFieldCompiler.Compile("FOO");
            Assert.Contains("FOO", result);
        }
    }
}
