using SeadQueryCore;
using SeadQueryCore.QueryBuilder;
using SQT.Infrastructure;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace SQT.SqlCompilers
{
    [Collection("SeadJsonFacetContextFixture")]
    public class DiscreteFacetPickFilterCompilerTests : DisposableFacetContextContainer
    {
        public DiscreteFacetPickFilterCompilerTests(SeadJsonFacetContextFixture fixture) : base(fixture)
        {
        }

        [Fact]
        public void Compile_WhenTargetAndCurrentFacetAreTheSame_ReturnsEmptyString()
        {
            var discreteFacetPickFilterCompiler = new DiscreteFacetPickFilterCompiler();
            Facet targetFacet = Registry.Facets.GetByCode("sites");
            Facet currentFacet = targetFacet;
            var config = new FacetConfig2(
                targetFacet,
                1,
                "A = B",
                FacetConfigPick.CreateByList(new List<int>() { 1, 2, 3 })
            );

            var result = discreteFacetPickFilterCompiler.Compile(targetFacet, currentFacet, config);

            Assert.Equal("", result);
        }

        [Fact]
        public void Compile_WhenNoPicks_ReturnsEmptyString()
        {
            // Arrange
            var discreteFacetPickFilterCompiler = new DiscreteFacetPickFilterCompiler();
            Facet targetFacet = Registry.Facets.GetByCode("sites");
            Facet currentFacet = Registry.Facets.GetByCode("country");
            var config = new FacetConfig2(
                targetFacet,
                1,
                "",
                FacetConfigPick.CreateByList(new List<int>())
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
            FacetConfig2 config = new FacetConfig2(targetFacet, 1, "", FacetConfigPick.CreateByList(picks));

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
