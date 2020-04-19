using Moq;
using SeadQueryCore;
using System;
using System.Linq;
using Xunit;

namespace SQT.SqlCompilers
{
    public class RangeIntervalSqlCompilerTests
    {
        private RangeIntervalSqlCompiler CreateRangeIntervalSqlCompiler()
        {
            return new RangeIntervalSqlCompiler();
        }

        private string removeWhiteSpace(string str)
        {
            return new String((from c in str where !char.IsWhiteSpace(c) select c).ToArray()).ToLower();
        }

        [Fact]
        public void Compile_Interval_ContainsGenerateSeries()
        {
            // Arrange
            var compiler = this.CreateRangeIntervalSqlCompiler();
            int interval = 10;
            int min = 0;
            int max = 120;
            int interval_count = 0;

            // Act
            var result = compiler.Compile(
                interval,
                min,
                max,
                interval_count
            );

            // Assert
            var expected = $"generate_series({min},{max},{interval})";
            Assert.Contains(expected, removeWhiteSpace(result));
        }
    }
}
