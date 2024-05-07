using Moq;
using SeadQueryCore;
using SeadQueryCore.Plugin.Range;
using System;
using System.Linq;
using Xunit;

namespace SQT.Plugins.Range
{
    public class CategoryInfoSqlCompilerTests
    {
        private string RemoveWhiteSpace(string str)
        {
            return new string((from c in str where !char.IsWhiteSpace(c) select c).ToArray()).ToLower();
        }

        [Theory]
        [InlineData(0, 120, 10, "integer")]
        [InlineData(0, 500, 10, "decimal(20,5)")]
        public void Compile_Interval_ContainsGenerateSeries(int min, int max, int count, string typeName)
        {
            // Arrange
            var facet = new Facet() { CategoryIdType = typeName };

            var compiler = new RangeCategoryInfoSqlCompiler();

            var payload = new TickerInfo(min, max, count);

            // Act
            var result = compiler.Compile(null, facet, payload);

            // Assert
            var expected = $"generate_series({min}::{typeName},{max}::{typeName},{payload.Interval}::{typeName})";
            Assert.Contains(expected, RemoveWhiteSpace(result));
        }
    }
}
