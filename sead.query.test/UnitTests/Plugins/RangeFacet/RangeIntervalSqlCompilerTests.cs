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
        [InlineData(10, 0, 120)]
        [InlineData(10, 0, 500)]
        public void Compile_Interval_ContainsGenerateSeries(int interval, int min, int max)
        {
            // Arrange
            var compiler = new RangeCategoryInfoSqlCompiler();

            Tuple<int, int, int> payload = new Tuple<int, int, int>(min, max, interval);

            // Act
            var result = compiler.Compile(null, null, payload);

            // Assert
            var expected = $"generate_series({min},{max},{interval})";
            Assert.Contains(expected, RemoveWhiteSpace(result));
        }
    }
}
