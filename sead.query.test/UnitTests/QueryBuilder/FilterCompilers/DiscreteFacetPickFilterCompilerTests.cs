using SeadQueryCore;
using SeadQueryCore.QueryBuilder;
using SQT.Infrastructure;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace SQT.SqlCompilers
{
    [Collection("JsonSeededFacetContext")]
    public class DiscreteFacetPickFilterCompilerTests : DisposableFacetContextContainer
    {

        public DiscreteFacetPickFilterCompilerTests(JsonSeededFacetContextFixture fixture) : base(fixture)
        {
        }

        private DiscreteFacetPickFilterCompiler CreateDiscreteFacetPickFilterCompiler()
        {
            return new DiscreteFacetPickFilterCompiler();
        }

        [Fact]
        public void Compile_WhenTargetAndCurrentFacetAreTheSame_ReturnsEmptyString()
        {
            // Arrange
            var discreteFacetPickFilterCompiler = new DiscreteFacetPickFilterCompiler();
            Facet targetFacet = Registry.Facets.GetByCode("sites");
            Facet currentFacet = targetFacet;
            FacetConfig2 config = new FacetConfig2(
                targetFacet,
                1,
                "A = B",
                FacetConfigPick.CreateDiscrete(new List<int>() { 1, 2, 3})
            );

            // Act
            var result = discreteFacetPickFilterCompiler.Compile(targetFacet, currentFacet, config);

            // Assert
            Assert.Equal("", result);
        }

        [Fact]
        public void Compile_WhenNoPicks_ReturnsEmptyString()
        {
            // Arrange
            var discreteFacetPickFilterCompiler = new DiscreteFacetPickFilterCompiler();
            Facet targetFacet = Registry.Facets.GetByCode("sites");
            Facet currentFacet = Registry.Facets.GetByCode("country");
            FacetConfig2 config = new FacetConfig2(
                targetFacet,
                1,
                "",
                FacetConfigPick.CreateDiscrete(new List<int>() {  })
            );

            // Act
            var result = discreteFacetPickFilterCompiler.Compile(targetFacet, currentFacet, config);

            // Assert
            Assert.Equal("", result);
        }

        [Fact]
        public void Compile_WhenHasPicksAndTargetAndCurrentFacetAreNotTheSame_ReturnsCriteria()
        {
            // Arrange
            var discreteFacetPickFilterCompiler = new DiscreteFacetPickFilterCompiler();
            var picks = new List<int>() { 1, 2, 3 };
            Facet targetFacet = Registry.Facets.GetByCode("sites");
            Facet currentFacet = Registry.Facets.GetByCode("country");
            FacetConfig2 config = new FacetConfig2(targetFacet, 1, "", FacetConfigPick.CreateDiscrete(picks));

            // Act
            var result = discreteFacetPickFilterCompiler.Compile(targetFacet, currentFacet, config);

            // Assert
            Assert.NotEqual($"({currentFacet.CategoryIdExpr}::text in ({picks.BuildString<int>(", ", "'")})) ", result);
        }

    }

    public static class IEnumerableExtensions
    {
        public static string BuildString<T>(this IEnumerable<T> self, string delim = ",", string apos = "")
        {
            return string.Join(delim, self.Select(x => $"{apos}{x}{apos}"));
        }
    }
}
