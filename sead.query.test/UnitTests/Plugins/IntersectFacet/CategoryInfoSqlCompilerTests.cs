using SeadQueryCore;
using SeadQueryCore.Plugin.Intersect;
using System.Linq;
using Xunit;

namespace SQT.Plugins.Intersect
{
    public class CategoryInfoSqlCompilerTests
    {
        private string RemoveWhiteSpace(string str)
        {
            return new string((from c in str where !char.IsWhiteSpace(c) select c).ToArray()).ToLower();
        }

        [Theory]
        [InlineData(0, 120, 10, "int4range")]
        [InlineData(0, 500, 10, "numrange")]
        public void Compiled_SqlQuery_ContainsGenerateSeries(int dataLow, int dataHigh, int desiredCount, string typeName)
        {
            // Arrange
            var facet = new Facet() { CategoryIdType = typeName };
            var compiler = new IntersectCategoryInfoSqlCompiler();
            var payload = new TickerInfo(dataLow, dataHigh, desiredCount);
            var rangeType = typeName.StartsWith("int") ? "integer" : "decimal(20,2)";
            // Act
            var result = compiler.Compile(null, facet, payload);

            // Assert
            var expected = $"generate_series({dataLow}::{rangeType},{dataHigh}::{rangeType},{payload.Interval}::{rangeType})";
            Assert.Contains(expected, RemoveWhiteSpace(result));
        }
    }
}
