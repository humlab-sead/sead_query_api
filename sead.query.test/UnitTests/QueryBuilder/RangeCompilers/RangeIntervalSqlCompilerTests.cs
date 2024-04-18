using Moq;
using SeadQueryCore;
using System;
using System.Linq;
using Xunit;

namespace SQT.SqlCompilers
{
    public class RangeIntervalSqlCompilerTests
    {
        private string RemoveWhiteSpace(string str)
        {
            return new String((from c in str where !char.IsWhiteSpace(c) select c).ToArray()).ToLower();
        }

        [Theory]
        [InlineData(10, 0, 120, 0)]
        [InlineData(10, 0, 500, 0)]
        public void Compile_Interval_ContainsGenerateSeries(int interval, int min, int max, int interval_count)
        {
            // Arrange
            var compiler = new RangeCategoryInfoSqlCompiler();

            // Act
            var result = compiler.Compile(interval, min, max, interval_count);

            // Assert
            var expected = $"generate_series({min},{max},{interval})";
            Assert.Contains(expected, RemoveWhiteSpace(result));
        }
    }
}
