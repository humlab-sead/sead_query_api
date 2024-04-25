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
        [InlineData(0, 120, 10)]
        [InlineData(0, 500, 10)]
        public void Compile_Interval_ContainsGenerateSeries(int min, int max, int count)
        {
            // Arrange
            var compiler = new RangeCategoryInfoSqlCompiler();

            var payload = Interval.Create([min, max, count]);

            // Act
            var result = compiler.Compile(null, null, payload);

            // Assert
            var expected = $"generate_series({min},{max},{payload.Width})";
            Assert.Contains(expected, RemoveWhiteSpace(result));
        }
    }
}
