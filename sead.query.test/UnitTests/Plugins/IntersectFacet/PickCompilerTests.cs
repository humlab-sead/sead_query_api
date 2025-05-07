using SeadQueryCore;
using SeadQueryCore.Plugin.Intersect;
using SQT.Infrastructure;
using SQT.Scaffolding;
using System;
using System.Collections.Generic;
using Xunit;

namespace SQT.Plugins.Intersect
{
    [Collection("UsePostgresDockerSession")]
    public class PickCompilerTests() : MockerWithFacetContext()
    {
        [Fact]
        public void Compile_WhenTargetAndCurrentFacetAreTheSame_ReturnsEmptyString()
        {
            var compiler = new IntersectPickFilterCompiler();
            Facet targetFacet = Registry.Facets.GetByCode("analysis_entity_ages");
            var config = new FacetConfig2(targetFacet, 1, "A = B", []);
            var result = compiler.Compile(targetFacet, targetFacet, config);
            Assert.Equal("", result);
        }

        [Fact]
        public void Compile_WhenNoPicks_ReturnsFacetCriteria()
        {
            var compiler = new IntersectPickFilterCompiler();
            Facet targetFacet = Registry.Facets.GetByCode("analysis_entity_ages");
            Facet currentFacet = Registry.Facets.GetByCode("country");
            var config = new FacetConfig2(targetFacet, 1, "", []);
            var result = compiler.Compile(targetFacet, currentFacet, config);
            Assert.Equal(currentFacet.Criteria, result);
        }

        [Fact]
        public void Compile_InvalidBound_RaisesArgumentException()
        {
            var compiler = new IntersectPickFilterCompiler();
            Facet targetFacet = Registry.Facets.GetByCode("analysis_entity_ages");
            Assert.Throws<ArgumentException>(() =>
            {
                var picks = new List<int>() { 1 };
                FacetConfig2 config = new FacetConfig2(targetFacet, 1, "", FacetConfigPick.CreateByList(picks));
                var result = compiler.Compile(targetFacet, targetFacet, config);
            });
        }

        [Fact]
        public void Compile_WhenHasPicks_ReturnsCriteria()
        {
            var compiler = new IntersectPickFilterCompiler();
            var picks = new List<int>() { 1, 2 };
            Facet targetFacet = Registry.Facets.GetByCode("analysis_entity_ages");
            FacetConfig2 config = new FacetConfig2(targetFacet, 1, "", FacetConfigPick.CreateByList(picks));
            var result = compiler.Compile(targetFacet, targetFacet, config);
            Assert.Equal($"int4range({picks.BuildString<int>(", ", "")}, '[]') && {targetFacet.CategoryIdExpr}", result);
        }

        [Fact]
        public void Compile_OtherTarget()
        {
            var compiler = new IntersectPickFilterCompiler();
            Facet currentFacet = Registry.Facets.GetByCode("analysis_entity_ages");
            Facet targetFacet = Registry.Facets.GetByCode("country");

            var picks = new List<int>() { 1, 2 };
            FacetConfig2 config = new FacetConfig2(targetFacet, 1, "", FacetConfigPick.CreateByList(picks));
            var result = compiler.Compile(targetFacet, currentFacet, config);
            Assert.Equal($"int4range({picks.BuildString<int>(", ", "")}, '[]') && {currentFacet.CategoryIdExpr}", result);

        }

    }

}
